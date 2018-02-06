using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazy.Kernel.Module
{
    /// <summary>
    /// 模块配置接口
    /// </summary>
    public interface IModuleConfigure
    {
        void Configure(ILazyBuilder lazyBuilder);
    }
}
