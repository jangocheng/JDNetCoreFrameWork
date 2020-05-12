using System;
using System.Collections.Generic;
using System.Text;

namespace JDNetCore.Common.Interface
{
    /// <summary>
    /// 缓存
    /// </summary>
    public interface ICache
    {
        /// <summary>
        /// 按键存储值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="obj">值</param>
        /// <param name="dt">过期时间</param>
        void Set(string key, object obj, DateTime dt);
        /// <summary>
        /// 按键存储值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="obj">值</param>
        void Set(string key, object obj);
        /// <summary>
        /// 按键存储值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="obj">值</param>
        /// <param name="dt">过期时间</param>
        void Set(string key, object obj, TimeSpan dt);
        /// <summary>
        /// 按键获取值
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>值</returns>
        object Get(string key);
        /// <summary>
        /// 按键删除值
        /// </summary>
        /// <param name="key">键</param>
        void Remove(string key);
        /// <summary>
        /// 键模糊删除
        /// </summary>
        /// <param name="key">模糊键</param>
        void FilterRemove(string key);

        /// <summary>
        /// 按键获取强类型值
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">键</param>
        /// <returns>值</returns>
        T Get<T>(string key) where T : class;
    }
}
