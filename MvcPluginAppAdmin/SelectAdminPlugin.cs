using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Kusog.Mvc;

namespace MvcPluginAppAdminPlugin
{
    [Export(typeof(Kusog.Mvc.IMvcPlugin))]
    [MvcPluginMetadata("DemoAppAdmin", null, "Demo App Site Admin", "")]
    class SelectAdminPlugin : BaseMvcPlugin
    {
        public override void SetupExtensions(IMvcPluginApplication app)
        {
            base.SetupExtensions(app);
        }
    }
}
