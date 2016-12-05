using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.AspNetCore.DataProtection.Repositories;

namespace Visibility.AspNetCore.DataProtection.DistributedCache
{
    public class DistributedDataXmlRepository : IXmlRepository
    {
        private readonly IDistributedCache cache;
        private readonly string prefix;

        public DistributedDataXmlRepository(IDistributedCache cache, string prefix)
        {
            this.cache = cache;
            this.prefix = prefix;
        }

        public IReadOnlyCollection<XElement> GetAllElements()
        {
            return GetAllElementsCore().ToList().AsReadOnly();
        }

        private IEnumerable<XElement> GetAllElementsCore()
        {
            var document = cache.GetStringAsync(prefix).Result;
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
            var document = cache.GetStringAsync(prefix).Result;
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
            cache.SetStringAsync(prefix, xdoc.ToString(SaveOptions.DisableFormatting));
        }
    }
}
