using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JDNetCore.ApiSite.Admin
{
    [Route("admin/[controller]/[action]")]
    [ApiController]
    public abstract class AreaAdminController : ControllerBase
    {
    }
}