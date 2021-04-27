using System.Web.Optimization;

namespace FinalProject.UI.MVC
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Content/scripts").Include(
                        "~/Content/js/jquery.min.js",
                        "~/Content/js/jquery.easing.1.3.js",
                        "~/Content/js/bootstrap.min.js",
                        "~/Content/js/jquery.waypoints.min.js",
                        "~/Content/js/owl.carousel.min.js",
                        "~/Content/js/jquery.magnific-popup.min.js",
                        "~/Content/js/magnific-popup-options.js",
                        "~/Content/js/main.js"));

            

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Content/js/modernizr-*"));

            
            bundles.Add(new StyleBundle("~/Content/styles").Include(
                      "~/Content/css/animate.css",
                      "~/Content/css/icomoon.css",
                      "~/Content/css/themify-icons.css",
                      "~/Content/css/bootstrap.css",
                      "~/Content/css/magnific-popup.css",
                      "~/Content/css/owl.carousel.min.css",
                      "~/Content/css/owl.theme.default.min.css",
                      "~/Content/css/style.css"
                      ));
        }
    }
}
