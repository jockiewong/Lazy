using Lazy.Kernel.Module;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazy.AspNetCore.Pluggable
{
    /// <summary>
    /// 插件模块,如果插件中需要配置路由,则继承该类
    /// </summary>
    public abstract class PluginModule : LazyModule
    {
        public virtual void MapRoute(IRouteBuilder routeBuilder)
        {

        }
    }
}
