using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.Extensions.Caching.Distributed;

namespace Visibility.AspNetCore.DataProtection.DistributedCache
{
    public class DistributedPropertiesDataFormat : ISecureDataFormat<AuthenticationProperties>
    {
        public const string CacheKeyPrefix = "DistributedPropertiesDataFormat-";
        
        private readonly IDistributedCache cache;
        private readonly IDataProtector dataProtector;
        private readonly DistributedPropertiesDataFormatOptions options;


        public DistributedPropertiesDataFormat(IDistributedCache cache, IDataProtector dataProtector, 
            DistributedPropertiesDataFormatOptions options = null)
        {
            this.cache = cache;
            this.dataProtector = dataProtector;
            this.options = options ?? new DistributedPropertiesDataFormatOptions();
        }

        public string Protect(AuthenticationProperties data)
        {
            return Protect(data, null);
        }

        public string Protect(AuthenticationProperties data, string purpose)
        {
            var key = Guid.NewGuid().ToString();
            var cacheKey = $"{CacheKeyPrefix}{key}";

            var serialized = options.Serializer.Serialize(data);
            
            cache.Set(cacheKey, serialized, options.CacheOptions);
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

            return options.Serializer.Deserialize(serialized);
        }

    }
}