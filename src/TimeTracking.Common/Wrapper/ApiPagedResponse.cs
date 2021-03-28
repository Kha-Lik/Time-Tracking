using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TimeTracking.Common.Mappers;
using TimeTracking.Common.Pagination;

namespace TimeTracking.Common.Wrapper
{
    public class ApiPagedResponse<T> : ApiResponse
        where T : class, new()
    {
        public List<T> Data { get; set; }
        public int CurrentPage { get; set; }
        public int ResultsPerPage { get; set; }
        public int TotalPages { get; set; }
        public long TotalResults { get; set; }

        public ApiPagedResponse<T> FromPagedResult<TEntity>(PagedResult<TEntity> pagedResult,
            Func<TEntity, T> mapper)
        where TEntity : class, new()
        {
            return new ApiPagedResponse<T>()
            {
                Data = pagedResult.Items.Select(mapper).ToList(),
                CurrentPage = pagedResult.CurrentPage,
                ResultsPerPage = pagedResult.ResultsPerPage,
                TotalPages = pagedResult.TotalPages,
                TotalResults = pagedResult.TotalResults,
            };
        }
    }
}