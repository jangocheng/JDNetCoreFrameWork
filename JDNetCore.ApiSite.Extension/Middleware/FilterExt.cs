// Copyright 
// CLR版本:            4.0.30319.42000
// 机器名称:           ANDY-PC
// 命名空间/文件名:    JDNetCore.ApiSite.Middleware/FilterExt 
// 创建人:             研小艾   
// 创建时间:           2020/5/29 21:13:10

using JDNetCore.ApiSite.Filter;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace JDNetCore.ApiSite.Middleware
{
    /// <summary>
    /// 过滤器拓展
    /// </summary>
    public static class FilterExt
    {
        /// <summary>
        /// 添加过滤器
        /// </summary>
        /// <param name="services"></param>
        public static void AddFilterSetup(this IServiceCollection services)
        {
            services.AddControllers(o=> {
                o.Filters.Add<OutputFilter>();
                o.Filters.Add<ExceptionFilter>();
            });
        }
    }
}
