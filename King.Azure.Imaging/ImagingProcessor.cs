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
        protected readonly IDictionary<string, IImageVersion> versions = null;

        /// <summary>
        /// Container
        /// </summary>
        protected readonly IContainer container = null;

        /// <summary>
        /// Table
        /// </summary>
        protected readonly ITableStorage table = null;
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
                    byte[] resized;
                    string mimeType;
                    var filename = string.Format(data.FileNameFormat, key.ToLowerInvariant());
                    var format = new JpegFormat { Quality = 70 };
                    using (var input = new MemoryStream(bytes))
                    {
                        using (var output = new MemoryStream())
                        {
                            var size = new Size(version.Width, version.Height);
                            using (ImageFactory imageFactory = new ImageFactory(preserveExifData: true))
                            {
                                imageFactory.Load(input)
                                            .Resize(size)
                                            .Format(format)
                                            .Save(output);
                            }

                            resized = output.ToArray();
                        }
                    }

                    await this.container.Save(filename, resized, format.MimeType);
                    var entity = new ImageEntity()
                    {
                        PartitionKey = data.Identifier.ToString(),
                        RowKey = key.ToLowerInvariant(),
                        FileName = filename,
                        ContentType = format.MimeType,
                        FileSize = resized.LongLength,
                        Width = version.Width,
                        Height = version.Height,
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