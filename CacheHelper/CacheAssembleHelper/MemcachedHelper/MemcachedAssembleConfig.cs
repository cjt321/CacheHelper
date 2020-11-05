using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Enyim.Caching.Configuration;
using Enyim.Caching.Memcached;

namespace CacheHelper.CacheAssembleHelper.MemcachedHelper
{
    internal static class MemcachedAssembleConfig
    {
        public static string Ip { get; set; }

        public static string Port { get; set; }

        public static MemcachedProtocol Protocol { get; set; }

        public static bool OpenAuth { get; set; }

        public static Dictionary<string, object> AuthPara { get; set; }

        public static MemcachedClientConfiguration MemClientInit()
        {
            //初始化缓存
            MemcachedClientConfiguration memConfig = new MemcachedClientConfiguration();
            IPAddress newaddress = IPAddress.Parse(Dns.GetHostEntry(Ip).AddressList[0].ToString()); //xxxx替换为ocs控制台上的“内网地址”
            IPEndPoint ipEndPoint = new IPEndPoint(newaddress, int.Parse(Port));
            //配置文件 - ip
            memConfig.Servers.Add(ipEndPoint);
            //   配置文件 - 协议
            memConfig.Protocol = Protocol;
            // 配置文件 - 权限，如果使用了免密码功能，则无需设置userName和password

            if (OpenAuth)
            {
                memConfig.Authentication.Type = typeof(PlainTextAuthenticator);
                memConfig.Authentication.Parameters["userName"] = AuthPara["userName"];
                memConfig.Authentication.Parameters["password"] = AuthPara["password"];
                memConfig.Authentication.Parameters["zone"] = AuthPara["zone"];
            }

            // memConfig.Authentication.Type = typeof(PlainTextAuthenticator);
            //memConfig.Authentication.Parameters["zone"] = "";
            //memConfig.Authentication.Parameters["userName"] = "admin";
            //memConfig.Authentication.Parameters["password"] = "admin";

            //下面请根据实例的最大连接数进行设置
            //memConfig.SocketPool.MinPoolSize = 5;
            // memConfig.SocketPool.MaxPoolSize = 200;
            //   MemClient = new MemcachedClient(memConfig);

            return memConfig;
        }
    }
}
