// Copyright 
// CLR版本:            4.0.30319.42000
// 机器名称:           ANDY-PC
// 命名空间/文件名:    JDNetCore.ApiSite.Aop/AopServiceExtension 
// 创建人:             研小艾   
// 创建时间:           2020/5/6 15:04:41

using Autofac;
using Autofac.Extras.DynamicProxy;
using JDNetCore.Common;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace JDNetCore.ApiSite.Aop
{
    public static class AopServiceExtension
    {
        /// <summary>
        /// 注册aop服务拦截器
        /// 同时注册了各业务层接口与实现
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="serviceAssemblyName">业务层程序集名称</param>
        public static void AddAopService(this ContainerBuilder builder)//, IConfiguration configuration
        {
            //注册拦截器，同步异步都要
            builder.RegisterType<LogInterceptor>().AsSelf();
            builder.RegisterType<LogInterceptorAsync>().AsSelf();

            //注册业务层，同时对业务层的方法进行拦截
            builder.RegisterAssemblyTypes(Assembly.Load(Appsettings.app<string>("IOC:ServiceAssembly")))
                .AsImplementedInterfaces().InstancePerLifetimeScope()
                .EnableInterfaceInterceptors()//引用Autofac.Extras.DynamicProxy;
                .InterceptedBy(new Type[] { typeof(LogInterceptor) })//这里只有同步的，因为异步方法拦截器还是先走同步拦截器 
                ;
            //可以从配置加载Repository类库的名称

            builder.RegisterAssemblyTypes(Assembly.Load(Appsettings.app<string>("IOC:RepositoryAssembly")))
                   .AsImplementedInterfaces()
                   .InstancePerDependency();

            //业务层注册拦截器也可以使用[Intercept(typeof(LogInterceptor))]加在类上，但是上面的方法比较好，没有侵入性
        }
    }
}
