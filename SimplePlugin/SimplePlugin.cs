using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Mvc.Html;

using Kusog.Mvc;
using PluginDemo;

namespace SimplePlugin
{
    /// <summary>
    /// Provides basic plugin functionality to show how to set it up.
    /// </summary>
    /// <remarks>This plugin inherits from DemoAppPlugin rather than BaseMvcPlugin because it wants to use extended functionality made available in DemoAppPlugin, such as adding menu items.
    /// If a plugin doesn't need to do things other than what is provided in BaseMvcPlugin, it should inherit from that to be usable in other implementations of the plugin framework.</remarks>
    [Export(typeof(Kusog.Mvc.IMvcPlugin))]
    [MvcPluginMetadata("ksgSimple", null, "Simple Demo Plugin", "This is a simple demo plugin that can be used as a starting point for your new plugins.")]
    public class SimplePlugin : DemoAppPlugin
    {
        public override void SetupExtensions(IMvcPluginApplication app)
        {
            base.SetupExtensions(app);

            RegisterLocalCss("/Content/simple.css");
            RegisterFooterScript("ksgSimple", "/Scripts/ksgSimple.js", null);
            DefineMenuItem("main", "view", new Syrinx2.MenuItem() {Text="Simple View", NavigateUrl="/Simple" });
            RegisterWidget(new Widget("SimpleWidget", "Simple Widget", "Simple", "Widget"));
            RegisterWidget(new Widget("SimpleWidget2", "Second Simple Widget", "Simple", "SecondWidget"));
            RegisterActionFilter("Metatags", "PluginDemo", new ActionExtension("Metatags", "Simple"));
            RegisterActionFilter("WidgetContainer", "PluginDemo", new WrapAction("<div class='my-special-container'>", "</div>"));
            RegisterActionFilter("PageFooter", "PluginDemo", new WrapAction("<div>Prowdly powered by Plugin Demo.</div>"));
        }
    }
}
