using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryAppCore.Domain.Entities.PostgreSql
{
    public class SectionsPostgreSql
    {
        public int Id { get; set; }
        public bool New { get; set; }
        public bool ForFamily { get; set; }
        public bool Technical { get; set; }
        public bool Fiction { get; set; }
        public bool ForBusness { get; set; }

        public BookPostgreSql book { get; set; }
        public int BookId { get; set; }

    }
}
