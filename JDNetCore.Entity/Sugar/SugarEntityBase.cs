using JDNetCore.Entity.Base;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace JDNetCore.Entity.Sugar
{
    /// <summary>
    /// sqlsugar实体父类
    /// </summary>
    public partial class SugarEntityBase
    {
        /// <summary>
        /// 主键
        /// </summary>
        [SugarColumn(IsPrimaryKey = true,Length = 19, ColumnDescription="主键")]
        public string id { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        [SugarColumn(ColumnDescription = "是否删除")]
        public bool is_delete { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [SugarColumn(ColumnDescription = "状态")]
        public int state { set; get; }

        /// <summary>
        /// 创建人
        /// </summary>
        [SugarColumn(Length = 19, ColumnDescription = "创建人")]
        public string created_id { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [SugarColumn(ColumnDescription = "创建时间")]
        public DateTime created_on { get; set; }

        /// <summary>
        /// 更新人
        /// </summary>
        [SugarColumn(Length = 19, ColumnDescription = "更新人")]
        public string modifid_id { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        [SugarColumn(ColumnDescription = "更新时间")]
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
}
