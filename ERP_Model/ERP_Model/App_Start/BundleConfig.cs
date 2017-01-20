using System.Web.Optimization;

namespace ERP_Model
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
                "~/Scripts/Angular_Core/angular.js",
                "~/Scripts/Angular_Core/angular-resource.js",
                "~/Scripts/ERPModelApp.js",
                "~/Scripts/Services/AngularProductsService.js",
                "~/Scripts/Services/AngularStocksService.js",
                "~/Scripts/Services/AngularAdminService.js",
                "~/Scripts/Services/AngularOrdersService.js",
                "~/Scripts/Services/AngularDeliveryNotesService.js",
                "~/Scripts/Controllers/AngularProductsController.js",
                "~/Scripts/Controllers/AngularLoginController.js",
                "~/Scripts/Controllers/AngularAdminController.js",
                "~/Scripts/Controllers/AngularOrdersController.js",
                "~/Scripts/Controllers/AngularStocksController.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/bootstrap.css",
                "~/Content/site.css"));
        }
    }
}