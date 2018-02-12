using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Lazy.AspNetCore.Pluggable
{
    public static class Server
    {
        static IHostingEnvironment _hostingEnvironment;

        internal static void Init(IHostingEnvironment hostingEnvironment)
        {
            if (_hostingEnvironment != null)
                return;
            _hostingEnvironment = hostingEnvironment;
        }

        public static string MapPath(string path)
        {
            MapPath(_hostingEnvironment.ContentRootPath, path);
            return path;
        }

        public static string MapPathWebRoot(string path)
        {
            MapPath(_hostingEnvironment.WebRootPath, path);
            return path;
        }

        //public static string MapPathPlugin(string path)
        //{
        //    return path;
        //}

        //public static string MapPathPluginWebRoot(string path)
        //{
        //    return path;
        //}

        private static string MapPath(string root, string path)
        {
            string replaced = path.Replace("~/", "").TrimStart('/').Replace('/', '\\');
            return Path.Combine(root, replaced);
        }
    }
}
