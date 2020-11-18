using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CacheHelper
{
    internal interface ICacheAssemble
    {

        #region 异步

        /// <summary>
        /// 设置对象缓存且支持基础数据类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        Task<bool> SetAsync<T>(string key, T obj, TimeSpan? expiry = default(TimeSpan?));

        /// <summary>
        /// 设置List对象缓存且支持基础数据类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="expiry"></param>
        /// <returns>失败的键值</returns>
        Task<List<string>> SetListAsync<T>(Dictionary<string, object> obj, TimeSpan? expiry = default(TimeSpan?));

        /// <summary>
        /// 通过key删除缓存
        /// </summary> 
        /// <param name="key"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync(string key);

        /// <summary>
        /// 通过List key删除缓存
        /// </summary> 
        /// <param name="keys"></param>
        /// <returns></returns>
        Task<List<string>> DeleteListAsync(List<string> keys);

        /// <summary>
        /// 查询对象缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<T> GetAsync<T>(string key);

        /// <summary>
        /// 查询对象缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keys"></param>
        /// <returns></returns>
        Task<Dictionary<string, T>> GetListAsync<T>(List<string> keys);

        /// <summary>
        /// 查询字符串缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<string> GetStringAsync(string key);

        /// <summary>
        /// 查询List字符串缓存
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        Task<Dictionary<string, string>> GetStringListAsync(List<string> keys);

        /// <summary>
        /// 修改缓存时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        Task<bool> UpdateExpiryAsync(string key, TimeSpan expiry);

        /// <summary>
        /// 修改List key缓存时间
        /// </summary>
        /// <param name="keys"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        Task<List<string>> UpdateExpiryListAsync(List<string> keys, TimeSpan expiry);


        /// <summary>
        /// 是否存在指定键值的缓存对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<bool> ExistsAsync(string key);

        #endregion

        #region 同步

        /// <summary>
        /// 设置对象缓存且支持基础数据类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        bool Set<T>(string key, T obj, TimeSpan? expiry = default(TimeSpan?));

        /// <summary>
        /// 设置List对象缓存且支持基础数据类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="expiry"></param>
        /// <returns>失败的键值</returns>
        List<string> SetList<T>(Dictionary<string, object> obj, TimeSpan? expiry = default(TimeSpan?));

        /// <summary>
        /// 通过key删除缓存
        /// </summary> 
        /// <param name="key"></param>
        /// <returns></returns>
        bool Delete(string key);

        /// <summary>
        /// 通过List key删除缓存
        /// </summary> 
        /// <param name="key"></param>
        /// <returns></returns>
        List<string> DeleteList(List<string> keys);

        /// <summary>
        /// 查询对象缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        T Get<T>(string key);

        /// <summary>
        /// 查询对象缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        Dictionary<string, T> GetList<T>(List<string> keys);

        /// <summary>
        /// 查询字符串缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string GetString(string key);

        /// <summary>
        /// 查询List字符串缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Dictionary<string, string> GetStringList(List<string> keys);

        /// <summary>
        /// 修改缓存时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        bool UpdateExpiry(string key, TimeSpan expiry);

        /// <summary>
        /// 修改List key缓存时间
        /// </summary>
        /// <param name="keys"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        List<string> UpdateExpiryList(List<string> keys, TimeSpan expiry);

        /// <summary>
        /// 是否存在指定键值的缓存对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool Exists(string key);

        long Increase(string key, int value = 1);

        long Decrease(string key, int value = 1);

        Task<long> IncreaseAsync(string key, int value = 1);

        Task<long> DecreaseAsync(string key, int value = 1);

        #endregion


    }
}
