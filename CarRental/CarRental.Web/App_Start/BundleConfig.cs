using System.Web.Optimization;

namespace CarRental.Web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/bower_components/jquery/dist/jquery.js",
                        "~/bower_components/jquery-ui/jquery-ui.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            /*bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/bower_components/jquery.validate/dist/jquery.validate.js"));*/

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/bower_components/modernizr/modernizr.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/bower_components/tether/dist/js/tether.min.js",
                      "~/bower_components/bootstrap/dist/js/bootstrap.js",
                      "~/bower_components/respond/src/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/vendors").Include(
                        "~/bower_components/raty/lib/jquery.raty.js"));

            bundles.Add(new ScriptBundle("~/bundles/datatables").Include(
                        "~/bower_components/datatables.net/js/jquery.dataTables.min.js",
                        "~/bower_components/datatables.net-bs/js/dataTables.bootstrap.js"));

            // css files

            bundles.Add(new StyleBundle("~/Content/datatables").Include(
                      "~/bower_components/datatables.net-bs/css/dataTables.bootstrap.css"));

            bundles.Add(new StyleBundle("~/Content/css/bootstrap").Include(
                "~/Content/css/bootstrap.css", new CssRewriteUrlTransform()));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/css/Site.css")
                      .Include("~/bower_components/font-awesome/css/font-awesome.min.css", new CssRewriteUrlTransform())
                      .Include("~/bower_components/jquery-ui/themes/base/jquery-ui.css"));
        }
    }
}