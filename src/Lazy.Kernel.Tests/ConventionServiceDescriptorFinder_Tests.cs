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
    public class ConventionServiceDescriptorFinder_Tests
    {
        [Fact]
        public void Tests()
        {
            ConventionServiceDescriptorProvider conventionServiceDescriptorFinder = new ConventionServiceDescriptorProvider();


            var services = conventionServiceDescriptorFinder.FromAssembly(this.GetType().Assembly);


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
        }
    }

    public interface IA : ITransient
    {

    }

    public class A : IA
    {

    }

    public interface IB : ISingleton
    {

    }

    public class B : IB
    {

    }

    public interface IC
    {

    }

    public class C : IScoped, IC
    {

    }

    public interface ID<t1, t2> { }

    public class D : ID<string, int>, ITransient { }

    public interface IE<t1, t2> { }

    public class E<t1, t2> : IE<t1, t2>, ITransient { }
}
