// Copyright 
// CLR版本:            4.0.30319.42000
// 机器名称:           ANDY-PC
// 命名空间/文件名:    JDNetCore.ApiSite.Filter/OutputFilter 
// 创建人:             研小艾   
// 创建时间:           2020/5/18 10:57:20

using JDNetCore.Entity.Sugar;
using JDNetCore.Model.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace JDNetCore.ApiSite.Filter
{
    public class OutputFilter : IResultFilter
    {
        public void OnResultExecuted(ResultExecutedContext context)
        {
            
        }

        public class OutPutFormat
        {

            public OutPutFormat(dynamic value)
            {
                data = value;
            }
            public dynamic data { set; get; }
        }

        public class OutPutPaged
        {
            public OutPutPaged(IPagedList plist)
            {
                data = plist.data;
                pageIndex = plist.pageIndex;
                pageSize = plist.pageSize;
                totalCount = plist.totalCount;
                totalPage = plist.totalPage;
                hasNext = plist.hasNext;
                hasPrev = plist.hasPrev;
            }

            public dynamic data { private set; get; }

            public int pageSize { private set; get; }

            public int pageIndex { private set; get; }

            public int totalCount { private set; get; }

            public bool hasNext { private set; get; }

            public bool hasPrev { private set; get; }

            public int totalPage { private set; get; }
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = new UnprocessableEntityObjectResult(context.ModelState);
                return;
            }

            if (context.Result is ObjectResult)
            {
                var val = (context.Result as ObjectResult).Value;
                var method = context.HttpContext.Request.Method.ToUpper();
                if (val == null || method == "DELETE" || method == "PUT" || method == "PATCH")
                {
                    context.Result = new NoContentResult();
                } 
                else if (method =="POST") 
                {
                    var request = context.HttpContext.Request;
                    context.Result = new CreatedResult(request.Scheme + "://" + request.Host + (request.Path.Value.Replace(request.RouteValues["action"].ToString(), "Get")) + "?id=" + val.ToString(), val);
                }
                else if (val is IPagedList)
                {
                    var result = (val as IPagedList);
                    context.Result = new ObjectResult(new OutPutPaged(result));
                }
                else
                {
                    context.Result = new ObjectResult(new OutPutFormat(val));
                }
            }
        }

    }

    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is FriendlyException)
            {
                var friendly = context.Exception as FriendlyException;
                var result = new ModelStateDictionary();
                result.AddModelError(friendly.field, friendly.message);
                context.Result = new UnprocessableEntityObjectResult(result);
            }
            else if (context.Exception is NotImplementedException)
            {
                context.Result = new ObjectResult("接口未实现") { StatusCode = 501 };
            }
            // patch 请求由于未知问题会引起 NotSupportedException 异常
            else if (context.Exception is NotSupportedException)
            {

            }
            else
            {
                // todo 这里需要记录日志
                context.Result = new ObjectResult("请联系管理员,或稍后再试") { StatusCode = 500 };
            }
        }
    }
}
