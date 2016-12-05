using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.Extensions.Caching.Distributed;

namespace Visibility.AspNetCore.DataProtection.DistributedCache
{
    public class DistributedPropertiesDataFormat : ISecureDataFormat<AuthenticationProperties>
    {
        public const string CacheKeyPrefix = "CachedPropertiesData-";

        private readonly IDistributedCache cache;
        private readonly IDataProtector dataProtector;
        private readonly IDataSerializer<AuthenticationProperties> serializer;

        public DistributedPropertiesDataFormat(IDistributedCache cache, IDataProtector dataProtector, IDataSerializer<AuthenticationProperties> serializer)
        {
            this.cache = cache;
            this.dataProtector = dataProtector;
            this.serializer = serializer;
        }

        public string Protect(AuthenticationProperties data)
        {
            return Protect(data, null);
        }

        public string Protect(AuthenticationProperties data, string purpose)
        {
            var key = Guid.NewGuid().ToString();
            var cacheKey = $"{CacheKeyPrefix}{key}";

            var serialized = serializer.Serialize(data);
            
            cache.Set(cacheKey, serialized);
            var p = dataProtector.Protect(key);
            return p;
        }

        public AuthenticationProperties Unprotect(string protectedText)
        {
            return Unprotect(protectedText, null);
        }

        public AuthenticationProperties Unprotect(string protectedText, string purpose)
        {
            var key = dataProtector.Unprotect(protectedText);
            var cacheKey = $"{CacheKeyPrefix}{key}";
            var serialized = cache.Get(cacheKey);

            return serializer.Deserialize(serialized);
        }

    }
}