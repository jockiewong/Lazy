using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using Lazy.Utilities.Extensions;
using Lazy.Kernel;
using Lazy.Kernel.Module;
using Lazy.Kernel.Module.PluginModule;
using System.Linq;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Lazy.AspNetCore.Pluggable.Plugin
{
    class DefaultPluginManager : IPluginManager
    {
        ILogger<DefaultPluginManager> _logger;
        public DefaultPluginManager(
            ILogger<DefaultPluginManager> logger
            )
        {
            _logger = logger;
        }

        public PluginCollection Plugins
        {

            get
            {
                if (_pluginCollection == null)
                    throw new KernelException("plugin manager is not loaded.");

                return _pluginCollection;
            }
        }

        public PluginDescriptor GetPlugin(string id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            return Plugins.FirstOrDefault(r => r.Id.EqualsIgnoreCase(id));
        }

        public PluginDescriptor GetPlugin(Assembly pluginAssembly)
        {
            if (pluginAssembly == null)
            {
                throw new ArgumentNullException(nameof(pluginAssembly));
            }

            return Plugins.FirstOrDefault(r => r.PluginAssembly == pluginAssembly);
        }

        PluginCollection _pluginCollection;


        public void LoadAllPlugin(string pluginSourceLocation)
        {
            if (_pluginCollection != null)
                return;

            if (pluginSourceLocation == null)
            {
                throw new ArgumentNullException(nameof(pluginSourceLocation));
            }
            var folders = Directory.EnumerateDirectories(Server.MapPath(pluginSourceLocation));

            _pluginCollection = new PluginCollection();

            foreach (string item in folders)
            {
                string pluginPath = Path.Combine(item, Const.PluginDesciptorFileName);
                if (!File.Exists(pluginPath))
                    continue;
                try
                {
                    string pluginFolderName = Path.GetFileName(item);
                    PluginDescriptor pluginDescriptor = new PluginDescriptor();
                    var model = JsonHelper.DeserializeFromFile<PluginModel>(pluginPath);

                    pluginDescriptor.PluginModel = model;
                    model.DllFileName = model.DllFileName ?? pluginFolderName;
                    model.Name = model.Name ?? pluginFolderName;

                    var dll = Directory.EnumerateFiles(item, model.DllFileName, SearchOption.AllDirectories)?.FirstOrDefault();
                    if (dll.IsNullOrWhiteSpace())
                    {
                        _logger.LogWarning($"Plugin file [{pluginPath}] can't find dll file [{model.DllFileName}]");
                        continue;
                    }

                    var pluginAssembly = Assembly.LoadFrom(dll);

                    pluginDescriptor.FileLocation = pluginPath;
                    pluginDescriptor.Id = pluginFolderName;
                    pluginDescriptor.PluginAssembly = pluginAssembly;
                    pluginDescriptor.PluginInitialize = new DefaultPluginInitialize();
                    _pluginCollection.Add(pluginDescriptor);

                    _logger.LogDebug("Find a {0} plugin [{1}],location:[{2}],desciption:[{3}]," +
                        "author:[{4}],dependencies:[{5}],category:[{6}],version:[{7}],dll file:[{8}].",
                        model.Enabled ? "enabled" : "disabled",
                        model.Name,
                        item,
                        model.Description,
                        model.Author,
                        model.Dependencies?.Join(","),
                        model.Category,
                        model.Version,
                        model.DllFileName
                        );
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Load plugin [{pluginPath}] error:{ex.Message}");
                }
            }
        }

        public async Task EnablePluginAsync(string id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            var plugin = GetPlugin(id);
            if (plugin == null)
            {
                _logger.LogError($"Plugin [{id}] is not exists.");
                return;
            }
            plugin.PluginModel.Enabled = true;

            await JsonHelper.SerializeToFileAsync(plugin.PluginModel, plugin.FileLocation);
            _logger.LogInformation($"Plugin [{id}] has been enabled.");
        }

        public async Task DisablePluginAsync(string id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            var plugin = GetPlugin(id);
            if (plugin == null)
            {
                _logger.LogError($"Plugin [{id}] is not exists.");
                return;
            }
            plugin.PluginModel.Enabled = false;
            await JsonHelper.SerializeToFileAsync(plugin.PluginModel, plugin.FileLocation);
            _logger.LogInformation($"Plugin [{id}] has been disabled.");
        }
    }
}
