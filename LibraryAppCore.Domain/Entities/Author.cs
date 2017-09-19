using LibraryAppCore.Domain.Entities.MsSql;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryAppCore.Domain.Entities
{
    public class Author
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }

        private AuthorMsSql authorMsSql;

        public Author(AuthorMsSql author)
        {
            Id = Convert.ToString(author.Id);
            Name = author.Name;
            Surname = author.Surname;
            authorMsSql = author;
        }

        public Author() { }
    }
}
