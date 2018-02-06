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
        //public virtual void ConfigureService(IServiceCollection serviceCollection, ILazyBuilder lazyBuilder) { }

        /// <summary>
        /// 初始化
        /// </summary>
        public virtual void OnInit() { }

        /// <summary>
        /// 初始化后
        /// </summary>
        public virtual void OnInited() { }
    }
}
