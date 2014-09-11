namespace King.Azure.Imaging
{
    using ImageProcessor;
using ImageProcessor.Imaging.Formats;
using King.Azure.Data;
using King.Azure.Imaging.Entities;
using King.Azure.Imaging.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
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
        protected readonly IDictionary<string, string> versions;

        /// <summary>
        /// Container
        /// </summary>
        protected readonly IContainer container;

        /// <summary>
        /// Table
        /// </summary>
        protected readonly ITableStorage table;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="container"></param>
        /// <param name="versions"></param>
        public ImagingProcessor(IContainer container, ITableStorage table, IDictionary<string, string> versions)
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
                    byte[] resized;
                    string mimeType;
                    var filename = string.Format(data.FileNameFormat, key.ToLowerInvariant());
                    using (var input = new MemoryStream(bytes))
                    {
                        using (var output = new MemoryStream())
                        {
                            var format = new JpegFormat { Quality = 70 };
                            var size = new Size(150, 0);
                            using (ImageFactory imageFactory = new ImageFactory(preserveExifData: true))
                            {
                                imageFactory.Load(input)
                                            .Resize(size)
                                            .Format(format)
                                            .Save(output);
                            }

                            resized = output.ToArray();
                            mimeType = "unknown";//BUG
                        }
                    }

                    await this.container.Save(filename, resized, mimeType);
                    var entity = new ImageEntity()
                    {
                        PartitionKey = data.Identifier.ToString(),
                        RowKey = key.ToLowerInvariant(),
                        FileName = filename,
                        ContentType = mimeType,
                        FileSize = resized.LongLength,
                        RelativePath = string.Format("{0}/{1}", this.container.Name, filename),
                    };

                    await this.table.InsertOrReplace(entity);
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