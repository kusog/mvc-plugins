using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using System.Web.Mvc;

using Kusog.Mvc;

using MvcPluginAppAdminPlugin.Models;

namespace MvcPluginAppAdminPlugin.Controllers
{
    public class PluginAdminController : Controller
    {
        public ActionResult Index()
        {
            List<AppPlugin> plugins = new List<AppPlugin>();
            Dictionary<string, AppAssembly> asses = new Dictionary<string, AppAssembly>();

            BaseMvcPluginApplication app = BaseMvcPluginApplication.Instance;
            foreach (Lazy<IMvcPlugin, IMvcPluginData> plugin in app.Plugins)
            {
                AppPlugin p;
                plugins.Add(p = new AppPlugin()
                {
                    Id = plugin.Metadata.Id,
                    Name = plugin.Metadata.Name,
                    ParentId = plugin.Metadata.ParentId,
                    Description = plugin.Metadata.Description,
                    Plugin = plugin.Value
                });

                if (!string.IsNullOrWhiteSpace(p.ParentId))
                {
                    AppPlugin parent = findPlugin(plugins, p.ParentId);
                    if (parent != null)
                    {
                        parent.Children.Add(p);
                    }
                }

                if (plugin.Value != null)
                {
                    AppAssembly apass = null;
                    Assembly a = plugin.Value.GetType().Assembly;
                    string key = a.CodeBase;
                    if (!asses.ContainsKey(key))
                    {
                        DateTime lastModified = DateTime.Now;
                        try
                        {
                            System.IO.FileInfo fileInfo = new System.IO.FileInfo(a.Location);
                            lastModified = fileInfo.LastWriteTime;
                        }
                        catch(Exception)
                        {
                        }

                        asses[key] = apass = new AppAssembly() { Name = a.ManifestModule.Name, CodeBase = a.CodeBase, Version = a.GetName().Version.ToString(), Date = lastModified.ToString() };
                    }
                    else
                        apass = asses[key];

                    p.AssemblyInfo = apass;
                }
            }

            return View(plugins);
        }

        protected virtual AppPlugin findPlugin(List<AppPlugin> plugins, string id)
        {
            foreach(AppPlugin plugin in plugins)
            {
                if (string.Compare(plugin.Id, id, true) == 0)
                    return plugin;
            }

            return null;
        }
    }
}