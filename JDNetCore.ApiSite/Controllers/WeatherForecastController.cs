using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace JDNetCore.ApiSite.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    ////[AllowAnonymous]
    //[Authorize]
    public class WeatherForecastController : ControllerBase
    {
        public string Get()
        {
            return "from JDNetCore.ApiSite";
        }
    }
}
