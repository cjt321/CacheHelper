using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CacheHelper.Configuration
{
    /// <summary>
    /// cache类别
    /// </summary>
    public enum CacheEnum
    {
        WebCache = 1,
        Memcached,
        Redis
    }

    /// <summary>
    /// cache配置
    /// </summary>
    class CacheConfiguration
    {

        /// <summary>
        /// 使用默认缓存方式
        /// </summary>
        public static CacheEnum DefaultCacheType = CacheEnum.WebCache;

    }
}
