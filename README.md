CacheHelper类库说明
使用说明:先进行第四点”配置nuget“后搜索”Cachehelper“引用即可

一、初始化说明
如果为redis，则使用RedisCacheInitConfiguration；如果为memorycache则使用RedisCacheInitConfiguration；如果为本地内存则为Null。
以Redis为例：
            CacheAssembleExtensions.AddCache(CacheEnum.Redis, new RedisCacheInitConfiguration()
            {
                ConnectionString = "127.0.0.1:6379,password=abcd1234,connectRetry=3,connectTimeout=3000,syncTimeout=2000,keepAlive=300,abortConnect=false",
                DbNumber = 10,
                SystemKey = null
            });




二、方法说明

 
三、注意点
1.  使用Mecached删除时若缓存已经不存在会返回false，redis和cache会返回true 