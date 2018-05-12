using LibraryAppCore.Domain.Abstracts;
using LibraryAppCore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryAppCore.Domain.Concrete.DataRequired
{
    public class BookDataRequired : IDataRequired<Book>
    {
        public bool IsDataNoEmpty(Book book)
        {
            bool isDataNoEmpty = false;

            try
            {
                if (book.Year > 0 && !String.IsNullOrEmpty(book.Name) && !String.IsNullOrEmpty(book.Description) && !String.IsNullOrEmpty(book.AuthorId))
                {
                    isDataNoEmpty = true;
                }
            }
            catch
            {
                return isDataNoEmpty = false;
            }

            return isDataNoEmpty;
        }
    }
}
