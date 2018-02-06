using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace Lazy.Kernel.Dependency
{
    /// <summary>
    /// 包装IServiceCollection实例,并实现静态实例,可以在静态类中使用
    /// </summary>
    public class IocManager : IIocManager
    {
        public IocManager(IServiceCollection serviceCollection)
        {
            this.ServiceCollection = serviceCollection;
            Instance = this;
        }

        public IServiceCollection ServiceCollection { get; }

        public static IIocManager Instance { get; private set; }

    }
}
