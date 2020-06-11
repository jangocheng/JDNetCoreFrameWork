// Copyright 
// CLR版本:            4.0.30319.42000
// 机器名称:           ANDY-PC
// 命名空间/文件名:    JDNetCore.Entity.Base/RepositoryIgnoreAttribute 
// 创建人:             研小艾   
// 创建时间:           2020/5/13 11:01:00

using System;
using System.Collections.Generic;
using System.Text;

namespace JDNetCore.Entity.Base
{
    [AttributeUsage(AttributeTargets.Class,AllowMultiple =false)]
    public class RepositoryIgnoreAttribute:Attribute
    {
    }
}
