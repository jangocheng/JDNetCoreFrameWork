// Copyright 
// CLR版本:            4.0.30319.42000
// 机器名称:           ANDY-PC
// 命名空间/文件名:    JDNetCore.ApiSite.Middleware/HangfireExt 
// 创建人:             研小艾   
// 创建时间:           2020/5/14 23:03:47

using Hangfire;
using Hangfire.Annotations;
using Hangfire.Console;
using Hangfire.Dashboard;
using Hangfire.HttpJob;
using JDNetCore.Common;
using JDNetCore.Common.Interface;
using JDNetCore.Model.DTO;
using JDNetCore.Service.Interface;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TimeZoneConverter;

namespace JDNetCore.ApiSite.Middleware
{
    /// <summary>
    /// hangfire 开启
    /// </summary>
    public static class HangfireExt
    {
        private static JobStorage _storage = null;
        private static Action<IGlobalConfiguration> _useTag = null;
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="services"></param>
        /// <param name="storage">数据源</param>
        public static IServiceCollection AddHangfireSetup(this IServiceCollection services, JobStorage storage, Action<IGlobalConfiguration> useTag)
        {
            _storage = storage;

            _useTag = useTag;

            services.AddHangfire(Configuration);

            return services;
        }

        private static void Configuration(IGlobalConfiguration hangfire)
        {

            hangfire
                .UseStorage(_storage)
                .UseConsole(new ConsoleOptions()
                {
                    BackgroundColor = "#000079"
                })
                .UseHangfireHttpJob(new HangfireHttpJobOptions
                {
                    
                    MailOption = Appsettings.app<MailOption>("Hangfire:MailOption"),
                    DefaultRecurringQueueName = Appsettings.app<string>("Hangfire:DefaultRecurringQueueName"),
                    DefaultBackGroundJobQueueName = "DEFAULT",
                    RecurringJobTimeZone = TZConvert.GetTimeZoneInfo("Asia/Shanghai"), //这里指定了添加周期性job时的时区
                    // RecurringJobTimeZone = TimeZoneInfo.Local
                    // CheckHttpResponseStatusCode = code => (int)code < 400   //===》(default)
                });
            _useTag(hangfire);
        }

        /// <summary>
        /// 使用
        /// </summary>
        /// <param name="app"></param>
        /// <param name="accountService">用户状态接口</param>
        /// <param name="cache">用户状态缓存</param>
        public static IApplicationBuilder UseHangfirePlugin(this IApplicationBuilder app, IAccountService accountService, ICache cache)
        {
            //app.UseMiddleware<HangfireJsBugFilter>();
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("zh-CN");

            var queues = Appsettings.app<string[]>("Hangfire:Queues");
            app.UseHangfireServer(new BackgroundJobServerOptions
            {
                ServerTimeout = TimeSpan.FromMinutes(4),
                SchedulePollingInterval = TimeSpan.FromSeconds(15), //秒级任务需要配置短点，一般任务可以配置默认时间，默认15秒
                ShutdownTimeout = TimeSpan.FromMinutes(30), //超时时间
                Queues = queues, //队列
                WorkerCount = Math.Max(System.Environment.ProcessorCount, 40) //工作线程数，当前允许的最大线程，默认20

            });

            //var hangfireStartUpPath = JsonConfig.GetSection("HangfireStartUpPath").Get<string>();
            var hangfireStartUpPath = Appsettings.app<string>("Hangfire:StartUpPath");
            var dashbordConfig = new DashboardOptions
            {
                AppPath = "#",
                DisplayStorageConnectionString = false,//是否展示链接字符串, 其实可以按环境是否测试判断
                IsReadOnlyFunc = Context => false,
                Authorization = new[] { new MyDashboardAuthorizationFilter(accountService, cache) }
            };
            app.UseHangfireDashboard(hangfireStartUpPath, dashbordConfig);
            return app;
            
        }
    }

    /// <summary>
    /// hangfire 登陆验证
    /// </summary>
    public class MyDashboardAuthorizationFilter : IDashboardAuthorizationFilter
    {
        private readonly IAccountService accountService;
        private readonly ICache cache;
        public MyDashboardAuthorizationFilter(IAccountService _accountService, ICache _cache)
        {
            accountService = _accountService;
            cache = _cache;
        }

        public bool Authorize([NotNull] DashboardContext context)
        {
            var httpContext = context.GetHttpContext();

            var header = httpContext.Request.Headers["Authorization"];

            if (string.IsNullOrWhiteSpace(header))
            {
                SetChallengeResponse(httpContext);
                return false;
            }

            var authValues = System.Net.Http.Headers.AuthenticationHeaderValue.Parse(header);

            if (!"Basic".Equals(authValues.Scheme, StringComparison.InvariantCultureIgnoreCase))
            {
                SetChallengeResponse(httpContext);
                return false;
            }

            var parameter = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(authValues.Parameter));
            var parts = parameter.Split(':');

            if (parts.Length < 2)
            {
                SetChallengeResponse(httpContext);
                return false;
            }

            var username = parts[0];
            var password = parts[1];

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                SetChallengeResponse(httpContext);
                return false;
            }
            //可以在此处注入
            AccountResult accountResult = new AccountResult();
            var ssouser = cache.Get<SSOUser>(username + "|" + password);
            if (ssouser != null)
            {
                return true;
            }
            accountResult = accountService.SignIn(username, password);
            if (accountResult.state == Model.DTO.SSOState.success)
            {
                return true;
            }
            SetChallengeResponse(httpContext);
            return false;
        }

        private void SetChallengeResponse(HttpContext httpContext)
        {
            httpContext.Response.StatusCode = 401;
            httpContext.Response.Headers.Append("WWW-Authenticate", "Basic realm=\"Hangfire Dashboard\"");
            httpContext.Response.WriteAsync("Authentication is required.");
        }
    }

    public class HangfireJsBugFilter
    {
        private readonly RequestDelegate _next;
        public HangfireJsBugFilter(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.Value.StartsWith("/hangfire/js"))
            {
                using (var ms = new MemoryStream())
                {
                    var orgBodyStream = context.Response.Body;
                    context.Response.Body = ms;
                    context.Response.ContentType = "text/javascript;charset=utf-8-sig";
                    await _next(context);
                    var encode = Encoding.UTF8;
                    var bom = encode.GetString(encode.GetPreamble()); 
                    using (var sr = new StreamReader(ms))
                    {
                        ms.Seek(0, SeekOrigin.Begin);
                        //得到Action的返回值
                        var responseJsonResult = sr.ReadToEnd();
                        ms.Seek(0, SeekOrigin.Begin);
                        context.Response.Body = orgBodyStream;
                        //显示修改后的数据 
                        //Regex.Replace(responseJsonResult, "/[\x00-\x1F\x80-\xFF]/", "")

                        responseJsonResult = responseJsonResult.Replace(bom, @"
");

                        await context.Response.WriteAsync(responseJsonResult, new UTF8Encoding(false));

                    }
                }
            }
            else
            {
                await _next(context);
            }


        }
    }
}
