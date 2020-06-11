// Copyright 
// CLR版本:            4.0.30319.42000
// 机器名称:           ANDY-PC
// 命名空间/文件名:    JDNetCore.Service/AccountService 
// 创建人:             研小艾   
// 创建时间:           2020/5/6 14:10:28

using JDNetCore.Model.DTO;
using JDNetCore.Service.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JDNetCore.Service
{
    public class AccountService : IAccountService
    {
        public AccountResult SignIn(string userId, string passWord)
        {
            return TestSignIn(userId, passWord);
        }

        public async Task<AccountResult> SignInAsync(string userId, string passWord)
        {
            return TestSignIn(userId, passWord);
        }

        private static AccountResult TestSignIn(string userId, string passWord)
        {
            var accountResult = new AccountResult();
            if (userId == "admin" && passWord == "Ww123123")
            {
                accountResult.state = SSOState.success;
                accountResult.user = new SSOUser()
                {
                    birth = new DateTime(1991, 1, 17),
                    desc = "超级管理员",
                    id = long.MaxValue.ToString(),
                    nickname = "管理员",
                    sex = true,
                    tel = "18888888888",
                    username = "admin",
                };
            }
            else
            {
                accountResult.state = SSOState.faild;
            }
            return accountResult;
        }
    }
}
