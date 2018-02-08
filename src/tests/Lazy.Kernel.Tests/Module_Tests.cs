using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Shouldly;
using Microsoft.Extensions.DependencyInjection;
using Lazy.Kernel;
using Lazy.Kernel.Dependency;
using Lazy.Kernel.Module;
using System.Reflection;
using System.IO;
using Test1;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;
using Microsoft.Extensions.Logging.Console;
namespace Lazy.Kernel.Tests
{
    public class Module_Tests
    {
        [Fact]
        public void Test()
        {

            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging();
            serviceCollection.AddLazy<TestStartupModule>(r =>
            {
                var path = Path.Combine(
                                AppDomain.CurrentDomain.BaseDirectory.Replace(@"\bin\Debug\netcoreapp2.0", ""),
                                @"TestAssemblies\Test2\bin\Debug\netstandard2.0\Test2.dll");
                var test2Ass = Assembly.LoadFrom(path);
                r.Plugins.AddByDefaultResolver(test2Ass);
                r.ServiceContainsSelf = true;
            })
            .Test1Option(r =>
            {
                r.Name = "老王1";
            });

            var provider = serviceCollection.BuildServiceProvider();
            provider.UseLazy();

            var moduleManager = provider.GetService<IModuleManager>();

            moduleManager.ModuleResolvedResult.ParallelResult.Count.ShouldBe(4);

            moduleManager.ModuleResolvedResult.ParallelResult.Values.FirstOrDefault(r => r.ModuleType == typeof(TestStartupModule)).ShouldNotBeNull();
            moduleManager.ModuleResolvedResult.ParallelResult.Values.FirstOrDefault(r => r.ModuleType == typeof(Module1)).ShouldNotBeNull();
        }
    }

    public class TestStartupModule : LazyModule
    {

    }
}
