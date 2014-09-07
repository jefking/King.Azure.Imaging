namespace King.Azure.Imaging
{
    using ImageResizer;
    using King.Azure.Data;
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
        private readonly IDictionary<string, string> versions;
        private readonly IContainer container;
        private readonly ITableStorage table;
        #endregion

        #region Constructors
        public ImagingProcessor(IContainer container, IDictionary<string, string> versions)
        {
            this.container = container;
            this.versions = versions;
        }
        #endregion

        #region Methods
        public async Task<bool> Process(ImageQueued data)
        {
            var result = false;

            try
            {
                var bytes = await container.Get(data.FileName);
                foreach (var key in this.versions.Keys)
                {
                    using (var input = new MemoryStream(bytes))
                    {
                        using (var output = new MemoryStream())
                        {
                            var job = new ImageJob(input, output, new Instructions(versions[key]));
                            job.Build();

                            var filename = data.FileName.Replace("_original", key);
                            await container.Save(filename, output.ToArray(), job.ResultMimeType);
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