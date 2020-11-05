using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Enyim.Caching.Memcached;

namespace CacheHelper.Configuration
{
    /// <summary>
    /// memchache 初始化配置类
    /// </summary>
    public class MemcachedInitConfiguration:ICacheInitConfiguration
    {
        /// <summary>
        /// ip地址
        /// </summary>
        public string Ip { get; set; }

        /// <summary>
        /// 端口号
        /// </summary>
        public string Port { get; set; }

        /// <summary>
        /// 协议
        /// </summary>
        public MemcachedProtocol Protocol { get; set; }

        /// <summary>
        /// 是否开启权限
        /// </summary>
        public bool OpenAuth { get; set; }

        /// <summary>
        /// 权限参数
        /// </summary>
        public Dictionary<string, object> AuthPara { get; set; }
    }
}
