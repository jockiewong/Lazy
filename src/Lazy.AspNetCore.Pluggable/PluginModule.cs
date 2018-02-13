using Lazy.AspNetCore.Pluggable.Mvc;
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
        /// <summary>
        /// 是否使用默认的插件路由
        /// </summary>
        public virtual bool UseDefaultPluginRoute { get; } = true;

        /// <summary>
        /// 增加插件路由
        /// </summary>
        /// <param name="routes">当前路由上下文</param>
        public virtual void MapPluginRoute(PluginRouteContext routes)
        {

        }
    }
}
