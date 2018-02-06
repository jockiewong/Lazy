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
        protected IServiceProvider Services { get; }

        public void Init() { }

        public void Inited() { }
    }
}
