using JDNetCore.Entity.Base;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace JDNetCore.Entity
{
    public class program_user:SugarEntityBase
    {
        /// <summary>
        /// 用户表 ; 
        /// </summary>
        public program_user()
        {
        }

        /// <summary>
        /// 用户名
        /// </summary>   
        [SugarColumn(IsNullable = true)]
        public string realname { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string nickname { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        public string account { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string phone { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string password { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string headimage { get; set; }

        /// <summary>
        /// 所属部门
        /// </summary>
        public long department_id { get; set; }
    }
}
