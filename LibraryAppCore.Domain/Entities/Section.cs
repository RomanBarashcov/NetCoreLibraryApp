using LibraryAppCore.Domain.Entities.PostgreSql;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryAppCore.Domain.Entities
{
    public class Section
    {
        public string Id { get; set; }
        public bool New { get; set; }
        public bool ForFamily { get; set; }
        public bool Technical { get; set; }
        public bool Fiction { get; set; }
        public bool ForBusness { get; set; }
        public string BookId { get; set; }

        public SectionsPostgreSql postgreSqlSection { get; set; }

        public Section(SectionsPostgreSql Section)
        {
            Id = Convert.ToString(Section.Id);
            New = Section.New;
            ForFamily = Section.ForFamily;
            Technical = Section.Technical;
            Fiction = Section.Fiction;
            ForBusness = Section.ForBusness;
            BookId = Convert.ToString(Section.BookId);
        }
        public Section() { }
    }
}
