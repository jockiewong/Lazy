using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Lazy.Utilities.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Lazy.Kernel.Module
{
    /// <summary>
    /// 模块管理器
    /// </summary>
    public class ModuleManagaer : IModuleManager
    {
        public ModuleManagaer(
            ILazyBuilder lazyBuilder,
            IModuleDependencyResolver moduleDependencyResolver)
        {
            this.lazyBuilder = lazyBuilder;
            ModuleDependencyResolver = moduleDependencyResolver;
        }

        ILazyBuilder lazyBuilder { get; }

        public ICollection<ModuleDescriptor> AllModule
        {
            get
            {
                if (_allModule == null)
                    throw new Exception("IModuleManager no init");

                return _allModule;
            }
        }

        IModuleDependencyResolver ModuleDependencyResolver { get; }

        ICollection<ModuleDescriptor> _allModule;

        public void Init(Assembly entryAssembly, StartupOptions options)
        {
            var assemblies = new HashSet<Assembly>();
            assemblies.Add(entryAssembly);
            options.Plugins.ForEach_(r =>
            {
                assemblies.Add(r.PluginAssembly);
                if (r.DependAssemblyResolver != null)
                {
                    AppDomain.CurrentDomain.AssemblyResolve += (sender, e) =>
                    {
                        return r.DependAssemblyResolver.AssemblyResolve(r.PluginAssembly, sender, e);
                    };
                }
            });

            HashSet<ModuleDescriptor> allModule = new HashSet<ModuleDescriptor>();

            var resolvedResult = ModuleDependencyResolver.DependencyResolve(assemblies);

            _allModule = resolvedResult.ParallelResult.Values;

            _allModule.ForEach_(r =>
            {
                if (r.ModuleConfigureInstance != null)
                    r.ModuleConfigureInstance.Configure(lazyBuilder);
            });
        }
    }
}
