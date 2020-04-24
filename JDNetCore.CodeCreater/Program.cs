using JDNetCore.Common;
using JDNetCore.Common.Crypto;
using JDNetCore.Entity;
using JDNetCore.Entity.Sugar;
using SqlSugar;
using System;
using System.Diagnostics;
using System.Reflection;

namespace JDNetCore.CodeCreater
{
    class Program
    {
        static void Main(string[] args)
        {
            new Appsettings();
            DBContext context = new DBContext();
            Console.WriteLine($"数据库类型【{DBContext.DbType.ToString()}】");
            Console.WriteLine($"连接字符串：{context.Db.Ado.Connection.ConnectionString}");
            Console.WriteLine($"是否更新数据库(Y/N)?");
            var str = Console.ReadLine();
            if (!str.ToLower().Contains("y"))
            {
                return;
            }
            bool isInitialize = IsInitialize(context);
            if (isInitialize)
            {
                Console.WriteLine($"数据库【{context.Db.Ado.Connection.Database}】尚未创建，是否创建数据库(Y/N)?");
                var str2 = Console.ReadLine();
                if (!str2.ToLower().Contains("y"))
                {
                    return;
                }
                context.Db.DbMaintenance.CreateDatabase();
                Console.WriteLine("数据库创建成功！");
            }

            Console.WriteLine("开始更新表字段...");

            // 反射找到对应命名空间的下的表来创建数据库实体
            var types = Assembly.Load("JDNetCore.Entity").GetTypes();
            bool isAllSuccess = true;
            foreach (var item in types)
            {
                if (item.Namespace == "JDNetCore.Entity")
                {
                    Console.Write($"正在更新表：{item.Name }   ==>   ");
                    try
                    {
                        context.CreateTableByEntity(false, item);
                        //context.CreateTableByEntity(false, typeof(common_enums));
                        Console.WriteLine($"更新成功");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"更新失败：{ex.Message}");
                        isAllSuccess = false;
                    }
                }
            }

            // 后期单独处理某些表
            // myContext.Db.CodeFirst.InitTables(typeof(sysUserInfo));

            if (isInitialize)
            {
                InitializeData(context);
                Console.WriteLine("基础数据初始化完成！");
            }
            Console.WriteLine("");
            if (!isAllSuccess)
            {
                Console.WriteLine("部分实体更新失败，请注意！！！");
            }
            else
            {
                Console.WriteLine("数据库更新完成，请按任意键退出！");
            }
            Console.ReadKey();
        }

        /// <summary>
        /// 是否是初始化
        /// </summary>
        /// <param name="context"></param>
        static bool IsInitialize(DBContext context)
        {
            if (DBContext.DbType == DbType.MySql)
            {
                string connectionString = context.Db.CurrentConnectionConfig.ConnectionString;
                string thisConnectionString = connectionString.Replace(context.Db.Ado.Connection.Database, "mysql");
                var newDb = new SqlSugarClient(new ConnectionConfig()
                {
                    DbType = DBContext.DbType,
                    IsAutoCloseConnection = true,
                    ConnectionString = thisConnectionString
                });
                var com = newDb.Ado.SqlQuerySingle<string>($"show databases like '{context.Db.Ado.Connection.Database}';");
                if (com == null)
                    return true;
            }
            else if (DBContext.DbType == DbType.SqlServer)
            {
                string connectionString = context.Db.CurrentConnectionConfig.ConnectionString;
                string thisConnectionString = connectionString.Replace(context.Db.Ado.Connection.Database, "master");
                var newDb = new SqlSugarClient(new ConnectionConfig()
                {
                    DbType = DBContext.DbType,
                    IsAutoCloseConnection = true,
                    ConnectionString = thisConnectionString
                });
                var com = /*DBContext.Context.Db*/newDb.Ado.SqlQuerySingle<int?>($"select 1 from sys.databases where name = '{context.Db.Ado.Connection.Database}';");
                if (com == null)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <param name="context"></param>
        static void InitializeData(DBContext context)
        {
            var user = new program_user()
            {
                account = "admin",
                headimage = "",
                realname = "系统管理员",
                password = MD5Helper.MD5Encrypt64("123123"),
                phone = "18888888888",
                state = 1,
            };
            context.Db.Insertable(user).ExecuteCommand();
        }
    }
}


