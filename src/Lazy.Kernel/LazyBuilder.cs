using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace Lazy.Kernel
{
    class LazyBuilder : ILazyBuilder
    {
        public IServiceCollection ServiceCollection { get; }

        public static ILazyBuilder Instance { get; private set; }

        private LazyBuilder(IServiceCollection serviceCollection)
        {
            ServiceCollection = serviceCollection;
        }

        public static void Init(IServiceCollection serviceCollection)
        {
            if (Instance != null)
                return;
            Instance = new LazyBuilder(serviceCollection);
        }
    }
}
