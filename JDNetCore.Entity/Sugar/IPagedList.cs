// Copyright 
// CLR版本:            4.0.30319.42000
// 机器名称:           ANDY-PC
// 命名空间/文件名:    JDNetCore.Entity.Sugar/IPagedList 
// 创建人:             研小艾   
// 创建时间:           2020/5/8 19:41:23

using System;
using System.Collections.Generic;
using System.Text;

namespace JDNetCore.Entity.Sugar
{
    public partial interface IPagedList<T> : IEnumerable<T>
    {
        int pageSize { get; }

        int pageIndex { get; }
        /// <summary>
        /// 总条数
        /// </summary>
        int totalCount { get; }
        /// <summary>
        /// 是否有下一页
        /// </summary>
        bool hasNext { get; }
        /// <summary>
        /// 是否有上一页
        /// </summary>
        bool hasPrev { get; }

        int totalPage { get; }
    }
}
