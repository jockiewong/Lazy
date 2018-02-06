using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazy.Kernel.Dependency
{
    /// <summary>
    /// ioc管理器,包装IServiceCollection实例
    /// </summary>
    public interface IIocManager
    {
        IServiceCollection ServiceCollection { get; }
    }
}
