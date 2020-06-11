using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JDNetCore.ApiSite.Middleware;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace JDNetCore.ApiSite
{
    /// <summary>
    /// 账户和Token相关
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        /// <summary>
        /// 获取测试Token admin / Ww123123
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public dynamic GetTestToken()
        {
            return IdentityExt.GetToken();
        }

        /// <summary>
        /// 获取用户Claims
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public dynamic GetClaims()
        {
            return (from c in User.Claims select new { c.Type, c.Value });
        }

        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="id">用户米/电话/邮箱</param>
        /// <param name="pwd">密码</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public dynamic SignIn(string id,string pwd)
        {
            return IdentityExt.GetToken(id, pwd);
        }
    }
}