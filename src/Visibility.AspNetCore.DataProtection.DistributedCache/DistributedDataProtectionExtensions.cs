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
        public const string DataProtectionKeysName = "DataProtection-Keys";

        public static IDataProtectionBuilder PersistKeysToDistributedCache(this IDataProtectionBuilder builder, string key = DataProtectionKeysName)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Services.TryAddSingleton<IXmlRepository>(services => new DistributedDataXmlRepository(services.GetRequiredService<IDistributedCache>(), key));
            return builder;
        }
    }
}