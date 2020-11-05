using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CacheHelper.CacheAssembleHelper.RedisHelper
{
    internal static class RedisAssembleConfig
    {
        /// <summary>
        /// redis连接字符串
        /// </summary>
        public static string ConnectionString { get; set; }

        /// <summary>
        /// 指定redis的数据库
        /// </summary>
        public static int DbNumber { get; set; }

        /// <summary>
        /// 所有key的前缀
        /// </summary>
        public static string SystemKey { get; set; }
    }
}
