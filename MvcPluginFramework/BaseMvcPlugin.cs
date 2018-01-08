//Copyright 2012-2013 Kusog Software, inc. (http://kusog.org)
//This file is part of the ASP.NET Mvc Plugin Framework.
// == BEGIN LICENSE ==
//
// Licensed under the terms of any of the following licenses at your
// choice:
//
//  - GNU General Public License Version 3 or later (the "GPL")
//    http://www.gnu.org/licenses/gpl.html
//
//  - GNU Lesser General Public License Version 3 or later (the "LGPL")
//    http://www.gnu.org/licenses/lgpl.html
//
//  - Mozilla Public License Version 1.1 or later (the "MPL")
//    http://www.mozilla.org/MPL/MPL-1.1.html
//
// == END LICENSE ==
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Optimization;
using System.Web.WebPages;

using System.Web.Mvc;
using System.Web.Routing;

namespace Kusog.Mvc
{
    /// <summary>
    /// Base class for all MvcPlugins which provides support for child classes to register various things.
    /// </summary>
    /// <remarks>It is typical for a real plugin based application to have its own base class, which inherits from this class, that all plugins for that plugin application would inherit from.
    /// Generic plugins may still inherit diretly from this class in order to function in different types of plugin applications.
    /// </remarks>
    public class BaseMvcPlugin : IMvcPlugin
    {
        public static readonly string StandardViewLocation = "/Views.{1}.{0}.cshtml";
        protected IMvcPluginApplication m_app = null;
        protected bool m_enabled = true;

        public BaseMvcPlugin(bool ensureStandardViewLocation = true)
        {
            Css = new List<SiteResource>();
            JavaScript = new List<SiteResource>();
            RazorViewLocations = new List<string>();
            FooterJavaScript = new List<SiteResource>();
            Widgets = new List<Widget>();
            ActionFilters = new List<DynamicActionFilter>();
            Routes = new List<Route>();

            if (ensureStandardViewLocation)
                ensureViewLocation(BaseMvcPlugin.StandardViewLocation);
        }

        public virtual bool Enabled { get { return m_enabled; } set { m_enabled = value; } }

        public List<SiteResource> Css { get; private set; }
        public List<SiteResource> JavaScript { get; private set; }
        public List<SiteResource> FooterJavaScript { get; private set; }

        public List<string> RazorViewLocations { get; private set; }

        public List<Widget> Widgets { get; private set; }

        public List<DynamicActionFilter> ActionFilters { get; private set; }
        public List<Route> Routes { get; private set; }

        public virtual void SetupExtensions(IMvcPluginApplication app)
        {
            m_app = app;
        }

        /// <summary>
        /// Called by child classes that want to register a css file name.
        /// </summary>
        /// <param name="cssFileName">A relative file name to the css contained in the plugin assembly such as /Content/styles.css</param>
        protected void RegisterLocalCss(string cssFileName)
        {
            Css.Add(new SiteResource() { IsAssemblyResource = true, LocalUrl = calcBaseLocationName() + cssFileName });
        }

        /// <summary>
        /// Called by child classes that want to register a footer JavaScript file to be placed at the bottom of the page, after all the main content in body.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="url"></param>
        /// <param name="depends"></param>
        protected void RegisterFooterScript(string name, string url, IEnumerable<string> depends)
        {
            FooterJavaScript.Add(new SiteResource() { IsAssemblyResource = true, LocalUrl = calcBaseLocationName() + url });
        }

        protected void RegisterHeaderScript(string name, string url, IEnumerable<string> depends = null)
        {
            JavaScript.Add(new SiteResource() { IsAssemblyResource = true, LocalUrl = calcBaseLocationName() + url });
        }

        protected void RegisterHeaderScriptBlock(string name, string script, IEnumerable<string> depends = null)
        {
            JavaScript.Add(new SiteResource() { IsInline = true, ResourceContent = script });
        }

        protected void RegisterFooterScriptBlock(string name, string script, IEnumerable<string> depends = null)
        {
            FooterJavaScript.Add(new SiteResource() { IsInline = true, ResourceContent = script });
        }

        protected void RegisterWidget(Widget widget)
        {
            Widgets.Add(widget);
        }

        protected void RegisterActionFilter(string action, string controller, IActionFilter filter)
        {
            ActionFilters.Add(new DynamicActionFilter() { Action = action, Controller = controller, Filter = filter });
        }

        public void RegisterRoute(string name, string url)
        {
            RegisterRoute(name, url, null /* defaults */, (object)null /* constraints */);
        }

