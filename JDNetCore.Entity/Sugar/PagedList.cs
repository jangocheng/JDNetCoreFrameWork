// Copyright 
// CLR版本:            4.0.30319.42000
// 机器名称:           ANDY-PC
// 命名空间/文件名:    JDNetCore.Entity.Base/PagedList 
// 创建人:             研小艾   
// 创建时间:           2020/5/8 15:25:22

using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JDNetCore.Entity.Sugar
{
    /// <summary>
    /// 分页模型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial class PagedList<T> : List<T>, IPagedList<T>
    {
        /// <summary>
        /// sugar 分页器
        /// </summary>
        /// <param name="list"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="async"></param>
        public PagedList(ISugarQueryable<T> list, int pageIndex, int pageSize, bool async = false)
        {
            this.pageIndex = pageIndex;
            this.pageSize = pageSize;
            if (!async)
            {
                RefAsync<int> totalCount = 0;
                base.AddRange(list.ToPageListAsync(pageIndex, pageSize, totalCount).Result);
                this.totalCount = totalCount.Value;
            }
            else
            {
                RefAsync<int> totalCount = 0;
                list.ToPageListAsync(pageIndex, pageSize, totalCount).ContinueWith(o =>
                {
                    base.AddRange(o.Result);
                    this.totalCount = totalCount.Value;
                });
            }
        }

        /// <summary>
        /// 分页组装
        /// </summary>
        /// <param name="list"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        public PagedList(IEnumerable<T> list, int pageIndex, int pageSize, int totalCount)
        {
            this.pageIndex = pageIndex;
            this.pageSize = pageSize;
            this.totalCount = totalCount;
            base.AddRange(list);
        }

        public int pageSize { get;private set; }
        public int pageIndex { get;private set; }
        public int totalCount { get;private set; }

        public bool hasNext => this.pageIndex * this.pageSize < this.totalCount;

        public bool hasPrev => this.pageIndex > 1;

        public int totalPage => this.totalCount / this.pageSize + (this.totalCount % this.pageSize > 0 ? 1 : 0);
    }

    public static class PagedListExt
    {
        public static IPagedList<T> ToPagedList<T>(this IEnumerable<T> list, int pageIndex, int pageSize, int totalCount)
        {
            return new PagedList<T>(list, pageIndex, pageSize, totalCount);
        }

        public static IPagedList<T> ToPagedList<T>(this ISugarQueryable<T> list, int pageIndex, int pageSize)
        {
            return new PagedList<T>(list, pageIndex, pageSize, false);
        }

        public static Task<IPagedList<T>> ToPagedListAsync<T>(this ISugarQueryable<T> list, int pageIndex, int pageSize)
        {
            return Task.FromResult((IPagedList<T>)new PagedList<T>(list, pageIndex, pageSize, true));
        }
    }
}
