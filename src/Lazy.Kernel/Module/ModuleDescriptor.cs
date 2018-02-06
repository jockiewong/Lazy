using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Lazy.Kernel.Module
{
    /// <summary>
    /// 模块信息描述
    /// </summary>
    public class ModuleDescriptor
    {
        /// <summary>
        /// Dependencies的平行结构,去重复
        /// </summary>
        public IReadOnlyList<ModuleDescriptor> AllDependencies { get; set; }

        public TypeInfo ModuleType { get; }

        public Assembly Assembly { get; }

        public LazyModule Instance { get; }

        public IReadOnlyList<ModuleDescriptor> Dependencies { get; }

        public ModuleDescriptor(Assembly entryAssembly)
        {

        }
    }
}
