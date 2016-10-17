namespace King.Azure.Imaging
{
    using King.Azure.Imaging.Models;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Image Meta Data Controller
    /// </summary>
    public class ImageDataApi : Controller
    {
        #region Members
        /// <summary>
        /// Data Store (Image Meta-Data)
        /// </summary>
        protected readonly IQueryDataStore dataStore = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="connectionString">Table Storage Connection String</param>
        public ImageDataApi(string connectionString)
            : this(new QueryDataStore(connectionString))
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="connectionString">Table Storage Connection String</param>
        /// <param name="elements">Storage Elements</param>
        public ImageDataApi(string connectionString, IStorageElements elements)
            : this(new QueryDataStore(connectionString, elements))
        {
        }

        /// <summary>
        /// Mockable Constructor
        /// </summary>
        /// <param name="table">Data Store</param>
        public ImageDataApi(IQueryDataStore dataStore)
        {
            if (null == dataStore)
            {
                throw new ArgumentNullException("dataStore");
            }

            this.dataStore = dataStore;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Get image data
        /// </summary>
        /// <returns>Image Data</returns>
        public virtual async Task<HttpResponseMessage> Get(Guid? id = null, string version = null, string file = null)
        {
            var images = await this.dataStore.Query(id, version, file);

            return null == images || !images.Any() ? new HttpResponseMessage(HttpStatusCode.NoContent)
                : new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StringContent(JsonConvert.SerializeObject(images), Encoding.UTF8, "application/json"),
                    };
        }
        #endregion
    }
}