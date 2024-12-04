using Application.Interfaces;
using Domain.Entities;

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

        public string ForEntity<T>(string id) where T : BaseEntity
        {
            return Build(typeof(T).Name.ToLowerInvariant(), id);
        }

        public string ForCollection<T>(string? filter = null) where T : IEnumerable<T>
        {
            return Build($"{typeof(T).Name.ToLowerInvariant()}_list", filter ?? "all");
        }
    }
}
