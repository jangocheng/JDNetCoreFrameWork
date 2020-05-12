// Copyright 
// CLR版本:            4.0.30319.42000
// 机器名称:           ANDY-PC
// 命名空间/文件名:    JDNetCore.ApiSite.Middleware/MetricsExt 
// 创建人:             研小艾   
// 创建时间:           2020/4/25 1:41:16

using App.Metrics;
using JDNetCore.Common;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using App.Metrics.Extensions.Hosting;

namespace JDNetCore.ApiSite.Middleware
{
    public static class MetricsExt
    {
        public static void AddMetricsSetup(this IServiceCollection services)
        {
            if (!Appsettings.app<bool>("AppMetrics:IsOpen")) return;

            var metrics = AppMetrics.CreateDefaultBuilder().Configuration.Configure(options =>
            {
                options.AddAppTag(Appsettings.app<string>("AppMetrics:App"));
                options.AddEnvTag(Appsettings.app<string>("AppMetrics:Env"));
            }).Report.ToInfluxDb(options =>
            {
                options.InfluxDb.BaseUri = new Uri(Appsettings.app<string>("AppMetrics:ConnectionString"));
                options.InfluxDb.Database = Appsettings.app<string>("AppMetrics:DatabaseName");
                options.InfluxDb.UserName = Appsettings.app<string>("AppMetrics:UserName");
                options.InfluxDb.Password = Appsettings.app<string>("AppMetrics:Password");
                options.HttpPolicy.BackoffPeriod = TimeSpan.FromSeconds(30);
                options.HttpPolicy.FailuresBeforeBackoff = 5;
                options.HttpPolicy.Timeout = TimeSpan.FromSeconds(10);
                options.FlushInterval = TimeSpan.FromSeconds(5);
            }).Build();
            services.AddMetrics(metrics);
            services.AddMetricsReportingHostedService();
            services.AddMetricsTrackingMiddleware();
            services.AddMetricsEndpoints();
            services.AddMetricsTrackingMiddleware(options => options.IgnoredHttpStatusCodes = new[] { 404 });
        }
    }
}
