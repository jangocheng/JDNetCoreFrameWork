// Copyright 
// CLR版本:            4.0.30319.42000
// 机器名称:           ANDY-PC
// 命名空间/文件名:    JDNetCore.Entity.Sugar/BaseRepository 
// 创建人:             研小艾   
// 创建时间:           2020/5/8 21:05:29

using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace JDNetCore.Entity.Sugar
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : SugarEntityBase, new()
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISqlSugarClient _db;

        protected string GetParas(SugarParameter[] pars)
        {
            string key = "【SQL参数】：";
            foreach (var param in pars)
            {
                key += $"{param.ParameterName}:{param.Value}\n";
            }

            return key;
        }

        protected string GetParas(Dictionary<string, object> pars)
        {
            string key = "【SQL参数】：";
            foreach (var param in pars)
            {
                key += $"{param.Key}:{param.Value}\n";
            }

            return key;
        }

        public BaseRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _db = unitOfWork.GetDbClient();
        }

        #region 添加
        public int Add(T entity, Expression<Func<T, object>> insertColumns = null, params string[] tableNames)
        {
            var line = 0;
            var cmd = _db.Insertable(entity);
            if (insertColumns != null) cmd = cmd.InsertColumns(insertColumns);
            line = Exec(cmd, tableNames);
            return line;
        }

        public async Task<int> AddAsync(T entity, Expression<Func<T, object>> insertColumns = null, params string[] tableNames)
        {
            var line = 0;
            var cmd = _db.Insertable(entity);
            if (insertColumns != null) cmd = cmd.InsertColumns(insertColumns);
            line = await ExecAsync(cmd, tableNames);
            return line;
        }


        public int AddRange(IEnumerable<T> entities, Expression<Func<T, object>> insertColumns = null, params string[] tableNames)
        {
            var line = 0;
            var cmd = _db.Insertable(entities.ToArray());
            if (insertColumns != null) cmd = cmd.InsertColumns(insertColumns);
            line = Exec(cmd, tableNames);
            return line;
        }


        public async Task<int> AddRangeAsync(IEnumerable<T> entities, Expression<Func<T, object>> insertColumns = null, params string[] tableNames)
        {
            var line = 0;
            var cmd = _db.Insertable(entities.ToArray());
            if (insertColumns != null) cmd = cmd.InsertColumns(insertColumns);
            line = await ExecAsync(cmd, tableNames);
            return line;
        }

        private static int Exec(IInsertable<T> cmd, params string[] tableNames)
        {
            var line = 0;
            if (tableNames != null && tableNames.Length != 0 )
            {
                foreach (var table in tableNames)
                {
                    line += cmd.AS(table).ExecuteCommand();
                }
            }
            else
            {
                line = cmd.ExecuteCommand();
            }

            return line;
        }
        private static async Task<int> ExecAsync(IInsertable<T> cmd, params string[] tableNames)
        {
            var line = 0;
            if (tableNames != null && tableNames.Length != 0 )
            {
                foreach (var table in tableNames)
                {
                    line += await cmd.AS(table).ExecuteCommandAsync();
                }
            }
            else
            {
                line = await cmd.ExecuteCommandAsync();
            }
            return line;
        }

        #endregion

        #region 删除

        public int Delete(string id, params string[] tableNames)
        {
            var line = 0;
            if (tableNames != null && tableNames.Length != 0 )
            {
                foreach (var table in tableNames)
                {
                    line += _db.Deleteable<T>(id).AS(table).ExecuteCommand();
                }
            }
            else
            {
                line = _db.Deleteable<T>(id).ExecuteCommand();
            }
            return line;
        }

        public async Task<int> DeleteAsync(string id, params string[] tableNames)
        {
            var line = 0;
            if (tableNames != null && tableNames.Length != 0 )
            {
                foreach (var table in tableNames)
                {
                    line += await _db.Deleteable<T>(id).AS(table).ExecuteCommandAsync();
                }
            }
            else
            {
                line = await _db.Deleteable<T>(id).ExecuteCommandAsync();
            }
            return line;
        }

        public int DeleteIn(string[] ids, params string[] tableNames)
        {
            var line = 0;
            if (tableNames != null && tableNames.Length != 0 )
            {
                foreach (var table in tableNames)
                {
                    line += _db.Deleteable<T>(ids).AS(table).ExecuteCommand();
                }
            }
            else
            {
                line = _db.Deleteable<T>(ids).ExecuteCommand();
            }
            return line;
        }

        public async Task<int> DeleteInAsync(string[] ids, params string[] tableNames)
        {
            var line = 0;
            if (tableNames != null && tableNames.Length != 0 )
            {
                foreach (var table in tableNames)
                {
                    line += await _db.Deleteable<T>(ids).AS(table).ExecuteCommandAsync();
                }
            }
            else
            {
                line = await _db.Deleteable<T>(ids).ExecuteCommandAsync();
            }
            return line;
        }

        public int DeleteWith(Expression<Func<T, bool>> whereExpression, params string[] tableNames)
        {
            var line = 0;
            if (tableNames != null && tableNames.Length != 0 )
            {
                foreach (var table in tableNames)
                {
                    line += _db.Deleteable<T>(whereExpression).AS(table).ExecuteCommand();
                }
            }
            else
            {
                line = _db.Deleteable<T>(whereExpression).ExecuteCommand();
            }
            return line;
        }

        public async Task<int> DeleteWithAsync(Expression<Func<T, bool>> whereExpression, params string[] tableNames)
        {
            var line = 0;
            if (tableNames != null && tableNames.Length != 0 )
            {
                foreach (var table in tableNames)
                {
                    line += await _db.Deleteable<T>(whereExpression).AS(table).ExecuteCommandAsync();
                }
            }
            else
            {
                line = await _db.Deleteable<T>(whereExpression).ExecuteCommandAsync();
            }
            return line;
        }

        #endregion

        #region 主键查找

        public T Find(string id, params string[] tableNames)
        {
            T result = null;
            if (tableNames != null && tableNames.Length != 0 )
            {
                foreach (var table in tableNames)
                {
                    result = _db.Queryable<T>().AS(table).InSingle(id);
                    if (result != null) break;
                }
            }
            else
            {
                result = _db.Queryable<T>().InSingle(id);
            }
            return result;
        }

        public async Task<T> FindAsync(string id, params string[] tableNames)
        {
            T result = null;
            if (tableNames != null && tableNames.Length != 0 )
            {
                foreach (var table in tableNames)
                {
                    result = await _db.Queryable<T>().AS(table).InSingleAsync(id);
                    if (result != null) break;
                }
            }
            else
            {
                result = await _db.Queryable<T>().InSingleAsync(id);
            }
            return result;
        }

        public IEnumerable<T> FindIn(string[] ids, params string[] tableNames)
        {
            IEnumerable<T> result = null;
            if (tableNames != null && tableNames.Length != 0 )
            {
                foreach (var table in tableNames)
                {
                    result = _db.Queryable<T>().AS(table).In(ids).ToArray();
                    if (result != null) break;
                }
            }
            else
            {
                result = _db.Queryable<T>().In(ids).ToArray();
            }
            return result;
        }

        public async Task<IEnumerable<T>> FindInAsync(string[] ids, params string[] tableNames)
        {
            IEnumerable<T> result = null;
            if (tableNames != null && tableNames.Length != 0 )
            {
                foreach (var table in tableNames)
                {
                    result = await _db.Queryable<T>().AS(table).In(ids).ToListAsync();
                    if (result != null) break;
                }
            }
            else
            {
                result = await _db.Queryable<T>().In(ids).ToListAsync();
            }
            return result;
        }

        #endregion

        public virtual string[] GetTables()
        {
            return new string[] { typeof(T).Name.ToLower() };
        }

        #region 分页

        public IPagedList<T> Paged(int pageIndex, int pageSize, params string[] tableNames)
        {
            var result = ReQuery(tableNames).ToPagedList(pageIndex, pageSize);
            return result;
        }

        public IPagedList<T> Paged(int pageIndex, int pageSize, string whereCondition, params string[] tableNames)
        {
            var result = ReQuery(whereCondition, tableNames).ToPagedList(pageIndex, pageSize);
            return result;
        }

        public IPagedList<T> Paged(int pageIndex, int pageSize, string whereCondition, string orderByFields, params string[] tableNames)
        {
            var result = ReQuery(whereCondition, orderByFields, tableNames).ToPagedList(pageIndex, pageSize);
            return result;
        }

        public IPagedList<T> Paged(int pageIndex, int pageSize, Expression<Func<T, bool>> whereExpression, params string[] tableNames)
        {
            var result = ReQuery(whereExpression, tableNames).ToPagedList(pageIndex, pageSize);
            return result;
        }

        public IPagedList<T> Paged(int pageIndex, int pageSize, Expression<Func<T, object>> orderByExpression, OrderByType type = OrderByType.Asc, params string[] tableNames)
        {
            var result = ReQuery(orderByExpression, type, tableNames).ToPagedList(pageIndex, pageSize);
            return result;
        }

        public IPagedList<T> Paged(int pageIndex, int pageSize, Expression<Func<T, bool>> whereExpression, Expression<Func<T, object>> orderByExpression, OrderByType type = OrderByType.Asc, params string[] tableNames)
        {
            var result = ReQuery(whereExpression, orderByExpression, type, tableNames).ToPagedList(pageIndex, pageSize);
            return result;
        }

        public async Task<IPagedList<T>> PagedAsync(int pageIndex, int pageSize, params string[] tableNames)
        {
            var result = await ReQuery(tableNames).ToPagedListAsync(pageIndex, pageSize);
            return result;
        }

        public async Task<IPagedList<T>> PagedAsync(int pageIndex, int pageSize, string whereCondition, params string[] tableNames)
        {
            var result = await ReQuery(whereCondition, tableNames).ToPagedListAsync(pageIndex, pageSize);
            return result;
        }

        public async Task<IPagedList<T>> PagedAsync(int pageIndex, int pageSize, string whereCondition, string orderByFields, params string[] tableNames)
        {
            var result = await ReQuery(whereCondition, orderByFields, tableNames).ToPagedListAsync(pageIndex, pageSize);
            return result;
        }

        public async Task<IPagedList<T>> PagedAsync(int pageIndex, int pageSize, Expression<Func<T, bool>> whereExpression, params string[] tableNames)
        {
            var result = await ReQuery(whereExpression, tableNames).ToPagedListAsync(pageIndex, pageSize);
            return result;
        }

        public async Task<IPagedList<T>> PagedAsync(int pageIndex, int pageSize, Expression<Func<T, object>> orderByExpression, OrderByType type = OrderByType.Asc, params string[] tableNames)
        {
            var result = await ReQuery(orderByExpression, type, tableNames).ToPagedListAsync(pageIndex, pageSize);
            return result;
        }

        public async Task<IPagedList<T>> PagedAsync(int pageIndex, int pageSize, Expression<Func<T, bool>> whereExpression, Expression<Func<T, object>> orderByExpression, OrderByType type = OrderByType.Asc, params string[] tableNames)
        {
            var result = await ReQuery(whereExpression, orderByExpression, type, tableNames).ToPagedListAsync(pageIndex, pageSize);
            return result;
        }

        #endregion

        #region 查询同步

        public IEnumerable<T> Query(params string[] tableNames)
        {
            var result = ReQuery(tableNames).ToArray();
            return result;
        }

        public IEnumerable<T> Query(string whereCondition, params string[] tableNames)
        {
            var result = ReQuery(whereCondition, tableNames).ToArray();
            return result;
        }

        public IEnumerable<T> Query(int topCount, params string[] tableNames)
        {
            var result = ReQuery(topCount, tableNames).ToArray();
            return result;
        }

        public IEnumerable<T> Query(int topCount, string whereCondition, params string[] tableNames)
        {
            var result = ReQuery(topCount, whereCondition, tableNames).ToArray();
            return result;
        }

        public IEnumerable<T> Query(string whereCondition, string orderByFields, params string[] tableNames)
        {
            var result = ReQuery(whereCondition, orderByFields, tableNames).ToArray();
            return result;
        }

        public IEnumerable<T> Query(int topCount, string whereCondition, string orderByFields, params string[] tableNames)
        {
            var result = ReQuery(topCount, whereCondition, orderByFields, tableNames).ToArray();
            return result;
        }

        public IEnumerable<T> Query(Expression<Func<T, bool>> whereExpression, params string[] tableNames)
        {
            var result = ReQuery(whereExpression, tableNames).ToArray();
            return result;
        }

        public IEnumerable<T> Query(int topCount, Expression<Func<T, bool>> whereExpression, params string[] tableNames)
        {
            var result = ReQuery(topCount, whereExpression, tableNames).ToArray();
            return result;
        }


        public IEnumerable<T> Query(int topCount, Expression<Func<T, object>> orderByExpression, OrderByType type = OrderByType.Asc, params string[] tableNames)
        {
            var result = ReQuery(topCount, orderByExpression, type, tableNames).ToArray();
            return result;
        }

        public IEnumerable<T> Query(int topCount, Expression<Func<T, bool>> whereExpression, Expression<Func<T, object>> orderByExpression, OrderByType type = OrderByType.Asc, params string[] tableNames)
        {
            var result = ReQuery(topCount, whereExpression, orderByExpression, type, tableNames).ToArray();
            return result;
        }

        public IEnumerable<T> Query(Expression<Func<T, bool>> whereExpression, Expression<Func<T, object>> orderByExpression, OrderByType type = OrderByType.Asc, params string[] tableNames)
        {
            var result = ReQuery(whereExpression, orderByExpression, type, tableNames).ToArray();
            return result;
        }

        public IEnumerable<T> Query(Expression<Func<T, object>> orderByExpression, OrderByType type = OrderByType.Asc, params string[] tableNames)
        {
            var result = ReQuery(orderByExpression, type, tableNames).ToArray();
            return result;
        }
        #endregion

        #region 查询异步

        public async Task<IEnumerable<T>> QueryAsync(params string[] tableNames)
        {
            var result = await ReQuery(tableNames).ToListAsync();
            return result;
        }

        public async Task<IEnumerable<T>> QueryAsync(string whereCondition, params string[] tableNames)
        {
            var result = await ReQuery(whereCondition, tableNames).ToListAsync();
            return result;
        }

        public async Task<IEnumerable<T>> QueryAsync(int topCount, params string[] tableNames)
        {
            var result = await ReQuery(topCount, tableNames).ToListAsync();
            return result;
        }

        public async Task<IEnumerable<T>> QueryAsync(int topCount, string whereCondition, params string[] tableNames)
        {
            var result = await ReQuery(topCount, whereCondition, tableNames).ToListAsync();
            return result;
        }

        public async Task<IEnumerable<T>> QueryAsync(string whereCondition, string orderByFields, params string[] tableNames)
        {
            var result = await ReQuery(whereCondition, orderByFields, tableNames).ToListAsync();
            return result;
        }

        public async Task<IEnumerable<T>> QueryAsync(int topCount, string whereCondition, string orderByFields, params string[] tableNames)
        {
            var result = await ReQuery(topCount, whereCondition, orderByFields, tableNames).ToListAsync();
            return result;
        }

        public async Task<IEnumerable<T>> QueryAsync(Expression<Func<T, bool>> whereExpression, params string[] tableNames)
        {
            var result = await ReQuery(whereExpression, tableNames).ToListAsync();
            return result;
        }

        public async Task<IEnumerable<T>> QueryAsync(int topCount, Expression<Func<T, bool>> whereExpression, params string[] tableNames)
        {
            var result = await ReQuery(topCount, whereExpression, tableNames).ToListAsync();
            return result;
        }

        public async Task<IEnumerable<T>> QueryAsync(int topCount, Expression<Func<T, object>> orderByExpression, OrderByType type = OrderByType.Asc, params string[] tableNames)
        {
            var result = await ReQuery(topCount, orderByExpression, type, tableNames).ToListAsync();
            return result;
        }

        public async Task<IEnumerable<T>> QueryAsync(int topCount, Expression<Func<T, bool>> whereExpression, Expression<Func<T, object>> orderByExpression, OrderByType type = OrderByType.Asc, params string[] tableNames)
        {
            var result = await ReQuery(topCount, whereExpression, orderByExpression, type, tableNames).ToListAsync();
            return result;
        }

        public async Task<IEnumerable<T>> QueryAsync(Expression<Func<T, bool>> whereExpression, Expression<Func<T, object>> orderByExpression, OrderByType type = OrderByType.Asc, params string[] tableNames)
        {
            var result = await ReQuery(whereExpression, orderByExpression, type, tableNames).ToListAsync();
            return result;
        }

        public async Task<IEnumerable<T>> QueryAsync(Expression<Func<T, object>> orderByExpression, OrderByType type = OrderByType.Asc, params string[] tableNames)
        {
            var result = await ReQuery(orderByExpression, type, tableNames).ToListAsync();
            return result;
        }

        #endregion

        #region SQL语句

        public ISugarQueryable<T> ReQuery(params string[] tableNames)
        {
            ISugarQueryable<T> result = null;
            if (tableNames != null && tableNames.Length != 0 )
            {
                var list = new List<ISugarQueryable<T>>();
                foreach (var table in tableNames)
                {
                    list.Add(_db.Queryable<T>().AS(table));
                }
                result = _db.Union(list);
            }
            else
            {
                result = _db.Queryable<T>();
            }
            return result;
        }

        public ISugarQueryable<T> ReQuery(string whereCondition, params string[] tableNames)
        {
            ISugarQueryable<T> result = null;
            if (tableNames != null && tableNames.Length != 0 )
            {
                var list = new List<ISugarQueryable<T>>();
                foreach (var table in tableNames)
                {
                    list.Add(_db.Queryable<T>().AS(table).WhereIF(!string.IsNullOrWhiteSpace(whereCondition), whereCondition));
                }
                result = _db.Union(list);
            }
            else
            {
                result = _db.Queryable<T>().WhereIF(!string.IsNullOrWhiteSpace(whereCondition), whereCondition);
            }
            return result;
        }

        public ISugarQueryable<T> ReQuery(string whereCondition, string orderByFields, params string[] tableNames)
        {
            ISugarQueryable<T> result = null;
            if (tableNames != null && tableNames.Length != 0 )
            {
                var list = new List<ISugarQueryable<T>>();
                foreach (var table in tableNames)
                {
                    list.Add(_db.Queryable<T>().AS(table).WhereIF(!string.IsNullOrWhiteSpace(whereCondition), whereCondition)
                        .OrderByIF(!string.IsNullOrWhiteSpace(orderByFields), orderByFields));
                }
                result = _db.Union(list).OrderByIF(!string.IsNullOrWhiteSpace(orderByFields), orderByFields);
            }
            else
            {
                result = _db.Queryable<T>().WhereIF(!string.IsNullOrWhiteSpace(whereCondition), whereCondition)
                    .OrderByIF(!string.IsNullOrWhiteSpace(orderByFields), orderByFields);
            }
            return result;
        }

        public ISugarQueryable<T> ReQuery(int topCount, params string[] tableNames)
        {
            ISugarQueryable<T> result = null;
            if (tableNames != null && tableNames.Length != 0 )
            {
                var list = new List<ISugarQueryable<T>>();
                foreach (var table in tableNames)
                {
                    list.Add(_db.Queryable<T>().AS(table).Take(topCount));
                }
                result = _db.Union(list).Take(topCount);
            }
            else
            {
                result = _db.Queryable<T>().Take(topCount);
            }
            return result;
        }

        public ISugarQueryable<T> ReQuery(int topCount, string whereCondition, params string[] tableNames)
        {
            ISugarQueryable<T> result = null;
            if (tableNames != null && tableNames.Length != 0 )
            {
                var list = new List<ISugarQueryable<T>>();
                foreach (var table in tableNames)
                {
                    list.Add(_db.Queryable<T>().AS(table).WhereIF(!string.IsNullOrWhiteSpace(whereCondition), whereCondition).Take(topCount));
                }
                result = _db.Union(list).Take(topCount);
            }
            else
            {
                result = _db.Queryable<T>().WhereIF(!string.IsNullOrWhiteSpace(whereCondition), whereCondition).Take(topCount);
            }
            return result;
        }

        public ISugarQueryable<T> ReQuery(int topCount, string whereCondition, string orderByFields, params string[] tableNames)
        {
            ISugarQueryable<T> result = null;
            if (tableNames != null && tableNames.Length != 0 )
            {
                var list = new List<ISugarQueryable<T>>();
                foreach (var table in tableNames)
                {
                    list.Add(_db.Queryable<T>().AS(table).WhereIF(!string.IsNullOrWhiteSpace(whereCondition),whereCondition)
                        .OrderByIF(!string.IsNullOrWhiteSpace(orderByFields), orderByFields)
                        .Take(topCount));
                }
                result = _db.Union(list).OrderByIF(!string.IsNullOrWhiteSpace(orderByFields),orderByFields).Take(topCount);
            }
            else
            {
                result = _db.Queryable<T>().WhereIF(!string.IsNullOrWhiteSpace(whereCondition), whereCondition)
                    .OrderByIF(!string.IsNullOrWhiteSpace(orderByFields), orderByFields).Take(topCount);
            }
            return result;
        }

        public ISugarQueryable<T> ReQuery(Expression<Func<T, bool>> whereExpression, params string[] tableNames)
        {
            ISugarQueryable<T> result = null;
            if (tableNames != null && tableNames.Length != 0 )
            {
                var list = new List<ISugarQueryable<T>>();
                foreach (var table in tableNames)
                {
                    list.Add(_db.Queryable<T>().AS(table).WhereIF(whereExpression!=null, whereExpression));
                }
                result = _db.Union(list);
            }
            else
            {
                result = _db.Queryable<T>().WhereIF(whereExpression != null, whereExpression);
            }
            return result;
        }

        public ISugarQueryable<T> ReQuery(int topCount, Expression<Func<T, bool>> whereExpression, params string[] tableNames)
        {
            ISugarQueryable<T> result = null;
            if (tableNames != null && tableNames.Length != 0 )
            {
                var list = new List<ISugarQueryable<T>>();
                foreach (var table in tableNames)
                {
                    list.Add(_db.Queryable<T>().AS(table).WhereIF(whereExpression != null, whereExpression).Take(topCount));
                }
                result = _db.Union(list).Take(topCount);
            }
            else
            {
                result = _db.Queryable<T>().WhereIF(whereExpression != null, whereExpression).Take(topCount);
            }
            return result;
        }

        public ISugarQueryable<T> ReQuery(int topCount, Expression<Func<T, object>> orderByExpression, OrderByType type = OrderByType.Asc, params string[] tableNames)
        {
            ISugarQueryable<T> result = null;
            if (tableNames != null && tableNames.Length != 0 )
            {
                var list = new List<ISugarQueryable<T>>();
                foreach (var table in tableNames)
                {
                    list.Add(_db.Queryable<T>().AS(table).OrderByIF(orderByExpression != null, orderByExpression, type).Take(topCount));
                }
                result = _db.Union(list).OrderByIF(orderByExpression != null, orderByExpression, type).Take(topCount);
            }
            else
            {
                result = _db.Queryable<T>().OrderByIF(orderByExpression != null, orderByExpression, type).Take(topCount);
            }
            return result;
        }

        public ISugarQueryable<T> ReQuery(int topCount, Expression<Func<T, bool>> whereExpression, Expression<Func<T, object>> orderByExpression, OrderByType type = OrderByType.Asc, params string[] tableNames)
        {
            ISugarQueryable<T> result = null;
            if (tableNames != null && tableNames.Length != 0 )
            {
                var list = new List<ISugarQueryable<T>>();
                foreach (var table in tableNames)
                {
                    list.Add(_db.Queryable<T>().AS(table).WhereIF(whereExpression != null, whereExpression).OrderByIF(orderByExpression != null, orderByExpression, type).Take(topCount));
                }
                result = _db.Union(list).OrderByIF(orderByExpression != null, orderByExpression, type).Take(topCount);
            }
            else
            {
                result = _db.Queryable<T>().WhereIF(whereExpression != null, whereExpression).OrderByIF(orderByExpression != null, orderByExpression, type).Take(topCount);
            }
            return result;
        }

        public ISugarQueryable<T> ReQuery(Expression<Func<T, bool>> whereExpression, Expression<Func<T, object>> orderByExpression, OrderByType type = OrderByType.Asc, params string[] tableNames)
        {
            ISugarQueryable<T> result = null;
            if (tableNames != null && tableNames.Length != 0 )
            {
                var list = new List<ISugarQueryable<T>>();
                foreach (var table in tableNames)
                {
                    list.Add(_db.Queryable<T>().AS(table).WhereIF(whereExpression != null, whereExpression).OrderByIF(orderByExpression != null, orderByExpression, type));
                }
                result = _db.Union(list).OrderByIF(orderByExpression != null, orderByExpression, type);
            }
            else
            {
                result = _db.Queryable<T>().WhereIF(whereExpression != null, whereExpression).OrderByIF(orderByExpression != null, orderByExpression, type);
            }
            return result;
        }

        public ISugarQueryable<T> ReQuery(Expression<Func<T, object>> orderByExpression, OrderByType type = OrderByType.Asc, params string[] tableNames)
        {
            ISugarQueryable<T> result = null;
            if (tableNames != null && tableNames.Length != 0 )
            {
                var list = new List<ISugarQueryable<T>>();
                foreach (var table in tableNames)
                {
                    list.Add(_db.Queryable<T>().AS(table));
                }
                result = _db.Union(list).OrderByIF(orderByExpression != null, orderByExpression, type);
            }
            else
            {
                result = _db.Queryable<T>().OrderByIF(orderByExpression != null, orderByExpression, type);
            }
            return result;
        }

        #endregion

        #region 更新

        public int Update(T entity, params string[] tableNames)
        {
            var line = 0;
            if (tableNames != null && tableNames.Length != 0 )
            {
                foreach (var table in tableNames)
                {
                    line += _db.Updateable<T>(entity).IgnoreColumns(o=>new { o.created_id,o.created_on }).AS(table).ExecuteCommand();
                }
            }
            else
            {
                line = _db.Updateable<T>(entity).IgnoreColumns(o => new { o.created_id, o.created_on }).ExecuteCommand();
            }
            return line;
        }

        public async Task<int> UpdateAsync(T entity, params string[] tableNames)
        {
            var line = 0;
            if (tableNames != null && tableNames.Length != 0 )
            {
                foreach (var table in tableNames)
                {
                    line += await _db.Updateable<T>(entity).IgnoreColumns(o => new { o.created_id, o.created_on }).AS(table).ExecuteCommandAsync();
                }
            }
            else
            {
                line = await _db.Updateable<T>(entity).IgnoreColumns(o => new { o.created_id, o.created_on }).ExecuteCommandAsync();
            }
            return line;
        }

        public int UpdateRange(IEnumerable<T> entities, params string[] tableNames) 
        {
            var line = 0;
            if (tableNames != null && tableNames.Length != 0 )
            {
                foreach (var table in tableNames)
                {
                    line += _db.Updateable<T>(entities).IgnoreColumns(o => new { o.created_id, o.created_on }).AS(table).ExecuteCommand();
                }
            }
            else
            {
                line = _db.Updateable<T>(entities).IgnoreColumns(o => new { o.created_id, o.created_on }).ExecuteCommand();
            }
            return line;
        }

        public async Task<int> UpdateRangeAsync(IEnumerable<T> entities, params string[] tableNames) 
        {
            var line = 0;
            if (tableNames != null && tableNames.Length != 0 )
            {
                foreach (var table in tableNames)
                {
                    line += await _db.Updateable<T>(entities).IgnoreColumns(o => new { o.created_id, o.created_on }).AS(table).ExecuteCommandAsync();
                }
            }
            else
            {
                line = await _db.Updateable<T>(entities).IgnoreColumns(o => new { o.created_id, o.created_on }).ExecuteCommandAsync();
            }
            return line;
        }

        public int UpdateWith(T entity, Expression<Func<T, bool>> whereExpression, params string[] tableNames)
        {
            var line = 0;
            if (tableNames != null && tableNames.Length != 0 )
            {
                foreach (var table in tableNames)
                {
                    line += _db.Updateable<T>(entity).AS(table).IgnoreColumns(o => new { o.created_id, o.created_on }).Where(whereExpression).ExecuteCommand();
                }
            }
            else
            {
                line = _db.Updateable<T>(entity).IgnoreColumns(o => new { o.created_id, o.created_on }).Where(whereExpression).ExecuteCommand();
            }
            return line;
        }

        public async Task<int> UpdateWithAsync(T entity, Expression<Func<T, bool>> whereExpression, params string[] tableNames)
        {
            var line = 0;
            if (tableNames != null && tableNames.Length != 0 )
            {
                foreach (var table in tableNames)
                {
                    line += await _db.Updateable<T>(entity).AS(table).IgnoreColumns(o => new { o.created_id, o.created_on }).Where(whereExpression).ExecuteCommandAsync();
                }
            }
            else
            {
                line = await _db.Updateable<T>(entity).IgnoreColumns(o => new { o.created_id, o.created_on }).Where(whereExpression).ExecuteCommandAsync();
            }
            return line;
        }

        public int UpdateTable(Expression<Func<T, T>> columns, Expression<Func<T, bool>> whereExpression, params string[] tableNames)
        {
            var line = 0;
            if (tableNames != null && tableNames.Length != 0 )
            {
                foreach (var table in tableNames)
                {
                    line += _db.Updateable<T>().AS(table).SetColumns(columns).Where(whereExpression).ExecuteCommand();
                }
            }
            else
            {
                line = _db.Updateable<T>().SetColumns(columns).Where(whereExpression).ExecuteCommand();
            }
            return line;
        }

        public async Task<int> UpdateTableAsync(Expression<Func<T, T>> columns, Expression<Func<T, bool>> whereExpression, params string[] tableNames)
        {
            var line = 0;
            if (tableNames != null && tableNames.Length != 0 )
            {
                foreach (var table in tableNames)
                {
                    line += await _db.Updateable<T>().AS(table).SetColumns(columns).Where(whereExpression).ExecuteCommandAsync();
                }
            }
            else
            {
                line = await _db.Updateable<T>().SetColumns(columns).Where(whereExpression).ExecuteCommandAsync();
            }
            return line;
        }

        #endregion
    }
}
