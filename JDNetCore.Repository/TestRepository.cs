
using JDNetCore.Entity;
using JDNetCore.Entity.Sugar;
using JDNetCore.Repository.Interface;
using System;
using System.Collections.Generic;

namespace JDNetCore.Repository
{
	/// <summary>
	/// 自动生成 
	/// </summary>
    public partial class TestRepository : BaseRepository<Test>, ITestRepository
    {
        public TestRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
