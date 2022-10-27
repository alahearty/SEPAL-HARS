using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HARS.Shared.Extensions
{
    public static class CollectionExtension
    {
        public static bool NotAny<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null) return true;
            return !source.Any();
        }

        public static bool NotContains<TSource>(this IEnumerable<TSource> source, TSource value)
        {
            if (source == null) return true;
            return !source.Contains(value);
        }
        public static bool NotAny<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null) return true;
            return !source.Any(predicate);
        }

        public static PaginatedResponse<TSource> Paginate<TSource>(this IQueryable<TSource> source, int page, int pageSize)
        {
            return new PaginatedResponse<TSource>(page, pageSize, source);
        }
    }
}
