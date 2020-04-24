using Snowflake.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace JDNetCore.Entity.Base
{
    internal class SnowSeed
    {
        private static SnowSeed Singleton = new SnowSeed();
        private IdWorker snowFlake;
        SnowSeed()
        {
            //工作机器编号 
            var workerId = 0;
            //数据中心编号
            var datacenterId = 0;
            snowFlake = new IdWorker(workerId,datacenterId);
        }
        IdWorker getInstance()
        {
            return snowFlake;
        }

        internal static long NewID
        {
            get
            {
                return Singleton.getInstance().NextId();
            }
        }
    }




}
