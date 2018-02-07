using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Shouldly;
using Lazy.Utilities.Extensions;
using Lazy.Kernel.Dependency;
using System.Linq;
using System.Reflection;

namespace Lazy.Kernel.Tests
{
    public class BaseOnServiceDescriptorProvider_Tests
    {
        [Fact]
        public void Tests()
        {

            var b = typeof(IBase<int>).IsGenericTypeDefinition;

            BaseOnServiceDescriptorProvider provider = new BaseOnServiceDescriptorProvider(typeof(IBase), Microsoft.Extensions.DependencyInjection.ServiceLifetime.Transient);


            var services = provider.FromAssembly(this.GetType().Assembly, true);


            var ib = services.FirstOrDefault(r => r.ServiceType == typeof(IBase));
            ib.ShouldBeNull();

            var @if = services.FirstOrDefault(r => r.ServiceType == typeof(IF));
            @if.ShouldNotBeNull();
            @if.ImplementationType.ShouldBe(typeof(F));
            @if.Lifetime.ShouldBe(Microsoft.Extensions.DependencyInjection.ServiceLifetime.Transient);

            var f = services.FirstOrDefault(r => r.ServiceType == typeof(F));
            f.ShouldNotBeNull();
            f.ImplementationType.ShouldBe(typeof(F));
            f.Lifetime.ShouldBe(Microsoft.Extensions.DependencyInjection.ServiceLifetime.Transient);

            var g = services.FirstOrDefault(r => r.ServiceType == typeof(G));
            g.ShouldNotBeNull();
            g.ImplementationType.ShouldBe(typeof(G));
            g.Lifetime.ShouldBe(Microsoft.Extensions.DependencyInjection.ServiceLifetime.Transient);


            BaseOnServiceDescriptorProvider provider1 = new BaseOnServiceDescriptorProvider(typeof(IBase<>), Microsoft.Extensions.DependencyInjection.ServiceLifetime.Transient);
            var service1 = provider1.FromAssembly(this.GetType().Assembly, true);

            var ib_ = service1.FirstOrDefault(r => r.ServiceType == typeof(IBase<>));
            ib_.ShouldNotBeNull();
            ib_.ImplementationType.ShouldBe(typeof(H<>));
            ib_.Lifetime.ShouldBe(Microsoft.Extensions.DependencyInjection.ServiceLifetime.Transient);

            var ih_ = service1.FirstOrDefault(r => r.ServiceType == typeof(IH<>));
            ih_.ShouldNotBeNull();
            ih_.ImplementationType.ShouldBe(typeof(H<>));
            ih_.Lifetime.ShouldBe(Microsoft.Extensions.DependencyInjection.ServiceLifetime.Transient);

            var h_ = service1.FirstOrDefault(r => r.ServiceType == typeof(H<>));
            h_.ShouldNotBeNull();
            h_.ImplementationType.ShouldBe(typeof(H<>));
            h_.Lifetime.ShouldBe(Microsoft.Extensions.DependencyInjection.ServiceLifetime.Transient);

            BaseOnServiceDescriptorProvider provider2 = new BaseOnServiceDescriptorProvider(typeof(IBase<int>), Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton);
            var service2 = provider2.FromAssembly(this.GetType().Assembly, false);

            var ij = service2.FirstOrDefault(r => r.ServiceType == typeof(IJ));
            ij.ShouldNotBeNull();
            ij.ImplementationType.ShouldBe(typeof(J));
            ij.Lifetime.ShouldBe(Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton);

            var j = service2.FirstOrDefault(r => r.ServiceType == typeof(J));
            j.ShouldBeNull();
        }
    }


}
