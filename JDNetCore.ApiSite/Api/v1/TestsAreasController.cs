using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JDNetCore.ApiSite.Api.v1
{
    [Route("identity")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        public dynamic Get()
        {
            return (from c in User.Claims select new { c.Type, c.Value });
        }
    }
}