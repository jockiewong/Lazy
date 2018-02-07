using System;
using System.Collections.Generic;
using System.Text;

namespace Lazy.Kernel.Module
{
    /// <summary>
    /// 模块配置参数提供器
    /// </summary>
    /// <typeparam name="TOption"></typeparam>
    public interface IModuleOptionProvider<TOption> where TOption : class, new()
    {
        /// <summary>
        /// 获取已配置的参数
        /// </summary>
        /// <returns>已配置完成后的参数对象</returns>
        TOption GetConfiguredOptions();
    }
}
