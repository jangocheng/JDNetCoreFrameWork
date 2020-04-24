using JDNetCore.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace JDNetCore.Entity.Sugar
{
    public class BaseDBConfig
    {
        /* 之前的单库操作已经删除，如果想要之前的代码，可以查看我的GitHub的历史记录
         * 目前是多库操作，默认加载的是appsettings.json设置为true的第一个db连接。
         */
        private static List<MutiDBOperate> _MutiConnectionString;
        public static List<MutiDBOperate> MutiConnectionString
        {
            get
            {
                return _MutiConnectionString = _MutiConnectionString ?? MutiInitConn();
            }
        }

        public static List<MutiDBOperate> MutiInitConn()
        {

            List<MutiDBOperate> listdatabase = new List<MutiDBOperate>();
            List<MutiDBOperate> listdatabaseSimpleDB = new List<MutiDBOperate>();

            var dbs = Appsettings.app<List<MutiDBOperate>>("DBS");
            if (dbs != null)
                listdatabase.AddRange(dbs.Where(d => d.Enabled));
            // 单库，只保留一个
            if (!Appsettings.app<bool>( "MutiDBEnabled" ))
            {
                if (listdatabase.Count == 1)
                {
                    return listdatabase;
                }
                else
                {
                    var dbFirst = listdatabase.FirstOrDefault(d => d.ConnId == Appsettings.app<string>("MainDB"));
                    if (dbFirst == null)
                    {
                        dbFirst = listdatabase.FirstOrDefault();
                    }
                    listdatabaseSimpleDB.Add(dbFirst);
                    return listdatabaseSimpleDB;
                }
            }

            return listdatabase;
            
        }

    }
}
