using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JDNetCore.ApiSite.Extension;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using JDNetCore.ApiSite.Middleware;
using JDNetCore.Common;
using IdentityServer4.Configuration;
using IdentityServer4.AccessTokenValidation;
using Swashbuckle.AspNetCore.SwaggerUI;
using JDNetCore.Service.Interface;
using JDNetCore.Common.Interface;
using Autofac;
using JDNetCore.ApiSite.Aop;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Authorization;
using JDNetCore.Entity.Sugar;
using Hangfire;
//using Hangfire.Oracle.Core;
using Hangfire.Storage;
using Hangfire.Dashboard;
using JDNetCore.Common.Crypto;
using JDNetCore.Service;
using Hangfire.HttpJob;
using Hangfire.Console;
using TimeZoneConverter;
using System.Data;
using JDNetCore.ApiSite.Filter;
using StackExchange.Redis;
using Hangfire.Redis;
using Hangfire.Tags.Redis;
using Hangfire.SqlServer;
using Hangfire.Tags.SqlServer;
//using Ocelot.DependencyInjection;
//using Ocelot.Middleware;
//using Ocelot.Values;
//using Ocelot.Provider.Consul;

namespace JDNetCore.ApiSite
{
    /// <summary>
    /// 起点
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="env"></param>
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {

        }




        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="hangfire"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            //添加配置单例注入 不知道有毛用
            services.AddSingleton(new Appsettings((builder) => {
                //builder.AddOcelot(null);
                //builder.AddJsonFile("ocelot.json");
            }));
            //文档处理
            services.AddSingleton(typeof(IApiDescriptionGroupCollectionProvider), typeof(SwaggerApiDescriptionGroupCollectionProvider));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<ICache, MemoryCacheExt>();

            services.AddSqlsugarSetup();



            var useRedis = Appsettings.app<bool>("Hangfire:UseRedis");

            if (useRedis)
            {
                var con = ConnectionMultiplexer.Connect(new ConfigurationOptions
                {
                    SyncTimeout = (int)TimeSpan.FromMinutes(10).TotalMilliseconds,
                    EndPoints = { Appsettings.app<string>("Hangfire:EndPoint") },
                    ConnectTimeout = (int)TimeSpan.FromMinutes(10).TotalMilliseconds,
                    AsyncTimeout = (int)TimeSpan.FromMinutes(10).TotalMilliseconds,
                    HighPrioritySocketThreads = true,
                });
                services.AddHangfireSetup(new RedisStorage(con), (hangfire) =>
                {
                    hangfire.UseTagsWithRedis(con);
                });
            }
            else
            {
                var hangfireStorageType = Appsettings.app<int>("Hangfire:DBType");

                var hangfireConnection = Appsettings.app<string>("Hangfire:Connection");

                //判断

                services.AddHangfireSetup(new SqlServerStorage(hangfireConnection), (hangfire) =>
                {
                    hangfire.UseTagsWithSql();
                });
            }

            services.AddScoped<DBContext>();

            //添加过滤器
            services.AddFilterSetup();


            services.AddIdentityServerSetup();
            //services.AddMvc();
            //负载均衡Api网关层 

            //services.AddOcelot().AddConsul().AddConfigStoredInConsul();

            //services.AddOcelot(new ConfigurationBuilder()
            //                    .AddJsonFile($"ocelot.json", optional: true, reloadOnChange: true)
            //                    .Build()).AddConsul();

            //services.AddOcelot(new ConfigurationBuilder()
            //                    .AddJsonFile($"ocelot.{Appsettings.app<string>("Environment")}.json", optional: true, reloadOnChange: true)
            //                    .Build()).AddConsul();//.AddConfigStoredInConsul();

            //监控
            services.AddMetricsSetup();

            //Swagger 安装
            services.AddSwaggerSetup();


        }

        // Application_Start Part 6 ConfigureContainer
        // 注意在CreateDefaultBuilder中，添加Autofac服务工厂
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        public void ConfigureContainer(ContainerBuilder builder)
        {

            builder.AddAopService();

            return;
        }



        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="provider"></param>
        /// <param name="applicationLeftTime"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider provider, IHostApplicationLifetime applicationLeftTime)
        {
            applicationLeftTime.ApplicationStarted.Register(() =>
            {
                //缓存初始化 之类的操作
                Console.WriteLine("ApplicationStarted");
            });

            applicationLeftTime.ApplicationStopped.Register(() => 
            {
                //资源回收之类的操作
                Console.WriteLine("ApplicationStopped");
            });

            applicationLeftTime.ApplicationStopping.Register(() => 
            {
                //日志buffer推送 之类的操作
                Console.WriteLine("ApplicationStopping");
            });

            //def
            if (env.IsDevelopment())
            {
                //def
                app.UseDeveloperExceptionPage();

                //app.UseCors("default");

                app.UseSwagger(c => {

                });

                app.UseSwaggerUI(c =>
                {
                    var ApiName = Appsettings.app<string>("Swagger:Title");
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{ApiName} {"v1"}");
                    c.RoutePrefix = string.Empty;
                    c.DocumentTitle = ApiName + "接口文档";
                });
            }

            app.UseHangfirePlugin((IAccountService)provider.GetService(typeof(IAccountService)),
                (ICache)provider.GetService(typeof(ICache)));

            app.UseIdentityServer();
            //def
            app.UseHttpsRedirection();
            //def
            app.UseRouting();
            //def

            //app.UseCors("LimitRequests");

            app.UseAuthorization();

            app.UseAuthentication();



            if (Appsettings.app<bool>("AppMetrics:IsOpen"))
            {
                app.UseMetricsAllMiddleware();
                app.UseMetricsAllEndpoints();
            }

            //app.UseOcelot().Wait();

            //def
            app.UseEndpoints(endpoints =>
            {
                //def
                endpoints.MapControllers();
            });
        }
    }
}
