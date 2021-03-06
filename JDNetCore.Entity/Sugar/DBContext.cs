﻿using JDNetCore.Common;
using JDNetCore.Entity.Base;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;

namespace JDNetCore.Entity.Sugar
{
    public partial class DBContext
    {
        private static MutiDBOperate connectObject => GetMainConnectionDb();
        private static string _connectionString = connectObject.Connection;
        private static DbType _dbType = (DbType)connectObject.DbType;
        private SqlSugarClient _db;

        /// <summary>
        /// 连接字符串 
        /// </summary>
        public static MutiDBOperate GetMainConnectionDb()
        {
            var mainConnetctDb = BaseDBConfig.MutiConnectionString.Find(x => x.ConnId == Appsettings.app<string>("MainDB"));
            if (BaseDBConfig.MutiConnectionString.Count > 0)
            {
                if (mainConnetctDb == null)
                {
                    mainConnetctDb = BaseDBConfig.MutiConnectionString[0];
                }
            }
            else
            {
                throw new Exception("请确保appsettigns.json中配置连接字符串,并设置Enabled为true;");
            }
            return mainConnetctDb;
        }
        /// <summary>
        /// 连接字符串 
        /// </summary>
        public static string ConnectionString
        {
            get { return _connectionString; }
            set { _connectionString = value; }
        }

        /// <summary>
        /// 数据库类型 
        /// </summary>
        public static DbType DbType
        {
            get { return _dbType; }
            set { _dbType = value; }
        }

        /// <summary>
        /// 数据连接对象 
        /// </summary>
        public SqlSugarClient Db
        {
            get { return _db; }
            private set { _db = value; }
        }



        /// <summary>
        /// 数据库上下文实例（自动关闭连接）
        /// </summary>
        public static DBContext Context
        {
            get
            {
                return new DBContext();
            }

        }

