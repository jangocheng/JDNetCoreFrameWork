using JDNetCore.Model.VO;
using JDNetCore.Model.VO.In;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JDNetCore.ApiSite.Interface
{
    /// <summary>
    /// Restfull风格的接口定式
    /// </summary>
    /// <typeparam name="DTO">传输模型</typeparam>
    public interface IRestfull<DTO> where DTO : class
    {
        [HttpGet]
        DTO Get([FromQuery]string id);
        [HttpGet]
        IEnumerable<DTO> Gets([FromQuery]BQM query);
        [HttpPost]
        long Post([FromBody]DTO data);
        [HttpPut]
        string Put([FromQuery]string id, [FromBody]DTO data);
        [HttpDelete]
        string Delete([FromQuery]string id);
        [HttpDelete]
        string DeleteAll([FromQuery]string[] ids);
        [HttpPatch]
        string Patch([FromQuery]string id, [FromBody] JsonPatchDocument<DTO> data);
    }

    /// <summary>
    /// Restfull风格的接口定式(Task)
    /// </summary>
    /// <typeparam name="DTO">传输模型</typeparam>
    public interface IRestfullAsync<DTO> where DTO : class
    {
        Task<DTO> Get([FromQuery]string id);
        Task<IEnumerable<DTO>> Gets([FromQuery]BQM query);
        Task<long> Post([FromBody]DTO data);
        Task<string> Put([FromQuery]string id, [FromBody]DTO data);
        Task<string> Delete([FromQuery]string id);
        Task<string> DeleteAll([FromQuery]string[] ids);
        Task<string> Patch([FromQuery]string id, [FromBody] JsonPatchDocument<DTO> data);
    }
}
