namespace King.Azure.Imaging.Mvc.Controllers.api
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Configuration;

    public class DataController : ImageDataApiController
    {
        /// <summary>
        /// Connection String
        /// </summary>
        private static readonly string connection = ConfigurationManager.AppSettings["StorageAccount"];

        public DataController()
            : base(connection)
        {
        }
    }
}