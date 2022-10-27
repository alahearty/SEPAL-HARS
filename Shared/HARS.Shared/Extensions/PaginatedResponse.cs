using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HARS.Shared.Extensions
{
    public class PaginatedResponse<T>
    {
        public PaginatedResponse(int currentPage, int pageSize, IQueryable<T> source)
        {
            CurrentPage = currentPage;
            PageSize = pageSize;
            TotalCount = source.Count();
            TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);
            Entities = source.Skip((CurrentPage - 1) * pageSize).Take(PageSize).ToList();
        }
        public int CurrentPage { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }
        public List<T> Entities { get; private set; }
    }
}
