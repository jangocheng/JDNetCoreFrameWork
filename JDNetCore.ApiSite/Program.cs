using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace JDNetCore.ApiSite
{
    public class Program
    {
        /// <summary>
        /// Application_Start Part 1 Main
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// Application_Start Part 2 CreateHostBuilder
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.ConfigureKestrel(o => {
                        o.AllowSynchronousIO = false;//true启用同步 IO, 启用此项配置会降低性能, 但是在拦截器层可以获取InputStream. 需要谨慎选择
                        o.AddServerHeader = false;//隐藏Response Server 标识
                        //o.ListenUnixSocket 
                        //o.UseSystemd();
                    });
                });
    }
}
