﻿namespace King.Azure.Imaging
{
    using King.Azure.Data;
    using King.Azure.Imaging.Entities;
    using King.Azure.Imaging.Models;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading.Tasks;

    /// <summary>
    /// Imaging Task
    /// </summary>
    public class ImagingProcessor : IProcessor<ImageQueued>
    {
        #region Members
        /// <summary>
        /// Versions
        /// </summary>
        protected readonly IDictionary<string, IImageVersion> versions = null;

        /// <summary>
        /// Container
        /// </summary>
        protected readonly IContainer container = null;

        /// <summary>
        /// Table
        /// </summary>
        protected readonly ITableStorage table = null;

        /// <summary>
        /// Imaging
        /// </summary>
        protected readonly Imaging img = new Imaging();
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="container"></param>
        /// <param name="versions"></param>
        public ImagingProcessor(IContainer container, ITableStorage table, IDictionary<string, IImageVersion> versions)
        {
            if (null == container)
            {
                throw new ArgumentNullException("container");
            }
            if (null == table)
            {
                throw new ArgumentNullException("table");
            }
            if (null == versions)
            {
                throw new ArgumentNullException("versions");
            }

            this.container = container;
            this.table = table;
            this.versions = versions;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Process Image Queued
        /// </summary>
        /// <param name="data">Data</param>
        /// <returns>Successful</returns>
        public virtual async Task<bool> Process(ImageQueued data)
        {
            var result = false;
            var original = string.Format(data.FileNameFormat, ImagePreprocessor.Original);

            try
            {
                var bytes = await container.Get(original);
                foreach (var key in this.versions.Keys)
                {
                    var version = this.versions[key];
                    var filename = string.Format(data.FileNameFormat, key.ToLowerInvariant());
                    string mimeType;

                    var resized = img.Resize(bytes, version, out mimeType);

                    //Store in Blob
                    await this.container.Save(filename, resized, mimeType);

                    var size = img.Size(resized);

                    //Store in Table
                    await this.table.InsertOrReplace(new ImageEntity()
                    {
                        PartitionKey = data.Identifier.ToString(),
                        RowKey = key.ToLowerInvariant(),
                        FileName = filename,
                        ContentType = mimeType,
                        FileSize = resized.LongLength,
                        Width = size.Width,
                        Height = size.Height,
                        RelativePath = string.Format("{0}/{1}", this.container.Name, filename),
                    });
                }

                result = true;
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
            }

            return result;
        }
        #endregion
    }
}