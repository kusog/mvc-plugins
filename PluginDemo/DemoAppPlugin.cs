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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Kusog.Mvc;
using Syrinx2;

namespace PluginDemo
{
    public class DemoAppPlugin : BaseMvcPlugin
    {
        public List<IBasicMenuItem> MenuItems { get; set; }
        public PluginDemoApplication DemoApp { get { return (PluginDemoApplication)App; } }

        public DemoAppPlugin(bool ensureStandardViewLocation = true)
            :base(ensureStandardViewLocation)
        {
            MenuItems = new List<IBasicMenuItem>();
        }

        public void DefineMenuItem(string menuName, string menuLocation, IBasicMenuItem item, params string[] views)
        {
            MenuItems.Add(item);
            DemoApp.RegisterMenuItem(menuName, menuLocation, item, this, views);
        }

        public virtual bool ShouldIncludeResource(PluginDemoApplication.DemoAppResourceTypes type, object content)
        {
            return true;
        }
    }
}
