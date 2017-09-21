using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryAppCore.Domain.Entities.MsSql
{
    public class BookPostgreSql
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int AuthorId { get; set; }
        public AuthorPostgreSql Author { get; set; }
    }
}
