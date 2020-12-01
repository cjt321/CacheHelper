using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CacheHelper.CacheAssembleHelper.MemcachedHelper;
using CacheHelper.CacheAssembleHelper.RedisHelper;
using CacheHelper.CacheAssembleHelper.WebCacheHelper;
using CacheHelper.Configuration;

namespace CacheHelper.CacheAssembleHelper
{
    public class CachedHelper
    {

        //private static readonly CacheEnum DefaultCacheType = CacheConfiguration.DefaultCacheType;

        #region 异步

        #region Set

        /// <summary>
        /// 设置对象缓存且支持基础数据类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <param name="expiry"></param>
        /// <param name="cacheType"></param>
        /// <returns></returns>
        public static async Task<bool> SetAsync<T>(string key, T obj, TimeSpan? expiry = default(TimeSpan?), CacheEnum cacheType = 0)
        {
            //读取缓存。读取默认缓存与方法传进来的缓存，以方法传进来的缓存为优先级最高。
            cacheType = GetCacheType(cacheType);
            ICacheAssemble cache = GetCache(cacheType);

            return await cache.SetAsync(key, obj, expiry);
        }

        /// <summary>
        /// 设置List对象缓存且支持基础数据类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="expiry"></param>
        /// <param name="cacheType"></param>
        /// <returns>失败的键值</returns>
        public static async Task<List<string>> SetListAsync<T>(Dictionary<string, object> obj, TimeSpan? expiry = default(TimeSpan?), CacheEnum cacheType = 0)
        {
            cacheType = GetCacheType(cacheType);
            ICacheAssemble cache = GetCache(cacheType);

            return await cache.SetListAsync<T>(obj, expiry);
        }

        #endregion

        #region Delete

        /// <summary>
        /// 通过key删除缓存
        /// </summary> 
        /// <param name="key"></param>
        /// <returns></returns>
        public static async Task<bool> DeleteAsync(string key, CacheEnum cacheType = 0)
        {
            cacheType = GetCacheType(cacheType);
            ICacheAssemble cache = GetCache(cacheType);

            return await cache.DeleteAsync(key);
        }

        /// <summary>
        /// 通过List key删除缓存
        /// </summary> 
        /// <param name="keys"></param>
        /// <param name="cacheType"></param>
        /// <returns></returns>
        public static async Task<List<string>> DeleteListAsync(List<string> keys, CacheEnum cacheType = 0)
        {
            cacheType = GetCacheType(cacheType);
            ICacheAssemble cache = GetCache(cacheType);

            return await cache.DeleteListAsync(keys);
        }

        #endregion

        #region Get

        /// <summary>
        /// 查询对象缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="cacheType"></param>
        /// <returns></returns>
        public static async Task<T> GetAsync<T>(string key, CacheEnum cacheType = 0)
        {
            cacheType = GetCacheType(cacheType);
            ICacheAssemble cache = GetCache(cacheType);

            return await cache.GetAsync<T>(key);
        }

        /// <summary>
        /// 查询对象缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keys"></param>
        /// <param name="cacheType"></param>
        /// <returns></returns>
        public static async Task<Dictionary<string, T>> GetListAsync<T>(List<string> keys, CacheEnum cacheType = 0)
        {
            cacheType = GetCacheType(cacheType);
            ICacheAssemble cache = GetCache(cacheType);

            return await cache.GetListAsync<T>(keys);
        }

        /// <summary>
        /// 查询字符串缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="cacheType"></param>
        /// <returns></returns>
        public static async Task<string> GetStringAsync(string key, CacheEnum cacheType = 0)
        {
            cacheType = GetCacheType(cacheType);
            ICacheAssemble cache = GetCache(cacheType);

            return await cache.GetStringAsync(key);
        }

        /// <summary>
        /// 查询List字符串缓存
        /// </summary>
        /// <param name="keys"></param>
        /// <param name="cacheType"></param>
        /// <returns></returns>
        public static async Task<Dictionary<string, string>> GetStringListAsync(List<string> keys, CacheEnum cacheType = 0)
        {
            cacheType = GetCacheType(cacheType);
            ICacheAssemble cache = GetCache(cacheType);

            return await cache.GetStringListAsync(keys);
        }

        #endregion

        #region Update

        /// <summary>
        /// 修改缓存时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="expiry"></param>
        /// <param name="cacheType"></param>
        /// <returns></returns>
        public static async Task<bool> UpdateExpiryAsync(string key, TimeSpan expiry, CacheEnum cacheType = 0)
        {
            cacheType = GetCacheType(cacheType);
            ICacheAssemble cache = GetCache(cacheType);

            return await cache.UpdateExpiryAsync(key, expiry);
        }

