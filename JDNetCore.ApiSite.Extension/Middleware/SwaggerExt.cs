using JDNetCore.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JDNetCore.ApiSite.Middleware
{
    /// <summary>
    /// Swagger mw
    /// </summary>
    public static class SwaggerExt
    {
        /// <summary>
        /// Swagger 服务安装
        /// </summary>
        /// <param name="services"></param>
        public static void AddSwaggerSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            
            var basePath = AppContext.BaseDirectory;
            //var basePath2 = Microsoft.DotNet.PlatformAbstractions.ApplicationEnvironment.ApplicationBasePath;
            var ApiName = Appsettings.app<string>("Swagger:Title");

            services.AddSwaggerGen(c =>
            {
                var version = "v1";
                c.SwaggerDoc(version, new OpenApiInfo
                {
                    Version = version,
                    Title = $"{ApiName} 接口文档——Netcore 3.1",
                    Description = $"{ApiName} HTTP API " + version,
                    Contact = new OpenApiContact { Name = ApiName, Email = "zy_try@live.cn", Url = new Uri("https://www.baidu.com") },
                    //License = new OpenApiLicense { Name = ApiName + " 官方文档", Url = new Uri("https://www.baidu.com") }
                });
                c.OrderActionsBy(o => o.RelativePath);

                c.DocInclusionPredicate((docName, apiDesc) =>
                {
                    apiDesc.TryGetMethodInfo(out MethodInfo methodInfo);
                    var attrs = methodInfo.GetCustomAttributes(true).OfType<HttpMethodAttribute>().ToList();
                    return true;
                });

                try
                {
                    //就是这里
                    var xmlPath = Path.Combine(basePath, Appsettings.app<string>("Swagger:ApiDescXmlPath"));//这个就是刚刚配置的xml文件名
                    c.IncludeXmlComments(xmlPath, true);//默认的第二个参数是false，这个是controller的注释，记得修改

                    var xmlModelPath = Path.Combine(basePath, Appsettings.app<string>("Swagger:ModelDescXmlPath"));//这个就是Model层的xml文件名
                    c.IncludeXmlComments(xmlModelPath);
                }
                catch (Exception ex)
                {
                    //log.Error("Blog.Core.xml和Blog.Core.Model.xml 丢失，请检查并拷贝。\n" + ex.Message);
                }

                // 开启加权小锁
                c.OperationFilter<AddResponseHeadersFilter>();
                c.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();

                // 在header中添加token，传递到后台
                c.OperationFilter<SecurityRequirementsOperationFilter>();


                // 必须是 oauth2
                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Description = "JWT授权(数据将在请求头中进行传输) 直接在下框中输入Bearer {token}（注意两者之间是一个空格）\"",
                    Name = "Authorization",//jwt默认的参数名称
                    In = ParameterLocation.Header,//jwt默认存放Authorization信息的位置(请求头中)
                    Type = SecuritySchemeType.ApiKey
                });
            });
        }
    }

    /// <summary>
    /// 配合Swagger 的文档重写 让Action头上的[HttpGet] 见鬼去吧
    /// </summary>
    public class SwaggerApiDescriptionGroupCollectionProvider : IApiDescriptionGroupCollectionProvider
    {
        private readonly IActionDescriptorCollectionProvider _actionDescriptorCollectionProvider;
        private readonly IApiDescriptionProvider[] _apiDescriptionProviders;

        private ApiDescriptionGroupCollection _apiDescriptionGroups;

        /// <summary>
        /// Creates a new instance of <see cref="ApiDescriptionGroupCollectionProvider"/>.
        /// </summary>
        /// <param name="actionDescriptorCollectionProvider">
        /// The <see cref="IActionDescriptorCollectionProvider"/>.
        /// </param>
        /// <param name="apiDescriptionProviders">
        /// The <see cref="IEnumerable{IApiDescriptionProvider}"/>.
        /// </param>
        public SwaggerApiDescriptionGroupCollectionProvider(
            IActionDescriptorCollectionProvider actionDescriptorCollectionProvider,
            IEnumerable<IApiDescriptionProvider> apiDescriptionProviders)
        {
            _actionDescriptorCollectionProvider = actionDescriptorCollectionProvider;
            _apiDescriptionProviders = apiDescriptionProviders.OrderBy(item => item.Order).ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        public ApiDescriptionGroupCollection ApiDescriptionGroups
        {
            get
            {
                var actionDescriptors = _actionDescriptorCollectionProvider.ActionDescriptors;
                if (_apiDescriptionGroups == null || _apiDescriptionGroups.Version != actionDescriptors.Version)
                {
                    _apiDescriptionGroups = GetCollection(actionDescriptors);
                }

                return _apiDescriptionGroups;
            }
        }

        private ApiDescriptionGroupCollection GetCollection(ActionDescriptorCollection actionDescriptors)
        {
            var context = new ApiDescriptionProviderContext(actionDescriptors.Items);

            foreach (var provider in _apiDescriptionProviders)
            {
                provider.OnProvidersExecuting(context);
            }

            for (var i = _apiDescriptionProviders.Length - 1; i >= 0; i--)
            {
                _apiDescriptionProviders[i].OnProvidersExecuted(context);
            }

            Parallel.ForEach(context.Results, (apiDesc) => {
                var actionName = apiDesc.ActionDescriptor.RouteValues["action"]?.ToLower();
                if (actionName.StartsWith("get"))
                    apiDesc.HttpMethod = "GET";
                else if (actionName.StartsWith("post"))
                    apiDesc.HttpMethod = "POST";
                else if (actionName.StartsWith("delete"))
                    apiDesc.HttpMethod = "DELETE";
                else if (actionName.StartsWith("put"))
                    apiDesc.HttpMethod = "PUT";
                else if (actionName.StartsWith("patch"))
                    apiDesc.HttpMethod = "PATCH";

            });


            var groups = context.Results
                .GroupBy(d => d.GroupName)
                .Select(g => new ApiDescriptionGroup(g.Key, g.ToArray()))
                .ToArray();

            return new ApiDescriptionGroupCollection(groups, actionDescriptors.Version);
        }
    }
}
