using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace Lazy.Kernel.Module
{
    /// <summary>
    /// 模块管理器
    /// </summary>
    public class ModuleManagaer : IModuleManagaer
    {
        public ModuleManagaer(IServiceCollection serviceCollection)
        {
            Services = serviceCollection;
        }

        public IServiceCollection Services { get; }

        public void Init(Assembly entryAssembly, StartupOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
