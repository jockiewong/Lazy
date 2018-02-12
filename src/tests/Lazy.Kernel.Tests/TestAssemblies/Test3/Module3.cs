using Lazy.Kernel.Dependency;
using Lazy.Kernel.Module;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Options;
using Lazy.Kernel;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
namespace Test3
{
    public class Module3 : LazyModule
    {
        public Module3(IModuleOptionProvider<Test3Options> configureOptions
            )
        {
            configureOptions.GetConfiguredOptions().Name.ShouldBe("老王3");
        }

        public override void ConfigureService(ILazyBuilder lazyBuilder)
        {
            lazyBuilder.Test3Option(r =>
            {
                r.Name = "老王33333";
            });
        }
    }

    public class Test3Options
    {
        public string Name { get; set; }
    }


    public static class Test3OptionsExtension
    {
        public static ILazyBuilder Test3Option(this ILazyBuilder lazyBuilder, Action<Test3Options> action)
        {
            lazyBuilder.ServiceCollection.Configure(action);
            return lazyBuilder;
        }
    }


    public interface ITest3 : ISingleton { }
    public class Test3 : ITest3 { }
}
