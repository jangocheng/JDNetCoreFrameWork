// Copyright 
// CLR版本:            4.0.30319.42000
// 机器名称:           ANDY-PC
// 命名空间/文件名:    JDNetCore.Entity.Sugar/IUnitOfWork 
// 创建人:             研小艾   
// 创建时间:           2020/5/8 18:12:31

using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace JDNetCore.Entity.Sugar
{
    public partial interface IUnitOfWork
    {
        ISqlSugarClient GetDbClient();

        void BeginTran();

        void CommitTran();

        void RollbackTran();
    }
}
