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
using System.Web;

namespace Kusog.Mvc
{
    public class SiteResource
    {
        public string ResourceUrl
        {
            get
            {
                if(UseRemote)
                    return RemoteUrl;

                var l = VirtualPathUtility.ToAbsolute(LocalUrl);

                return l;
            }
        }
        public bool IsAssemblyResource { get; set; }
        public bool IsInline { get; set; }
        public string LocalUrl { get; set; }
        public string RemoteUrl { get; set; }
        public bool UseRemote { get; set; }
        public string ResourceContent { get; set; }
    }
}
