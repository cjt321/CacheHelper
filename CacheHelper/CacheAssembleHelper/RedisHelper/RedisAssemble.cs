using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace CacheHelper.CacheAssembleHelper.RedisHelper
{
    /// <summary>
    /// redis缓存方法
    /// </summary>
    class RedisAssemble : ICacheAssemble
    {
        /// <summary>
        /// 连接db
        /// </summary>
        private readonly int _dbNumger;

        /// <summary>
        /// 系统自定义Key前缀
        /// </summary>
        private readonly string _sysCustomKey;

        private readonly ConnectionMultiplexer _conn;

        private static readonly ConcurrentDictionary<string, ConnectionMultiplexer> ConnectionCache =
            new ConcurrentDictionary<string, ConnectionMultiplexer>();
        private static readonly RedisAssemble Instance = new RedisAssemble();

        private RedisAssemble()
        {
            _dbNumger = RedisAssembleConfig.DbNumber;
            _conn = GetConnectionMultiplexer(RedisAssembleConfig.ConnectionString);
            _sysCustomKey = RedisAssembleConfig.SystemKey;
        }

        public static RedisAssemble GetInstance()
        {
            return Instance;
        }

        /// <summary>
        /// 缓存获取
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        private static ConnectionMultiplexer GetConnectionMultiplexer(string connectionString)
        {
            if (!ConnectionCache.ContainsKey(connectionString))
            {
                ConnectionCache[connectionString] = ConnectionMultiplexer.Connect(connectionString);
            }

            return ConnectionCache[connectionString];
        }


        #region 异步方法

        #region Set

        /// <summary>
        /// 设置对象缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        public async Task<bool> SetAsync<T>(string key, T obj, TimeSpan? expiry = default(TimeSpan?))
        {
            key = AddSysCustomKey(key);
            string json = ConvertJson(obj);
            //  var database = _conn.GetDatabase(_dbNumger);
            // return await database.StringSetAsync(key, json, expiry);
            return await Do(db => db.StringSetAsync(key, json, expiry));
        }

        /// <summary>
        /// 设置List对象缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <param name="expiry"></param>
        /// <returns>返回失败的键值</returns>
        public async Task<List<string>> SetListAsync<T>(Dictionary<string, object> obj, TimeSpan? expiry = default(TimeSpan?))
        {
            List<string> result = new List<string>();
            foreach (var item in obj)
            {
                var key = AddSysCustomKey(item.Key);
                string json = ConvertJson(item.Value);
                var empty = await Do(db => db.StringSetAsync(key, json, expiry));
                if (empty == false)
                {
                    result.Add(item.Key);
                }
            }

            return result;
        }

        #endregion

        #region Delete

        /// <summary>
        /// 通过key删除缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(string key)
        {
            key = AddSysCustomKey(key);
            return await Task.FromResult(Do(db => db.KeyDelete(key)));
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
                var eKey = AddSysCustomKey(key);
                var empty = await Task.FromResult(Do(db => db.KeyDelete(eKey)));
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
        public async Task<T> GetAsync<T>(string key)
        {
            key = AddSysCustomKey(key);
            string result = await Do(db => db.StringGetAsync(key));
            return ConvertObj<T>(result);
        }

        /// <summary>
        /// 通过list key获取对象缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<Dictionary<string, T>> GetListAsync<T>(List<string> keys)
        {
            var result = new Dictionary<string, T>();
            foreach (var key in keys)
            {
                var eKey = AddSysCustomKey(key);
                var empty = await Do(db => db.StringGetAsync(eKey));
                result.Add(key, ConvertObj<T>(empty));
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
            key = AddSysCustomKey(key);
            return await Do(db => db.StringGetAsync(key));
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
                var eKey = AddSysCustomKey(key);
                var empyt = await Do(db => db.StringGetAsync(eKey));

                result.Add(key, empyt);
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
        public async Task<bool> UpdateExpiryAsync(string key, TimeSpan expiry)
        {
            key = AddSysCustomKey(key);
            return await Do(db => db.KeyExpireAsync(key, expiry));
        }


        /// <summary>
        /// 修改List key过期时间
        /// </summary>
        /// <param name="keys"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        public async Task<List<string>> UpdateExpiryListAsync(List<string> keys, TimeSpan expiry)
        {
            List<string> result = new List<string>();

            foreach (var key in keys)
            {
                var ekey = AddSysCustomKey(key);
                var empty = await Do(db => db.KeyExpireAsync(ekey, expiry));

                if (empty == false)
                {
                    result.Add(key);
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
        public async Task<bool> ExistsAsync(string key)
        {
            key = AddSysCustomKey(key);
            return await Do(db => db.KeyExistsAsync(key));
        }

        #endregion

        #endregion


        #region 同步方法

        #region Set

        /// <summary>
        /// 设置对象缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        public bool Set<T>(string key, T obj, TimeSpan? expiry = default(TimeSpan?))
        {
            key = AddSysCustomKey(key);
            string json = ConvertJson(obj);
            //  var database = _conn.GetDatabase(_dbNumger);
            // return await database.StringSet(key, json, expiry);
            return Do(db => db.StringSet(key, json, expiry));
        }

        /// <summary>
        /// 设置List对象缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <param name="expiry"></param>
        /// <returns>返回失败的键值</returns>
        public List<string> SetList<T>(Dictionary<string, object> obj, TimeSpan? expiry = default(TimeSpan?))
        {
            List<string> result = new List<string>();
            foreach (var item in obj)
            {
                var key = AddSysCustomKey(item.Key);
                string json = ConvertJson(item.Value);
                var empty = Do(db => db.StringSet(key, json, expiry));
                if (empty == false)
                {
                    result.Add(item.Key);
                }
            }

            return result;
        }

        #endregion

        #region Delete

        /// <summary>
        /// 通过key删除缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Delete(string key)
        {
            key = AddSysCustomKey(key);
            return Do(db => db.KeyDelete(key));
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
                var eKey = AddSysCustomKey(key);
                var empty = Do(db => db.KeyDelete(eKey));
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
            key = AddSysCustomKey(key);
            string result = Do(db => db.StringGet(key));
            if (typeof(T) == typeof(string))
                return (T)(object)result;
            return ConvertObj<T>(result);
        }

        /// <summary>
        /// 通过list key获取对象缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public Dictionary<string, T> GetList<T>(List<string> keys)
        {
            var result = new Dictionary<string, T>();
            foreach (var key in keys)
            {
                var eKey = AddSysCustomKey(key);
                var empty = Do(db => db.StringGet(eKey));
                result.Add(key, ConvertObj<T>(empty));
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
            key = AddSysCustomKey(key);
            return Do(db => db.StringGet(key));
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
                var eKey = AddSysCustomKey(key);
                var empyt = Do(db => db.StringGet(eKey));

                result.Add(key, empyt);
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
            key = AddSysCustomKey(key);
            return Do(db => db.KeyExpire(key, expiry));
        }


        /// <summary>
        /// 修改List key过期时间
        /// </summary>
        /// <param name="keys"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        public List<string> UpdateExpiryList(List<string> keys, TimeSpan expiry)
        {
            List<string> result = new List<string>();

            foreach (var key in keys)
            {
                var ekey = AddSysCustomKey(key);
                var empty = Do(db => db.KeyExpire(ekey, expiry));

                if (empty == false)
                {
                    result.Add(key);
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
            key = AddSysCustomKey(key);
            return Do(db => db.KeyExists(key));
        }

        #endregion


        #endregion

        #region 辅助方法 By CommonLib.Redis

        private string AddSysCustomKey(string oldKey)
        {
            return _sysCustomKey + oldKey;
        }

        private T Do<T>(Func<IDatabase, T> func)
        {
            var database = _conn.GetDatabase(_dbNumger);
            return func(database);
        }

        private async Task<T> Do<T>(Func<IDatabase, Task<T>> func)
        {
            var database = _conn.GetDatabase(_dbNumger);
            return await func(database);

        }

        private void Do(Action<IDatabase> action)
        {
            var database = _conn.GetDatabase(_dbNumger);
            action(database);
        }

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

        private T ConvertObj<T>(RedisValue value)
        {
            if (typeof(T) == typeof(string))
                return (T)(object)value;
            if (value.IsNullOrEmpty) return default(T);
            return JsonConvert.DeserializeObject<T>(value);
        }

        private T ConvertObj<T>(RedisValue value, TypeNameHandling typeNameHandling)
        {
            if (value.IsNullOrEmpty) return default(T);
            return JsonConvert.DeserializeObject<T>(value, new JsonSerializerSettings() { TypeNameHandling = typeNameHandling });
        }

        private List<T> ConvetList<T>(RedisValue[] values)
        {
            List<T> result = new List<T>();
            foreach (var item in values)
            {
                var model = ConvertObj<T>(item);
                result.Add(model);
            }

            return result;
        }

        private List<T> ConvetList<T>(RedisValue[] values, TypeNameHandling typeNameHandling)
        {
            List<T> result = new List<T>();
            foreach (var item in values)
            {
                var model = ConvertObj<T>(item, typeNameHandling);
                result.Add(model);
            }

            return result;
        }

        private List<T> ConvetList<T>(SortedSetEntry[] values)
        {
            List<T> result = new List<T>();
            foreach (var item in values)
            {

                var model = ConvertObj<T>(item.Element);
                if (typeof(T).GetProperties().Any(p => p.Name.Equals("Score")))
                {
                    //model.GetType().GetProperty("Score").SetValue(model,item.Score);
                    var setter = GetPropSetter<T, Double>("Score");
                    setter(model, item.Score);
                }

                result.Add(model);
            }

            return result;
        }

        private List<T> ConvetList<T>(SortedSetEntry[] values, TypeNameHandling typeNameHandling)
        {
            List<T> result = new List<T>();
            foreach (var item in values)
            {

                var model = ConvertObj<T>(item.Element, typeNameHandling);
                if (typeof(T).GetProperties().Any(p => p.Name.Equals("Score")))
                {
                    //model.GetType().GetProperty("Score").SetValue(model,item.Score);
                    var setter = GetPropSetter<T, Double>("Score");
                    setter(model, item.Score);
                }

                result.Add(model);
            }

            return result;
        }



        private RedisKey[] ConvertRedisKeys(List<string> redisKeys)
        {
            return redisKeys.Select(redisKey => (RedisKey)redisKey).ToArray();
        }

        private Action<TObject, TProperty> GetPropSetter<TObject, TProperty>(string propertyName)
        {
            ParameterExpression paramExpression = Expression.Parameter(typeof(TObject));

            ParameterExpression paramExpression2 = Expression.Parameter(typeof(TProperty), propertyName);

            MemberExpression propertyGetterExpression = Expression.Property(paramExpression, propertyName);

            Action<TObject, TProperty> result = Expression.Lambda<Action<TObject, TProperty>>
            (
                Expression.Assign(propertyGetterExpression, paramExpression2), paramExpression, paramExpression2
            ).Compile();

            return result;
        }

        #endregion 辅助方法


    }
}
