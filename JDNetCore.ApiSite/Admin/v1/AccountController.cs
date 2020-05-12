using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JDNetCore.ApiSite.Middleware;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace JDNetCore.ApiSite.Admin.v1
{
    [AllowAnonymous]
    public class AccountController : AreaAdminController
    {
        /// <summary>
        /// 获取测试Token admin / Ww123123
        /// </summary>
        /// <returns></returns>
        public dynamic GetTestToken()
        {
            return IdentityExt.GetToken();
        }

        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="id">用户米/电话/邮箱</param>
        /// <param name="pwd">密码</param>
        /// <returns></returns>
        [HttpPost]
        public dynamic SignIn(string id,string pwd)
        {
            return IdentityExt.GetToken(id, pwd);
        }
    }
}