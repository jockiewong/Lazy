using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazy.Kernel
{
    public interface ILazyBuilder
    {
        IServiceCollection ServiceCollection { get; }
    }
}
