using StagwellTech.SEIU.CommonCoreEntities.Data;
using System;
using System.Collections.Generic;

namespace ApiTests.Services.MockServices
{
    public class MockRedisClient : IRedisClient
    {
        public void Delete(string key)
        {
            throw new NotImplementedException();
        }

        public T Get<T>(string key)
        {
            return default(T);
        }

        public List<T> GetCollection<T>(List<string> keys)
        {
            throw new NotImplementedException();
        }

        public string PING()
        {
            // ping
            return "";
        }

        public void Set<T>(string key, T value)
        {
            // set
        }

        public void Set<T>(string key, T value, TimeSpan expiration)
        {
            // set
        }

        public bool SetCollection<T>(List<KeyValuePair<string, T>> objects)
        {
            throw new NotImplementedException();
        }

        public bool TryGetValue<T>(string key, out T value)
        {
            // trygetvalue
            value = default(T);
            return true;
        }
    }
}
