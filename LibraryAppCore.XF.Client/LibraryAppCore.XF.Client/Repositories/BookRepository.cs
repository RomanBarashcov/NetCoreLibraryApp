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
    public class BookRepository
    {
        SQLiteConnection db;

        public BookRepository(string filename)
        {
            string databasePath = DependencyService.Get<ISqlLite>().GetDatabasePath(filename);
            db = new SQLiteConnection(databasePath);
            db.CreateTable<Book>();
        }

        public IEnumerable<Book> GetBooks()
        {
            return (from i in db.Table<Book>() select i).ToList();
        }

        public Book GetBook(int id)
        {
            return db.Get<Book>(id);
        }

        public int SaveBook(Book book)
        {
            if (book.Id != 0)
            {
                db.Update(book);
                return book.Id;
            }
            else
            {
                return db.Insert(book);
            }
        }

        public int DeleteBook(int id)
        {
            return db.Delete<Book>(id);
        }
    }
}
