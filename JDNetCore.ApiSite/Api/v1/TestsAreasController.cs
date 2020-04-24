using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JDNetCore.ApiSite.Api.v1
{

    public class TestsAreasController : AreaApiController
    {
        public dynamic get()
        {
            return "TestsAreasController/get";
        }
    }
}