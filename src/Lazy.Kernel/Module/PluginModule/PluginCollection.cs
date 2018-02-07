using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Lazy.Kernel.Module.PluginModule
{
    public class PluginCollection : List<PluginDescriptor>
    {
        public void AddByDefaultResolver(Assembly pluginAssembly)
        {
            this.Add(new PluginDescriptor
            {
                PluginAssembly = pluginAssembly,
                DependAssemblyResolver = new DefaultPluginDependAssemblyResolver()
            });
        }
    }
}
