using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Instrumentation;
using System.Net.Configuration;
using System.Runtime.Caching;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace TechArt.Common.Utilities
{
    public sealed class CacheUtility
    {

        public static bool AddCache<T>(string key, T value, object expiry)
        {
            try
            {
                MemoryCache memoryCache = MemoryCache.Default;

                CacheItemPolicy cacheItemPolicy = new CacheItemPolicy();

                if ((expiry != null))
                {
                    DateTimeOffset offset = ObjectCache.InfiniteAbsoluteExpiration;
                    if (expiry is DateTimeOffset)
                    {
                        offset = (DateTimeOffset)expiry;
                    }
                    else if (expiry is DateTime)
                    {
                        offset = new DateTimeOffset((DateTime)expiry);
                    }
                    else if (expiry is TimeSpan)
                    {
                        offset = new DateTimeOffset(DateTime.Now.Add((TimeSpan)expiry));
                    }
                    cacheItemPolicy.AbsoluteExpiration = offset;
                }

                memoryCache.Set(key, value, cacheItemPolicy);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public static T GetCache<T>(string key)
            where T : class
        {
            MemoryCache memoryCache = MemoryCache.Default;

            return (T)memoryCache.Get(key);
        }
    }
}
