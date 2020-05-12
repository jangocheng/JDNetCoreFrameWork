// Copyright 
// CLR版本:            4.0.30319.42000
// 机器名称:           ANDY-PC
// 命名空间/文件名:    JDNetCore.Model.VO.Out/Token 
// 创建人:             研小艾   
// 创建时间:           2020/4/23 15:52:52

using System;
using System.Collections.Generic;
using System.Text;

namespace JDNetCore.Model.DTO
{
    /// <summary>
    /// jwt接口返回的 token
    /// </summary>

    public class Token
    {
        /// <summary>
        /// token
        /// </summary>
        public string access_token { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public string token_type { get; set; }
        /// <summary>
        /// 有效期
        /// </summary>
        public int expires_in { get; set; }
        /// <summary>
        /// 刷新
        /// </summary>
        public string refresh_token { get; set; }
        /// <summary>
        /// 刷新有效期
        /// </summary>
        public int refresh_token_expires_in { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string scope { set; get; }
    }

}
