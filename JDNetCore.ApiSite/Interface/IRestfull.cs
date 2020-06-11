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
    /// <typeparam name="CQM">查询模型</typeparam>
    public interface IRestfull<DTO,CQM> where DTO : class where CQM :BQM
    {
        [HttpGet]
        DTO Get([FromQuery]string id);
        [HttpGet]
        IEnumerable<DTO> Gets([FromQuery]CQM query);
        [HttpPost]
        string Post([FromBody]DTO data);
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
    /// <typeparam name="CQM">查询模型</typeparam>
    public interface IRestfullAsync<DTO,CQM> where DTO : class where CQM : BQM
    {
        Task<DTO> GetAsync([FromQuery]string id);
        Task<IEnumerable<DTO>> GetsAsync([FromQuery]CQM query);
        Task<string> PostAsync([FromBody]DTO data);
        Task<string> PutAsync([FromQuery]string id, [FromBody]DTO data);
        Task<string> DeleteAsync([FromQuery]string id);
        Task<string> DeleteAllAsync([FromQuery]string[] ids);
        Task<string> PatchAsync([FromQuery]string id, [FromBody] JsonPatchDocument<DTO> data);
    }
}
