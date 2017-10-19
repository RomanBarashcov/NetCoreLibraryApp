using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryAppCore.Domain.Entities.PostgreSql
{
    public class AuthorPostgreSql
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }

        public ICollection<BookPostgreSql> books { get; set; }
        public AuthorPostgreSql()
        {
            books = new List<BookPostgreSql>();
        }
    }
}
