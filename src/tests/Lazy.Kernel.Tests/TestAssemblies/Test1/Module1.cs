using Lazy.Kernel.Dependency;
using Lazy.Kernel.Module;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Options;
using Lazy.Kernel;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
namespace Test1
{
    public class Module1 : LazyModule
    {
        public Module1(
            IModuleOptionProvider<Test1Options> moduleOption,
            IModuleOptionProvider<Test1Options2> moduleOption2
            )
        {
            var test1Options = moduleOption.GetConfiguredOptions();

            test1Options.Name.ShouldBe("老王1");
        }
    }

    public class Test1Options
    {
        public string Name { get; set; }
    }

    public class Test1Options2
    {

    }

    public static class Test1OptionsExtension
    {
        public static ILazyBuilder Test1Option(this ILazyBuilder lazyBuilder, Action<Test1Options> action)
        {
            lazyBuilder.ServiceCollection.Configure(action);
            return lazyBuilder;
        }
    }


    public interface ITest1 : ISingleton { }
    public class Test1 : ITest1 { }
}
