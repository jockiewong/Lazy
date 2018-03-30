using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Routing;
using Lazy.Kernel.Module;

namespace Lazy.AspNetCore.Pluggable
{
    /// <summary>
    /// 静态 服务端路径相关
    /// </summary>
    public static class Server
    {
        //TODO: 优化
        static IHostingEnvironment _hostingEnvironment;

        internal static void Init(IHostingEnvironment hostingEnvironment)
        {
            if (_hostingEnvironment != null)
                return;
            _hostingEnvironment = hostingEnvironment;
        }

        public static string MapPath(string path)
        {
            return MapPath(_hostingEnvironment.ContentRootPath, path);
        }

        public static string MapPathWebRoot(string path)
        {
            return MapPath(_hostingEnvironment.WebRootPath, path);
        }

        public static string MapPathPlugin(this HttpContext httpContext, string path)
        {
            var id = httpContext.GetRouteData()?.DataTokens[Const.PluginAreaKey]?.ToString();
            if (id == null)
                return MapPath(path);

            var option = httpContext.RequestServices.GetRequiredService<IModuleOptionProvider<PluggableOptions>>().GetConfiguredOptions();

            return MapPath(MapPath(Path.Combine(option.PluginSourceLocation, id)), path);
        }

        public static string MapPathPluginWebRoot(this HttpContext httpContext, string path)
        {
            var id = httpContext.GetRouteData()?.DataTokens[Const.PluginAreaKey]?.ToString();
            if (id == null)
                return MapPath(path);

            var option = httpContext.RequestServices.GetRequiredService<IModuleOptionProvider<PluggableOptions>>().GetConfiguredOptions();
            return MapPath(MapPath(Path.Combine(option.PluginSourceLocation, id, "wwwroot")), path);
        }

        private static string MapPath(string root, string path)
        {
            string replaced = path.Replace("~/", "").TrimStart('/').Replace('/', '\\');
            return Path.Combine(root, replaced);
        }
    }
}
