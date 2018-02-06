using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Lazy.Kernel.Module
{
    /// <summary>
    /// 依赖关系解析后结果包裹类
    /// </summary>
    public class ModuleDependencyResolvedWrapper
    {
        public ModuleDependencyResolvedWrapper(IList<ModuleDescriptor> recursionResult, IDictionary<TypeInfo, ModuleDescriptor> parallelResult)
        {
            RecursionResult = recursionResult;
            ParallelResult = parallelResult;
        }

        /// <summary>
        /// 递归结构
        /// </summary>
        public IList<ModuleDescriptor> RecursionResult { get; }

        /// <summary>
        /// 平行结构
        /// </summary>
        public IDictionary<TypeInfo, ModuleDescriptor> ParallelResult { get; }
    }
}
