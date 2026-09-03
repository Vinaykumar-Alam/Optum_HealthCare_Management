using System;
using System.Collections.Generic;
using System.Text;

namespace optum_healthcare_system.Models.DTOs
{
    public class PaginationDto<T>
    {
        public IEnumerable<T> Data { get; set; } = [];
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages => pageSize<=0? 0 : (int)Math.Ceiling((double)TotalCount/pageSize);
        public bool hasNextPage => pageNumber < TotalPages;
        public bool hasPreviousPage => pageNumber > 1;
    }
}
