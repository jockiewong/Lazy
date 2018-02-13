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
    }
}
