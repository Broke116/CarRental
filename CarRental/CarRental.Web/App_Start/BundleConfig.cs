using System.Web.Optimization;

namespace CarRental.Web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/bower_components/jquery/dist/jquery.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/bower_components/jquery.validate/dist/jquery.validate.js"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/bower_components/modernizr/modernizr.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/bower_components/bootstrap/dist/js/bootstrap.js",
                      "~/bower_components/respond/src/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css/bootstrap").Include(
                "~/bower_components/bootstrap/dist/css/bootstrap.css", new CssRewriteUrlTransform()));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/site.css").Include("~/bower_components/font-awesome/css/font-awesome.min.css", new CssRewriteUrlTransform()));
        }
    }
}
