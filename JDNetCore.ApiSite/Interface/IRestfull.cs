using JDNetCore.Model.VO;
using JDNetCore.Model.VO.In;
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
    /// <typeparam name="QueryModel">查询模型</typeparam>
    public interface IRestfull<DTO,QueryModel> where QueryModel: BQM
    {
        [HttpGet]
        DTO Get([FromQuery]long id);
        [HttpGet]
        IEnumerable<DTO> Gets([FromQuery]QueryModel query);
        [HttpPost]
        long Post([FromBody]DTO data);
        [HttpPut]
        string Put([FromQuery]long id, [FromBody]DTO data);
        [HttpDelete]
        string Delete([FromQuery]long id);
        [HttpDelete]
        string DeleteAll([FromQuery]long[] ids);
        [HttpPatch]
        string Patch([FromQuery]long id, [FromQuery]string filter, [FromBody]DTO data);
    }

    /// <summary>
    /// Restfull风格的接口定式(Task)
    /// </summary>
    /// <typeparam name="DTO">传输模型</typeparam>
    /// <typeparam name="QueryModel">查询模型</typeparam>
    public interface IRestfullAsync<DTO, QueryModel> where QueryModel : BQM
    {
        Task<DTO> Get([FromQuery]long id);
        Task<IEnumerable<DTO>> Gets([FromQuery]QueryModel query);
        Task<long> Post([FromBody]DTO data);
        Task<string> Put([FromQuery]long id, [FromBody]DTO data);
        Task<string> Delete([FromQuery]long id);
        Task<string> DeleteAll([FromQuery]long[] ids);
        Task<string> Patch([FromQuery]long id, [FromQuery]string filter, [FromBody]DTO data);
    }
}
