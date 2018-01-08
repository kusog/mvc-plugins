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

namespace Kusog.Mvc
{
    public class WidgetContainer
    {
        public class ContainerWidget
        {
            public int Position { get; set; }
            public string WidgetId { get; set; }
            public object WidgetOptions { get; set; }
        }
        public class WidgetDetails
        {
            public WidgetDetails(string widgetId, object widgetOptions = null)
            {
                WidgetId = widgetId;
                WidgetOptions = widgetOptions;
            }
            public string WidgetId { get; set; }
            public object WidgetOptions { get; set; }
        }

        public WidgetContainer(string id, params WidgetDetails[] widgets)
        {
            Id = id;
            Widgets = new List<ContainerWidget>();

            int pos = 0;
            foreach (WidgetDetails widge in widgets)
                Widgets.Add(new ContainerWidget() { Position = pos++, WidgetId = widge.WidgetId, WidgetOptions = widge.WidgetOptions });
        }

        public string Id { get; private set; }
        public List<ContainerWidget> Widgets { get; private set; }

        public void AddWidget(string widgetId, int position = -1)
        {
            Widgets.Add(new ContainerWidget() { Position = position, WidgetId = widgetId });
        }
    }
}
