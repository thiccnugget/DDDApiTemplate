using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Classes
{
    public class PagedResult<T>
    {
        public IEnumerable<T> Data { get; set; }
        public readonly PagingMetadata Paging;

        public PagedResult(IEnumerable<T> data, PagingMetadata paging)
        {
            Data = data;
            Paging = paging;
        }
    }
    public partial class PagingMetadataInput()
    {
        public bool HasPreviousPage;
        public bool HasNextPage;
        public int CurrentPage;
        public int PageSize;
        public long TotalItems;
        public int TotalPages;
        public int Limit;
    };

    public class PagingMetadata
    {
        public readonly int CurrentPage;
        public readonly int PageSize;
        public readonly long TotalItems;
        public readonly int TotalPages;
        public readonly bool HasPreviousPage;               
        public readonly bool HasNextPage;
        public readonly int Skip;
        public readonly int Limit;

        public PagingMetadata(PagingMetadataInput data)
        {
            CurrentPage = data.CurrentPage;
            PageSize = data.PageSize;
            TotalItems = data.TotalItems;
            TotalPages = data.TotalPages;
            HasPreviousPage = data.HasPreviousPage;
            HasNextPage = data.HasNextPage;
            Limit = data.Limit;
        }

    }
}
