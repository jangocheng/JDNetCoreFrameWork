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
                        o.AllowSynchronousIO = false;//true����ͬ�� IO, ���ô������ûή������, ����������������Ի�ȡInputStream. ��Ҫ����ѡ��
                        o.AddServerHeader = false;//����Response Server ��ʶ
                        //o.ListenUnixSocket 
                        //o.UseSystemd();
                    });
                });
    }
}
