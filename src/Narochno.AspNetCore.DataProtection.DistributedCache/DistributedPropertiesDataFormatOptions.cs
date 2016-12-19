using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.Extensions.Caching.Distributed;

namespace Visibility.AspNetCore.DataProtection.DistributedCache
{
    public class DistributedPropertiesDataFormatOptions
    {
        public DistributedCacheEntryOptions CacheOptions { get; set; } = new DistributedCacheEntryOptions();
        public IDataSerializer<AuthenticationProperties> Serializer { get; set; } = new PropertiesSerializer();
        public string Prefix { get; set; } = "DistributedPropertiesDataFormat-";
    }
}