using Microsoft.AspNetCore.Mvc.Razor;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazy.AspNetCore.Pluggable.Mvc
{
    /// <summary>
    /// 插件视图配置
    /// </summary>
    public interface IPluginViewConfigure
    {
        void Configure(RazorViewEngineOptions options);
    }
}
