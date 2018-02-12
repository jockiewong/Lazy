using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Lazy.AspNetCore.Pluggable.Plugin
{
    public interface IPluginManager
    {
        PluginCollection Plugins { get; }

        void LoadAllPlugin(string pluginSourceLocation);

        PluginDescriptor GetPlugin(string id);

        PluginDescriptor GetPlugin(Assembly pluginAssembly);

        Task EnablePluginAsync(string id);

        Task DisablePluginAsync(string id);
    }
}
