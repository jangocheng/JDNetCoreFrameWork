// Copyright 
// CLR版本:            4.0.30319.42000
// 机器名称:           ANDY-PC
// 命名空间/文件名:    JDNetCore.Entity/Test 
// 创建人:             研小艾   
// 创建时间:           2020/5/15 10:37:00

using JDNetCore.Entity.Sugar;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace JDNetCore.Entity
{
    public class Test:SugarEntityBase
    {
        [Required(ErrorMessage ="请输入姓名")]
        public string NAME { set; get; }
    }
}
