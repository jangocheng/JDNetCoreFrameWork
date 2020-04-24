using System;
using System.Collections.Generic;
using System.Text;

namespace JDNetCore.Entity.Sugar
{
    public class MutiDBOperate
    {
        public string ConnId { get; set; }
        public string Connection { set; get; }
        public DataBaseType DbType { get; set; }

        public bool Enabled { set; get; }
        public string ProviderName { set; get; }


    }
}
