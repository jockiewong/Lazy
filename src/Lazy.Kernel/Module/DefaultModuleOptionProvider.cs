using Lazy.Utilities.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Concurrent;
namespace Lazy.Kernel.Module
{
    public class DefaultModuleOptionProvider<TOption> : IModuleOptionProvider<TOption> where TOption : class, new()
    {
        IEnumerable<IConfigureOptions<TOption>> _configureActions;
        public DefaultModuleOptionProvider(IEnumerable<IConfigureOptions<TOption>> configureActions)
        {
            _configureActions = configureActions;
        }

        //缓存配置过的值,GetConfiguredOptions调用一次后,后面再获取就会从缓存中读取
        static ConcurrentDictionary<Type, object> _cacheOptionDic = new ConcurrentDictionary<Type, object>();

        public TOption GetConfiguredOptions()
        {
            var type = typeof(TOption);

            if (_cacheOptionDic.ContainsKey(type))
            {
                _cacheOptionDic.TryGetValue(type, out object op);
                return op as TOption;
            };

            var option = Activator.CreateInstance<TOption>();
            if (!_configureActions.IsNullOrEmpty())
                _configureActions.ForEach_(r =>
                {
                    r.Configure(option);
                });

            _cacheOptionDic.TryAdd(type, option);
            return option;
        }
    }
}
