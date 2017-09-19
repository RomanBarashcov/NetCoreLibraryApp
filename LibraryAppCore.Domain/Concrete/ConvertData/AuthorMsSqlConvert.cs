using LibraryAppCore.Domain.Abstracts;
using LibraryAppCore.Domain.Entities;
using LibraryAppCore.Domain.Entities.MsSql;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryAppCore.Domain.Concrete.ConvertData
{
    public class AuthorMsSqlConvert : IConvertDataHelper<AuthorMsSql, Author>
    {
        private List<AuthorMsSql> Authotrs = new List<AuthorMsSql>();
        private AuthorMsSql AuthorMssql = new AuthorMsSql();
        private Author authorsNode = new Author();
        private List<Author> ListAuthor = new List<Author>();
        private IEnumerable<Author> result = null;

        public void InitData(List<AuthorMsSql> authors)
        {
            Authotrs = authors;
        }

        public IEnumerable<Author> GetIEnumerubleDbResult()
        {
            foreach (AuthorMsSql a in Authotrs)
            {
                AuthorMssql = new AuthorMsSql { Id = a.Id, Name = a.Name, Surname = a.Surname };
                authorsNode = new Author(AuthorMssql);
                ListAuthor.Add(authorsNode);
                result = ListAuthor;
            }

            return result;
        }
    }
}
