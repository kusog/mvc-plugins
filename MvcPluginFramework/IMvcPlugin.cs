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
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;

namespace Kusog.Mvc
{
    public interface IMvcPlugin : IMvcPluginSys
    {
        List<SiteResource> Css { get; }
        List<SiteResource> JavaScript { get; }
        List<SiteResource> FooterJavaScript { get; }
        List<string> RazorViewLocations { get; }
        List<Widget> Widgets { get; }
        List<DynamicActionFilter> ActionFilters { get; }
        List<Route> Routes { get; }

        void SetupExtensions(IMvcPluginApplication app);

        bool Enabled { get; set; }
    }

    public interface IMvcPluginSys
    {
        void getFooterResources(StringBuilder footerOut);
        void getHeaderResources(StringBuilder headerOut);

        IMvcPluginApplication App { get; }
    }

    public interface IMvcPluginData
    {
        string Id { get; }
        string ParentId { get; }
        string Name { get; }
        string Description { get; }
    }

    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class MvcPluginMetadataAttribute : ExportAttribute
    {
        public MvcPluginMetadataAttribute(string id, string parentId, string name, string description)
            : base(typeof(IMvcPluginData))
        {
            Id = id;
            ParentId = parentId;
            Name = name;
            Description = description;
        }

        public string Id { get; set; }
        public string ParentId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }  
}
