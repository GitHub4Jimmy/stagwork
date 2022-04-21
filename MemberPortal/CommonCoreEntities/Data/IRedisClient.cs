using System;
using System.Collections.Generic;

namespace StagwellTech.SEIU.CommonCoreEntities.Data
{
    public interface IRedisClient
    {
        void Delete(string key);
        T Get<T>(string key);
        string PING();
        void Set<T>(string key, T value);
        void Set<T>(string key, T value, TimeSpan expiration);
        bool TryGetValue<T>(string key, out T value);
        List<T> GetCollection<T>(List<string> keys);
        bool SetCollection<T>(List<KeyValuePair<string, T>> objects);
    }
}