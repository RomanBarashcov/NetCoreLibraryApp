using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryAppCore.Domain.Abstracts
{
    public interface IDataRequired<T>
    {
        bool IsDataNoEmpty(T data);
    }
}
