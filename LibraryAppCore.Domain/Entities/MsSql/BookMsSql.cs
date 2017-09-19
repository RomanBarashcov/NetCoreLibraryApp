using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryAppCore.Domain.Entities.MsSql
{
    public class BookMsSql
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int AuthorId { get; set; }
        public AuthorMsSql Author { get; set; }
    }
}
