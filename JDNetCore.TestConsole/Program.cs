﻿using JDNetCore.Entity;
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
            //Test1();
        }

        private static void Test1()
        {
            do
            {
                //压测雪花
                ConcurrentDictionary<string, bool> temp = new ConcurrentDictionary<string, bool>();
                Console.WriteLine("请输入产生的数量");
                long.TryParse(Console.ReadLine(), out long loop);
                if (loop == 0) break;
                Parallel.For(0, loop, (i) =>
                {
                    if (!temp.TryAdd(new Test().id, false)) Console.WriteLine("插入失败");
                });
                Console.WriteLine("ConcurrentDictionary 插入" + loop + "次成功");
            }
            while (true);
        }

        private static void Test2()
        {
        }
    }
}
