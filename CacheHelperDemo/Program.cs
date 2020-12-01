using System;
using System.Collections.Generic;
using System.Linq;
using CacheHelper;
using CacheHelper.CacheAssembleHelper;
using CacheHelper.Configuration;
using CacheHelperDemo.TestEntity;

namespace CacheHelperDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Config();

            /*TestPeople p1 = new TestPeople();

            var r1 = CachedHelper.SetAsync("p1", p1, TimeSpan.FromDays(1)).Result;
            Console.WriteLine($"r1={r1}");

            var r1return = CachedHelper.Get<TestPeople>("p1");
            Console.WriteLine($"r1return={r1return}");


            Dictionary<string, object> pls = new Dictionary<string, object>();
            pls.Add("d1", new TestPeople(){Id = 1,Age=18,Birthday = DateTime.Now, Name = "rose"});
            pls.Add("d2", new TestPeople(){Id = 1,Age=19,Birthday = DateTime.Now, Name = "alun"});
            pls.Add("d3", new TestPeople(){Id = 1,Age=15,Birthday = DateTime.Now, Name = "vv"});
            CachedHelper.SetList<TestPeople>(pls);
            var r2return = CachedHelper.GetList<TestPeople>(new List<string>(){"d1","d2"});*/


            /*var x = CachedHelper.GetStringAsync("abcd").Result;


            var r1 = CachedHelper.ListRightPushStringAsync("testlist", Guid.NewGuid().ToString()).Result;*/



            var r2 = CachedHelper.ListRangeStringAsync("testlist").Result;

            var r3 = CachedHelper.ListRightPopStringAsync("testlist").Result;
                ;
            Console.WriteLine($"r2return={1}");


        }

        public static void Config()
        {


            CacheAssembleExtensions.AddCache(CacheEnum.Redis, new RedisCacheInitConfiguration()
            {
                ConnectionString = "106.52.207.114:6379,password=before$7878$,connectRetry=3,connectTimeout=3000,syncTimeout=2000,keepAlive=300,abortConnect=false",
                DbNumber = 14,
                SystemKey = null
            });

        }
    }
}
