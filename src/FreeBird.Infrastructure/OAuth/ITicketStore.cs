using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBird.Infrastructure.OAuth
{
    public interface ITicketStore
    {
        IEnumerable<object> GetAll();

        bool Remove(string key);

        T GetValue<T>(string key);

        bool Remove<T>(string key, out T value);

        bool GetValue<T>(string key, out T value);

        T Set<T>(string key, T obj, int? cacheTime = null);

    }
}
