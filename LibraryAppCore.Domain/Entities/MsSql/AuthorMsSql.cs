using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryAppCore.Domain.Entities.MsSql
{
    public class AuthorMsSql
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }

        public ICollection<BookMsSql> books { get; set; }
        public AuthorMsSql()
        {
            books = new List<BookMsSql>();
        }
    }
}
