using System;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Visibility.AspNetCore.DataProtection.DistributedCache
{
    public static class DistributedDataProtectionExtensions
    {
        public static IDataProtectionBuilder PersistKeysToDistributedCache(this IDataProtectionBuilder builder, DistributedDataXmlRepositoryOptions options = null)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Services.TryAddSingleton<IXmlRepository>(services => new DistributedDataXmlRepository(services.GetRequiredService<IDistributedCache>(), 
                options ?? new DistributedDataXmlRepositoryOptions()));
            return builder;
        }
    }
}