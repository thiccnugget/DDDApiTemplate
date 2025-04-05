using Application.Interfaces;
namespace Infrastructure.Cache
{
    public class CacheKeyGenerator : ICacheKeyGenerator
    {
        private const string Separator = ":";

        private string Build(string prefix, params string[] parts)
        {
            var sanitizedParts = parts.Select(p => p?.Replace(Separator, "_"));
            return string.Join(Separator, new[] { prefix }.Concat(sanitizedParts));
        }

        public string ForObject<T>(string identifier) where T : class
        {
            return Build(typeof(T).Name.ToLowerInvariant(), identifier.ToString());
        }

        public string ForCollection<T>(string filter = "all") where T : IEnumerable<T>
        {
            return Build($"{typeof(T).Name.ToLowerInvariant()}_list", filter);
        }
    }
}
