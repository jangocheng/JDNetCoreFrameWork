// Copyright 
// CLR版本:            4.0.30319.42000
// 机器名称:           ANDY-PC
// 命名空间/文件名:    JDNetCore.Common/MemoryCache 
// 创建人:             研小艾   
// 创建时间:           2020/5/6 14:36:25

using JDNetCore.Common.Interface;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace JDNetCore.Common
{
    public class MemoryCacheExt : ICache
    {
        static MemoryCacheExt() {
            _cache = new MemoryCache(Options.Create(new MemoryCacheOptions()));
        }
        private static MemoryCache _cache; 

        public void FilterRemove(string key)
        {
            throw new NotImplementedException();
        }

        public object Get(string key)
        {
            _cache.TryGetValue(key, out object result);
            return result;
        }

        public T Get<T>(string key) where T : class
        {
            _cache.TryGetValue<T>(key, out T result);

            return result;
        }

        public void Remove(string key)
        {
            _cache.Remove(key);
        }

        public void Set(string key, object obj, DateTime dt)
        {
            _cache.Set(key, obj, dt);
        }

        public void Set(string key, object obj)
        {
            _cache.Set(key, obj);
        }

        public void Set(string key, object obj, TimeSpan dt)
        {
            _cache.Set(key, obj, dt);
        }
    }
}
