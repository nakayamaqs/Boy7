using System.Web;
using System.Web.Optimization;

namespace Boy8
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

            bundles.Add(new ScriptBundle("~/bundles/jqueryUploader").Include(
                "~/Scripts/jquery.fileupload.js",
                "~/Scripts/jquery.iframe-transport.js",
                //"~/Scripts/jquery-ui-1.9.2.js",
                "~/Scripts/jquery.ui.widget.js"));


            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //            "~/Scripts/modernizr-*"));


            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));
            //"~/Scripts/doc.min.js",
            //"~/Scripts/respond.js"

            //For default layout and ZLayout
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap-theme.css",
                      "~/Content/bootstrap.css",
                      "~/Content/carousel.css",
                      "~/Content/zocial.css",
                      "~/Content/jquery.fileupload-ui-noscript.css",
                      "~/Content/jquery.fileupload-ui.css",
                      "~/Content/jquery.fileupload.baby.css",
                      "~/Content/site.css"));

            //For PicLayout
            bundles.Add(new StyleBundle("~/Content/css4pic").Include(
                      "~/Content/bootstrap-theme.css",
                      "~/Content/bootstrap.css",
                      "~/Content/carousel.css",
                      "~/Content/zocial.css",
                      "~/Content/site.css",
                      "~/Content/pic4site.css"));

            //For VideoLayout
            bundles.Add(new StyleBundle("~/Content/css4video").Include(
                      "~/Content/bootstrap-theme.css",
                      "~/Content/bootstrap.css",
                      "~/Content/carousel.css",
                      "~/Content/zocial.css",
                      "~/Content/site.css",
                      "~/Content/video4site.css"));
        }
    }
}
