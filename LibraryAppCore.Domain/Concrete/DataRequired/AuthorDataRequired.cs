﻿using LibraryAppCore.Domain.Abstracts;
using LibraryAppCore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryAppCore.Domain.Concrete.DataRequired
{
    public class AuthorDataRequired : IDataRequired<Author>
    {
        public bool IsDataNoEmpty(Author author)
        {
            bool isDataNoEmpty = false;

            try
            {
                if (!String.IsNullOrEmpty(author.Name) && !String.IsNullOrEmpty(author.Surname))
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
