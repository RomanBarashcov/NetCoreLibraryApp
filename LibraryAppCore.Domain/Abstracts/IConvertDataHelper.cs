using LibraryAppCore.Domain.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibraryAppCore.Domain.Abstracts
{
    public interface IConvertDataHelper<T,U>
    {
        void InitData(PagedResults<T> data);
        void InitData(List<T> data);
        PagedResults<U> GetFormatedPagedResults();
        IEnumerable<U> GetFormatedEnumResult();
    }
}
