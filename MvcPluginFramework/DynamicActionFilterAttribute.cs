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

using System.Web.Mvc;

namespace Kusog.Mvc
{
    public class DynamicActionFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            foreach (Lazy<IMvcPlugin, IMvcPluginData> plugin in BaseMvcPluginApplication.Instance.Plugins)
                foreach (DynamicActionFilter filter in plugin.Value.ActionFilters)
                    if(string.Compare(filter.Action, filterContext.ActionDescriptor.ActionName, true) == 0 &&
                        string.Compare(filter.Controller, filterContext.ActionDescriptor.ControllerDescriptor.ControllerName, true) == 0)
                    filter.Filter.OnActionExecuting(filterContext);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
            foreach (Lazy<IMvcPlugin, IMvcPluginData> plugin in BaseMvcPluginApplication.Instance.Plugins)
                foreach (DynamicActionFilter filter in plugin.Value.ActionFilters)
                    if (string.Compare(filter.Action, filterContext.ActionDescriptor.ActionName, true) == 0 &&
                        string.Compare(filter.Controller, filterContext.ActionDescriptor.ControllerDescriptor.ControllerName, true) == 0)
                        filter.Filter.OnActionExecuted(filterContext);
        }
    }
}
