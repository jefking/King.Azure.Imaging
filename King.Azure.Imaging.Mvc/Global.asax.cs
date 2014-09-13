namespace King.Azure.Imaging.Mvc
{
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
        private readonly RoleTaskManager manager = new RoleTaskManager(new ImageTaskFactory("UseDevelopmentStorage=true"));
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

            using (var process = new Process())
            {
                process.StartInfo = CreateProcessStartInfo(emulator, "start");
                process.Start();

                process.WaitForExit();
            }

            this.manager.OnStart();
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
        #endregion
    }
}