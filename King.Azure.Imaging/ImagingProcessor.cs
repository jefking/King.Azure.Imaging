namespace King.Azure.Imaging
{
    using ImageResizer;
    using King.Azure.Data;
    using King.Azure.Imaging.Entities;
    using King.Azure.Imaging.Models;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
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
        private readonly IDictionary<string, string> versions;

        /// <summary>
        /// Container
        /// </summary>
        private readonly IContainer container;

        /// <summary>
        /// Table
        /// </summary>
        private readonly ITableStorage table;
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
        public async Task<bool> Process(ImageQueued data)
        {
            var result = false;
            var original = string.Format(data.FileNameFormat, ImagePreprocessor.Original);

            try
            {
                var bytes = await container.Get(original);
                foreach (var key in this.versions.Keys)
                {
                    using (var input = new MemoryStream(bytes))
                    {
                        using (var output = new MemoryStream())
                        {
                            var job = new ImageJob(input, output, new Instructions(versions[key]));
                            job.Build();

                            var resized = output.ToArray();
                            var filename = string.Format(data.FileNameFormat, key.ToLowerInvariant());
                            await this.container.Save(filename, resized, job.ResultMimeType);
                            var entity = new ImageEntity()
                            {
                                PartitionKey = data.Identifier.ToString(),
                                RowKey = key.ToLowerInvariant(),
                                FileName = filename,
                                ContentType = job.ResultMimeType,
                                FileSize = resized.LongLength,
                            };

                            await this.table.InsertOrReplace(entity);
                        }
                    }
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