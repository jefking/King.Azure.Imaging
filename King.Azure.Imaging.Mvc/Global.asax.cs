namespace King.Azure.Imaging.Mvc
{
    using King.Service;
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;

    public class MvcApplication : System.Web.HttpApplication
    {
        #region Members
        private readonly RoleTaskManager manager = new RoleTaskManager(new ImageTaskFactory("UseDevelopmentStorage=true"));
        #endregion

        #region Methods
        public void Application_Init()
        {
            this.manager.OnStart();
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            this.manager.Run();
        }

        protected void Application_End()
        {
            this.manager.OnStop();
        }
        #endregion
    }
}