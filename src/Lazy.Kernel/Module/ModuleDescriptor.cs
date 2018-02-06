using Lazy.Utilities.Extensions;
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
        /// 模块类型信息
        /// </summary>
        public TypeInfo ModuleType { get; internal set; }

        /// <summary>
        /// 模块程序集
        /// </summary>
        public Assembly Assembly { get; internal set; }

        /// <summary>
        /// 模块实例
        /// </summary>
        public LazyModule Instance { get; internal set; }

        /// <summary>
        /// 模块服务配置类型,可能为空
        /// </summary>
        public TypeInfo ModuleConfigureType { get; internal set; }

        /// <summary>
        /// 模块服务配置类实例,可能为空
        /// </summary>
        public IModuleConfigure ModuleConfigureInstance { get; internal set; }

        /// <summary>
        /// 递归结构的依赖信息
        /// </summary>
        public ICollection<ModuleDescriptor> Dependencies { get; internal set; } = new List<ModuleDescriptor>();

        HashSet<ModuleDescriptor> all;

        /// <summary>
        /// 获取平行结构的依赖信息
        /// </summary>
        /// <returns></returns>
        public ISet<ModuleDescriptor> GetAllDependendies()
        {
            if (all != null)
                return all;

            all = new HashSet<ModuleDescriptor>();
            foreach (var item in Dependencies)
            {
                FillAll(item);
            }

            return all;
        }

        private void FillAll(ModuleDescriptor moduleDescriptor)
        {
            foreach (var item in moduleDescriptor.Dependencies)
            {
                all.Add(item);
            }
            all.Add(moduleDescriptor);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (this == null)
                return false;

            if (object.ReferenceEquals(obj, this))
                return true;

            return this.ModuleType == (TypeInfo)obj;
        }

        public override int GetHashCode()
        {
            return this.ModuleType.GetHashCode();
        }
    }
}
