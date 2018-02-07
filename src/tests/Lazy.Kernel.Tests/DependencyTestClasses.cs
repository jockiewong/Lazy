using Lazy.Kernel.Dependency;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazy.Kernel.Tests
{
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

    public interface IBase { }

    public interface IF : IBase { }

    public class F : IF { }

    public class G : IBase { }

    public interface IBase<T> { }

    public interface IH<T> : IBase<T> { }

    public class H<T> : IH<T> { }

    public interface IJ : IBase<int> { }

    public class J : IJ { }
}
