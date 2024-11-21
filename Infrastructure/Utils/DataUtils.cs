using Infrastructure.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Utils
{
    internal static class DataUtils
    {
        public static PagedResult<T> GeneratePagedResult<T>(IEnumerable<T> data, long totalItems, int page, int limit)
        {
            PagingMetadata metadata = new PagingMetadata(new PagingMetadataInput
            {
                HasPreviousPage = data.Count() > 1 && page > 1,
                HasNextPage = page < (int)Math.Ceiling(totalItems / (double)limit),
                CurrentPage = page,
                PageSize = data.Count(),
                TotalItems = totalItems,
                TotalPages = (int)Math.Ceiling(totalItems / (double)limit),
                Limit = limit
            });
            return new PagedResult<T>(data, metadata);
        }
    }
}
