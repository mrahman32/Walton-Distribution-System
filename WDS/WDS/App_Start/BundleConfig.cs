using System.Web;
using System.Web.Optimization;

namespace WDS
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            //            "~/Scripts/jquery-{version}.js"));//jquery-{version}.js

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      //"~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js",
                      
                      "~/assets/vendor/bootstrap/js/bootstrap.bundle.js",

                      "~/Scripts/jquery.dataTables.min.js",
                      //"~/Scripts/dataTables.bootstrap4.min.js",

                      //"~/Scripts/dataTables.buttons.min",
                      //"~/Scripts/jszip.min.js",
                      //"~/Scripts/pdfmake.min.js",
                      //"~/Scripts/vfs_fonts.js",
                      //"~/Scripts/buttons.html5.min.js",


                      "~/assets/vendor/bootstrap-select/js/bootstrap-select.js",
                      "~/assets/vendor/slimscroll/jquery.slimscroll.js",
                      "~/assets/libs/js/main-js.js",

                      "~/Scripts/toastr.min.js",

                      //"~/assets/vendor/charts/chartist-bundle/chartist.min.js",
                      //"~/assets/vendor/charts/sparkline/jquery.sparkline.js",
                      //"~/assets/vendor/charts/morris-bundle/raphael.min.js",
                      //"~/assets/vendor/charts/morris-bundle/morris.js",
                      //"~/assets/vendor/charts/c3charts/c3.min.js",
                      //"~/assets/vendor/charts/c3charts/d3-5.4.0.min.js",
                      //"~/assets/vendor/charts/c3charts/C3chartjs.js",
                      //"~/assets/libs/js/dashboard-ecommerce.js",
                      "~/Scripts/jquery-ui.js",
                      
                      "~/Scripts/iziToast.min.js"
                      
                      
                      ));

            bundles.Add(new ScriptBundle("~/bundles/amchart").Include(
                "~/amcharts4/core.js",
                "~/amcharts4/charts.js",
                "~/amcharts4/themes/material.js",
                "~/amcharts4/themes/animated.js",
                "~/amcharts4/lang/en_Us.js"
                ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      //"~/Content/bootstrap.css",
                      "~/Content/site.css",


                      "~/Content/bootstrap_4.css",
                      "~/Content/tataTables.bootstrap4.min.css",
                      "~/assets/vendor/bootstrap-select/css/bootstrap-select.css",
                      "~/assets/vendor/fonts/circular-std/style.css",
                      "~/assets/libs/css/style.css",
                      "~/assets/vendor/fonts/fontawesome/css/fontawesome-all.css",
                      "~/assets/vendor/charts/chartist-bundle/chartist.css",
                      "~/assets/vendor/charts/morris-bundle/morris.css",
                      "~/assets/vendor/fonts/material-design-iconic-font/css/materialdesignicons.min.css",
                      "~/assets/vendor/charts/c3charts/c3.css",
                      "~/assets/vendor/fonts/flag-icon-css/flag-icon.min.css",
                      "~/Content/jquery-ui.css",
                      "~/Content/jquery-ui.structure.css",
                      "~/Content/jquery-ui.theme.css",
                      "~/Content/toastr.min.css",
                      "~/Content/iziToast.min.css",
                      "~/Content/jquery.dataTables.min.css"
                      //"~/Content/buttons.dataTables.min.css"
                      
                      ));
        }
    }
}
