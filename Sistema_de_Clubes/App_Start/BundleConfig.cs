using System.Web;
using System.Web.Optimization;

namespace CABW
{
    public class BundleConfig
    {
        // Para obtener más información acerca de Bundling, consulte http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/site/css").Include(
                "~/Content/*.css"));

            bundles.Add(new ScriptBundle("~/site/libs").Include(
                "~/libs/angular.js",
                "~/libs/angular-route.js",
                "~/libs/angular-locale_es-ar.js",
                "~/libs/angular-messages.js",
                "~/libs/angular-animate.min.js"));

            bundles.Add(new ScriptBundle("~/site/js").Include(
                "~/js/ui-bootstrap-tpls-1.3.1.min.js",
                "~/js/jquery.min.js",
                "~/js/bootstrap.min.js",
                "~/js/loading-bar.js"));

            bundles.Add(new ScriptBundle("~/site/controllers").IncludeDirectory(
                "~/app/components/site", "*.js", true));

            bundles.Add(new ScriptBundle("~/site/directives").IncludeDirectory(
                "~/app/shared/directives/site", "*.js", true));

            bundles.Add(new ScriptBundle("~/site/services")
                .IncludeDirectory("~/app/shared/services/site", "*.js", true));


            /* admin */

            bundles.Add(new StyleBundle("~/admin/css").Include(
                "~/Content/admin/*.css"));

            bundles.Add(new ScriptBundle("~/admin/libs").Include(
                "~/libs/angular.js",
                "~/libs/angular-route.js",
                "~/libs/angular-locale_es-ar.js",
                "~/libs/angular-messages.js",
                "~/libs/angular-cookies.min.js",
                "~/libs/angular-animate.min.js"));

            bundles.Add(new ScriptBundle("~/admin/js").Include(
                "~/js/ui-bootstrap-tpls-1.3.1.min.js",
                "~/js/jquery.min.js",
                "~/js/bootstrap.min.js",
                "~/js/sb-admin-2.js",
                "~/js/metisMenu.js",
                "~/js/loading-bar.js"));

            bundles.Add(new ScriptBundle("~/admin/controllers").IncludeDirectory(
                "~/app/components/admin", "*.js", true));

            bundles.Add(new ScriptBundle("~/admin/directives").IncludeDirectory(
                "~/app/shared/directives/admin", "*.js", true));

            bundles.Add(new ScriptBundle("~/admin/filters").IncludeDirectory(
                "~/app/shared/filters/admin", "*.js", true));

            bundles.Add(new ScriptBundle("~/admin/services")
                .IncludeDirectory("~/app/shared/services/admin", "*.js", true));

            bundles.Add(new ScriptBundle("~/admin/factories")
                .IncludeDirectory("~/app/shared/factories/admin", "*.js", true));

            //BundleTable.EnableOptimizations = true;
        }
    }
}