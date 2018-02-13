using Lazy.AspNetCore.Pluggable.Plugin;
using Lazy.Kernel.Module;
using Lazy.Utilities.Extensions;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lazy.AspNetCore.Pluggable.Mvc
{
    public class DefaultPluginRouterRegister : IPluginRouterRegister
    {
        IPluginManager _pluginManager;
        IModuleManager _moduleManager;
        IInlineConstraintResolver _inlineConstraintResolver;

        public DefaultPluginRouterRegister(
            IPluginManager pluginManager,
            IModuleManager moduleManager,
            IInlineConstraintResolver inlineConstraintResolver)
        {
            _pluginManager = pluginManager;
            _moduleManager = moduleManager;
            _inlineConstraintResolver = inlineConstraintResolver;
        }

        public void Regist(IRouteBuilder routeBuilder)
        {
            List<PluginRouteContext> allContext = new List<PluginRouteContext>();

            _moduleManager.ModuleResolvedResult.ParallelResult.Values.ForEach_(r =>
            {
                var module = r.Instance as PluginModule;
                if (module == null)
                    return;
                var plugin = _pluginManager.GetPlugin(r.Assembly);
                if (plugin == null)
                    return;

                PluginRouteContext pluginRouteContext = new PluginRouteContext(plugin);
                module.MapPluginRoute(pluginRouteContext);
                if (module.UseDefaultPluginRoute)
                {
                    pluginRouteContext.MapPluginRoute(
                        plugin.Id,
                        plugin.PluginModel.Name + "/{controller=Home}/{action=Index}/{id?}",
                        new { area = plugin.Id },
                        null,
                        new { area = plugin.Id });
                }

                allContext.Add(pluginRouteContext);
            });

            allContext.SelectMany(r => r.Routes).ForEach_(r =>
            {
                var defaults = new RouteValueDictionary(r.Defaults);
                if (!defaults.ContainsKey("area"))
                    defaults.Add("area", r.PluginId);

                var tokens = new RouteValueDictionary(r.Defaults);
                if (!tokens.ContainsKey("area"))
                    tokens.Add("area", r.PluginId);

                routeBuilder.Routes.Add(new PluginRouter(
                                          _pluginManager,
                                          routeBuilder.DefaultHandler,
                                          r.RouteName,
                                          r.RouteTemplate,
                                          defaults,
                                          new RouteValueDictionary(r.Constraints),
                                          tokens,
                                          _inlineConstraintResolver
                      ));
            });
        }
    }
}