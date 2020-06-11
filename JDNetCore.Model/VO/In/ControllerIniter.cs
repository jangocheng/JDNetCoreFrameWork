// Copyright 
// CLR版本:            4.0.30319.42000
// 机器名称:           ANDY-PC
// 命名空间/文件名:    JDNetCore.Model.VO.In/ControllerIniter 
// 创建人:             研小艾   
// 创建时间:           2020/5/13 11:53:42

using System;
using System.Collections.Generic;
using System.Text;

namespace JDNetCore.Model.VO.In
{
    public class ControllerIniter
    {
        /// <summary>
        /// 表名
        /// </summary>
        public string tablename { set; get; }
        /// <summary>
        /// 备注
        /// </summary>
        public string remark { set; get; }
        /// <summary>
        /// 是否从服务层开始注入
        /// </summary>
        public bool fromService { set; get; } 
        /// <summary>
        /// 域, 默认api
        /// </summary>
        public string area { set; get; }
        /// <summary>
        /// Restful是否异步
        /// </summary>
        public bool? restfulAsync { set; get; }
    }
}
