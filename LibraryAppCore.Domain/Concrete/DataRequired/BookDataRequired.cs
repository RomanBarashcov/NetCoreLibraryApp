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
            if (IsDataConvertingCorrect(book.AuthorId))
            {
                if (book.Year > 0 && !String.IsNullOrEmpty(book.Name) && !String.IsNullOrEmpty(book.Description) && !String.IsNullOrEmpty(book.AuthorId))
                {
                    isDataNoEmpty = true;
                }
            }
            return isDataNoEmpty;
        }

        private bool IsDataConvertingCorrect(string bookAuthorId)
        {
            bool IsConveted = false;
            int bAuthorId = 0;

            try
            {
                bAuthorId = Convert.ToInt32(bookAuthorId);
                IsConveted = true;
            }
            catch
            {
                return IsConveted;
            }

            return IsConveted;
        }
    }
}
