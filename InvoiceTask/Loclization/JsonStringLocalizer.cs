using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace InvoiceTask.Loclization
{
    public class JsonStringLocalizer : IStringLocalizer
    {

        private readonly JsonSerializer _serializer = new();
        private readonly IDistributedCache cache;

        public JsonStringLocalizer(IDistributedCache cache)
        {
            this.cache = cache;
        }
        public LocalizedString this[string name]
        {
            get
            {
                var value = GetString(name);
                return new LocalizedString(name, value);
            }
        }
        public LocalizedString this[string name, params object[] arguments]
        {
            get
            {
                var actualValue = this[name];
                return !actualValue.ResourceNotFound
                    ? new LocalizedString(name, string.Format(actualValue.Value, arguments))
                    : actualValue;
            }
        }

        // all localized strings from the JSON file
        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            var filePath = $"Resources/{Thread.CurrentThread.CurrentCulture.Name}.json";

            using FileStream stream = new(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            using StreamReader streamReader = new(stream);
            using JsonTextReader reader = new(streamReader);

            while (reader.Read())
            {
                if (reader.TokenType != JsonToken.PropertyName)
                    continue;

                var key = reader.Value as string;
                reader.Read();
                var value = _serializer.Deserialize<string>(reader);
                yield return new LocalizedString(key, value);
            }
        }


        //  method to fetch a localized string for a specific key
        private string GetString(string key)
        {
            var filePath = $"Resources/{Thread.CurrentThread.CurrentCulture.Name}.json";
            var fullFilePath = Path.GetFullPath(filePath);

            if (File.Exists(fullFilePath))
            {
                var cacheKey = $"locale_{Thread.CurrentThread.CurrentCulture.Name}_{key}";
                var cacheValue = cache.GetString(cacheKey);

                if (!string.IsNullOrEmpty(cacheValue))
                    return cacheValue;

                var result = GetValueFromJSON(key, fullFilePath);

                if (!string.IsNullOrEmpty(result))
                    cache.SetString(cacheKey, result);

                return result;
            }
            return string.Empty;
        }


        //method to fetch a value from a JSON file for a specific key
        private string GetValueFromJSON(string propertyName, string filepath)
        {
            if (string.IsNullOrWhiteSpace(propertyName) || string.IsNullOrEmpty(filepath))
                return string.Empty;
            using FileStream stream = new(filepath, FileMode.Open, FileAccess.Read, FileShare.Read);
            using StreamReader streamreader = new(stream);
            using JsonTextReader reader = new(streamreader);

            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.PropertyName && reader.Value as string == propertyName)
                {
                    reader.Read();
                    return _serializer.Deserialize<string>(reader);

                }

            }
            return string.Empty;

        }
    }
}
