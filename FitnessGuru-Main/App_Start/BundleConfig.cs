using System.Web;
using System.Web.Optimization;

namespace FitnessGuru_Main
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));



            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/datatable").Include(
                "~/Scripts/DataTables/jquery.datatables.js",
                "~/Scripts/DataTables/datatables.bootstrap.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/fullcalendar").Include(
                "~/Scripts/moment.js",
                "~/Scripts/fullcalendar/fullcalendar.js",
                //"~/Scripts/sessioncreate.js",
                //"~/Scripts/sessionEdit.js",
                "~/Scripts/qTip/jquery.qtip.js",
                "~/Scripts/toastr.js"
            ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/bootstrap-superhero.css",
                      "~/Content/datatables/css/datatables.bootstrap.css",
                        "~/Content/fullcalendar.css",
                        "~/Scripts/qTip/jquery.qtip.css",
                        "~/Content/font-awesome.css",
                        "~/Content/toastr.css",
                      "~/Content/site.css"));
        }
    }
}
