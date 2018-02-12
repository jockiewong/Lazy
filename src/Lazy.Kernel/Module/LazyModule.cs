using Lazy.Utilities.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazy.Kernel.Module
{
    /// <summary>
    /// lazy模块基类
    /// </summary>
    public abstract class LazyModule
    {
        /// <summary>
        /// 配置模块服务
        /// </summary>
        /// <param name="lazyBuilder"></param>
        public virtual void ConfigureService(ILazyBuilder lazyBuilder) { }

        /// <summary>
        /// 初始化
        /// </summary>
        public virtual void OnInit(IServiceProvider serviceProvider) { }

        /// <summary>
        /// 初始化后
        /// </summary>
        public virtual void OnInited(IServiceProvider serviceProvider) { }
    }
}
