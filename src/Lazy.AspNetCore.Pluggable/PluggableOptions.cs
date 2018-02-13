using System;
using System.Collections.Generic;
using System.Text;

namespace Lazy.AspNetCore.Pluggable
{
    public class PluggableOptions
    {
        /// <summary>
        /// 插件源位置,相对路径,必须在web目录下面,以"/"开头
        /// </summary>
        public string PluginSourceLocation { get; set; }

        /// <summary>
        /// 是否使用默认的插件路由,默认true
        /// <para>默认插件路由使用插件的name作为区域area,即url为{name}/{controller}/{action}/{id}</para>
        /// </summary>
        public bool UseDefaultPluginRoute { get; set; } = true;
    }
}
