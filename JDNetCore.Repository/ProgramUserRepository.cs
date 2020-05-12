// Copyright 
// CLR版本:            4.0.30319.42000
// 机器名称:           ANDY-PC
// 命名空间/文件名:    JDNetCore.Repository/ProgramUserRepository 
// 创建人:             研小艾   
// 创建时间:           2020/5/8 21:15:17

using JDNetCore.Entity;
using JDNetCore.Entity.Sugar;
using JDNetCore.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace JDNetCore.Repository
{
    public partial class ProgramUserRepository : BaseRepository<program_user>, IProgramUserRepository
    {
    }
}
