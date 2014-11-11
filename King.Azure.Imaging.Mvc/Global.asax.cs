namespace King.Azure.Imaging.Mvc
{
    using King.Azure.Imaging.Models;
    using King.Azure.Imaging.Tasks;
    using King.Service;
    using System.Configuration;
    using System.Diagnostics;
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;

    /// <summary>
    /// MVC Application
    /// </summary>
    public class MvcApplication : System.Web.HttpApplication
    {
        #region Members
        /// <summary>
        /// Role Task Manager
        /// </summary>
        /// <remarks>
        /// Can be moved to a Worker Role (Azure)
        /// </remarks>
        private readonly IRoleTaskManager<ITaskConfiguration> manager = new RoleTaskManager<ITaskConfiguration>(new ImageTaskFactory());
        #endregion

        #region Methods
        /// <summary>
        /// Load site
        /// </summary>
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //Load storage emulator for Blob/Queue/Table storage
            var emulator = ConfigurationManager.AppSettings["AzureEmulator"];

            //This is only for testing, and can be removed when using Azure Storage
            using (var process = new Process())
            {
                process.StartInfo = CreateProcessStartInfo(emulator, "start");
                process.Start();

                process.WaitForExit();
            }

            var config = new TaskConfiguration
            {
                ConnectionString = "UseDevelopmentStorage=true;",
                StorageElements = new StorageElements(), // Modify for different storage names
                Versions = new Versions(), // Modify for custom sizing/formats
            };

            this.manager.OnStart(config);
            this.manager.Run();
        }

        /// <summary>
        /// Tear down worker process
        /// </summary>
        protected void Application_End()
        {
            this.manager.OnStop();
        }

        /// <summary>
        /// Start process
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        private static ProcessStartInfo CreateProcessStartInfo(string fileName, string arguments)
        {
            return new ProcessStartInfo(fileName, arguments)
            {
                UseShellExecute = false,
                ErrorDialog = false,
                CreateNoWindow = true,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
            };
        }

        /// <summary>
        /// Dispose Manager
        /// </summary>
        public override void Dispose()
        {
            if (null != this.manager)
            {
                this.manager.Dispose();
            }

            base.Dispose();
        }
        #endregion
    }
}