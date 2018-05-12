using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LibraryAppCore.Domain.Pagination;

namespace LibraryAppCore.Domain.Abstracts
{
    public interface IPagination<T>
    {
        Task<PagedResults<T>> CreatePagedResultsAsync(IQueryable<T> queryable, int page, int pageSize, string orderBy, bool ascending);
    }
}
