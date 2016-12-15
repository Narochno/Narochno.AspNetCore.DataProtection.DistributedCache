using Microsoft.Extensions.Caching.Distributed;

namespace Visibility.AspNetCore.DataProtection.DistributedCache
{
    public class DistributedDataXmlRepositoryOptions
    {
        public DistributedCacheEntryOptions CacheOptions { get; set; } = new DistributedCacheEntryOptions();
        public string Key { get; set; } = "DataProtection-Keys";
    }
}