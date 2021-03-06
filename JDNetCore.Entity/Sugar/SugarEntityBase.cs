﻿using JDNetCore.Entity.Base;
using Newtonsoft.Json;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace JDNetCore.Entity.Sugar
{
    /// <summary>
    /// sqlsugar实体父类 使用Oracle命名规范
    /// </summary>
    public abstract partial class SugarEntityBase
    {
        /// <summary>
        /// 主键
        /// </summary>
        [SugarColumn(IsPrimaryKey = true,Length = 19,ColumnName ="ID", ColumnDescription="主键")]
        [JsonProperty(PropertyName = "ID")]
        public string id { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        [SugarColumn(ColumnName="ISDELETED", ColumnDescription = "是否删除")]
        [JsonProperty(PropertyName = "ISDELETED")]
        public bool is_delete { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [SugarColumn(ColumnName = "STATUS", ColumnDescription = "状态")]
        [JsonProperty(PropertyName = "STATUS")]
        public int state { set; get; }

        /// <summary>
        /// 创建人
        /// </summary>
        [SugarColumn(ColumnName= "CREATEDBY", Length = 19, ColumnDescription = "创建人")]
        [JsonProperty(PropertyName = "CREATEDBY")]
        public string created_id { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [SugarColumn(ColumnName = "CREATEDON", ColumnDescription = "创建时间")]
        [JsonProperty(PropertyName = "CREATEDON")]
        public DateTime created_on { get; set; }

        /// <summary>
        /// 更新人
        /// </summary>
        [SugarColumn(ColumnName = "LASTUPDATEDBY", Length = 19, ColumnDescription = "更新人")]
        [JsonProperty(PropertyName = "LASTUPDATEDBY")]
        public string modifid_id { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        [SugarColumn(ColumnName = "LASTUPDATEON", ColumnDescription = "更新时间")]
        [JsonProperty(PropertyName = "LASTUPDATEON")]
        public DateTime modifid_on { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public SugarEntityBase()
        {
            //雪花id
            id = SnowSeed.NewID.ToString();
            created_id = modifid_id = "0000000000000000000";
            modifid_on = created_on = DateTime.Now;
            state = 1;
            is_delete = false;
            OnCreate(this);
        }

        public virtual void OnCreate(SugarEntityBase entity)
        {
        }
    }

    #region 
    //public abstract partial class SugarEntityBase
    //{
    //    /// <summary>
    //    /// 主键
    //    /// </summary>
    //    [SugarColumn(IsPrimaryKey = true, Length = 19, ColumnDescription = "主键")]
    //    public string id { get; set; }

    //    /// <summary>
    //    /// 是否删除
    //    /// </summary>
    //    [SugarColumn(ColumnDescription = "是否删除")]
    //    public bool is_delete { get; set; }

    //    /// <summary>
    //    /// 状态
    //    /// </summary>
    //    [SugarColumn(ColumnDescription = "状态")]
    //    public int state { set; get; }

    //    /// <summary>
    //    /// 创建人
    //    /// </summary>
    //    [SugarColumn(Length = 19, ColumnDescription = "创建人")]
    //    public string created_id { get; set; }

    //    /// <summary>
    //    /// 创建时间
    //    /// </summary>
    //    [SugarColumn(ColumnDescription = "创建时间")]
    //    public DateTime created_on { get; set; }

    //    /// <summary>
    //    /// 更新人
    //    /// </summary>
    //    [SugarColumn(Length = 19, ColumnDescription = "更新人")]
    //    public string modifid_id { get; set; }

    //    /// <summary>
    //    /// 更新时间
    //    /// </summary>
    //    [SugarColumn(ColumnDescription = "更新时间")]
    //    public DateTime modifid_on { get; set; }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public SugarEntityBase()
    //    {
    //        //雪花id
    //        id = SnowSeed.NewID.ToString();
    //        created_id = modifid_id = "0000000000000000000";
    //        modifid_on = created_on = DateTime.Now;
    //        state = 1;
    //        is_delete = false;
    //        OnCreate(this);
    //    }

    //    public virtual void OnCreate(SugarEntityBase entity)
    //    {
    //    }
    //}
    #endregion
}