        /// <summary>
        /// 功能描述:构造函数
        /// </summary>
        public DBContext()
        {
            if (string.IsNullOrEmpty(_connectionString))
            {
                throw new ArgumentNullException("数据库连接字符串为空");
            }
            _db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = _connectionString,
                DbType = _dbType,
                IsAutoCloseConnection = true,
                InitKeyType = InitKeyType.Attribute,//mark
                ConfigureExternalServices = new ConfigureExternalServices()
                {
                    //DataInfoCacheService = new HttpRuntimeCache()
                },
                MoreSettings = new ConnMoreSettings()
                {
                    //IsWithNoLockQuery = true,
                    IsAutoRemoveDataCache = true
                }
            });
        }

        #region 根据数据库表生产IRepository层
        /// <summary>
        /// 功能描述:根据数据库表生产IRepository层
        /// </summary>
        /// <param name="strPath">实体类存放路径</param>
        /// <param name="strNameSpace">命名空间</param>
        /// <param name="tableName">生产指定的表</param>
        /// <param name="logicName">逻辑名</param>
        /// <param name="remark">逻辑备注</param>
        public string Create_IRepository_ClassFileByDBTalbe(string strPath, string strNameSpace, string tableName, string logicName, string remark)
        {
            string fileName = "I" + logicName + "Repository";
            if (File.Exists(strPath + "\\" + fileName + ".cs"))
            {
                return $"IRepository层文件 {fileName} 已存在;  ";
            }
            var IDbFirst = _db.DbFirst.Where(tableName);
            _db.MappingTables.Clear();
            _db.MappingTables.Add(fileName, tableName);
            IDbFirst.IsCreateDefaultValue().IsCreateAttribute()
                .SettingClassTemplate(p => p = 
$@"
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using JDNetCore.Entity;
using JDNetCore.Entity.Sugar;

namespace " + strNameSpace + @"
{
	/// <summary>
	/// " + remark + @"
	/// </summary>	
    public interface I" + logicName + @"Repository : IBaseRepository<" + tableName + @">" + @"
    {
    }
}
")
                 .CreateClassFile(strPath, strNameSpace);
            return "创建成功";
        }
        #endregion



        #region 根据数据库表生产 Repository 层
        /// <summary>
        /// 功能描述:根据数据库表生产 Repository 层
        /// </summary>
        /// <param name="strPath">实体类存放路径</param>
        /// <param name="strNameSpace">命名空间</param>
        /// <param name="tableName">生产指定的表</param>
        /// <param name="logicName">逻辑名</param>
        /// <param name="remark">逻辑备注</param>
        public string Create_Repository_ClassFileByDBTalbe(string strPath, string strNameSpace, string tableName, string logicName, string remark)
        {
            string fileName = logicName + "Repository";
            if (File.Exists(strPath + "\\" + fileName + ".cs"))
            {
                return $"Repository层文件 {fileName} 已存在;  ";
            }
            var IDbFirst = _db.DbFirst.Where(tableName);
            _db.MappingTables.Clear();
            _db.MappingTables.Add(fileName, tableName);
            IDbFirst.IsCreateDefaultValue().IsCreateAttribute()
                .SettingClassTemplate(p => p =
@"
using JDNetCore.Entity;
using JDNetCore.Entity.Sugar;
using JDNetCore.Repository.Interface;
using System;
using System.Collections.Generic;

namespace " + strNameSpace + @"
{
	/// <summary>
	/// " + remark + @" 
	/// </summary>
    public partial class " + logicName + "Repository : BaseRepository<" + tableName + ">, I" + logicName + "Repository" + @"
    {
        public " + logicName + @"Repository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
")
                 .CreateClassFile(strPath, strNameSpace);
            return "创建成功";
        }
        #endregion

        #region 根据数据库表生产IServices层
        /// <summary>
        /// 功能描述:根据数据库表生产IServices层
        /// </summary>
        /// <param name="strPath">实体类存放路径</param>
        /// <param name="strNameSpace">命名空间</param>
        /// <param name="tableName">生产指定的表</param>
        /// <param name="logicName">逻辑名</param>
        /// <param name="remark">逻辑备注</param>
        public string Create_IServices_ClassFileByDBTalbe(string strPath, string strNameSpace, string tableName, string logicName, string remark)
        {
            string fileName = "I" + logicName + "Services";
            if (File.Exists(strPath + "\\" + fileName + ".cs"))
            {
                return $"IServices层文件 {fileName} 已存在;  ";
            }
            var IDbFirst = _db.DbFirst.Where(tableName);
            _db.MappingTables.Clear();
            _db.MappingTables.Add(fileName, tableName);
            IDbFirst.IsCreateDefaultValue().IsCreateAttribute()
                .SettingClassTemplate(p => p = @"using System.Collections.Generic;
using System.Threading.Tasks;
using Jd.Common.NetCore.Model;
using Jd.Common.NetCore.Model.Models;
using Jd.Common.NetCore.Model.ViewModels.RequestModel." + logicName + @";
using Jd.Common.NetCore.Model.ViewModels.ResponseModel." + logicName + @";

namespace " + strNameSpace + @"
{	
	/// <summary>
	/// " + remark + @"
	/// </summary>	
    public interface I" + logicName + @"Services :IBaseServices<" + tableName + ">" + @"
	{
    }
}
")
                 .CreateClassFile(strPath, strNameSpace);
            return "";
        }
        #endregion

        #region 根据数据库表生产 Services 层
        /// <summary>
        /// 功能描述:根据数据库表生产 Services 层
        /// </summary>
        /// <param name="strPath">实体类存放路径</param>
        /// <param name="strNameSpace">命名空间</param>
        /// <param name="tableName">生产指定的表</param>
        /// <param name="logicName">逻辑名</param>
        /// <param name="remark">逻辑备注</param>
        public string Create_Services_ClassFileByDBTalbe(string strPath, string strNameSpace, string tableName, string logicName, string remark)
        {
            string fileName = logicName + "Services";
            if (File.Exists(strPath + "\\" + fileName + ".cs"))
            {
                return $"Services层文件 {fileName} 已存在;  ";
            }
            var IDbFirst = _db.DbFirst.Where(tableName);
            _db.MappingTables.Clear();
            _db.MappingTables.Add(fileName, tableName);
            IDbFirst.IsCreateDefaultValue().IsCreateAttribute()
                .SettingClassTemplate(p => p = @"using System;
using Jd.Common.NetCore.IRepository;
using Jd.Common.NetCore.IService;
using Jd.Common.NetCore.Model;
using Jd.Common.NetCore.Model.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Jd.Common.NetCore.Model.ViewModels.RequestModel." + logicName + @";
using Jd.Common.NetCore.Model.ViewModels.ResponseModel." + logicName + @";
using SqlSugar;
using System.Linq;

namespace " + strNameSpace + @"
{
	/// <summary>
	/// " + remark + @"  
	/// </summary>
    public class " + logicName + @"Services : BaseServices<" + tableName + ">, I" + logicName + "Services" + @"
    {
        I" + logicName + @"Repository repository" + @";
        public " + logicName + @"Services(I" + logicName + @"Repository dal)
        {
            this.repository" + @" = dal;
            base.BaseDal = dal;
        }
    }
}
                    ")
                 .CreateClassFile(strPath, strNameSpace);
            return "";
        }
        #endregion

        #region 根据数据库表生成RequestViewModel        
        /// <summary>
        /// 根据数据库表生成RequestViewModel
        /// </summary>
        /// <param name="strPath">实体类存放路径</param>
        /// <param name="strNameSpace">命名空间</param>
        /// <param name="tableName">生产指定的表</param>
        /// <param name="logicName">逻辑名</param>
        public string Create_RequestViewModel_ClassFileByDBTalbe(string strPath, string strNameSpace, string tableName, string logicName)
        {
            string fileName = logicName + "Request";
            if (File.Exists(strPath + "\\" + fileName + ".cs"))
            {
                return $"RequestViewModel层文件 {fileName} 已存在;  ";
            }
            var IDbFirst = _db.DbFirst.Where(tableName);
            _db.MappingTables.Clear();
            _db.MappingTables.Add(fileName, tableName);
            IDbFirst.IsCreateDefaultValue().IsCreateAttribute()
                .SettingClassTemplate(p => p = @"using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace " + strNameSpace + @"
{
	/// <summary>
	/// 
	/// </summary>
    public class " + logicName + @"Request" + @" : QueryBase
    {
           
    }
}
                    ")
                 .CreateClassFile(strPath, strNameSpace);
            return "";
        }
        #endregion

        #region 根据数据库表生成ResponseViewModel       
        /// <summary>
        /// 根据数据库表生成ResponseViewModel
        /// </summary>
        /// <param name="strPath"></param>
        /// <param name="strNameSpace"></param>
        /// <param name="tableName"></param>
        /// <param name="logicName"></param>
        public string Create_ResponseViewModel_ClassFileByDBTalbe(string strPath, string strNameSpace, string tableName, string logicName)
        {
            string fileName = logicName + "Response";
            if (File.Exists(strPath + "\\" + fileName + ".cs"))
            {
                return $"ResponseViewModel层文件 {fileName} 已存在;  ";
            }
            var IDbFirst = _db.DbFirst.Where(tableName);
            _db.MappingTables.Clear();
            _db.MappingTables.Add(fileName, tableName);
            IDbFirst.IsCreateDefaultValue().IsCreateAttribute()
                .SettingClassTemplate(p => p = @"using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace " + strNameSpace + @"
{
	/// <summary>
	/// 
	/// </summary>
    public class " + logicName + @"Response" + @"
    {
           
    }
}
                    ")
                 .CreateClassFile(strPath, strNameSpace);
            return "";
        }
        #endregion

        #region 根据数据库表生成Controll
        /// <summary>
        /// 根据数据库表生成Controll
        /// </summary>
        /// <param name="strPath"></param>
        /// <param name="strNameSpace"></param>
        /// <param name="tableName"></param>
        /// <param name="logicName"></param>
        /// <param name="remark">逻辑备注</param>
        public string Create_Controller_ClassFileByDBTalbe(string strPath, string strNameSpace, string tableName, string logicName, string remark, string area)
        {
            string fileName = logicName + "Controller";
            if (File.Exists(strPath + "\\" + fileName + ".cs"))
            {
                return $"Controller层文件 {fileName} 已存在;  ";
            }
            var IDbFirst = _db.DbFirst.Where(tableName);
            _db.MappingTables.Clear();
            _db.MappingTables.Add(fileName, tableName);
            IDbFirst.IsCreateDefaultValue().IsCreateAttribute()
                .SettingClassTemplate(p => p = 
@"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Jd.Common.NetCore.Common.HttpContextUser;
using Jd.Common.NetCore.IService;
using Jd.Common.NetCore.Model;
using Jd.Common.NetCore.Model.Models;
using Microsoft.AspNetCore.Authorization;
using Jd.Common.NetCore.Model.ViewModels.RequestModel." + logicName + @";
using Jd.Common.NetCore.Model.ViewModels.ResponseModel." + logicName + @";
using Jd.Common.NetCore.Api.Filter;

namespace Jd.Common.NetCore.Api.Controllers
{
    /// <summary>
    /// " + remark + @"  
    /// </summary>
    " + $"[Route(\"{area}/" + logicName + "\"/[action])]" + @"
    [ApiController]
    [Authorize]
    [ServiceFilter(typeof(UseServiceDIAttribute))]
    public class " + logicName + @"Controller : ControllerBase
    {
        private readonly I" + logicName + @"Services service;       
        private readonly IUser user;
        public " + logicName + "Controller(I" + logicName + @"Services " + logicName.ToLowerInvariant() + @"Service, IUser user)
        {
            " + "this.service = " + logicName.ToLowerInvariant() + @"Service;    
            this.user = user;
        }

        /// <summary>
        /// (例子)根据id获取数据
        /// </summary>
        " + "/// <param name=\"id\"></param>" + @"
        /// <returns></returns>
        [HttpGet]" + @"
        public async Task Get" + logicName + @"ById(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}
                    ")
                 .CreateClassFile(strPath, strNameSpace);
            return "";
        }
        #endregion

        #region 根据数据库表生成Controll
        /// <summary>
        /// 根据数据库表生成Controll
        /// </summary>
        /// <param name="strPath"></param>
        /// <param name="strNameSpace"></param>
        /// <param name="tableName"></param>
        /// <param name="logicName"></param>
        /// <param name="remark">逻辑备注</param>
        public string Create_Controller_ClassFileFromRepositoryByDBTalbe(string strPath, string strNameSpace, string tableName, string logicName, string remark, string area,bool? restfulAsync)
        {
            area = area == null ? null : Rename.UnderLineToPascal(area);
            string fileName = logicName + "Controller";
            if (string.IsNullOrWhiteSpace(area))
            {
                strPath += "\\Controllers";
            }
            else
            {
                strPath += "\\" + Rename.UnderLineToPascal(area);
            }
            if (File.Exists(strPath + "\\" + fileName + ".cs"))
            {
                return $"Controller层文件 {fileName} 已存在;  ";
            }
            var IDbFirst = _db.DbFirst.Where(tableName);
            _db.MappingTables.Clear();
            _db.MappingTables.Add(fileName, tableName);
            IDbFirst.IsCreateDefaultValue().IsCreateAttribute()
                .SettingClassTemplate(p => p =
@"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JDNetCore.ApiSite.Interface;
using JDNetCore.Entity;
using JDNetCore.Model.DTO;
using JDNetCore.Model.VO;
using JDNetCore.Model.VO.In;
using JDNetCore.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace JDNetCore.ApiSite"+(string.IsNullOrWhiteSpace(area)? ".Controllers" : ("."+area))+
@"
{
    /// <summary>
    /// " + remark + @"  
    /// </summary>
    " + (string.IsNullOrWhiteSpace(area) ? ($"[Route(\"api/" + logicName + "/[action]\")]"): "")+ @"
    " + (string.IsNullOrWhiteSpace(area) ? ($"[ApiController]") : "") + @"
    public partial class " + logicName + @"Controller : "+(string.IsNullOrWhiteSpace(area)? "ControllerBase":("Area"+area+"Controller"))+(restfulAsync==null?"":(", "+(restfulAsync==true? $"IRestfullAsync<{tableName},BQM>" : $"IRestfull<{tableName},BQM>"))) +@"
    {
        private readonly I" + logicName + @"Repository _resp;       
        /// <summary>
        /// 构造...
        /// </summary>
        public " + logicName + "Controller(I" + logicName + @"Repository " + logicName.ToLowerInvariant() + @"Repository)
        {
            " + "this._resp = " + logicName.ToLowerInvariant() + @"Repository;    
        }"
        
        +(restfulAsync==null?"":(restfulAsync==true?(@"
        /// <summary>
        /// 按主键删除
        /// </summary>" +
        @"
        " +
        $"/// <param name=\"id\">主键</param>" +
        @"
        /// <returns></returns>
        public async Task<string> DeleteAsync([FromQuery] string id)
        {
            await _resp.DeleteAsync(id);
            return null;
        }

        /// <summary>
        /// 按主键删除多个
        /// </summary>" +
        @"
        " +
        $"/// <param name=\"ids\">主键数组</param>" +
        @"
        /// <returns></returns>
        public async Task<string> DeleteAllAsync([FromQuery] string[] ids)
        {
            await _resp.DeleteInAsync(ids);
            return null;
        }

        /// <summary>
        /// 按主键获取单条
        /// </summary>" +
        @"
        " +
        $"/// <param name=\"id\">主键</param>" +
        @"
        /// <returns></returns>
        public async Task<" + tableName + @"> GetAsync([FromQuery] string id)
        {
            var result = await _resp.FindAsync(id);
            return result;
        }

        /// <summary>
        /// 按条件获取多条
        /// </summary>" +
        @"
        " +
        $"/// <param name=\"query\">通用查询条件</param>" +
        @"
        /// <returns></returns>
        public async Task<IEnumerable<" + tableName + @">> GetsAsync([FromQuery] BQM query)
        {
            IEnumerable<" + tableName + @"> result = null;
            if (query.page != null && query.per_page != null)
            {
                result = await _resp.PagedAsync(query.page.Value, query.per_page.Value);
            }
            else if (query.limit != null)
            {
                result = await _resp.QueryAsync(query.limit.Value);
            }
            else
            {
                result = await _resp.QueryAsync();
            }
            return result;
        }

        /// <summary>
        /// 部分更新(暂未实现)
        /// </summary>" +
        @"
        " +
        $"/// <param name=\"id\">主键</param>" +
        @"
        " +
        $"/// <param name=\"data\">JsonPatch对象</param>" +
        @"
        /// <returns></returns>
        public Task<string> PatchAsync([FromQuery] string id, [FromBody] JsonPatchDocument<" + tableName + @"> data)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 添加
        /// </summary>" +
        @"
        " +
        $"/// <param name=\"data\">添加对象</param>" +
        @"
        /// <returns></returns>
        public async Task<string> PostAsync([FromBody] " + tableName + @" data)
        {
            await _resp.AddAsync(data);
            return data.id;
        }

        /// <summary>
        /// 全量更新
        /// </summary>" +
        @"
        " +
        $"/// <param name=\"id\">主键</param>" +
        @"
        " +
        $"/// <param name=\"data\">更新对象</param>" +
        @"
        /// <returns></returns>
        public async Task<string> PutAsync([FromQuery] string id, [FromBody] " + tableName + @" data)
        {
            data.id = id;
            await _resp.UpdateAsync(data);
            return data.id;
        }") :(@"
        /// <summary>
        /// 按主键删除
        /// </summary>
        " +
        $"/// <param name=\"id\">主键</param>" +
        @"
        /// <returns></returns>
        public string Delete([FromQuery] string id)
        {
            _resp.Delete(id);
            return null;
        }

        /// <summary>
        /// 按主键删除多个
        /// </summary>
        " +
        $"/// <param name=\"ids\">主键数组</param>" +
        @"
        /// <returns></returns>
        public string DeleteAll([FromQuery] string[] ids)
        {
            _resp.DeleteIn(ids);
            return null;
        }

        /// <summary>
        /// 按主键获取单条
        /// </summary>
        " +
        $"/// <param name=\"id\">主键</param>" +
        @"
        /// <returns></returns>
        public " + tableName + @" Get([FromQuery] string id)
        {
            var result = _resp.Find(id);
            return result;
        }

        /// <summary>
        /// 按条件获取多条
        /// </summary>
        " +
        $"/// <param name=\"query\">通用查询条件</param>" +
        @"
        /// <returns></returns>
        public IEnumerable<" + tableName + @"> Gets([FromQuery] BQM query)
        {
            IEnumerable<" + tableName + @"> result = null;
            if (query.page != null && query.per_page != null)
            {
                result = _resp.Paged(query.page.Value, query.per_page.Value);
            }
            else if (query.limit != null)
            {
                result = _resp.Query(query.limit.Value);
            }
            else
            {
                result = _resp.Query();
            }
            return result;
        }

        /// <summary>
        /// 部分更新(暂未实现)
        /// </summary>
        " +
        $"/// <param name=\"id\">主键</param>" +
        @"
        "+
        $"/// <param name=\"data\">JsonPatch对象</param>" +
        @"
        /// <returns></returns>
        public string Patch([FromQuery] string id, [FromBody] JsonPatchDocument<" + tableName + @"> data)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 添加
        /// </summary>
        " +
        $"/// <param name=\"data\">添加对象</param>" +
        @"
        /// <returns></returns>
        public string Post([FromBody] " + tableName + @" data)
        {
            _resp.Add(data);
            return data.id;
        }

        /// <summary>
        /// 全量更新
        /// </summary>
        " +
        $"/// <param name=\"id\">主键</param>" +
        @"
        "+
        $"/// <param name=\"data\">更新对象</param>" +
        @"
        /// <returns></returns>
        public string Put([FromQuery] string id, [FromBody] " + tableName + @" data)
        {
            data.id = id;
            _resp.Update(data);
            return data.id;
        }")))+
    @"
    }
}
                    ")
                 .CreateClassFile(strPath, strNameSpace);
            return "";
        }
        #endregion

        #region 实例方法
        /// <summary>
        /// 功能描述:获取数据库处理对象
        /// </summary>
        /// <returns>返回值</returns>
        public SimpleClient<T> GetEntityDB<T>() where T : class, new()
        {
            return new SimpleClient<T>(_db);
        }
        /// <summary>
        /// 功能描述:获取数据库处理对象
        /// </summary>
        /// <param name="db">db</param>
        /// <returns>返回值</returns>
        public SimpleClient<T> GetEntityDB<T>(SqlSugarClient db) where T : class, new()
        {
            return new SimpleClient<T>(db);
        }
        #endregion

        #region 根据实体类生成数据库表     
        /// <summary>
        /// 功能描述:根据实体类生成数据库表
        /// </summary>
        /// <param name="blnBackupTable">是否备份表</param>
        /// <param name="lstEntitys">指定的实体</param>
        public void CreateTableByEntity(bool blnBackupTable, params Type[] lstEntitys)
        {
            foreach (var item in lstEntitys)
            {
                if (!item.IsSubclassOf(typeof(SugarEntityBase)))
                {
                    throw new Exception($"实体 {item.Name} 未继承 SugarEntityBase 请确认");
                }
            }
            if (blnBackupTable)
            {
                _db.CodeFirst.BackupTable().InitTables(lstEntitys); //参考 http://www.codeisbug.com/Doc/8/1149   
            }
            else
            {
                _db.CodeFirst.InitTables(lstEntitys);
            }
        }
        #endregion

        #region 静态方法

        /// <summary>
        /// 功能描述:获得一个DbContext
        /// </summary>
        /// <returns></returns>
        public static DBContext GetDbContext()
        {
            return new DBContext();
        }

        /// <summary>
        /// 功能描述:设置初始化参数
        /// </summary>
        /// <param name="strConnectionString">连接字符串</param>
        /// <param name="enmDbType">数据库类型</param>
        public static void Init(string strConnectionString, DbType enmDbType = SqlSugar.DbType.SqlServer)
        {
            _connectionString = strConnectionString;
            _dbType = enmDbType;
        }

        /// <summary>
        /// 功能描述:创建一个链接配置
        /// </summary>
        /// <param name="blnIsAutoCloseConnection">是否自动关闭连接</param>
        /// <param name="blnIsShardSameThread">是否夸类事务</param>
        /// <returns>ConnectionConfig</returns>
        public static ConnectionConfig GetConnectionConfig(bool blnIsAutoCloseConnection = true, bool blnIsShardSameThread = false)
        {
            ConnectionConfig config = new ConnectionConfig()
            {
                ConnectionString = _connectionString,
                DbType = _dbType,
                IsAutoCloseConnection = blnIsAutoCloseConnection,
                ConfigureExternalServices = new ConfigureExternalServices()
                {
                    // DataInfoCacheService = new HttpRuntimeCache()
                },
                IsShardSameThread = blnIsShardSameThread
            };
            return config;
        }



        /// <summary>
        /// 功能描述:获取一个自定义的DB
        /// </summary>
        /// <param name="config">config</param>
        /// <returns>返回值</returns>
        public static SqlSugarClient GetCustomDB(ConnectionConfig config)
        {
            return new SqlSugarClient(config);
        }
        /// <summary>
        /// 功能描述:获取一个自定义的数据库处理对象
        /// </summary>
        /// <param name="sugarClient">sugarClient</param>
        /// <returns>返回值</returns>
        public static SimpleClient<T> GetCustomEntityDB<T>(SqlSugarClient sugarClient) where T : class, new()
        {
            return new SimpleClient<T>(sugarClient);
        }
        /// <summary>
        /// 功能描述:获取一个自定义的数据库处理对象
        /// </summary>
        /// <param name="config">config</param>
        /// <returns>返回值</returns>
        public static SimpleClient<T> GetCustomEntityDB<T>(ConnectionConfig config) where T : class, new()
        {
            SqlSugarClient sugarClient = GetCustomDB(config);
            return GetCustomEntityDB<T>(sugarClient);
        }
        #endregion
    }
}
