using LibraryAppCore.Domain.Abstracts;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LibraryAppCore.Domain.Extensions;
using LibraryAppCore.Domain.Entities.MsSql;
using System;

namespace LibraryAppCore.Domain.Pagination.Concrete
{
    public class Pagination<T> : IPagination<T>
    {
        public async Task<PagedResults<T>> CreatePagedResultsAsync(IQueryable<T> queryable, int page, int pageSize, string orderBy, bool ascending)
        {
            var skipAmount = pageSize * (page - 1);
            var projection = await queryable.OrderByPropertyOrField(orderBy, ascending).Skip(skipAmount).Take(pageSize).ToListAsync();
       
            var totalNumberOfRecords = await queryable.CountAsync();

            var mod = totalNumberOfRecords % pageSize;
            int totalPageCount = (totalNumberOfRecords / pageSize) + (mod == 0 ? 0 : 1);
            int[] totalPageArr = new int[totalPageCount];

            for (int p = 1; p <= totalPageCount; p++)
            {
                totalPageArr[p - 1] =  p;
            }

            return new PagedResults<T>
            {
                Results = projection,
                PageNumber = page,
                PageSize = projection.Count,
                TotalNumberOfPages = totalPageArr,
                TotalNumberOfRecords = totalNumberOfRecords,
            };
        }
    }
}

