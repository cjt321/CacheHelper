using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Caching;
using Newtonsoft.Json;

namespace CacheHelper.CacheAssembleHelper.WebCacheHelper
{
    /// <summary>
    /// Cache缓存方法
    /// </summary>
    class WebCacheAssemble : ICacheAssemble
    {
        private static readonly Cache Cache = new Cache();  //缓存接口

        private static readonly WebCacheAssemble Instance = new WebCacheAssemble();

        private WebCacheAssemble()
        {
        }

        public static WebCacheAssemble GetInstance()
        {
            return Instance;
        }

        #region 异步方法

        #region delete

        /// <summary>
        /// 通过key删除缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(string key)
        {
            var result = Cache.Remove(key);

            return await Task.FromResult(true);
        }

        /// <summary>
        /// 通过List key删除缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<List<string>> DeleteListAsync(List<string> keys)
        {
            var result = new List<string>();
            foreach (var key in keys)
            {
                Cache.Remove(key);
            }

            return await Task.FromResult(new List<string>());
        }


        #endregion

        #region Get

        /// <summary>
        /// 通过key获取对象缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<T> GetAsync<T>(string key)
        {
            var result = (T)Cache.Get(key);
            return await Task.FromResult(result);
        }

        /// <summary>
        /// 通过 List key获取对象缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<Dictionary<string, T>> GetListAsync<T>(List<string> keys)
        {
            Dictionary<string, T> result = new Dictionary<string, T>();
            foreach (var key in keys)
            {
                var empty = (T)Cache.Get(key);
                result.Add(key, empty);
            }

            return await Task.FromResult(result);
        }

        /// <summary>
        /// 通过key获取对象字符串
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<string> GetStringAsync(string key)
        {
            var result = Cache.Get(key);
            if (result is string)
            {
                return await Task.FromResult((string)result);
            }
            else if (result is null)
            {
                return null;
            }
            else
            {
                return await Task.FromResult(ConvertJson(result));
            }
        }

        /// <summary>
        /// 通过list key获取对象字符串
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public async Task<Dictionary<string, string>> GetStringListAsync(List<string> keys)
        {
            var result = new Dictionary<string, string>();
            foreach (var key in keys)
            {
                var emtpy = Cache.Get(key);
                if (emtpy is string)
                {
                    result.Add(key, (string)emtpy);
                }
                else if (emtpy is null)
                {
                    result.Add(key, null);
                }
                else
                {
                    result.Add(key, ConvertJson(emtpy));
                }
            }

            return await Task.FromResult(result);
        }

        #endregion

        #region Update

        /// <summary>
        /// 修改过期时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        public async Task<bool> UpdateExpiryAsync(string key, TimeSpan expiry)
        {
            var obj = Cache.Get(key);

            Cache.Insert(key, obj, null, DateTime.UtcNow.Add(expiry),  Cache.NoSlidingExpiration);

            return await Task.FromResult(true);
        }

        /// <summary>
        /// 修改List key 过期时间
        /// </summary>
        /// <param name="keys"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        public async Task<List<string>> UpdateExpiryListAsync(List<string> keys, TimeSpan expiry)
        {
            List<string> result = new List<string>();

            foreach (var key in keys)
            {
                var obj = Cache.Get(key);

                Cache.Insert(key, obj, null, DateTime.UtcNow.Add(expiry),  Cache.NoSlidingExpiration);
            }

            return await Task.FromResult(result);
        }

        #endregion

        #region Set

        /// <summary>
        /// 设置对象缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        public async Task<bool> SetAsync<T>(string key, T obj, TimeSpan? expiry = null)
        {
            if (expiry != null)
            {
                Cache.Insert(key, obj, null, DateTime.UtcNow.Add((TimeSpan)expiry),  Cache.NoSlidingExpiration);
            }
            else
            {
                Cache.Insert(key, obj);
            }

            return await Task.FromResult(true);
        }

        /// <summary>
        /// 设置List对象缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <param name="expiry"></param>
        /// <returns>返回失败的键值</returns>
        public async Task<List<string>> SetListAsync<T>(Dictionary<string, object> obj, TimeSpan? expiry = null)
        {
            foreach (var item in obj)
            {
                if (expiry != null)
                {
                    Cache.Insert(item.Key, item.Value, null, DateTime.UtcNow.Add((TimeSpan)expiry),  Cache.NoSlidingExpiration);
                }
                else
                {
                    Cache.Insert(item.Key, item.Value);
                }
            }

            return await Task.FromResult(new List<string>());
        }


        #endregion


        #region Other

        /// <summary>
        /// 是否存在指定键值的缓存对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<bool> ExistsAsync(string key)
        {
            var result = Cache.Get(key);

            return await Task.FromResult(result != null);
        }

        public Task<long> ListRemoveAsync<T>(string key, T value)
        {
            throw new NotImplementedException();
        }

        public Task<List<T>> ListRangeAsync<T>(string key)
        {
            throw new NotImplementedException();
        }

        public Task<long> ListRightPushAsync<T>(string key, T value)
        {
            throw new NotImplementedException();
        }

        public Task<T> ListRightPopAsync<T>(string key)
        {
            throw new NotImplementedException();
        }

        public Task<long> ListRemoveStringAsync(string key, string value)
        {
            throw new NotImplementedException();
        }

        public Task<List<string>> ListRangeStringAsync(string key)
        {
            throw new NotImplementedException();
        }

