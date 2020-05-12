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
//using Ocelot.DependencyInjection;
//using Ocelot.Middleware;
//using Ocelot.Values;
//using Ocelot.Provider.Consul;

namespace JDNetCore.ApiSite
{
    /// <summary>
    /// ���
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            //def
            Configuration = configuration;
            Environment = env;
        }

        /// <summary>
        /// ����
        /// </summary>
        public IConfiguration Configuration { get; }
        /// <summary>
        /// ����
        /// </summary>
        public IWebHostEnvironment Environment { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            //������õ���ע�� ��֪����ë��
            services.AddSingleton(new Appsettings((builder)=> {
                //builder.AddOcelot(null);
                //builder.AddJsonFile("ocelot.json");
            }));
            //�ĵ�����
            services.AddSingleton(typeof(IApiDescriptionGroupCollectionProvider), typeof(SwaggerApiDescriptionGroupCollectionProvider));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<ICache, MemoryCacheExt>();
            //��ӹ�ȫ��Ȩ�޿���
            services.AddControllers(options => options.Filters.Add(new AuthorizeFilter()));//

            services.AddIdentityServerSetup(); 

            //services.AddMvc();
            //���ؾ���Api���ز� 

            //services.AddOcelot().AddConsul().AddConfigStoredInConsul();

            //services.AddOcelot(new ConfigurationBuilder()
            //                    .AddJsonFile($"ocelot.json", optional: true, reloadOnChange: true)
            //                    .Build()).AddConsul();

            //services.AddOcelot(new ConfigurationBuilder()
            //                    .AddJsonFile($"ocelot.{Appsettings.app<string>("Environment")}.json", optional: true, reloadOnChange: true)
            //                    .Build()).AddConsul();//.AddConfigStoredInConsul();

            //���
            services.AddMetricsSetup();

            //Swagger ��װ
            services.AddSwaggerSetup();

            

        }

        // Application_Start Part 6 ConfigureContainer
        // ע����CreateDefaultBuilder�У����Autofac���񹤳�
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
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //def
            if (env.IsDevelopment())
            {
                //def
                app.UseDeveloperExceptionPage();

                //app.UseCors("default");

                app.UseSwagger(c=> { 
                    
                });

                app.UseSwaggerUI(c =>
                {
                    var ApiName = Appsettings.app<string>("Swagger:Title");
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{ApiName} {"v1"}");
                    c.RoutePrefix = string.Empty;
                    c.DocumentTitle = ApiName + "�ӿ��ĵ�";
                });
            }
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
