using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.Extensions.Caching.Distributed;

namespace Narochno.AspNetCore.DataProtection.DistributedCache
{
    public class DistributedDataXmlRepository : IXmlRepository
    {
        private readonly IDistributedCache cache;
        private readonly DistributedDataXmlRepositoryOptions options;

        public DistributedDataXmlRepository(IDistributedCache cache, DistributedDataXmlRepositoryOptions options)
        {
            this.cache = cache;
            this.options = options;
        }

        public IReadOnlyCollection<XElement> GetAllElements()
        {
            return GetAllElementsCore().ToList().AsReadOnly();
        }

        private IEnumerable<XElement> GetAllElementsCore()
        {
            var document = cache.GetString(options.Key);
            XDocument xdoc;
            if (document == null)
            {
                xdoc = new XDocument();
            }
            else
            {
                xdoc = XDocument.Parse(document);
            }
            return xdoc.Elements();
        }

        public void StoreElement(XElement element, string friendlyName)
        {
            var document = cache.GetString(options.Key);
            XDocument xdoc;
            if (document == null)
            {
                xdoc = new XDocument();
            }
            else
            {
                xdoc = XDocument.Parse(document);
            }
            xdoc.Add(element);
            cache.SetString(options.Key, xdoc.ToString(SaveOptions.DisableFormatting), options.CacheOptions);
        }
    }
}
