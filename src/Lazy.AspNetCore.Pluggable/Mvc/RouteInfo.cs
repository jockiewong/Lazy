using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazy.AspNetCore.Pluggable.Mvc
{
    public class RouteInfo
    {
        public string RouteName { get; set; }
        public string RouteTemplate { get; set; }
        public object Defaults { get; set; }
        public object Constraints { get; set; }
        public object DataTokens { get; set; }

        public string PluginId { get; set; }
    }
}
