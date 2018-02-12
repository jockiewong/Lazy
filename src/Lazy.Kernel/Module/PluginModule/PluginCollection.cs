using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Lazy.Kernel.Module.PluginModule
{
    public class PluginCollection : List<PluginInfo>
    {
        public void AddByDefaultResolver(Assembly pluginAssembly)
        {
            this.Add(new PluginInfo
            {
                PluginAssembly = pluginAssembly,
                PluginInitialize = new DefaultPluginInitialize()
            });
        }
    }
}
