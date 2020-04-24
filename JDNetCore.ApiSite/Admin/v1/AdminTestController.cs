using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JDNetCore.ApiSite.Interface;
using JDNetCore.Model.DTO;
using JDNetCore.Model.VO;
using JDNetCore.Model.VO.In;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JDNetCore.ApiSite.Admin.v1
{
    /// <summary>
    /// 测试
    /// </summary>
    public class AdminTestController : AreaAdminController, IRestfull<ProgramUser, BQM>
    {
        public string Delete([FromQuery] long id)
        {
            throw new NotImplementedException();
        }

        public string DeleteAll([FromQuery] long[] ids)
        {
            throw new NotImplementedException();
        }

        public ProgramUser Get([FromQuery] long id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ProgramUser> Gets([FromQuery] BQM query)
        {
            throw new NotImplementedException();
        }

        public string Patch([FromQuery] long id, [FromQuery] string filter, [FromBody] ProgramUser data)
        {
            throw new NotImplementedException();
        }

        public long Post([FromBody] ProgramUser data)
        {
            throw new NotImplementedException();
        }

        public string Put([FromQuery] long id, [FromBody] ProgramUser data)
        {
            throw new NotImplementedException();
        }
    }
}