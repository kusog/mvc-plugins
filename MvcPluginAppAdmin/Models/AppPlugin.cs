using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Kusog.Mvc;

namespace MvcPluginAppAdminPlugin.Models
{
    public class AppPlugin
    {
        public AppPlugin()
        {
            Children = new List<AppPlugin>();
        }

        public string Id { get; set; }
        public string ParentId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public List<AppPlugin> Children { get; set; }

        public AppAssembly AssemblyInfo {get;set;}

        public IMvcPlugin Plugin { get; set; }
    }
}
