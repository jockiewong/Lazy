using Lazy.Kernel.Module;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazy.AspNetCore.Pluggable
{
    public abstract class PluginModule : LazyModule
    {
        public virtual void MapRoute(IRouteBuilder routeBuilder)
        {

        }
    }
}
