using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TimeTracking.Common.Pagination
{
    public static class PaginationExtensions
    {
        private const int ResultsPerPage = 3;
        
        public static async Task<PagedResult<T>> PaginateAsync<T>(this IQueryable<T>  queryeable,
            int page = 1, int resultsPerPage = ResultsPerPage)
        {
            if (page <= 0)
            {
                page = 1;
            }
            if (resultsPerPage <= 0)
            {
                resultsPerPage = ResultsPerPage;
            }
            if (!queryeable.Any())
            {
                return PagedResult<T>.Empty;
            }
            var totalResults = await queryeable.CountAsync();
            var totalPages = (int)Math.Ceiling((decimal)totalResults / resultsPerPage);
            var data = await queryeable.Limit(page, resultsPerPage).ToListAsync();

            return PagedResult<T>.Paginate(data, page, resultsPerPage, totalPages, totalResults);
        }

        private static IQueryable<T> Limit<T>(this IQueryable<T> collection,
            int page = 1, int resultsPerPage = 10)
        {
            if (page <= 0)
            {
                page = 1;
            }
            if (resultsPerPage <= 0)
            {
                resultsPerPage = ResultsPerPage;
            }
            var skip = (page - 1) * resultsPerPage;
            var data = collection.Skip(skip)
                .Take(resultsPerPage);

            return data;
        }
    }
}