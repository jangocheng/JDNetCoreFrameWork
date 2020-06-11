// Copyright 
// CLR版本:            4.0.30319.42000
// 机器名称:           ANDY-PC
// 命名空间/文件名:    JDNetCore.Repository.Interface/IBaseRepository 
// 创建人:             研小艾   
// 创建时间:           2020/5/8 18:13:48

using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace JDNetCore.Entity.Sugar
{
    public partial interface IBaseRepository<T> where T : SugarEntityBase
    {
        #region Ext
        string[] GetTables();
        #endregion

        #region 主键查找
        T Find(string id, params string[] tableNames);

        Task<T> FindAsync(string id, params string[] tableNames);

        IEnumerable<T> FindIn(string[] ids, params string[] tableNames);

        Task<IEnumerable<T>> FindInAsync(string[] ids, params string[] tableNames);
        #endregion

        #region 添加
        int Add(T entity, Expression<Func<T, object>> insertColumns = null, params string[] tableNames);

        Task<int> AddAsync(T entity, Expression<Func<T, object>> insertColumns = null, params string[] tableNames);

        int AddRange(IEnumerable<T> entities, Expression<Func<T, object>> insertColumns = null, params string[] tableNames);

        Task<int> AddRangeAsync(IEnumerable<T> entities, Expression<Func<T, object>> insertColumns = null, params string[] tableNames);
        #endregion

        #region 主键删除
        int Delete(string id, params string[] tableNames);

        Task<int> DeleteAsync(string id, params string[] tableNames);

        int DeleteIn(string[] ids, params string[] tableNames);

        Task<int> DeleteInAsync(string[] ids, params string[] tableNames);

        int DeleteWith(Expression<Func<T, bool>> whereExpression, params string[] tableNames);

        Task<int> DeleteWithAsync(Expression<Func<T, bool>> whereExpression, params string[] tableNames);

        #endregion

        #region 更新
        int Update(T entity, params string[] tableNames);

        Task<int> UpdateAsync(T entity, params string[] tableNames);

        int UpdateRange(IEnumerable<T> entities, params string[] tableNames);

        Task<int> UpdateRangeAsync(IEnumerable<T> entities, params string[] tableNames);

        //int UpdateWith(T entity, string whereCondition, params string[] tableNames);

        //Task<int> UpdateWithAsync(T entity, string whereCondition, params string[] tableNames);

        int UpdateWith(T entity, Expression<Func<T, bool>> whereExpression, params string[] tableNames);

        Task<int> UpdateWithAsync(T entity, Expression<Func<T, bool>> whereExpression, params string[] tableNames);

        int UpdateTable(Expression<Func<T, T>> columns, Expression<Func<T, bool>> expressionWhere, params string[] tableNames);

        Task<int> UpdateTableAsync(Expression<Func<T, T>> columns, Expression<Func<T, bool>> expressionWhere, params string[] tableNames);

        #endregion

        #region 查询
        IEnumerable<T> Query(params string[] tableNames);

        ISugarQueryable<T> ReQuery(params string[] tableNames);

        Task<IEnumerable<T>> QueryAsync(params string[] tableNames);



        IEnumerable<T> Query(string whereCondition, params string[] tableNames);

        ISugarQueryable<T> ReQuery(string whereCondition, params string[] tableNames);

        Task<IEnumerable<T>> QueryAsync(string whereCondition, params string[] tableNames);

        IEnumerable<T> Query(int topCount,  params string[] tableNames);

        ISugarQueryable<T> ReQuery(int topCount, params string[] tableNames);

        Task<IEnumerable<T>> QueryAsync(int topCount, params string[] tableNames);

        IEnumerable<T> Query(int topCount, string whereCondition, params string[] tableNames);

        ISugarQueryable<T> ReQuery(int topCount, string whereCondition, params string[] tableNames);

        Task<IEnumerable<T>> QueryAsync(int topCount, string whereCondition, params string[] tableNames);


        IEnumerable<T> Query(string whereCondition, string orderByFields, params string[] tableNames);

        ISugarQueryable<T> ReQuery(string whereCondition, string orderByFields, params string[] tableNames);

        Task<IEnumerable<T>> QueryAsync(string whereCondition, string orderByFields, params string[] tableNames);



        IEnumerable<T> Query(int topCount, string whereCondition,string orderByFields, params string[] tableNames);

        ISugarQueryable<T> ReQuery(int topCount, string whereCondition, string orderByFields, params string[] tableNames);

        Task<IEnumerable<T>> QueryAsync(int topCount, string whereCondition, string orderByFields, params string[] tableNames);



        IEnumerable<T> Query(Expression<Func<T, bool>> whereExpression, params string[] tableNames);

        ISugarQueryable<T> ReQuery(Expression<Func<T, bool>> whereExpression, params string[] tableNames);

        Task<IEnumerable<T>> QueryAsync(Expression<Func<T, bool>> whereExpression, params string[] tableNames);



        IEnumerable<T> Query(int topCount, Expression<Func<T, bool>> whereExpression, params string[] tableNames);

        ISugarQueryable<T> ReQuery(int topCount, Expression<Func<T, bool>> whereExpression, params string[] tableNames);

        Task<IEnumerable<T>> QueryAsync(int topCount, Expression<Func<T, bool>> whereExpression, params string[] tableNames);


        IEnumerable<T> Query(int topCount, Expression<Func<T, object>> orderByExpression, OrderByType type = OrderByType.Asc, params string[] tableNames);

        ISugarQueryable<T> ReQuery(int topCount, Expression<Func<T, object>> orderByExpression, OrderByType type = OrderByType.Asc, params string[] tableNames);

        Task<IEnumerable<T>> QueryAsync(int topCount, Expression<Func<T, object>> orderByExpression, OrderByType type = OrderByType.Asc, params string[] tableNames);


        IEnumerable<T> Query(int topCount, Expression<Func<T, bool>> whereExpression, Expression<Func<T, object>> orderByExpression, OrderByType type = OrderByType.Asc, params string[] tableNames);

        ISugarQueryable<T> ReQuery(int topCount, Expression<Func<T, bool>> whereExpression, Expression<Func<T, object>> orderByExpression, OrderByType type = OrderByType.Asc, params string[] tableNames);

        Task<IEnumerable<T>> QueryAsync(int topCount, Expression<Func<T, bool>> whereExpression, Expression<Func<T, object>> orderByExpression, OrderByType type = OrderByType.Asc, params string[] tableNames);



        IEnumerable<T> Query(Expression<Func<T, bool>> whereExpression, Expression<Func<T, object>> orderByExpression, OrderByType type = OrderByType.Asc, params string[] tableNames);

        ISugarQueryable<T> ReQuery(Expression<Func<T, bool>> whereExpression, Expression<Func<T, object>> orderByExpression, OrderByType type = OrderByType.Asc, params string[] tableNames);

        Task<IEnumerable<T>> QueryAsync(Expression<Func<T, bool>> whereExpression, Expression<Func<T, object>> orderByExpression, OrderByType type = OrderByType.Asc, params string[] tableNames);



        IEnumerable<T> Query(Expression<Func<T, object>> orderByExpression, OrderByType type = OrderByType.Asc, params string[] tableNames);

        ISugarQueryable<T> ReQuery(Expression<Func<T, object>> orderByExpression, OrderByType type = OrderByType.Asc, params string[] tableNames);

        Task<IEnumerable<T>> QueryAsync(Expression<Func<T, object>> orderByExpression, OrderByType type = OrderByType.Asc, params string[] tableNames);
        #endregion

        #region 分页
        IPagedList<T> Paged(int pageIndex,int pageSize, params string[] tableNames);

        Task<IPagedList<T>> PagedAsync(int pageIndex, int pageSize, params string[] tableNames);


        //IPagedList<T> Paged(int pageIndex, int pageSize, ISugarQueryable<T> reQuery, params string[] tableNames);

        //Task<IPagedList<T>> PagedAsync(int pageIndex, int pageSize, ISugarQueryable<T> reQuery, params string[] tableNames);


        IPagedList<T> Paged(int pageIndex, int pageSize, string whereCondition, params string[] tableNames);

        Task<IPagedList<T>> PagedAsync(int pageIndex, int pageSize, string whereCondition, params string[] tableNames);


        IPagedList<T> Paged(int pageIndex, int pageSize, string whereCondition, string orderByFields, params string[] tableNames);

        Task<IPagedList<T>> PagedAsync(int pageIndex, int pageSize, string whereCondition,string orderByFields, params string[] tableNames);


        IPagedList<T> Paged(int pageIndex, int pageSize, Expression<Func<T, bool>> whereExpression, params string[] tableNames);

        Task<IPagedList<T>> PagedAsync(int pageIndex, int pageSize, Expression<Func<T, bool>> whereExpression, params string[] tableNames);


        IPagedList<T> Paged(int pageIndex, int pageSize, Expression<Func<T, object>> orderByExpression, OrderByType type = OrderByType.Asc, params string[] tableNames);

        Task<IPagedList<T>> PagedAsync(int pageIndex, int pageSize, Expression<Func<T, object>> orderByExpression, OrderByType type = OrderByType.Asc, params string[] tableNames);


        IPagedList<T> Paged(int pageIndex, int pageSize, Expression<Func<T, bool>> whereExpression, Expression<Func<T, object>> orderByExpression, OrderByType type = OrderByType.Asc, params string[] tableNames);

        Task<IPagedList<T>> PagedAsync(int pageIndex, int pageSize, Expression<Func<T, bool>> whereExpression, Expression<Func<T, object>> orderByExpression, OrderByType type = OrderByType.Asc, params string[] tableNames);

        #endregion
    }
}
