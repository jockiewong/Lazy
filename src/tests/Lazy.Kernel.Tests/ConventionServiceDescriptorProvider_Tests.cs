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
    public class ConventionServiceDescriptorProvider_Tests
    {
        [Fact]
        public void Tests()
        {
            ConventionServiceDescriptorProvider provider = new ConventionServiceDescriptorProvider();

            var services = provider.FromAssembly(this.GetType().Assembly, true);

            var ia = services.FirstOrDefault(r => r.ServiceType == typeof(IA));
            ia.ShouldNotBeNull();
            ia.ImplementationType.ShouldBe(typeof(A));
            ia.Lifetime.ShouldBe(Microsoft.Extensions.DependencyInjection.ServiceLifetime.Transient);

            var ib = services.FirstOrDefault(r => r.ServiceType == typeof(IB));
            ib.ShouldNotBeNull();
            ib.ImplementationType.ShouldBe(typeof(B));
            ib.Lifetime.ShouldBe(Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton);

            var ic = services.FirstOrDefault(r => r.ServiceType == typeof(IC));
            ic.ShouldNotBeNull();
            ic.ImplementationType.ShouldBe(typeof(C));
            ic.Lifetime.ShouldBe(Microsoft.Extensions.DependencyInjection.ServiceLifetime.Scoped);

            var a = services.FirstOrDefault(r => r.ServiceType == typeof(A));
            a.ShouldNotBeNull();
            a.ImplementationType.ShouldBe(typeof(A));
            a.Lifetime.ShouldBe(Microsoft.Extensions.DependencyInjection.ServiceLifetime.Transient);

            var b = services.FirstOrDefault(r => r.ServiceType == typeof(B));
            b.ShouldNotBeNull();
            b.ImplementationType.ShouldBe(typeof(B));
            b.Lifetime.ShouldBe(Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton);

            var c = services.FirstOrDefault(r => r.ServiceType == typeof(C));
            c.ShouldNotBeNull();
            c.ImplementationType.ShouldBe(typeof(C));
            c.Lifetime.ShouldBe(Microsoft.Extensions.DependencyInjection.ServiceLifetime.Scoped);


            var id = services.FirstOrDefault(r => r.ServiceType == typeof(ID<,>));
            id.ShouldBeNull();

            var idg = services.FirstOrDefault(r => r.ServiceType == typeof(ID<string, int>));
            idg.ShouldNotBeNull();
            idg.ImplementationType.ShouldBe(typeof(D));

            var d = services.FirstOrDefault(r => r.ServiceType == typeof(D));
            d.ShouldNotBeNull();
            d.ServiceType.ShouldBe(typeof(D));
            d.ImplementationType.ShouldBe(typeof(D));

            var ie = services.FirstOrDefault(r => r.ServiceType == typeof(IE<,>));
            ie.ShouldNotBeNull();
            ie.ImplementationType.ShouldBe(typeof(E<,>));


            ConventionServiceDescriptorProvider provider1 = new ConventionServiceDescriptorProvider();
            var services1 = provider.FromAssembly(this.GetType().Assembly, false);

            var a1 = services1.FirstOrDefault(r => r.ServiceType == typeof(A));
            a1.ShouldBeNull();
        }
    }
}
