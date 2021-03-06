## Lazy ##
> `Lazy`是一个`.net core` 的集成框架,类似Abp,但比Abp精简,目前几乎没有引用任何非官方库,从日志、缓存、依赖注入等等，都尽量使用官方库，目前仅完成以下功能：

 - 模块管理：

> Lazy的模块管理比Abp精简了太多,Lazy会自动解析模块间的依赖,不用去写Abp中的DependsOn,最简单的一个模块定义如下:
```c#
class Module : LazyModule {}
```

 - 依赖注入：

> 程序集中只要定义了模块,lazy会按照约定自动解析并注册,不用像abp中死板的复制粘贴:`IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());`

 - `Asp.net core mvc`插件化:

> 类似orchard与nopCommerce ,将mvc中的各个模块分离,实现插件化.

## Sample ##

> 设置Lazy.WebSample为启动项目,可以在Startup.cs看到启动配置代码:
```C#           
services
    //增加asp.net core插件化
    .AddLazyAspNetCoreMvcPluggable("/Sample.Plugins")
    //增加lazy框架服务
    .AddLazy<WebModule>(r =>
    {
    });
```
```C#
//使用Lazy
app.ApplicationServices.UseLazy();
//使用asp.net core插件化
app.UseLazyAspNetCoreMvcPluggable(r =>
{
});
```
- 访问user插件,输入地址http://localhost:52259/user/user 无法访问404,因为user已禁用
- 访问task插件,输入地址http://localhost:52259/task/task 得到页面内容this is Task Index.

    
     

                