        /// <summary>
        /// 修改List key缓存时间
        /// </summary>
        /// <param name="keys"></param>
        /// <param name="expiry"></param>
        /// <param name="cacheType"></param>
        /// <returns></returns>
        public static async Task<List<string>> UpdateExpiryListAsync(List<string> keys, TimeSpan expiry, CacheEnum cacheType = 0)
        {
            cacheType = GetCacheType(cacheType);
            ICacheAssemble cache = GetCache(cacheType);

            return await cache.UpdateExpiryListAsync(keys, expiry);
        }

        #endregion

        #region list

        public static async Task<long> ListRemoveAsync<T>(string key, T value, CacheEnum cacheType = 0)
        {
            cacheType = GetCacheType(cacheType);
            ICacheAssemble cache = GetCache(cacheType);

            return await cache.ListRemoveAsync(key, value);
        }

        public static async Task<List<T>> ListRangeAsync<T>(string key, CacheEnum cacheType = 0)
        {
            cacheType = GetCacheType(cacheType);
            ICacheAssemble cache = GetCache(cacheType);

            return await cache.ListRangeAsync<T>(key);
        }

        public static async Task<long> ListRightPushAsync<T>(string key, T value, CacheEnum cacheType = 0)
        {
            cacheType = GetCacheType(cacheType);
            ICacheAssemble cache = GetCache(cacheType);

            return await cache.ListRightPushAsync<T>(key, value);
        }

        public static async Task<T> ListRightPopAsync<T>(string key, CacheEnum cacheType = 0)
        {
            cacheType = GetCacheType(cacheType);
            ICacheAssemble cache = GetCache(cacheType);

            return await cache.ListRightPopAsync<T>(key);
        }

        #endregion

        #region Other

        /// <summary>
        /// 是否存在指定键值的缓存对象
        /// </summary>
        /// <param name="key"></param>
        /// <param name="cacheType"></param>
        /// <returns></returns>
        public static async Task<bool> ExistsAsync(string key, CacheEnum cacheType = 0)
        {
            cacheType = GetCacheType(cacheType);
            ICacheAssemble cache = GetCache(cacheType);

            return await cache.ExistsAsync(key);
        }

        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="cacheType"></param>
        /// <returns></returns>
        public static async Task<long> IncreaseAsync(string key, int value = 1, CacheEnum cacheType = 0)
        {
            cacheType = GetCacheType(cacheType);
            ICacheAssemble cache = GetCache(cacheType);

            return await cache.IncreaseAsync(key, value);
        }

        /// <summary>
        /// 减少
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="cacheType"></param>
        /// <returns></returns>
        public static async Task<long> DecreaseAsync(string key, int value = 1, CacheEnum cacheType = 0)
        {
            cacheType = GetCacheType(cacheType);
            ICacheAssemble cache = GetCache(cacheType);

            return await cache.DecreaseAsync(key, value);
        }

        #endregion

        #endregion

        #region 同步

        #region Set

        /// <summary>
        /// 设置对象缓存且支持基础数据类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <param name="expiry"></param>
        /// <param name="cacheType"></param>
        /// <returns></returns>
        public static bool Set<T>(string key, T obj, TimeSpan? expiry = default(TimeSpan?), CacheEnum cacheType = 0)
        {
            //读取缓存。读取默认缓存与方法传进来的缓存，以方法传进来的缓存为优先级最高。
            cacheType = GetCacheType(cacheType);
            ICacheAssemble cache = GetCache(cacheType);

            return cache.Set(key, obj, expiry);
        }

        /// <summary>
        /// 设置List对象缓存且支持基础数据类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="expiry"></param>
        /// <param name="cacheType"></param>
        /// <returns>失败的键值</returns>
        public static List<string> SetList<T>(Dictionary<string, object> obj, TimeSpan? expiry = default(TimeSpan?), CacheEnum cacheType = 0)
        {
            cacheType = GetCacheType(cacheType);
            ICacheAssemble cache = GetCache(cacheType);

            return cache.SetList<T>(obj, expiry);
        }

        #endregion

        #region Delete

        /// <summary>
        /// 通过key删除缓存
        /// </summary> 
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool Delete(string key, CacheEnum cacheType = 0)
        {
            cacheType = GetCacheType(cacheType);
            ICacheAssemble cache = GetCache(cacheType);

            return cache.Delete(key);
        }

        /// <summary>
        /// 通过List key删除缓存
        /// </summary> 
        /// <param name="keys"></param>
        /// <param name="cacheType"></param>
        /// <returns></returns>
        public static List<string> DeleteList(List<string> keys, CacheEnum cacheType = 0)
        {
            cacheType = GetCacheType(cacheType);
            ICacheAssemble cache = GetCache(cacheType);

            return cache.DeleteList(keys);
        }

