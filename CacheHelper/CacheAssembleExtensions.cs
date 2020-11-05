using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CacheHelper.CacheAssembleHelper.MemcachedHelper;
using CacheHelper.CacheAssembleHelper.RedisHelper;
using CacheHelper.CacheAssembleHelper.WebCacheHelper;
using CacheHelper.Configuration;
using Enyim.Caching.Memcached;

namespace CacheHelper
{
    /// <summary>
    /// 缓存配置使用方法
    /// </summary>
    public static class CacheAssembleExtensions
    {

        /// <summary>
        /// 初始化配置
        /// </summary>
        /// <param name="defaultCacheEnum">默认使用的缓存策略</param>
        /// <param name="configuration">如果为redis，则使用RedisCacheInitConfiguration；如果为memorycache则使用RedisCacheInitConfiguration；如果为本地内存则为Null</param>
        public static void AddCache(CacheEnum defaultCacheEnum, ICacheInitConfiguration configuration=null)
        {
            //设置默认cache
            CacheConfiguration.DefaultCacheType = defaultCacheEnum;
            if (defaultCacheEnum == 0)
                CacheConfiguration.DefaultCacheType = CacheEnum.WebCache;
            switch (defaultCacheEnum)
            {
                case CacheEnum.Redis:
                    RedisAssembleConfig.ConnectionString = ((RedisCacheInitConfiguration)configuration).ConnectionString;
                    RedisAssembleConfig.DbNumber = ((RedisCacheInitConfiguration)configuration).DbNumber;
                    RedisAssembleConfig.SystemKey = ((RedisCacheInitConfiguration)configuration).SystemKey;
                    break;
                case CacheEnum.WebCache:
                    break;
                case CacheEnum.Memcached:
                    MemcachedAssembleConfig.Ip = ((MemcachedInitConfiguration)configuration).Ip;
                    MemcachedAssembleConfig.Port = ((MemcachedInitConfiguration)configuration).Port;
                    MemcachedAssembleConfig.Protocol = ((MemcachedInitConfiguration)configuration).Protocol;
                    MemcachedAssembleConfig.AuthPara = ((MemcachedInitConfiguration)configuration).AuthPara;
                    MemcachedAssembleConfig.OpenAuth = ((MemcachedInitConfiguration)configuration).OpenAuth;
                    break;
            }



        }


        /// <summary>
        /// 添加redis的依赖注入方式
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="dbNumber"></param>
        /// <param name="sysCustomKey"></param>
        [Obsolete("过时，不要使用，请使用AddCache方法")]
        public static void AddRedis(string connectionString, int dbNumber, string sysCustomKey = "")
        {
            CacheConfiguration.DefaultCacheType = CacheEnum.Redis;

            RedisAssembleConfig.ConnectionString = connectionString;
            RedisAssembleConfig.DbNumber = dbNumber;
            RedisAssembleConfig.SystemKey = sysCustomKey;
        }

        /// <summary>
        /// 添加Cache配置
        /// </summary>
        [Obsolete("过时，不要使用，请使用AddCache方法")]
        public static void AddWebCache()
        {
            CacheConfiguration.DefaultCacheType = CacheEnum.WebCache;
        }

        /// <summary>
        /// 添加Memcached配置
        /// </summary>
        /// <param name="ip">ip地址</param>
        /// <param name="port">端口号</param>
        /// <param name="protocol">协议</param>
        /// <param name="openAuth">是否开启权限</param>
        /// <param name="authPara">权限参数</param>
        [Obsolete("过时，不要使用，请使用AddCache方法")]
        public static void AddMemcached(
            string ip,
            string port,
            MemcachedProtocol protocol = MemcachedProtocol.Binary,
            bool openAuth = false,
            Dictionary<string, object> authPara = null)
        {
            CacheConfiguration.DefaultCacheType = CacheEnum.Memcached;

            MemcachedAssembleConfig.Ip = ip;
            MemcachedAssembleConfig.Port = port;
            MemcachedAssembleConfig.Protocol = protocol;
            MemcachedAssembleConfig.AuthPara = authPara;
            MemcachedAssembleConfig.OpenAuth = openAuth;
        }

    }
}
