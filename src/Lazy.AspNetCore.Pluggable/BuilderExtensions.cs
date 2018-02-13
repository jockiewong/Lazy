using Lazy.AspNetCore.Pluggable.Mvc;
using Lazy.AspNetCore.Pluggable.Plugin;
using Lazy.Kernel;
using Lazy.Kernel.Module;
using Lazy.Utilities.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lazy.AspNetCore.Pluggable
{
    public static class BuilderExtensions
    {
        /// <summary>
        /// 增加mvc插件化,需要在AddLazy之前调用
        /// </summary>
        /// <param name="lazyBuilder"></param>
        /// <param name="optionsAction"></param>
        /// <returns></returns>
        public static IServiceCollection AddLazyAspNetCoreMvcPluggable(
           this IServiceCollection serviceCollection,
           string pluginSourceLocation,
           Action<PluggableOptions> optionsAction = null)
        {
            if (serviceCollection == null)
            {
                throw new ArgumentNullException(nameof(serviceCollection));
            }



            if (pluginSourceLocation == null)
            {
                throw new ArgumentNullException(nameof(pluginSourceLocation));
            }
            PluggableServiceInstaller.Installer(serviceCollection);

            serviceCollection.Configure<PluggableOptions>(r => r.PluginSourceLocation = pluginSourceLocation);
            if (optionsAction != null)
                serviceCollection.Configure(optionsAction);

            var mvcBuilder = serviceCollection.AddMvc();

            using (var scope = serviceCollection.BuildServiceProvider().CreateScope())
            {
                var host = scope.ServiceProvider.GetRequiredService<IHostingEnvironment>();
                Server.Init(host);

                PluggableOptions options = new PluggableOptions();

                scope
                   .ServiceProvider
                   .GetRequiredService<IConfigureOptions<PluggableOptions>>()
                   .Configure(options);

                var pluginManager = scope.ServiceProvider.GetRequiredService<IPluginManager>();

                pluginManager.LoadAllPlugin(Server.MapPath(options.PluginSourceLocation));

                if (options.PluginSourceLocation.IsNullOrWhiteSpace())
                    throw new KernelException("mvc pluggable option: PluginSourceLocation is null.");

                if (!options.PluginSourceLocation.StartsWith("/"))
                {
                    throw new KernelException($"{nameof(pluginSourceLocation)} is not relative path, should start with '/'.");
                }

                pluginManager.Plugins.ForEach(r => mvcBuilder.AddApplicationPart(r.PluginAssembly));

                serviceCollection.Configure<StartupOptions>(r =>
                {
                    r.Plugins.AddRange(pluginManager.Plugins);
                });

                mvcBuilder.AddRazorOptions(r =>
                {
                    r.AreaViewLocationFormats.Add(options.PluginSourceLocation.TrimEnd('/') + "/{2}/Views/{1}/{0}.cshtml");
                });

                serviceCollection.Replace(ServiceDescriptor.Singleton(pluginManager));
            }

            return serviceCollection;
        }


        public static IApplicationBuilder UseLazyAspNetCoreMvcPluggable(
            this IApplicationBuilder app,
            Action<IRouteBuilder> configureRoutes = null
            )
        {
            app.UseMvc(routeBuilder =>
            {
                configureRoutes?.Invoke(routeBuilder);

                routeBuilder.MapRoute(
                   name: "default",
                   template: "{controller=Home}/{action=Index}/{id?}");

                var register = routeBuilder.ServiceProvider.GetRequiredService<IPluginRouterRegister>();
                register.Regist(routeBuilder);
            });

            return app;
        }
    }
}
