using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CacheHelper.Configuration
{
    /// <summary>
    /// redis初始化配置类
    /// </summary>
    public class RedisCacheInitConfiguration:ICacheInitConfiguration
    {
        /// <summary>
        /// redis连接字符串
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// 指定redis的数据库
        /// </summary>
        public int DbNumber { get; set; }

        /// <summary>
        /// 所有key的前缀，无则填空
        /// </summary>
        public string SystemKey { get; set; }

    }
}
