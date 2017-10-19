using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryAppCore.Domain.Entities.PostgreSql
{
    public class BookPostgreSql
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public string Name { get; set; }
        public string Language { get; set; }
        public string Binding { get; set; }
        public int Pages { get; set; }
        public int Weight { get; set; }
        public Decimal Subscription { get; set; }
        public Decimal Price { get; set; }
        public string Description { get; set; }
        public int AuthorId { get; set; }
        public byte[] ImageBook { get; set; }
        public AuthorPostgreSql Author { get; set; }
        public SectionsPostgreSql Section { get; set; }
    }
}
