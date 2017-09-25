using LibraryAppCore.Domain.Abstracts;
using LibraryAppCore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryAppCore.Domain.Concrete.DataRequired
{
    public class BookDataRequiredMDb : IDataRequired<Book>
    {
        public bool IsDataNoEmpty(Book book)
        {
            bool isDataNoEmpty = false;

            if (book.Year > 0 && !String.IsNullOrEmpty(book.Name) && !String.IsNullOrEmpty(book.Description) && !String.IsNullOrEmpty(book.AuthorId))
            {
                isDataNoEmpty = true;
            }

            return isDataNoEmpty;
        }
    }
}
