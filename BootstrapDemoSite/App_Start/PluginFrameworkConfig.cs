using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using Kusog.Mvc;

namespace BootstrapDemoSite
{
    public class PluginFrameworkConfig
    {
        public static void SetupAppFramework(BundleCollection bundles, RouteCollection routes)
        {
            PluginDemo.PluginDemoApplication.SetupApplication(bundles, routes);
            PluginDemo.PluginDemoApplication.Instance.DefineWidgetContainer(new WidgetContainer("rightSidebar",
                new WidgetContainer.WidgetDetails("SimpleWidget"), new WidgetContainer.WidgetDetails("SimpleWidget2")));
            //PluginDemo.PluginDemoApplication.Instance.GetPlugin("ksgSlideshow").Enabled = false;   
        }
    }
}