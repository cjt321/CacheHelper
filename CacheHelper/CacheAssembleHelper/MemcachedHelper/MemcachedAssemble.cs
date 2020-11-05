using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Enyim.Caching;
using Enyim.Caching.Memcached;
using Newtonsoft.Json;

namespace CacheHelper.CacheAssembleHelper.MemcachedHelper
{
    /// <summary>
    /// Memcached缓存方法
    /// </summary>
    class MemcachedAssemble : ICacheAssemble
    {
        private readonly MemcachedClient _memcachedClient;
        private static readonly MemcachedAssemble Instance = new MemcachedAssemble();

        private MemcachedAssemble()
        {
            _memcachedClient = new MemcachedClient(MemcachedAssembleConfig.MemClientInit());
        }

        public static MemcachedAssemble GetInstance()
        {
            return Instance;
        }

        #region 异步方法

        #region Delete

        /// <summary>
        /// 通过key删除缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(string key)
        {
            var result = _memcachedClient.Remove(key);
            if (result == false)
            {
                var empty = _memcachedClient.Get(key);
                if (empty == null)
                {
                    result = true;
                }
            }

            return await Task.FromResult(result);
        }

        /// <summary>
        /// 通过list key删除缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<List<string>> DeleteListAsync(List<string> keys)
        {
            var result = new List<string>();
            foreach (var key in keys)
            {
                var empty = _memcachedClient.Remove(key);
                if (empty == false)
                {
                    var obj = _memcachedClient.Get(key);
                    if (obj != null)
                    {
                        result.Add(key);
                    }
                }
            }

            return await Task.FromResult(result);
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
            var result = (string)_memcachedClient.Get(key);

            return await Task.FromResult(ConvertObj<T>(result));
        }

        /// <summary>
        /// 通过List key获取对象缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<Dictionary<string, T>> GetListAsync<T>(List<string> keys)
        {
            var result = new Dictionary<string, T>();
            foreach (var key in keys)
            {
                var eResult = (string)_memcachedClient.Get(key);

                var empty = await Task.FromResult(ConvertObj<T>(eResult));
                result.Add(key, empty);
            }

            return result;
        }

        /// <summary>
        /// 通过key获取对象字符串
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<string> GetStringAsync(string key)
        {
            return await Task.FromResult((string)_memcachedClient.Get(key));
        }


        /// <summary>
        /// 通过list key获取对象字符串
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<Dictionary<string, string>> GetStringListAsync(List<string> keys)
        {
            var result = new Dictionary<string, string>();
            foreach (var key in keys)
            {
                var empty = (string)_memcachedClient.Get(key);
                result.Add(key, empty);
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
            var obj = _memcachedClient.Get(key);

            var result = _memcachedClient.Store(StoreMode.Set, key, ConvertJson(obj),
                DateTime.UtcNow.Add((TimeSpan)expiry));

            return await Task.FromResult(result);
        }

        /// <summary>
        /// 修改list key过期时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        public async Task<List<string>> UpdateExpiryListAsync(List<string> keys, TimeSpan expiry)
        {
            List<string> result = new List<string>();

            foreach (var key in keys)
            {
                var obj = _memcachedClient.Get(key);

                var empty = _memcachedClient.Store(StoreMode.Set, key, ConvertJson(obj),
                    DateTime.UtcNow.Add((TimeSpan)expiry));

                if (empty == false)
                {
                    result.Add(key);
                }
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
            bool result = false;
            if (expiry != null)
            {
                result = _memcachedClient.Store(StoreMode.Set, key, ConvertJson(obj),
                    DateTime.UtcNow.Add((TimeSpan)expiry));
            }
            else
            {
                result = _memcachedClient.Store(StoreMode.Set, key, ConvertJson(obj));
            }

            return await Task.FromResult(result);
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
            List<string> result = new List<string>();
            foreach (var item in obj)
            {
                var empty = false;
                if (expiry != null)
                {
                    empty = _memcachedClient.Store(StoreMode.Set, item.Key, ConvertJson(item.Value),
                        DateTime.UtcNow.Add((TimeSpan)expiry));
                }
                else
                {
                    empty = _memcachedClient.Store(StoreMode.Set, item.Key, ConvertJson(item.Value));
                }

                if (empty == false)
                {
                    result.Add(item.Key);
                }
            }

            return await Task.FromResult(result);
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
            var result = _memcachedClient.Get(key);

            return await Task.FromResult(result != null);
        }

        #endregion

        #endregion

        #region 同步方法

        #region Delete

        /// <summary>
        /// 通过key删除缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Delete(string key)
        {
            var result = _memcachedClient.Remove(key);

            return result;
        }

        /// <summary>
        /// 通过list key删除缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public List<string> DeleteList(List<string> keys)
        {
            var result = new List<string>();
            foreach (var key in keys)
            {
                var empty = _memcachedClient.Remove(key);
                if (empty == false)
                {
                    result.Add(key);
                }
            }

            return result;
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
            var result = (string)_memcachedClient.Get(key);

            return ConvertObj<T>(result);
        }

        /// <summary>
        /// 通过List key获取对象缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public Dictionary<string, T> GetList<T>(List<string> keys)
        {
            var result = new Dictionary<string, T>();
            foreach (var key in keys)
            {
                var eResult = (string)_memcachedClient.Get(key);

                var empty = ConvertObj<T>(eResult);
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
            return (string)_memcachedClient.Get(key);
        }


        /// <summary>
        /// 通过list key获取对象字符串
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetStringList(List<string> keys)
        {
            var result = new Dictionary<string, string>();
            foreach (var key in keys)
            {
                var empty = (string)_memcachedClient.Get(key);
                result.Add(key, empty);
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
            var obj = _memcachedClient.Get(key);

            var result = _memcachedClient.Store(StoreMode.Set, key, ConvertJson(obj),
                DateTime.UtcNow.Add((TimeSpan)expiry));

            return result;
        }

        /// <summary>
        /// 修改list key过期时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        public List<string> UpdateExpiryList(List<string> keys, TimeSpan expiry)
        {
            List<string> result = new List<string>();

            foreach (var key in keys)
            {
                var obj = _memcachedClient.Get(key);

                var empty = _memcachedClient.Store(StoreMode.Set, key, ConvertJson(obj),
                    DateTime.UtcNow.Add((TimeSpan)expiry));

                if (empty == false)
                {
                    result.Add(key);
                }
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
            bool result = false;
            if (expiry != null)
            {
                result = _memcachedClient.Store(StoreMode.Set, key, ConvertJson(obj),
                    DateTime.UtcNow.Add((TimeSpan)expiry));
            }
            else
            {
                result = _memcachedClient.Store(StoreMode.Set, key, ConvertJson(obj));
            }

            return result;
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
            List<string> result = new List<string>();
            foreach (var item in obj)
            {
                var empty = false;
                if (expiry != null)
                {
                    empty = _memcachedClient.Store(StoreMode.Set, item.Key, ConvertJson(item.Value),
                        DateTime.UtcNow.Add((TimeSpan)expiry));
                }
                else
                {
                    empty = _memcachedClient.Store(StoreMode.Set, item.Key, ConvertJson(item.Value));
                }

                if (empty == false)
                {
                    result.Add(item.Key);
                }
            }

            return result;
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
            var result = _memcachedClient.Get(key);

            return result != null;
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
