using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing;
using Lazy.AspNetCore.Pluggable.Plugin;
using System.Reflection;

namespace Lazy.AspNetCore.Pluggable.Mvc
{
    /// <summary>
    /// 插件状态中间件,插件被禁用时返回404状态
    /// </summary>
    public class PluginStatusMiddleware
    {
        private const string idKey = "area";

        IPluginManager _pluginManager;
        private readonly RequestDelegate _next;
        public PluginStatusMiddleware(RequestDelegate next, IPluginManager pluginManager)
        {
            _next = next;
            _pluginManager = pluginManager;
        }

        public async Task Invoke(HttpContext context)
        {
            var datas = context.GetRouteData().DataTokens;
            if (datas.ContainsKey(idKey))
            {
                var pluginId = datas[idKey]?.ToString();
                if (pluginId == null)
                    await _next.Invoke(context);
                else
                {
                    if (!_pluginManager.GetPlugin(pluginId).PluginModel.Enabled)
                    {
                        context.Response.Clear();
                        context.Response.StatusCode = 404;
                    }
                }
            }
            else
                await _next.Invoke(context);
        }
    }
}
