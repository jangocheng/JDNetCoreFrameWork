// Copyright 
// CLR版本:            4.0.30319.42000
// 机器名称:           ANDY-PC
// 命名空间/文件名:    JDNetCore.Entity.Sugar/UnitOfWork 
// 创建人:             研小艾   
// 创建时间:           2020/5/11 9:42:17

using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace JDNetCore.Entity.Sugar
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly List<ISqlSugarClient> _sqlSugarClients;

        public UnitOfWork(List<ISqlSugarClient> sqlSugarClients)
        {
            _sqlSugarClients = sqlSugarClients;
        }

        public void BeginTran()
        {
            GetDbClient().Ado.BeginTran();
        }

        public void CommitTran()
        {
            try
            {
                GetDbClient().Ado.CommitTran();
            }
            catch (Exception ex)
            {
                GetDbClient().Ado.RollbackTran();
                throw ex;
            }
        }

        public ISqlSugarClient GetDbClient()
        {
            return _sqlSugarClients[0];
        }

        public void RollbackTran()
        {
            GetDbClient().Ado.RollbackTran();
        }
    }
}
