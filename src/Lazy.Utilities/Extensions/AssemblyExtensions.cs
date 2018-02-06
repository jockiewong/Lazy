using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Lazy.Utilities.Extensions
{
    public static class AssemblyExtensions
    {
        /// <summary>
        /// 安全不会异常的调用GetTypes()
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static IEnumerable<Type> GetTypesSafely(this Assembly assembly)
        {
            // .net framework 才需要这样写, .net standard已经有DefinedTypes属性实现
            //try
            //{
            //    return assembly.GetTypes();
            //}
            //catch (ReflectionTypeLoadException ex)
            //{
            //    return ex.Types.Where(x => x != null);
            //}

            return assembly.DefinedTypes;
        }

    }
}
