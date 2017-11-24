using LibraryAppCore.XF.Client.Entities;
using LibraryAppCore.XF.Client.Interfaces;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LibraryAppCore.XF.Client.Repositories
{
    public class AuthorRepository
    {
        SQLiteConnection db;

        public AuthorRepository(string filename)
        {
            string databasePath = DependencyService.Get<ISqlLite>().GetDatabasePath(filename);
            db = new SQLiteConnection(databasePath);
            db.CreateTable<Author>();
        }

        public IEnumerable<Author> GetAuthors()
        {
            return (from i in db.Table<Author>() select i).ToList();
        }

        public Author GetAuthor(int id)
        {
            return db.Get<Author>(id);
        }

        public int SaveAuthor(Author author)
        {
            if(author.Id != 0)
            {
                db.Update(author);
                return author.Id;
            }
            else
            {
                return db.Insert(author);
            }
        }

        public int DeleteAuthor(int id)
        {
            return db.Delete<Author>(id);
        }
    }
}
