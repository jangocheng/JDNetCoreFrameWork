using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JDNetCore.ApiSite.Api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [AllowAnonymous]
    public abstract class AreaApiController : ControllerBase
    {
    }
}