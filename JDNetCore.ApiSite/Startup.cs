using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JDNetCore.ApiSite.Extension;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using JDNetCore.ApiSite.Middleware;
using JDNetCore.Common;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Authorization;
using IdentityServer4.Configuration;
using IdentityServer4.AccessTokenValidation;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Values;

namespace JDNetCore.ApiSite
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            //def
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //������õ���ע�� ��֪����ë��
            services.AddSingleton(new Appsettings());
            //�ĵ�����
            services.AddSingleton(typeof(IApiDescriptionGroupCollectionProvider), typeof(SwaggerApiDescriptionGroupCollectionProvider));
            //��ӹ�ȫ��Ȩ�޿���
            services.AddControllers(options=>options.Filters.Add(new AuthorizeFilter()));

            //services.AddMvc();
            //���ؾ���Api���ز� 
            services.AddOcelot(new ConfigurationBuilder()
                                .AddJsonFile($"Ocelot.{Appsettings.app<string>("Environment")}.json")
                                .Build());
            //Swagger ��װ
            services.AddSwaggerSetup();


            ////services.AddAuthorization();
            //var authenticationProviderKey = "OcelotKey";
            //var identityServerOptions = new IdentityServerOptions();
            //Configuration.Bind("IdentityServerOptions", identityServerOptions);
            //services.AddAuthentication(identityServerOptions.IdentityScheme)
            //    .AddIdentityServerAuthentication(authenticationProviderKey, options =>
            //    {
            //        options.RequireHttpsMetadata = false; //�Ƿ�����https
            //        options.Authority = $"http://{identityServerOptions.ServerIP}:{identityServerOptions.ServerPort}";//������Ȩ��֤�ĵ�ַ
            //        options.ApiName = identityServerOptions.ResourceName; //��Դ���ƣ�����֤������ע�����Դ�б������е�apiResourceһ��
            //        options.SupportedTokens = SupportedTokens.Both;
            //    }
            //    );

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //def
            if (env.IsDevelopment())
            {
                //def
                app.UseDeveloperExceptionPage();

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
            //def
            app.UseHttpsRedirection();
            //def
            app.UseRouting();
            //def
            app.UseAuthorization();

            app.UseOcelot().Wait();

            //def
            //app.UseEndpoints(endpoints =>
            //{
            //    //def
            //    endpoints.MapControllers();
            //});
        }
    }
}
