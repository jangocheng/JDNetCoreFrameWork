using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using JDNetCore.Common;
using JDNetCore.Entity.Sugar;
using JDNetCore.Model.VO.In;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JDNetCore.ApiSite.Controllers
{

    /// <summary>
    /// 结构产生器
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CodeRepositoryAndServiceController : ControllerBase
    {
        /// <summary>
        /// 创建控制器
        /// </summary>
        /// <returns></returns>
        public dynamic GetController([FromQuery]ControllerIniter data)
        {
            if (Appsettings.app<string>("Environment") != "Development")
            {
                throw new Exception("非开发环境");
            }
            string dir = Directory.GetCurrentDirectory();
            string path = dir.Substring(0, dir.LastIndexOf("\\"));
            try
            {
                var logicName = Rename.UnderLineToPascal(data.tablename);
                DBContext context = new DBContext();
                var irespStr = context.Create_IRepository_ClassFileByDBTalbe(path + @"\JDNetCore.Repository.Interface", "JDNetCore.Repository.Interface", data.tablename, logicName, data.remark ?? "自动生成");
                var respStr = context.Create_Repository_ClassFileByDBTalbe(path + @"\JDNetCore.Repository", "JDNetCore.Repository", data.tablename, logicName, data.remark ?? "自动生成");
                var isvStr = "跳过";
                var svStr = "跳过";
                var apiStr = data.fromService ? "从服务层注入" : "从仓储层注入";
                if (data.fromService)
                {

                }
                else
                {
                    context.Create_Controller_ClassFileFromRepositoryByDBTalbe(path + @"\JDNetCore.ApiSite\", "JDNetCore.ApiSite.Controllers", data.tablename, logicName, data.remark ?? "自动产生", data.area,data.restfulAsync);
                }

                return new { 仓储接口 = irespStr, 仓储实现 = respStr, 服务接口 = isvStr, 服务实现 = svStr, 控制器 = apiStr };
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }
}