using JDNetCore.Entity;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JDNetCore.TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            do
            {
                //压测雪花
                ConcurrentDictionary<long, bool> temp = new ConcurrentDictionary<long, bool>();
                Console.WriteLine("请输入产生的数量");
                long.TryParse(Console.ReadLine(), out long loop);
                if (loop == 0) break;
                Parallel.For(0, loop, (i) =>
                {
                    if (!temp.TryAdd(new program_user().id, false)) Console.WriteLine("插入失败");
                   
                });
                Console.WriteLine("ConcurrentDictionary 插入" + loop + "次成功");
            }
            while (true);
        }
    }
}
