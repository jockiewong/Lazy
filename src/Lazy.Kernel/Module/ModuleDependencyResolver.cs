using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.DependencyModel;
using Lazy.Utilities.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Lazy.Kernel.Module
{
    /// <summary>
    /// 模块依赖关系解析器
    /// </summary>
    public class ModuleDependencyResolver : IModuleDependencyResolver
    {
        /// <summary>
        /// 引用了如下程序集的库视为候选模块程序集
        /// </summary>
        private static HashSet<string> ReferenceAssemblies { get; } = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
           "Lazy.Kernel"
        };

        //所有的依赖信息
        IDictionary<string, LibraryInfo> moduleDependencyInfo = new Dictionary<string, LibraryInfo>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// 依赖关系解析
        /// </summary>
        /// <param name="entryAssemblies">入口程序集</param>
        /// <returns></returns>
        public ModuleResolvedResult DependencyResolve(IEnumerable<Assembly> entryAssemblies)
        {
            if (entryAssemblies.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(entryAssemblies));
            }

            List<ModuleDescriptor> modules = new List<ModuleDescriptor>();
            foreach (var entryAssembly in entryAssemblies)
            {
                var entryModuleType = GetModuleType(entryAssembly);
                if (entryModuleType == null)
                    throw new KernelException($"entryAssembly {entryAssembly} is not contain LazyModule Type");
                ModuleDescriptor entryModuleDescriptor = new ModuleDescriptor()
                {
                    Assembly = entryAssembly,
                    ModuleType = entryModuleType,
                    Instance = Activator.CreateInstance(entryModuleType) as LazyModule
                };

                var context = DependencyContext.Load(entryAssembly);

                var resolver = new CandidateResolver(context.RuntimeLibraries, ReferenceAssemblies);

                //候选运行时库
                var candidateLibs = resolver
                     .GetCandidates();

                foreach (var lib in candidateLibs)
                {
                    if (moduleDependencyInfo.ContainsKey(lib.Name))
                        continue;

                    lib
                        .GetDefaultAssemblyNames(context)
                        .Select(a => new LibraryInfo { Name = lib.Name, Assembly = Assembly.Load(a), Library = lib })
                        .ForEach_(r =>
                        {
                            if (moduleDependencyInfo.ContainsKey(r.Name))
                                return;
                            var type = GetModuleType(r.Assembly);
                            if (type != null)
                            {
                                r.ModuleType = type;
                                moduleDependencyInfo.Add(r.Name, r);
                            }
                        });

                }

                FillDependencies(entryModuleDescriptor);

                modules.Add(entryModuleDescriptor);
            }

            return new ModuleResolvedResult(modules, _allModuleDesciptors);
        }

        /// <summary>
        /// 所有的模块数据,平行结构
        /// </summary>
        private IDictionary<TypeInfo, ModuleDescriptor> _allModuleDesciptors { get; } = new Dictionary<TypeInfo, ModuleDescriptor>();

        private void FillDependencies(ModuleDescriptor moduleDescriptor)
        {
            if (moduleDescriptor == null)
            {
                throw new ArgumentNullException(nameof(moduleDescriptor));
            }
            if (_allModuleDesciptors.ContainsKey(moduleDescriptor.ModuleType))
            {
                var descriptor = _allModuleDesciptors[moduleDescriptor.ModuleType];
                FillDependencies(descriptor);
                moduleDescriptor.Dependencies.Add(descriptor);
                return;
            }
            else
            {
                var name = moduleDescriptor.Assembly.GetName().Name;

                var info = moduleDependencyInfo[name];

                foreach (var item in info.Library.Dependencies)
                {
                    if (!moduleDependencyInfo.ContainsKey(item.Name))
                        continue;

                    var dependency = moduleDependencyInfo[item.Name];
                    ModuleDescriptor descriptor = new ModuleDescriptor
                    {
                        Assembly = dependency.Assembly,
                        ModuleType = dependency.ModuleType,
                        Instance = Activator.CreateInstance(dependency.ModuleType) as LazyModule
                    };
                    FillDependencies(descriptor);
                    moduleDescriptor.Dependencies.Add(descriptor);
                }
                _allModuleDesciptors.Add(moduleDescriptor.ModuleType, moduleDescriptor);
            }
        }

        /// <summary>
        /// 从程序集中获取LazyModule类型,没有返回null
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        private TypeInfo GetModuleType(Assembly assembly)
        {
            return assembly.DefinedTypes.FirstOrDefault(r => !r.IsAbstract && r.IsClass && r.IsChildTypeOf<LazyModule>());
        }

        /// <summary>
        /// copy from mvc
        /// </summary>
        private class CandidateResolver
        {
            private readonly IDictionary<string, Dependency> _runtimeDependencies;

            public CandidateResolver(IReadOnlyList<RuntimeLibrary> runtimeDependencies, ISet<string> referenceAssemblies)
            {
                var dependenciesWithNoDuplicates = new Dictionary<string, Dependency>(StringComparer.OrdinalIgnoreCase);
                foreach (var dependency in runtimeDependencies)
                {
                    if (dependenciesWithNoDuplicates.ContainsKey(dependency.Name))
                    {
                        throw new InvalidOperationException($"assembly {dependency.Name} has multiple");
                    }
                    dependenciesWithNoDuplicates.Add(dependency.Name, CreateDependency(dependency, referenceAssemblies));
                }

                _runtimeDependencies = dependenciesWithNoDuplicates;
            }

            private Dependency CreateDependency(RuntimeLibrary library, ISet<string> referenceAssemblies)
            {
                var classification = DependencyClassification.Unknown;
                if (referenceAssemblies.Contains(library.Name))
                {
                    classification = DependencyClassification.LazyReference;
                }

                return new Dependency(library, classification);
            }

            private DependencyClassification ComputeClassification(string dependency)
            {
                if (!_runtimeDependencies.ContainsKey(dependency))
                {
                    // Library does not have runtime dependency. Since we can't infer
                    // anything about it's references, we'll assume it does not have a reference to Mvc.
                    return DependencyClassification.DoesNotReferenceLazy;
                }

                var candidateEntry = _runtimeDependencies[dependency];
                if (candidateEntry.Classification != DependencyClassification.Unknown)
                {
                    return candidateEntry.Classification;
                }
                else
                {
                    var classification = DependencyClassification.DoesNotReferenceLazy;
                    foreach (var candidateDependency in candidateEntry.Library.Dependencies)
                    {
                        var dependencyClassification = ComputeClassification(candidateDependency.Name);
                        if (dependencyClassification == DependencyClassification.ReferencesLazy ||
                            dependencyClassification == DependencyClassification.LazyReference)
                        {
                            classification = DependencyClassification.ReferencesLazy;
                            break;
                        }
                    }

                    candidateEntry.Classification = classification;

                    return classification;
                }
            }

            public IEnumerable<RuntimeLibrary> GetCandidates()
            {
                foreach (var dependency in _runtimeDependencies)
                {
                    if (ComputeClassification(dependency.Key) == DependencyClassification.ReferencesLazy)
                    {
                        yield return dependency.Value.Library;
                    }
                }
            }

            private class Dependency
            {
                public Dependency(RuntimeLibrary library, DependencyClassification classification)
                {
                    Library = library;
                    Classification = classification;
                }

                public RuntimeLibrary Library { get; }

                public DependencyClassification Classification { get; set; }

                public override string ToString()
                {
                    return $"Library: {Library.Name}, Classification: {Classification}";
                }
            }

            private enum DependencyClassification
            {
                Unknown = 0,

                /// <summary>
                /// References (directly or transitively) one of the Lazy packages listed in
                /// <see cref="ReferenceAssemblies"/>.
                /// </summary>
                ReferencesLazy = 1,

                /// <summary>
                /// Does not reference (directly or transitively) one of the Lazy packages listed by
                /// <see cref="ReferenceAssemblies"/>.
                /// </summary>
                DoesNotReferenceLazy = 2,

                /// <summary>
                /// One of the references listed in <see cref="ReferenceAssemblies"/>.
                /// </summary>
                LazyReference = 3,
            }
        }

        /// <summary>
        /// 程序集库信息
        /// </summary>
        private class LibraryInfo
        {
            public string Name { get; set; }

            public TypeInfo ModuleType { get; set; }

            public RuntimeLibrary Library { get; set; }

            public Assembly Assembly { get; set; }
        }
    }
}
