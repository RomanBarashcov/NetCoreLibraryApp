using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryAppCore.Domain.Abstracts
{
    public interface IConvertDataHelper<T,U>
    {
        void InitData(List<T> data);
        IEnumerable<U> GetIEnumerubleDbResult();
    }
}
