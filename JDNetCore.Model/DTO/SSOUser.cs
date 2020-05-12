// Copyright 
// CLR版本:            4.0.30319.42000
// 机器名称:           ANDY-PC
// 命名空间/文件名:    JDNetCore.Model.DTO/SSOUser 
// 创建人:             研小艾   
// 创建时间:           2020/5/6 13:46:27

using System;
using System.Collections.Generic;
using System.Text;

namespace JDNetCore.Model.DTO
{
    [Serializable]
    public class SSOUser
    {
        public string id { set; get; }

        public string username { set; get; }

        public string nickname { set; get; }

        public bool sex { set; get; }
        
        public string headimgurl { set; get; }

        public string tel { set; get; }

        public string desc { set; get; }

        public DateTime birth { set; get; }
    }
}
