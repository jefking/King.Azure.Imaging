namespace King.Azure.Imaging.Mvc
{
    using King.Azure.Imaging.Models;
    using King.Azure.Imaging.Tasks;
    using King.Service;
    using King.Service.Data;
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
        /// Can be moved to a Worker Role (Azure) or WebJob
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

            //Load your configuration
            var config = new TaskConfiguration
            {
                ConnectionString = "UseDevelopmentStorage=true;",
                StorageElements = new StorageElements(), // Modify for different storage names
                Versions = new Versions(), // Modify for custom sizing/formats
                Priority = QueuePriority.Medium, // Modify to change cost/throughput ratio
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