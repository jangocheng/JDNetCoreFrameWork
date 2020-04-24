
作者:andy
zy_try@live.cn

项目说明

该项目参考了Blog 和我司实际应用的.net Core项目

技术要点涵盖 
	.net core 
	ioc(autofac)
	orm(sqlsugar)
	log(log4net)
	apidocument(swagger)
	aop(castle)

数据库(按需)
	Sqlserver (目前有成熟维护能力)
	MySql (TiDB亲和性,分布式存储方向)


依赖说明
JDNetCore.Api
	.net core 3.1 站点
	考虑使用Areas切分后台接口和前段接口
JDNetCore.ApiMiddleware
	.net core api 站点的中间件层
	主要负责依赖注入, 定义公共Filter, 中间件使用
JDNetCore.CodeCreater
	代码生成器, 从Entity 生成数据库 ,Respoitory 和 Service
JDNetCore.Common
	公共类库 提供配置读取之类的 最基础方法(所有层都可以引用)
JDNetCore.Common.Interface(文件夹)
	公共接口定义 如ILog ICaching IMessageQueue
	可在Common层提供最基础的实现(前提是最小量依赖)
JDNetCore.Entity
	实体层 (参考实体层命名规范)
JDNetCore.Model
	模型层 (VO和DTO使用文件夹形式, 可按情况拆分)
JDNetCore.Model.VO(文件夹)
	值模型(Value-Object) MVVM中的ViewModel 
	可再细分为RequestModel 和ResponseModel
JDNetCore.Model.DTO(文件夹)
	传输层模型(Data-Transport-Object) 在DAO层和BLL层之间流转的模型
JDNetCore.Respoitory
	仓储层(DAO) 因为使用全类注入的模式 , 所以和接口仓储接口分离
	如果不使用Assembly全类注入并且依赖分离等级降级 完全可以不分离接口层
JDNetCore.Respoitory.Interface
	仓储接口
JDNetCore.Service
	服务层(BLL) 同上
JDNetCore.Service.Interface
	服务接口