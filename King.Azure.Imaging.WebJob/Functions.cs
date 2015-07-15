namespace King.Azure.Imaging.WebJob
{
    using King.Azure.Imaging.Models;
    using Microsoft.Azure;
    using Microsoft.Azure.WebJobs;
    using Newtonsoft.Json;

    /// <summary>
    /// Web Job Functions
    /// </summary>
    public class Functions
    {
        #region Members
        /// <summary>
        /// Image Versions
        /// </summary>
        private static readonly IVersions versions = new Versions();
        #endregion

        #region Methods
        /// <summary>
        /// Image Processing
        /// </summary>
        /// <param name="image">image</param>
        public static void ImageProcessing([QueueTrigger("imaging")] string img)
        {
            var connectionString = CloudConfigurationManager.GetSetting("StorageAccount");
            var image = JsonConvert.DeserializeObject<ImageQueued>(img);
            var processor = new Processor(new DataStore(connectionString), versions.Images);
            processor.Process(image).Wait();
        }
        #endregion
    }
}