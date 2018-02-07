using Lazy.Utilities.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazy.Kernel.Module
{
    public class DefaultModuleOptionProvider<TOption> : IModuleOptionProvider<TOption> where TOption : class, new()
    {
        IEnumerable<IConfigureOptions<TOption>> _configureActions;
        public DefaultModuleOptionProvider(IEnumerable<IConfigureOptions<TOption>> configureActions)
        {
            _configureActions = configureActions;
        }

        public TOption GetConfiguredOptions()
        {
            var option = Activator.CreateInstance<TOption>();
            if (!_configureActions.IsNullOrEmpty())
                _configureActions.ForEach_(r =>
                {
                    r.Configure(option);
                });

            return option;
        }
    }
}
