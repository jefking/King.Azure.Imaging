namespace King.Azure.Imaging
{
    using King.Azure.Data;
    using King.Azure.Imaging.Entities;
    using King.Azure.Imaging.Models;
    using Microsoft.WindowsAzure.Storage.Queue;
    using Newtonsoft.Json;
    using System;
    using System.Threading.Tasks;

    public class ImageStore
    {
        private readonly IContainer container = null;
        private readonly ITableStorage table = null;
        private readonly IImaging imaging = null;
        private readonly IQueue<ImageQueued> queue = null;
        public async Task Save(string fileName, byte[] content, string version, string mimeType, Guid identifier, bool toQueue = false, string extension = null, int quality = 100, int width = 0, int height = 0)
        {
            //Store in Blob
            await this.container.Save(fileName, content, mimeType);

            if (0 >= width || 0 >= height)
            {
                var size = this.imaging.Size(content);
                width = size.Width;
                height = size.Height;
            }

            //Store in Table
            await this.table.InsertOrReplace(new ImageEntity
            {
                PartitionKey = identifier.ToString(),
                RowKey = version,
                FileName = fileName,
                ContentType = mimeType,
                FileSize = content.LongLength,
                Width = width,
                Height = height,
                Quality = quality,
                RelativePath = string.Format("{0}/{1}", container.Name, fileName),
            });

            if (toQueue)
            {
                //Queue for Processing
                await this.queue.Save(new ImageQueued
                {
                    Identifier = identifier,
                    FileNameFormat = string.Format(ImagePreprocessor.FileNameFormat, identifier, "{0}", "{1}"),
                    OriginalExtension = extension,
                });
            }
        }
    }
}