using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Paging
{
    public class PagedResultMetadata
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public long TotalItems { get; set; }
        public int TotalPages => (int)Math.Ceiling(TotalItems / (double)PageSize);
        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;
        public int Skip => (CurrentPage - 1) * PageSize;

        public static PagedResultMetadata Create(int currentPage, int pageSize, long totalItems)
        {
            return new PagedResultMetadata
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                TotalItems = totalItems
            };
        }
    }
    
    public class PagedResultDto<T>
    {
        public IEnumerable<T> Data { get; set; } = new List<T>();
        public PagedResultMetadata Paging { get; set; } = new();

        public PagedResultDto(IEnumerable<T> data, PagedResultMetadata paging)
        {
            Data = data;
            Paging = paging;
        }
    }
}
