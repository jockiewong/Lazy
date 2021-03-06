﻿using Lazy.Kernel.Dependency;
using Lazy.Kernel.Module;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Options;
using Lazy.Kernel;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Test3;

namespace Test2
{
    public class Module2 : LazyModule
    {
        public override void ConfigureService(ILazyBuilder lazyBuilder)
        {
            lazyBuilder.Test2Option(r => { r.Name = "老王2"; })
                .Test3Option(r => r.Name = "老王3");
        }

        public override void OnInit(IServiceProvider serviceProvider)
        {
            serviceProvider.GetService<IModuleOptionProvider<Test2Options>>().GetConfiguredOptions().Name.ShouldBe("老王2");
            serviceProvider.GetService<IModuleOptionProvider<Test2Options2>>();
            base.OnInit(serviceProvider);
        }
    }

    public class Test2Options
    {
        public string Name { get; set; }
    }

    public class Test2Options2
    {

    }

    public static class Test2OptionsExtension
    {
        public static ILazyBuilder Test2Option(this ILazyBuilder lazyBuilder, Action<Test2Options> action)
        {
            lazyBuilder.ServiceCollection.Configure(action);
            return lazyBuilder;
        }
    }


    public interface ITest2 : ISingleton { }
    public class Test2 : ITest2 { }
}
