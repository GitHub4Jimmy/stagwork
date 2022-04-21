using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Nest;

namespace StagwellTech.SEIU.CommonCoreEntities.Data
{
    public class RedisClient : IRedisClient
    {
        private readonly ConnectionMultiplexer connection;
        //private readonly IDatabase redis;
        private readonly string environment;

        private RedisClient(string connectionString)
        {
            connection = ConnectionMultiplexer.Connect(connectionString);
            //redis = connection.GetDatabase();
            environment = string.IsNullOrEmpty(Environment.GetEnvironmentVariable("ENVIRONMENT_32BJ")) ? "DEV" : Environment.GetEnvironmentVariable("ENVIRONMENT_32BJ");

        }

        public static RedisClient Connect(string connectionString)
        {
            return new RedisClient(connectionString);
        }

        public string PING()
        {
            string cacheCommand = "PING";
            Console.WriteLine("\nCache command  : " + cacheCommand);
            var redis = connection.GetDatabase();
            var result = redis.Execute(cacheCommand).ToString();
            Console.WriteLine("Cache response : " + result);
            return result;
        }

        public List<T> GetCollection<T>(List<string> keys)
        {
            var redisKeys = keys.Select(k => new RedisKey(PrefixKey(k))).ToArray();
            var redis = connection.GetDatabase();
            var redisValues = redis.StringGet(redisKeys);

            //RedisValue[] values = redisValues.Select(v => new KeyValuePair<string,T>(v.))

            //var result = new Dictionary<string, T>();

            //for (var i = 0; i < keys.Count; i++)
            //{
            //    var key = keys[i];
            //    var value = JsonConvert.DeserializeObject<T>(redisValues[i]);
            //    result.Add(key, value);
            //}

            //redis.StringSet()

            var result = new List<T>();
            foreach (var redisValue in redisValues)
            {
                T cacheValue = redisValue.HasValue ? JsonConvert.DeserializeObject<T>(redisValue) : default(T);
                result.Add(cacheValue);
            }
            
            return result;
        }

        public bool SetCollection<T>(List<KeyValuePair<string, T>> objects)
        {
            var redisValues = objects.Select(o =>
                new KeyValuePair<RedisKey, RedisValue>(new RedisKey(PrefixKey(o.Key)), new RedisValue(JsonConvert.SerializeObject(o.Value))))
                .ToArray();

            var redis = connection.GetDatabase();
            var result = redis.StringSet(redisValues, When.Always, CommandFlags.None);

            return result;
        }

        private string PrefixKey(string key)
        {
            return environment + "_" + key;
        }

        public T Get<T>(string key)
        {
            var redis = connection.GetDatabase();
            string str = redis.StringGet(PrefixKey(key));
            if (str is null) return default(T);
            return JsonConvert.DeserializeObject<T>(str);
        }

        public bool TryGetValue<T>(string key, out T value)
        {
            try
            {
                value = Get<T>(key);
                return value != null;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                Debug.WriteLine(e.StackTrace);
                value = default;
                return false;
            }
        }

        public void Set<T>(string key, T value, TimeSpan expiration)
        {
            try
            {
                var redis = connection.GetDatabase();
                redis.StringSet(PrefixKey(key), JsonConvert.SerializeObject(value), expiration);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                Debug.WriteLine(e.StackTrace);
            }
        }

        public void Set<T>(string key, T value)
        {
            try
            {
                var redis = connection.GetDatabase();
                redis.StringSet(PrefixKey(key), JsonConvert.SerializeObject(value), TimeSpan.FromHours(1));
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                Debug.WriteLine(e.StackTrace);
            }
        }

        public void Delete(string key)
        {
            try
            {
                var redis = connection.GetDatabase();
                redis.KeyDelete(PrefixKey(key));
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                Debug.WriteLine(e.StackTrace);
            }
        }
    }
}
