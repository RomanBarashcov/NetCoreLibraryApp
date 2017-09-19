using LibraryAppCore.Domain.Abstracts;
using LibraryAppCore.Domain.Entities;
using LibraryAppCore.Domain.Entities.MondoDb;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryAppCore.Domain.Concrete.ConvertData
{
    public class AuthorMongoDbConvert : IConvertDataHelper<AuthorMongoDb, Author>
    {
        private List<AuthorMongoDb> Authors = new List<AuthorMongoDb>();
        private AuthorMongoDb AuthorMongoDB = new AuthorMongoDb();
        private Author authorNode = new Author();
        private List<Author> ListAuthor = new List<Author>();
        private IEnumerable<Author> result = null;

        public void InitData(List<AuthorMongoDb> authors)
        {
            Authors = authors;
        }

        public IEnumerable<Author> GetIEnumerubleDbResult()
        {
            foreach (AuthorMongoDb b in Authors)
            {
                AuthorMongoDB = new AuthorMongoDb { Id = b.Id, Name = b.Name, Surname = b.Surname };
                authorNode = new Author(AuthorMongoDB);
                ListAuthor.Add(authorNode);
                result = ListAuthor;
            }

            return result;
        }
    }
}
