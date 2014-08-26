namespace King.Azure.Imaging.Entities
{
    using Microsoft.WindowsAzure.Storage.Table;

    public class ImageEntity : TableEntity
    {
        /// <summary>
        /// Content Type
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// File Name
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// File Size
        /// </summary>
        public int FileSize { get; set; }
    }
}