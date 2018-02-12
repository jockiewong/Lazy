using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Lazy.Kernel.Module.PluginModule
{
    /// <summary>
    /// 默认的插件程序集依赖解析器,从插件程序集所在目录中加载依赖
    /// </summary>
    public class DefaultPluginInitialize : IPluginInitialize
    {

        private static readonly List<string> locations = new List<string>();

        public void Init(Assembly pluginAssembly)
        {
            var path = Path.GetDirectoryName(pluginAssembly.Location);
            if (locations.Contains(path))
                return;

            locations.Add(path);
            AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
            {
                var dll = Path.Combine(path, args.Name.Split(',')[0] + ".dll");
                if (File.Exists(dll))
                    return Assembly.LoadFrom(dll);
                return null;
            };
        }
    }
}
