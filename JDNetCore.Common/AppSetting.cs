using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace JDNetCore.Common
{
    /// <summary>
    /// appsettings.json操作类
    /// </summary>
    public class Appsettings
    {
        static IConfiguration Configuration { get; set; }
        static string contentPath { get; set; }

        static string evn;

        static bool loadedEvn;

        static List<string> files { set; get; }

        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="BeforeConfigurationBuild">配置builder之前的委托</param>
        public Appsettings(Action<IConfigurationBuilder> BeforeConfigurationBuild = null)
        {
            evn = null;
            string path = "appsettings.json";
            fromEvn:
            if (!string.IsNullOrWhiteSpace(evn))
            {
                path = $"appsettings.{evn}.json";
            }
            //如果你把配置文件 是 根据环境变量来分开了，可以这样写
            //Path = $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json";
            var builder = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())//AppContext.BaseDirectory
               .AddJsonFile(path: path, optional: true, reloadOnChange: true);
            if (files != null)
            {
                foreach (var item in files)
                {
                    builder.AddJsonFile(item + "." + evn + ".json");
                }
            }
            if(loadedEvn)
                BeforeConfigurationBuild?.Invoke(builder);
            Configuration = builder.Build();//这样的话，可以直接读目录里的json文件，而不是 bin 文件夹下的，所以不用修改复制属性



            if (!loadedEvn)
            {
                if (app<string>("Environment") == null) return;
                files = app<List<string>>("PluginsJson");
                goto fromEvn;
            }
            //测试
            //var a = app<string>("Environment");
            //var b = app<string>("End");
            
        }

        /// <summary>
        /// 泛型方法, 直接调用底层GetSection.Get<T> 谜之高级,各种类型都能识别
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xmlPath"></param>
        /// <returns></returns>

        public static T app<T>(string xmlPath)
        {
            if (xmlPath != "Environment" || evn==null)
            {
                var result = Configuration.GetSection(xmlPath).Get<T>(); 
                if (xmlPath == "Environment")
                {
                    evn = result?.ToString();
                    if (evn != null) loadedEvn = true;
                }
                return result;
            }
            return (T)(object)evn;
        }
    }
}
