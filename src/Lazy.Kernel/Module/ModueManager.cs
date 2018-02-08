using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Lazy.Utilities.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace Lazy.Kernel.Module
{
    /// <summary>
    /// 模块管理器
    /// </summary>
    public class ModuleManagaer : IModuleManager
    {
        public ModuleManagaer(
            IModuleDependencyResolver moduleDependencyResolver,
            IModuleOptionProvider<StartupOptions> startupOptions,
            ILogger<ModuleManagaer> logger
            )
        {
            ModuleDependencyResolver = moduleDependencyResolver;
            _startupOptions = startupOptions.GetConfiguredOptions();
            _logger = logger;
        }

        ILogger<ModuleManagaer> _logger;
        StartupOptions _startupOptions;
        IModuleDependencyResolver ModuleDependencyResolver { get; }

        public ICollection<ModuleDescriptor> LoadedAllModule
        {
            get
            {
                if (_allModule == null)
                {
                    var ex = new KernelException("module manager has not init");
                    _logger.LogError(ex, $"Modules has not load ,please call LoadAllModule method first");
                    throw ex;
                }
                return _allModule;
            }
        }

        ICollection<ModuleDescriptor> _allModule;

        public void LoadAllModule(Assembly entryAssembly)
        {
            if (entryAssembly == null)
            {
                throw new ArgumentNullException(nameof(entryAssembly));
            }

            var assemblies = new HashSet<Assembly>();
            assemblies.Add(entryAssembly);
            _startupOptions.Plugins.ForEach_(r =>
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
            _logger.LogDebug($"Begin load all module from entry assembly {entryAssembly},Plugin assemblys contains {_startupOptions.Plugins.Select(r => r.PluginAssembly.FullName).Join(",")}");
            HashSet<ModuleDescriptor> allModule = new HashSet<ModuleDescriptor>();

            var resolvedResult = ModuleDependencyResolver.DependencyResolve(assemblies);

            _allModule = resolvedResult.ParallelResult.Values;
            _logger.LogDebug($"All module has loaded,Plugin module contains {_allModule.Select(r => r.ModuleType.FullName).Join(",")}");
        }

        public void InitAllModule(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }

            LoadedAllModule.ForEach_(r =>
            {
                r.Instance = serviceProvider.GetRequiredService(r.ModuleType) as LazyModule;

                r.Instance.OnInit();
            });
            LoadedAllModule.ForEach_(r =>
            {
                r.Instance.OnInited();
            });
        }

        public void ConfigureAllModuleService(ILazyBuilder lazyBuilder)
        {
            if (lazyBuilder == null)
            {
                throw new ArgumentNullException(nameof(lazyBuilder));
            }

            LoadedAllModule.ForEach_(module =>
            {
                if (module.ModuleConfigureInstance != null)
                    module.ModuleConfigureInstance.Configure(lazyBuilder);

                lazyBuilder.ServiceCollection.TryAddSingleton(module.ModuleType, module.ModuleType);
                _startupOptions.ServiceDescriptorProviders.ForEach_(provider =>
                {
                    var serviceDescriptors = provider.FromAssembly(module.Assembly, _startupOptions.ServiceContainsSelf);
                    lazyBuilder.ServiceCollection.Add(serviceDescriptors);
                });
            });
        }
    }
}
