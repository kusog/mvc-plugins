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
using System.IO;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Web.Hosting;

namespace Kusog.Mvc
{
    public class AssemblyResourceProvider : VirtualPathProvider
    {
        protected Dictionary<string, Assembly> m_pluginAssemblies = new Dictionary<string, Assembly>();
        protected List<string> m_pluginLocations = new List<string>();
        public static string FixResourceUrl(string virtualPath)
        {
            return virtualPath;
        }


        protected static Dictionary<string, bool> s_foundPaths = new Dictionary<string, bool>();
         
        protected string m_virtualBase = "~/Plugins/";


        public AssemblyResourceProvider()
        {
            m_pluginLocations.Add(HttpRuntime.BinDirectory);
            m_pluginLocations.Add(Path.Combine(HttpRuntime.AppDomainAppPath, "Pluginz"));
        }

        private bool IsAppResourcePath(string virtualPath)
        {
            String checkPath = VirtualPathUtility.ToAppRelative(virtualPath);
            return checkPath.StartsWith(m_virtualBase, StringComparison.InvariantCultureIgnoreCase);
        }

        public override VirtualFile GetFile(string virtualPath)
        {
            
            if (IsAppResourcePath(virtualPath))
                return new AssemblyResourceVirtualFile(FixResourceUrl(virtualPath));
            else
                return base.GetFile(virtualPath);
        }

        public override CacheDependency GetCacheDependency(string virtualPath, IEnumerable virtualPathDependencies, DateTime utcStart)
        {
            if (IsAppResourcePath(virtualPath))
            {
                return null;
            }
            return base.GetCacheDependency(virtualPath, virtualPathDependencies, utcStart);
        }

        protected string findAssembly(string name)
        {
            string fullName = null;

            foreach (string loc in m_pluginLocations)
                if (File.Exists(fullName = Path.Combine(loc, name)))
                    return fullName;

            return null;
        }

        public override bool FileExists(string virtualPath)
        {
            virtualPath = FixResourceUrl(virtualPath);
            bool exists = false;
            lock (s_foundPaths)
            {
                if (s_foundPaths.ContainsKey(virtualPath))
                    exists = true;
            }

            if (!exists)
            {
                if (IsAppResourcePath(virtualPath))
                {
                    string path = VirtualPathUtility.ToAppRelative(virtualPath);
                    string[] parts = path.Split('/');
                    if (parts.Length >= 4)
                    {
                        string assemblyName = parts[2];
                        string resourceName = parts[3];
                        if (parts.Length > 4)
                        {
                            StringBuilder buff = new StringBuilder(parts[3]);
                            for (var p = 4; p < parts.Length; p++)
                                buff.Append(".").Append(parts[p]);
                            resourceName = buff.ToString();
                        }

                        if (!assemblyName.EndsWith(".dll", StringComparison.InvariantCultureIgnoreCase))
                        {
                            resourceName = assemblyName + "." + resourceName;
                            assemblyName += ".dll";
                        }

                        Assembly assembly = null;
                        assemblyName = findAssembly(assemblyName);
                        lock (m_pluginAssemblies)
                        {
                            if (!m_pluginAssemblies.TryGetValue(assemblyName, out assembly))
                            {
                                byte[] assemblyBytes = File.ReadAllBytes(assemblyName);
                                assembly = Assembly.Load(assemblyBytes);
                                m_pluginAssemblies[assemblyName] = assembly;
                            }
                        }

                        if (assembly != null)
                        {
                            string[] resourceList = assembly.GetManifestResourceNames();
                            bool found = Array.Exists(resourceList, delegate(string r) { return r.Equals(resourceName); });
                            if (found == true)
                            {
                                lock (s_foundPaths)
                                {
                                    s_foundPaths[virtualPath] = found;
                                }
                            }

                            exists = found;
                        }
                    }
                }
                else
                    exists = base.FileExists(virtualPath);
            }

            return exists;
        }
    }

    public class AssemblyResourceVirtualFile : System.Web.Hosting.VirtualFile
    {
        private string path;

        public AssemblyResourceVirtualFile(string virtualPath)
            : base(virtualPath)
        {
            virtualPath = AssemblyResourceProvider.FixResourceUrl(virtualPath);
            path = VirtualPathUtility.ToAppRelative(virtualPath);
        }

        public override Stream Open()
        {
            string[] parts = path.Split('/');
            string assemblyName = parts[2];
            string resourceName = parts[3];
            if (parts.Length > 4)
            {
                StringBuilder buff = new StringBuilder(parts[3]);
                for (var p = 4; p < parts.Length; p++)
                    buff.Append(".").Append(parts[p]);
                resourceName = buff.ToString();
            }
            if (!assemblyName.EndsWith(".dll", StringComparison.InvariantCultureIgnoreCase))
            {
                resourceName = assemblyName + "." + resourceName;
                assemblyName += ".dll";
            }

            assemblyName = Path.Combine(HttpRuntime.BinDirectory, assemblyName);
            byte[] assemblyBytes = File.ReadAllBytes(assemblyName);
            Assembly assembly = Assembly.Load(assemblyBytes);

            if (assembly != null)
                return assembly.GetManifestResourceStream(resourceName);

            return null;
        }
    }
}
