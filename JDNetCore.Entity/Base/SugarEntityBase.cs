using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace JDNetCore.Entity.Base
{
    /// <summary>
    /// sqlsugar实体父类
    /// </summary>
    public class SugarEntityBase
    {
        /// <summary>
        /// 主键
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public long id { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool is_delete { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int state { set; get; }

        /// <summary>
        /// 创建人
        /// </summary>
        public long created_id { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime created_on { get; set; }

        /// <summary>
        /// 更新人
        /// </summary>
        public long modifid_id { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime modifid_on { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public SugarEntityBase()
        {
            //雪花id
            id = SnowSeed.NewID;
            created_id = 0;
            created_on = DateTime.Now;
            modifid_id = 0;
            modifid_on = DateTime.Now;
            state = 1;
            is_delete = false;
        }
    }
}