        #endregion

        #region Get

        /// <summary>
        /// 查询对象缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="cacheType"></param>
        /// <returns></returns>
        public static T Get<T>(string key, CacheEnum cacheType = 0)
        {
            cacheType = GetCacheType(cacheType);
            ICacheAssemble cache = GetCache(cacheType);

            return cache.Get<T>(key);
        }

        /// <summary>
        /// 查询对象缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keys"></param>
        /// <param name="cacheType"></param>
        /// <returns></returns>
        public static Dictionary<string, T> GetList<T>(List<string> keys, CacheEnum cacheType = 0)
        {
            cacheType = GetCacheType(cacheType);
            ICacheAssemble cache = GetCache(cacheType);

            return cache.GetList<T>(keys);
        }

        /// <summary>
        /// 查询字符串缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="cacheType"></param>
        /// <returns></returns>
        public static string GetString(string key, CacheEnum cacheType = 0)
        {
            cacheType = GetCacheType(cacheType);
            ICacheAssemble cache = GetCache(cacheType);

            return cache.GetString(key);
        }

        /// <summary>
        /// 查询List字符串缓存
        /// </summary>
        /// <param name="keys"></param>
        /// <param name="cacheType"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetStringList(List<string> keys, CacheEnum cacheType = 0)
        {
            cacheType = GetCacheType(cacheType);
            ICacheAssemble cache = GetCache(cacheType);

            return cache.GetStringList(keys);
        }

        #endregion

        #region Update

        /// <summary>
        /// 修改缓存时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="expiry"></param>
        /// <param name="cacheType"></param>
        /// <returns></returns>
        public static bool UpdateExpiry(string key, TimeSpan expiry, CacheEnum cacheType = 0)
        {
            cacheType = GetCacheType(cacheType);
            ICacheAssemble cache = GetCache(cacheType);

            return cache.UpdateExpiry(key, expiry);
        }

        /// <summary>
        /// 修改List key缓存时间
        /// </summary>
        /// <param name="keys"></param>
        /// <param name="expiry"></param>
        /// <param name="cacheType"></param>
        /// <returns></returns>
        public static List<string> UpdateExpiryList(List<string> keys, TimeSpan expiry, CacheEnum cacheType = 0)
        {
            cacheType = GetCacheType(cacheType);
            ICacheAssemble cache = GetCache(cacheType);

            return cache.UpdateExpiryList(keys, expiry);
        }

        #endregion

        #region Other

        /// <summary>
        /// 是否存在指定键值的缓存对象
        /// </summary>
        /// <param name="key"></param>
        /// <param name="cacheType"></param>
        /// <returns></returns>
        public static bool Exists(string key, CacheEnum cacheType = 0)
        {
            cacheType = GetCacheType(cacheType);
            ICacheAssemble cache = GetCache(cacheType);

            return cache.Exists(key);
        }

        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="cacheType"></param>
        /// <returns></returns>
        public static long Increase(string key, int value=1, CacheEnum cacheType = 0)
        {
            cacheType = GetCacheType(cacheType);
            ICacheAssemble cache = GetCache(cacheType);

            return cache.Increase(key, value);
        }

        /// <summary>
        /// 减少
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="cacheType"></param>
        /// <returns></returns>
        public static long Decrease(string key, int value = 1, CacheEnum cacheType = 0)
        {
            cacheType = GetCacheType(cacheType);
            ICacheAssemble cache = GetCache(cacheType);

            return cache.Decrease(key, value);
        }

        #endregion

        #endregion


        #region 配置辅助方法

        /// <summary>
        /// 读取缓存。
        /// 读取默认缓存与方法传进来的缓存，以方法传进来的缓存为优先级最高。
        /// </summary>
        /// <param name="cacheType"></param>
        /// <returns></returns>
        private static CacheEnum GetCacheType(CacheEnum cacheType)
        {
            if (cacheType == 0)
                cacheType = CacheConfiguration.DefaultCacheType;
            return cacheType;
        }

        /// <summary>
        /// 根据type读取缓存
        /// </summary>
        /// <param name="cacheType"></param>
        /// <returns></returns>
        private static ICacheAssemble GetCache(CacheEnum cacheType)
        {
            switch (cacheType)
            {
                case CacheEnum.Redis:
                    return RedisAssemble.GetInstance();
                case CacheEnum.WebCache:
                    return WebCacheAssemble.GetInstance();
                case CacheEnum.Memcached:
                    return MemcachedAssemble.GetInstance();
            }

            return null;
        }

        #endregion

    }
}
