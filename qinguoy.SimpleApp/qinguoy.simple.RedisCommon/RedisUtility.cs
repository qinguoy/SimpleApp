using StackExchange.Redis;
using System;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace qinguoy.simple.RedisCommon
{
    /// <summary>
    /// Redis工具类
    /// </summary>
    public class RedisUtility
    {
        /// <summary>
        ///  ConnectionMultiplexer是线程安全的，且是昂贵的。所以我们尽量重用。 
        /// </summary>
        private static ConnectionMultiplexer connectionMultiplexer;
        private static IDatabase dataBase;
        private static object lockObj = new object();
        //可读取配置文件中的连接
        private static string connectionString = "127.0.0.1:6379,defaultDatabase=3,password=pwd123456,connectTimeout=1000,connectRetry=1,syncTimeout=10000";
        private static RedisUtility instance;
        /// <summary>
        /// 构造函数
        /// </summary>
        private RedisUtility()
        {

            connectionMultiplexer = ConnectionMultiplexer.Connect(connectionString);
            dataBase = connectionMultiplexer.GetDatabase();

        }
        /// <summary>
        /// 单例
        /// </summary>
        public static RedisUtility Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObj)
                    {
                        if (instance == null)
                        {
                            instance = new RedisUtility();
                        }
                    }
                }
                return instance;
            }
        }
        /// <summary>
        /// 存储string缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="timespan"></param>
        /// <returns></returns>
        public bool StringSet(string key, string value, TimeSpan? expiry = default(TimeSpan?))
        {
            return dataBase.StringSet(key, value, expiry);
        }
        /// <summary>
        /// 异步存储string缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        public async Task<bool> StringSetAsync(string key, string value, TimeSpan? expiry = default(TimeSpan?))
        {
           return await dataBase.StringSetAsync(key, value, expiry); 
        }
        /// <summary>
        /// 储存对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="t"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        public bool StringSet<T>(string key, T t, TimeSpan? expiry = default(TimeSpan?))
        {
            string objToJsonString = JsonConvert.SerializeObject(t);
            return dataBase.StringSet(key, objToJsonString, expiry);
        }
        /// <summary>
        /// 异步存储对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="t"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        public async Task<bool> StringSetAync<T>(string key, T t, TimeSpan? expiry = default(TimeSpan?))
        {
            string objJsonString = JsonConvert.SerializeObject(t);
            return await dataBase.StringSetAsync(key,objJsonString,expiry);

        }
        /// <summary>
        /// 获取string
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string StringGet(string key)
        {
            return dataBase.StringGet(key);
        }
        /// <summary>
        /// 获取string
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<string> StringGetAsync(string key)
        {
            return await dataBase.StringGetAsync(key);
        }

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T ObjectGet<T>(string key)
        {
            if (!dataBase.KeyExists(key))
            {
                return default(T);
            }
            string objectString = dataBase.StringGet(key);
            return JsonConvert.DeserializeObject<T>(objectString);
        }
        /// <summary>
        /// 异步获取对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<T> ObjectGetAsync<T>(string key)
        {
            if (!dataBase.KeyExists(key))
            {
                return default(T);
            }
            string objString = await dataBase.StringGetAsync(key); 
            return JsonConvert.DeserializeObject<T>(objString);
        }
        /// <summary>
        /// 删除key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool KeyDelete(string key)
        {
            return dataBase.KeyDelete(key);
        }
    }
}
