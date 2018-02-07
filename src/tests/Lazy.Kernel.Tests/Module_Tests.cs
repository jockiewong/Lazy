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

namespace Lazy.Kernel.Tests
{
    public class Module_Tests
    {
        [Fact]
        public void Test()
        {
            IServiceCollection serviceCollection = new ServiceCollection();

            serviceCollection.AddLazy<TestStartupModule>(r =>
            {
                var path = Path.Combine(
                                AppDomain.CurrentDomain.BaseDirectory.Replace(@"\bin\Debug\netcoreapp2.0", ""),
                                @"TestAssemblies\Test2\bin\Debug\netstandard2.0\Test2.dll");
                var test2Ass = Assembly.LoadFrom(path);
                r.Plugins.AddByDefaultResolver(test2Ass);
                r.ServiceContainSelf = true;
            })
            .Test1Option(r =>
            {
                r.Name = "老王1";
            });

            var provider = serviceCollection.BuildServiceProvider();

            provider.UseLazy();


            var moduleManager = provider.GetService<IModuleManager>();

            moduleManager.AllModule.Count.ShouldBe(4);
        }
    }

    public class TestStartupModule : LazyModule
    {

    }
}
