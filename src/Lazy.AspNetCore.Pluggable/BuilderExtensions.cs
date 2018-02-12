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
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazy.AspNetCore.Pluggable
{
    public static class BuilderExtensions
    {
        /// <summary>
        /// 增加mvc插件化
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

            if (pluginSourceLocation.StartsWith("/"))
            {
                throw new KernelException($"{nameof(pluginSourceLocation)} is not relative path, should start with '/'.");
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

                var options = scope
                    .ServiceProvider
                    .GetRequiredService<Lazy.Kernel.Module.IModuleOptionProvider<PluggableOptions>>()
                    .GetConfiguredOptions();

                var pluginManager = scope.ServiceProvider.GetRequiredService<IPluginManager>();

                pluginManager.LoadAllPlugin(Server.MapPath(options.PluginSourceLocation));

                if (options.PluginSourceLocation.IsNullOrWhiteSpace())
                    throw new KernelException("mvc pluggable option: PluginSourceLocation is null.");

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
            var pluginManager = app.ApplicationServices.GetRequiredService<IPluginManager>();
            var moduleMamager = app.ApplicationServices.GetRequiredService<IModuleManager>();

            app.UseMiddleware<PluginStatusMiddleware>();
            app.UseMvc(routes =>
            {
                configureRoutes?.Invoke(routes);

                routes.MapRoute(
                   name: "default",
                   template: "{controller=Home}/{action=Index}/{id?}");

                pluginManager.Plugins.ForEach(r =>
                {
                    routes.MapRoute(r.Id, r.Id + "/{controller=Home}/{action=Index}/{id?}", null, null, new
                    {
                        area = r.Id
                    });
                });

                moduleMamager.ModuleResolvedResult.ParallelResult.Values.ForEach_(r =>
                {
                    var module = r.Instance as PluginModule;
                    module?.MapRoute(routes);
                });
            });

            return app;
        }
    }
}
