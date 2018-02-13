using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazy.AspNetCore.Pluggable.Mvc
{
    /// <summary>
    /// 插件路由注册器
    /// </summary>
    public interface IPluginRouterRegister
    {
        void Regist(IRouteBuilder routeBuilder);
    }
}