        public void RegisterRoute(string name, string url, object defaults)
        {
            RegisterRoute(name, url, defaults, (object)null /* constraints */);
        }

        public void RegisterRoute(string name, string url, object defaults, object constraints)
        {
            RegisterRoute(name, url, defaults, constraints, null /* namespaces */);
        }

        public void RegisterRoute(string name, string url, string[] namespaces)
        {
            RegisterRoute(name, url, null /* defaults */, null /* constraints */, namespaces);
        }

        public void RegisterRoute(string name, string url, object defaults, string[] namespaces)
        {
            RegisterRoute(name, url, defaults, null /* constraints */, namespaces);
        }

        public void RegisterRoute(string name, string url, object defaults, object constraints, string[] namespaces)
        {
            Route r = ((BaseMvcPluginApplication)App).RegisterRoute(this, name, url, defaults, constraints, namespaces);
            Routes.Add(r);
        }



        /// <summary>
        /// A plugin should register all the base view locations.
        /// </summary>
        /// <param name="localName"></param>
        protected void ensureViewLocation(string localName)
        {
            string ln = "/" + localName.Substring(1).Replace('/', '.');
            string name = calcBaseLocationName() + ln;
            if(!RazorViewLocations.Contains(name))
                RazorViewLocations.Add(name);
        }

        protected string calcBaseLocationName()
        {
            string assemblyName = this.GetType().Assembly.FullName;
            int i = assemblyName.IndexOf(",");
            if (i >= 0)
                assemblyName = assemblyName.Substring(0, i);
            string newName = "~/Plugins/" + assemblyName;
            return newName;
        }

        public IMvcPluginApplication App {get{return m_app;}}


        protected static string s_ScriptLink = "<script src='{0}' ></script>";
        protected static string s_ScriptBlockLink = "<script type='text/javascript'>{0}</script>";
        protected static string s_CssLink = "<link href='{0}' media='screen' rel='stylesheet' type='text/css' />";

        public void getFooterResources(StringBuilder footerOut)
        {
            addScriptResources(footerOut, FooterJavaScript);
        }

        public void getHeaderResources(StringBuilder headerOut)
        {
            foreach (SiteResource rsc in Css)
                headerOut.Append(string.Format(s_CssLink, rsc.IsAssemblyResource ? (rsc.ResourceUrl + "?" + CalcCacheBuster()) : rsc.ResourceUrl));
            addScriptResources(headerOut, JavaScript);
        }

        protected void addScriptResources(StringBuilder htmlOut, List<SiteResource> scripts)
        {
            //TODO: Very close to making bundles work from plugins.  For now its disabled, but should consider this a top priority to stay
            //in pariety with VS2012 MVC4 Project Templates.

            //App.Bundles.Add(new ScriptBundle("~/bundles/syrinxslideshow").Include(
            //            "~/Plugins/SyrinxSlideshowPlugin/scripts.jquery.syrinx-slideshow-.08.js",
            //            "~/Plugins/SyrinxSlideshowPlugin/scripts.jquery.syrinx-slideshow-controllers-.02.js",
            //            "~/Plugins/SyrinxSlideshowPlugin/scripts.jquery.syrinx-slideshow-editor-.05.js",
            //            "~/Plugins/SyrinxSlideshowPlugin/scripts.jquery.syrinx-slideshow-mvc.01.js"
            //));

            foreach (SiteResource rsc in scripts)
                if (rsc.IsInline)
                    htmlOut.AppendFormat(s_ScriptBlockLink, rsc.ResourceContent);
                else
                    htmlOut.AppendFormat(s_ScriptLink, rsc.IsAssemblyResource ? (rsc.ResourceUrl + "?" + CalcCacheBuster()) : rsc.ResourceUrl);
        }

        static Dictionary<string, string> s_assemblies = new Dictionary<string, string>();
        protected string CalcCacheBuster()
        {
            string loc = this.GetType().Assembly.Location;
            lock (s_assemblies)
            {
                if (s_assemblies.ContainsKey(loc))
                    return s_assemblies[loc];

                System.IO.FileInfo fileInfo = new System.IO.FileInfo(loc);
                DateTime lastModified = fileInfo.LastWriteTime;
                string val = null;
                s_assemblies[loc] = val = string.Format("v={0}", lastModified.Ticks);
                return val;
            }
            
        }

        public virtual bool ShouldIncludeResource(BaseMvcPluginApplication.ResourceTypes type, object content)
        {
            return true;
        }
    }
}
