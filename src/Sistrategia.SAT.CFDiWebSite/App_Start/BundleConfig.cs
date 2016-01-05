using System.Web;
using System.Web.Optimization;

namespace Sistrategia.SAT.CFDiWebSite
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles) {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));
            //bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
            //          "~/Scripts/bootstrap.js",
            //          "~/Scripts/respond.js"));
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/select2").Include(
                      "~/Scripts/select2.js"
                      , "~/Scripts/select2/i18n/es.js"));

            bundles.Add(new ScriptBundle("~/bundles/KOAmplify").Include(
                      "~/Scripts/amplify/amplify.store.min.js"
                    , "~/Scripts/knockout-3.4.0.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css"
                      //,"~/Content/select2.css"
                      , "~/Content/Site.css"
                //, "~/Content/site.css"
                      ));

            bundles.Add(new StyleBundle("~/Content/welcome").Include(
                      "~/Content/bootstrap.css"
                      ,"~/Content/select2.css"
                , "~/Content/site.css"
                      //, "~/Content/Welcome.css"

                      ));

            bundles.Add(new StyleBundle("~/Content/cover").Include(
                "~/Content/bootstrap.css",
               "~/Content/cover.css"
               ));   

            BundleTable.EnableOptimizations = true;
        }
    }
}