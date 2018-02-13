using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Lazy.Utilities.Extensions;
using Lazy.AspNetCore.Pluggable.Plugin;
using Microsoft.AspNetCore.Http;

namespace Lazy.AspNetCore.Pluggable.Mvc
{
    /// <summary>
    /// 插件路由
    /// </summary>
    public class PluginRouter : RouteBase
    {
        private readonly IRouter _target;
        private IPluginManager _pluginManager;
        public PluginRouter(
            IPluginManager pluginManager,
            IRouter target,
            string routeName,
            string routeTemplate,
            RouteValueDictionary defaults,
            IDictionary<string, object> constraints,
            RouteValueDictionary dataTokens,
            IInlineConstraintResolver inlineConstraintResolver)
            : base(
                  routeTemplate,
                  routeName,
                  inlineConstraintResolver,
                  defaults,
                  constraints,
                  dataTokens)
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            _target = target;
            _pluginManager = pluginManager;
        }

        protected override Task OnRouteMatched(RouteContext context)
        {
            var id = context.RouteData?.DataTokens[Const.PluginAreaKey]?.ToString();

            //根据区域,即插件id,判断插件是否启用,禁用的插件直接返回404
            if (!id.IsNullOrWhiteSpace())
            {
                var plugin = _pluginManager.GetPlugin(id);
                if (plugin != null && !plugin.PluginModel.Enabled)
                {
                    context.Handler = async (http) =>
                    {
                        http.Response.Clear();
                        http.Response.StatusCode = 404;
                        await Task.CompletedTask;
                    };

                    return Task.CompletedTask;
                }
            }


            context.RouteData.Routers.Add(_target);
            return _target.RouteAsync(context);
        }

        protected override VirtualPathData OnVirtualPathGenerated(VirtualPathContext context)
        {
            return _target.GetVirtualPath(context);
        }
    }

}
