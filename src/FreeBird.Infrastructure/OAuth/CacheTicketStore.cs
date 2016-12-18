using FreeBird.Infrastructure.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBird.Infrastructure.OAuth
{
    public class CacheTicketStore : ITicketStore
    {
        private const string TicketPrefix = "$CacheTicket$";

        private ICacheManager _cacheManager;

        public CacheTicketStore(ICacheManager cacheManager)
        {
            _cacheManager = cacheManager;
        }

        public IEnumerable<object> GetAll()
        {
            foreach (var obj in _cacheManager.Entries.Where(w => w.Key.StartsWith(TicketPrefix)))
            {
                if (obj.Value !=null )
                    yield return obj.Value;
            }
        }

        public bool Remove(string key)
        {
            key = GetKey(key);
            if (_cacheManager.Contains(key))
            {
                _cacheManager.Remove(key);
                return true;
            }
            return false;
        }

        public T GetValue<T>(string key)
        {
            key = GetKey(key);
            if (_cacheManager.Contains(key))
            {
                return _cacheManager.Get(key, () => default(T), 1);
            }
            return default(T);
        }

        public bool Remove<T>(string key, out T value)
        {
            key = GetKey(key);
            if (_cacheManager.Contains(key))
            {
                value = _cacheManager.Get(key, () => default(T), 1);
                _cacheManager.Remove(key);
                return true;
            }
            value = default(T);
            return false;
        }

        public bool GetValue<T>(string key, out T value)
        {
            key = GetKey(key);
            if (_cacheManager.Contains(key))
            {
                value = _cacheManager.Get(key, () => default(T), 1);
                return true;
            }
            value = default(T);
            return false;
        }

        public T Set<T>(string key, Func<T> acquirer, int? cacheTime = null)
        {
            key = GetKey(key);
            return _cacheManager.Get(key, acquirer, cacheTime);
        }

        public T Set<T>(string key, T obj, int? cacheTime = null)
        {
            key = GetKey(key);
            return this.Set(key, () => obj, cacheTime);
        }

        private string GetKey(string key)
        {
            if (key == null || key.StartsWith(TicketPrefix))
                return key;
            return TicketPrefix + key;
        }

    }
}
