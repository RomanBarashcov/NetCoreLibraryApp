using LibraryAppCore.Domain.Entities.MondoDb;
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

        private AuthorPostgreSql authorPostgreSql;
        private AuthorMongoDb AuthorMongoDb;

        public Author(AuthorPostgreSql author)
        {
            Id = Convert.ToString(author.Id);
            Name = author.Name;
            Surname = author.Surname;
            authorPostgreSql = author;
        }

        public Author(AuthorMongoDb author)
        {
            Id = author.Id;
            Name = author.Name;
            Surname = author.Surname;
            AuthorMongoDb = author;
        }

        public Author() { }
    }
}