        public Task<long> ListRightPushStringAsync(string key, string value)
        {
            throw new NotImplementedException();
        }

        public Task<string> ListRightPopStringAsync(string key)
        {
            throw new NotImplementedException();
        }

        #endregion


        #endregion

        #region 同步方法

        #region delete

        /// <summary>
        /// 通过key删除缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Delete(string key)
        {
            var result = Cache.Remove(key);

            return true;
        }

        /// <summary>
        /// 通过List key删除缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public List<string> DeleteList(List<string> keys)
        {
            var result = new List<string>();
            foreach (var key in keys)
            {
                Cache.Remove(key);
            }

            return new List<string>();
        }


        #endregion

        #region Get

        /// <summary>
        /// 通过key获取对象缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T Get<T>(string key)
        {
            var result = (T)Cache.Get(key);
            return result;
        }

        /// <summary>
        /// 通过 List key获取对象缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public Dictionary<string, T> GetList<T>(List<string> keys)
        {
            Dictionary<string, T> result = new Dictionary<string, T>();
            foreach (var key in keys)
            {
                var empty = (T)Cache.Get(key);
                result.Add(key, empty);
            }

            return result;
        }

        /// <summary>
        /// 通过key获取对象字符串
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetString(string key)
        {
            var result = Cache.Get(key);
            if (result is string)
            {
                return (string)result;
            }
            else if (result is null)
            {
                return null;
            }
            else
            {
                return ConvertJson(result);
            }
        }

        /// <summary>
        /// 通过list key获取对象字符串
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetStringList(List<string> keys)
        {
            var result = new Dictionary<string, string>();
            foreach (var key in keys)
            {
                var emtpy = Cache.Get(key);
                if (emtpy is string)
                {
                    result.Add(key, (string)emtpy);
                }
                else if (emtpy is null)
                {
                    result.Add(key, null);
                }
                else
                {
                    result.Add(key, ConvertJson(emtpy));
                }
            }

            return result;
        }

        #endregion

        #region Update

        /// <summary>
        /// 修改过期时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        public bool UpdateExpiry(string key, TimeSpan expiry)
        {
            var obj = Cache.Get(key);

            Cache.Insert(key, obj, null, DateTime.UtcNow.Add(expiry),  Cache.NoSlidingExpiration);

            return true;
        }

        /// <summary>
        /// 修改List key 过期时间
        /// </summary>
        /// <param name="keys"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        public List<string> UpdateExpiryList(List<string> keys, TimeSpan expiry)
        {
            List<string> result = new List<string>();

            foreach (var key in keys)
            {
                var obj = Cache.Get(key);

                Cache.Insert(key, obj, null, DateTime.UtcNow.Add(expiry),Cache.NoSlidingExpiration);
            }

            return result;
        }

        #endregion

        #region Set

        /// <summary>
        /// 设置对象缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        public bool Set<T>(string key, T obj, TimeSpan? expiry = null)
        {
            if (expiry != null)
            {
                Cache.Insert(key, obj, null, DateTime.UtcNow.Add((TimeSpan)expiry),  Cache.NoSlidingExpiration);
            }
            else
            {
                Cache.Insert(key, obj);
            }

            return true;
        }

        /// <summary>
        /// 设置List对象缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <param name="expiry"></param>
        /// <returns>返回失败的键值</returns>
        public List<string> SetList<T>(Dictionary<string, object> obj, TimeSpan? expiry = null)
        {
            foreach (var item in obj)
            {
                if (expiry != null)
                {
                    Cache.Insert(item.Key, item.Value, null, DateTime.UtcNow.Add((TimeSpan)expiry),  Cache.NoSlidingExpiration);
                }
                else
                {
                    Cache.Insert(item.Key, item.Value);
                }
            }

            return new List<string>();
        }


        #endregion

        #region Other

        /// <summary>
        /// 是否存在指定键值的缓存对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Exists(string key)
        {
            var result = Cache.Get(key);

            return result != null;
        }

        public long Increase(string key, int value = 1)
        {
            throw new NotImplementedException();
        }

        public long Decrease(string key, int value = 1)
        {
            throw new NotImplementedException();
        }

        public Task<long> IncreaseAsync(string key, int value = 1)
        {
            throw new NotImplementedException();
        }

        public Task<long> DecreaseAsync(string key, int value = 1)
        {
            throw new NotImplementedException();
        }

        #endregion

        #endregion

        #region 辅助类

        private string ConvertJson<T>(T value)
        {
            string result = value is string ? value.ToString() : JsonConvert.SerializeObject(value);
            return result;
        }

        private string ConvertJson<T>(T value, TypeNameHandling typeNameHandling)
        {
            string result = value is string ? value.ToString() : JsonConvert.SerializeObject(value, new JsonSerializerSettings()
            {
                TypeNameHandling = typeNameHandling
            });
            return result;
        }

        private T ConvertObj<T>(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return default(T);
            }
            return JsonConvert.DeserializeObject<T>(value);
        }

        private T ConvertObj<T>(string value, TypeNameHandling typeNameHandling)
        {
            if (string.IsNullOrEmpty(value))
            {
                return default(T);
            }
            return JsonConvert.DeserializeObject<T>(value, new JsonSerializerSettings() { TypeNameHandling = typeNameHandling });
        }

        #endregion
    }
}
