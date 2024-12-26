using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Localization;

namespace InvoiceTask.Loclization
{
    public class JsonStringLocalizerFactory : IStringLocalizerFactory
    {
        private readonly IDistributedCache cache;

        public JsonStringLocalizerFactory(IDistributedCache cache)
        {
            this.cache = cache;
        }

        public IStringLocalizer Create(Type resourceSource)
        {
            return new JsonStringLocalizer(cache);
        }

        public IStringLocalizer Create(string baseName, string location)
        {
            return new JsonStringLocalizer(cache);
        }
    }
}
