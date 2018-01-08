using System;
using System.Web;
using System.Web.Optimization;
using System.Web.Mvc;

namespace BootstrapDemoSite
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-2.0.3*"));
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                        "~/Scripts/bootstrap*"));
            bundles.Add(new StyleBundle("~/Content/styles").Include(
                "~/Content/bootstrap.*",
                "~/Content/bootstrap-theme.*",
                "~/Content/site.css"
                ));
        }
    }
}