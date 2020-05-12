// Copyright 
// CLR版本:            4.0.30319.42000
// 机器名称:           ANDY-PC
// 命名空间/文件名:    JDNetCore.Model.DTO/AccountResult 
// 创建人:             研小艾   
// 创建时间:           2020/5/6 13:45:31

using System;
using System.Collections.Generic;
using System.Text;

namespace JDNetCore.Model.DTO
{
    public class AccountResult
    {
        public SSOState state { set; get; }

        public SSOUser user { set; get; }

        public string msg { set; get; }
    }

    public enum SSOState
    {
        success = 0,
        cachesuccess = 1,
        faild = 2,
    }
}
