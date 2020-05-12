using System;
using System.Collections.Generic;
using System.Text;

namespace JDNetCore.Model.VO.In
{
    /// <summary>
    /// BaseQueryModel
    /// </summary>
    public class BQM
    {
        public BQM()
        {
            sortby = "id";
            order = "desc";
            states = new List<int>();
            types = new List<int>();
        }
        /// <summary>
        /// 取limit条
        /// </summary>
        public virtual int? limit { set; get; }
        /// <summary>
        /// 分页参数 第page页
        /// </summary>
        public virtual int? page { set; get; }
        /// <summary>
        /// 分页参数 一页per_page条
        /// </summary>
        public virtual int? per_page { set; get; }
        /// <summary>
        /// 排序参数 排序依据
        /// </summary>
        public virtual string sortby { set; get; }
        /// <summary>
        /// 排序参数 升序/降序(asc/desc)
        /// </summary>
        public virtual string order { set; get; }
        /// <summary>
        /// 状态
        /// </summary>
        public virtual int? state { set; get; }
        /// <summary>
        /// 类型
        /// </summary>
        public virtual int? type { set; get; }
        /// <summary>
        /// 多状态
        /// </summary>
        public virtual ICollection<int> states { set; get; }
        /// <summary>
        /// 多类型
        /// </summary>
        public virtual ICollection<int> types { set; get; }
        /// <summary>
        /// 查询字段
        /// </summary>
        public virtual string search_word { set; get; }

        private DateTime? _start_on;
        /// <summary>
        /// 开始时间
        /// </summary>
        public virtual DateTime? start_on 
        {
            set
            {
                _start_on = value;
            }
            get
            {
                if (_start_on != null) return DateTime.Parse(_start_on.Value.ToString("yyyy/MM/dd 00:00:00"));
                else return null;
            }
        }

        private DateTime? _end_on;
        /// <summary>
        /// 结束时间
        /// </summary>
        public virtual DateTime? end_on {
            set
            {
                _end_on = value;
            }
            get
            {
                if (_end_on != null) return DateTime.Parse(_end_on.Value.ToString("yyyy/MM/dd 23:59:59"));
                else return null;
            }
        }

        /// <summary>
        /// 多id
        /// </summary>
        public virtual ICollection<string> ids { set; get; }
        /// <summary>
        /// 用户id
        /// </summary>
        public virtual string user_id { set; get; }

    }
}
