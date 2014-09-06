using System.Web.Optimization;

namespace King.Azure.Imaging.Mvc
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/style.css",
                      "~/Content/jquery.fileupload-ui.css",
                      "~/Content/jquery.fileupload-noscript.css",
                      "~/Content/jquery.fileupload-ui-noscript.css",
                      "~/Content/jquery.fileupload.css"));

            bundles.Add(new ScriptBundle("~/bundles/upload").Include(
                      "~/Scripts/jquery.fileupload-jquery-ui.js",
                      "~/Scripts/jquery.fileupload-process.js",
                      "~/Scripts/jquery.iframe-transport.js",
                      "~/Scripts/jquery.fileupload-validate.js",
                      "~/Scripts/jquery.fileupload-ui.js",
                      "~/Scripts/main.js",
                      "~/Scripts/app.js",
                      "~/Scripts/jquery.fileupload-image.js"));

            // Set EnableOptimizations to false for debugging. For more information,
            // visit http://go.microsoft.com/fwlink/?LinkId=301862
            BundleTable.EnableOptimizations = true;
        }
    }
}
