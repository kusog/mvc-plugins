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
    public class CompoundActionResult : ActionResult
    {
        List<ActionResult> m_results = new List<ActionResult>();

        public void AddResult(ActionResult result)
        {
            m_results.Add(result);
        }
        public void AddResultToBegining(ActionResult result)
        {
            m_results.Insert(0, result);
        }
        public void WrapResults(ActionResult beginResult, ActionResult endResult)
        {
            AddResultToBegining(beginResult);
            AddResult(endResult);
        }
        public void WrapResults(string beginResult, string endResult)
        {
            AddResultToBegining(new ContentResult() { Content = beginResult });
            AddResult(new ContentResult() { Content = endResult });
        }

        public override void ExecuteResult(ControllerContext context)
        {
            foreach (ActionResult result in m_results)
                if(result != null)
                    result.ExecuteResult(context);
        }
    }
}
