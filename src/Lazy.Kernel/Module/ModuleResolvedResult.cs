using Lazy.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Text;

namespace Lazy.Kernel.Module
{
    /// <summary>
    /// 依赖关系解析后结果包裹类
    /// </summary>
    public class ModuleResolvedResult
    {
        public ModuleResolvedResult(IList<ModuleDescriptor> recursionResult, IDictionary<TypeInfo, ModuleDescriptor> parallelResult)
        {
            RecursionResult = recursionResult.AsReadOnly();
            ParallelResult = new ReadOnlyDictionary<TypeInfo, ModuleDescriptor>(parallelResult);
        }

        /// <summary>
        /// 递归结构
        /// </summary>
        public IReadOnlyList<ModuleDescriptor> RecursionResult { get; }

        /// <summary>
        /// 平行结构
        /// </summary>
        public IReadOnlyDictionary<TypeInfo, ModuleDescriptor> ParallelResult { get; }
    }
}
