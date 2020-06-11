
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JDNetCore.ApiSite.Interface;
using JDNetCore.Entity;
using JDNetCore.Model.DTO;
using JDNetCore.Model.VO;
using JDNetCore.Model.VO.In;
using JDNetCore.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace JDNetCore.ApiSite.Admin
{
    /// <summary>
    /// 测试  
    /// </summary>
    
    //[Authorize]
    public partial class TestController : AreaAdminController, IRestfullAsync<Test,BQM>
    {
        private readonly ITestRepository _resp;       
        /// <summary>
        /// 构造...
        /// </summary>
        public TestController(ITestRepository testRepository)
        {
            this._resp = testRepository;    
        }
        /// <summary>
        /// 按主键删除
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public async Task<string> DeleteAsync([FromQuery] string id)
        {
            var one = await _resp.FindAsync(id);
            if (one == null) return null;
            await _resp.DeleteAsync(id);
            return null;
        }

        /// <summary>
        /// 按主键删除多个
        /// </summary>
        /// <param name="ids">主键数组</param>
        /// <returns></returns>
        public async Task<string> DeleteAllAsync([FromQuery] string[] ids)
        {
            await _resp.DeleteInAsync(ids);
            return null;
        }

        /// <summary>
        /// 按主键获取单条
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public async Task<Test> GetAsync([FromQuery] string id)
        {
            var result = await _resp.FindAsync(id);
            return result;
        }

        /// <summary>
        /// 按条件获取多条
        /// </summary>
        /// <param name="query">通用查询条件</param>
        /// <returns></returns>
        public async Task<IEnumerable<Test>> GetsAsync([FromQuery] BQM query)
        {
            IEnumerable<Test> result = null;
            if (query.page != null && query.per_page != null)
            {
                result = await _resp.PagedAsync(query.page.Value, query.per_page.Value);
            }
            else if (query.limit != null)
            {
                result = await _resp.QueryAsync(query.limit.Value);
            }
            else
            {
                result = await _resp.QueryAsync();
            }
            return result;
        }

        /// <summary>
        /// 部分更新(暂未实现)
        /// </summary>
        /// <param name="id">主键</param>
        /// <param name="data">JsonPatch对象</param>
        /// <returns></returns>
        public Task<string> PatchAsync([FromQuery] string id, [FromBody] JsonPatchDocument<Test> data)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="data">添加对象</param>
        /// <returns></returns>
        public async Task<string> PostAsync([FromBody] Test data)
        {
            await _resp.AddAsync(data);
            return data.id;
        }

        /// <summary>
        /// 全量更新
        /// </summary>
        /// <param name="id">主键</param>
        /// <param name="data">更新对象</param>
        /// <returns></returns>
        public async Task<string> PutAsync([FromQuery] string id, [FromBody] Test data)
        {
            data.id = id;
            await _resp.UpdateAsync(data);
            return data.id;
        }
    }
}
                    