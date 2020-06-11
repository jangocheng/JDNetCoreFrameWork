// Copyright 
// CLR版本:            4.0.30319.42000
// 机器名称:           ANDY-PC
// 命名空间/文件名:    JDNetCore.Model.DTO/ModelValidException 
// 创建人:             研小艾   
// 创建时间:           2020/5/29 21:06:23

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace JDNetCore.Model.DTO
{
    /// <summary>
    /// 模型验证异常
    /// </summary>
    public class FriendlyException<T>: FriendlyException
    {
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="message">消息</param>
        public FriendlyException(Expression<Func<T, object>> field,string message):base(field.Body.ToString().Split('.')[1], message)
        {
            
        }
    }
    /// <summary>
    /// 模型验证异常 上级
    /// </summary>
    public abstract class FriendlyException : Exception
    {
        public FriendlyException(string field, string message)
        {
            this.field = field;
            this.message = message;
        }

        public string field {private set; get; }

        public string message { private set; get; }

    }
}
