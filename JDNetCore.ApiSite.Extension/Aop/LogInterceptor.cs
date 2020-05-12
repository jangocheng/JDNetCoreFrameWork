// Copyright 
// CLR版本:            4.0.30319.42000
// 机器名称:           ANDY-PC
// 命名空间/文件名:    JDNetCore.ApiSite.Aop/LogInterceptor 
// 创建人:             研小艾   
// 创建时间:           2020/5/6 15:05:48

using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Text;

namespace JDNetCore.ApiSite.Aop
{
    public class LogInterceptor : IInterceptor
    {
        private readonly LogInterceptorAsync _logInterceptorAsync;

        public LogInterceptor(LogInterceptorAsync logInterceptorAsync)
        {
            _logInterceptorAsync = logInterceptorAsync;
        }

        public void Intercept(IInvocation invocation)
        {
            _logInterceptorAsync.ToInterceptor().Intercept(invocation);
        }
    }
}
