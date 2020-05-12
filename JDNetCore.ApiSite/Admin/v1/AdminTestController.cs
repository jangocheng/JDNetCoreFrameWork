using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JDNetCore.ApiSite.Interface;
using JDNetCore.Model.DTO;
using JDNetCore.Model.VO;
using JDNetCore.Model.VO.In;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace JDNetCore.ApiSite.Admin.v1
{

    /// <summary>
    /// 测试
    /// </summary>
    [AllowAnonymous]
    public class AdminTestController : AreaAdminController, IRestfull<ProgramUser>
    {
        public string Delete([FromQuery] string id)
        {
            throw new NotImplementedException();
        }

        public string DeleteAll([FromQuery] string[] ids)
        {
            throw new NotImplementedException();
        }

        public ProgramUser Get([FromQuery] string id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ProgramUser> Gets([FromQuery] BQM query)
        {
            throw new NotImplementedException();
        }

        public string Patch([FromQuery] string id, [FromBody] JsonPatchDocument<ProgramUser> data)
        {
            throw new NotImplementedException();
        }

        public long Post([FromBody] ProgramUser data)
        {
            throw new NotImplementedException();
        }

        public string Put([FromQuery] string id, [FromBody] ProgramUser data)
        {
            throw new NotImplementedException();
        }
    }
}