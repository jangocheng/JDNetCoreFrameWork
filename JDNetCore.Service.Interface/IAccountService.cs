// Copyright 
// CLR版本:            4.0.30319.42000
// 机器名称:           ANDY-PC
// 命名空间/文件名:    JDNetCore.Service.Interface/IAccountService 
// 创建人:             研小艾   
// 创建时间:           2020/5/6 13:43:29

using JDNetCore.Model.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JDNetCore.Service.Interface
{
    public interface IAccountService
    {
        public Task<AccountResult> SignInAsync(string userId, string passWord);
    }
}
