# DistributedCache extensions for Data Protection [![Build status](https://ci.appveyor.com/api/projects/status/y11tw8rhxdms2i8v/branch/master?svg=true)](https://ci.appveyor.com/project/Narochno/narochno-aspnetcore-dataprotection-distributedcach/branch/master)

This contains two simple classes:

* DistributedCache DataProtection Provider
* DistributedCache PropertiesDataFormat

## DataProtection Provider ##
When having a distributed and stateless ASP.NET Core web server, you need to have your Data Protection keys saved to a location to be shared among your servers.

The default providers that the ASP.NET Core team provides are [here](https://github.com/aspnet/DataProtection/tree/dev/src)

I was just going to use Redis but the implementation is hard-coded to Redis.  I'm already using the [DistributedCache Redis provider](https://github.com/aspnet/Caching/tree/dev/src/Microsoft.Extensions.Caching.Redis), so why not just link in to that?  I don't need to configure two different things now.

### Usage ###

```
 services.AddDataProtection()
                .PersistKeysToDistributedCache();
```

Boom, now if you're using [IDistributedCache](https://github.com/aspnet/Caching/blob/dev/src/Microsoft.Extensions.Caching.Abstractions/IDistributedCache.cs) you now persist your generated DataProtection keys there.

## DistributedCache PropertiesDataFormat ##

Another issue is that the state on the URL can used for Authentication can be large.  Why not use cache?

This is inspired and mostly copied from: [https://github.com/IdentityServer/IdentityServer4/issues/407](https://github.com/IdentityServer/IdentityServer4/issues/407)

### Usage ###

Useful for any Authentication middleware.  You need to hook it into the `AuthenticationOptions` for your protocol like so:

I'm using [CAS Authentication](https://github.com/akunzai/GSS.Authentication.CAS)

```
 var dataProtectionProvider = app.ApplicationServices.GetRequiredService<IDataProtectionProvider>();
 var distributedCache = app.ApplicationServices.GetRequiredService<IDistributedCache>();

 var dataProtector = dataProtectionProvider.CreateProtector(
            typeof(CasAuthenticationMiddleware).FullName,
            typeof(string).FullName, schemeName,
            "v1");

//TODO: think of a better way to create
var dataFormat = new DistributedPropertiesDataFormat(distributedCache, dataProtector);

...

 app.UseCasAuthentication(x =>
            {
                x.StateDataFormat = dataFormat;
                ...
            };
```

OpenId and OAuth have `StateDataFormat` in their options.  I'm sure others do too.