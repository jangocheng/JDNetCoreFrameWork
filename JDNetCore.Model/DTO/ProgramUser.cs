using JDNetCore.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace JDNetCore.Model.DTO
{
    public class ProgramUser:program_user
    {
        public string state_name { set; get; }

        public string type_name { set; get; }
    }
}
