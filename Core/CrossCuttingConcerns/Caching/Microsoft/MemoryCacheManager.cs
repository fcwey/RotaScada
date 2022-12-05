using Core.Utilities.IoC;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Core.CrossCuttingConcerns.Caching.Microsoft
{
    public class MemoryCacheManager : ICacheManager
    {
        private IMemoryCache _cache;
        public MemoryCacheManager()
        {
            _cache = ServiceTool.ServiceProvider.GetService<IMemoryCache>();
        }

        public void Add(string key, object data, int duration)
        {
            _cache.Set(key, data, TimeSpan.FromMinutes(duration));
        }

        public T Get<T>(string key)
        {
            throw new NotImplementedException();
        }

        public object Get(string key)
        {
            return _cache.Get(key);
        }

        public bool IsAdd(string key)
        {
            return _cache.TryGetValue(key, out _);//_ demek değeri önemsemiyor sadece varmı yokmu ona bakıyor demek
        }

        public void Remove(string key)
        {
            _cache.Remove(key);
        }

        public void RemoveByPattern(string pattern)
        {
            //var cacheEntriesCollectionDefinition = typeof(MemoryCache).GetProperty(
            //    "EntriesCollection", 
            //    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            //var cacheEntriesCollection = cacheEntriesCollectionDefinition.GetValue(_cache) as dynamic;


            //List<ICacheEntry> cacheCollectionValues = new List<ICacheEntry>();
            //if (cacheEntriesCollection!=null)
            //foreach (var cacheItem in cacheEntriesCollection)
            //{

            //    ICacheEntry cacheItemValue = cacheItem.GetType().GetProperty("Value").GetValue(cacheItem, null);


            //    cacheCollectionValues.Add(cacheItemValue);
            //}

            //var regex = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
            //var keysToRemove = cacheCollectionValues.Where(d => regex.IsMatch(d.Key.ToString())).Select(d => d.Key).ToList();
            //foreach (var key in keysToRemove)
            //{
            //_cache.Remove(key);
            //}

            var coherentState = typeof(MemoryCache).GetField("_coherentState", BindingFlags.NonPublic | BindingFlags.Instance);

            var coherentStateValue = coherentState.GetValue(_cache);

            var entriesCollection = coherentStateValue.GetType().GetProperty("EntriesCollection", BindingFlags.NonPublic | BindingFlags.Instance);

            var cacheEntriesCollection = entriesCollection.GetValue(coherentStateValue) as dynamic;


            List<ICacheEntry> cacheCollectionValues = new List<ICacheEntry>();

            foreach (var cacheItem in cacheEntriesCollection)
            {

                ICacheEntry cacheItemValue = cacheItem.GetType().GetProperty("Value").GetValue(cacheItem, null);
                cacheCollectionValues.Add(cacheItemValue);
            }

            var regex = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
            var keysToRemove = cacheCollectionValues.Where(d => regex.IsMatch(d.Key.ToString())).Select(d => d.Key).ToList();
            foreach (var key in keysToRemove)
            {
                _cache.Remove(key);
            }

        }
    }
}
