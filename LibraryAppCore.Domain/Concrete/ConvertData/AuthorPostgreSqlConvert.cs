using LibraryAppCore.Domain.Abstracts;
using LibraryAppCore.Domain.Entities;
using LibraryAppCore.Domain.Entities.MsSql;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryAppCore.Domain.Concrete.ConvertData
{
    public class AuthorPostgreSqlConvert : IConvertDataHelper<AuthorPostgreSql, Author>
    {
        private List<AuthorPostgreSql> Authotrs = new List<AuthorPostgreSql>();
        private AuthorPostgreSql AuthorPostgreSql = new AuthorPostgreSql();
        private Author authorsNode = new Author();
        private List<Author> ListAuthor = new List<Author>();
        private IEnumerable<Author> result = null;

        public void InitData(List<AuthorPostgreSql> authors)
        {
            Authotrs = authors;
        }

        public IEnumerable<Author> GetIEnumerubleDbResult()
        {
            foreach (AuthorPostgreSql a in Authotrs)
            {
                AuthorPostgreSql = new AuthorPostgreSql { Id = a.Id, Name = a.Name, Surname = a.Surname };
                authorsNode = new Author(AuthorPostgreSql);
                ListAuthor.Add(authorsNode);
                result = ListAuthor;
            }

            return result;
        }
    }
}
