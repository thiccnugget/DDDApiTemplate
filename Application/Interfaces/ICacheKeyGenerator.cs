namespace Application.Interfaces
{
    public interface ICacheKeyGenerator
    {
        string ForObject<T>(string identifier) where T : class;
        string ForCollection<TCollection>(string filter = "all") where TCollection : IEnumerable<TCollection>;
    }
}
