using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Collections;
using Lazy.AspNetCore.Pluggable.Plugin;

namespace Lazy.AspNetCore.Pluggable.Mvc
{
    /// <summary>
    /// 插件路由上下文,一个插件只能注册管理当前插件的路由,无权干涉其他插件路由
    /// </summary>
    public class PluginRouteContext
    {
        PluginDescriptor _pluginDescriptor;

        public PluginRouteContext(PluginDescriptor pluginDescriptor)
        {
            _pluginDescriptor = pluginDescriptor;
        }

        public IList<RouteInfo> Routes { get; } = new List<RouteInfo>();

        /// <summary>
        /// 增加插件路由
        /// </summary>
        /// <param name="name">路由名称</param>
        /// <param name="template">路由模板</param>
        /// <param name="defaults">路由默认值</param>
        /// <param name="constraints">路由约束</param>
        /// <param name="dataTokens">路由token值</param>
        public void MapPluginRoute(string name, string template, object defaults = null, object constraints = null, object dataTokens = null)
        {
            Routes.Add(new RouteInfo
            {
                RouteName = name,
                RouteTemplate = template,
                Defaults = defaults,
                Constraints = constraints,
                DataTokens = dataTokens,
                PluginId = _pluginDescriptor.Id
            });
        }
    }
}
