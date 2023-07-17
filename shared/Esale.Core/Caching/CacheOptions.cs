using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Esale.Core.Caching
{
    public class CacheOptions
    {
        public CacheProviderEnum Provider { get; set; }
        public bool RedisHash { get; set; } = true;
    }
}
