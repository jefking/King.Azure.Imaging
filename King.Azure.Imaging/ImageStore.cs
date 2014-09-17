namespace King.Azure.Imaging
{
    using King.Azure.Data;
    using King.Azure.Imaging.Entities;
    using System;
    using System.Threading.Tasks;

    public class ImageStore
    {
        private readonly IContainer container = null;
        private readonly ITableStorage table = null;
        public async Task Save(string fileName, byte[] content, string version, string mimeType, Guid identifier, int width, int height, int quality)
        {
            //Store in Blob
            await this.container.Save(fileName, content, mimeType);

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
        }
    }
}