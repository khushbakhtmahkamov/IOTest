using System.Collections.Generic;

namespace ProductService.DTOs
{
    public class PaginatedDto<T>
    {
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public IEnumerable<T> Items { get; set; }
    }
}
