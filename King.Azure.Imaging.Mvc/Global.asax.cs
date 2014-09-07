namespace King.Azure.Imaging.Mvc
{
    using King.Service;
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
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            this.manager.OnStart();
            this.manager.Run();
        }

        protected void Application_End()
        {
            this.manager.OnStop();
        }
        #endregion
    }
}