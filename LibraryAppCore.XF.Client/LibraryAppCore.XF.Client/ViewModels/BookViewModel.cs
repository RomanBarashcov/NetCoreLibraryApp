using LibraryAppCore.XF.Client.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryAppCore.XF.Client.ViewModels
{
    public class BookViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public Book Book { get; private set; }
        BooksListViewModel Blvm;

        public BookViewModel()
        {
            Book = new Book();

        }

        public BooksListViewModel ListViewModel
        {
            get { return Blvm; }
            set
            {
                if (Blvm != value)
                {
                    Blvm = value;
                    OnPropertyChanged("ListViewModel");
                }
            }
        }

        public string Id
        {
            get { return Book.Id; }
            set
            {
                if (Book.Id != value)
                {
                    Book.Id = value;
                    OnPropertyChanged("BookId");
                }

            }
        }

        public int Year
        {
            get { return Book.Year; }
            set
            {
                if (Book.Year != value)
                {
                    Book.Year = value;
                    OnPropertyChanged("Year");
                }
            }
        }

        public string Name
        {
            get { return Book.Name; }
            set
            {
                if (Book.Name != value)
                {
                    Book.Name = value;
                    OnPropertyChanged("Name");
                }
            }
        }

        public string Description
        {
            get { return Book.Description; }
            set
            {
                if (Book.Description != value)
                {
                    Book.Description = value;
                    OnPropertyChanged("Description");
                }
            }
        }

        public string AuthorId
        {
            get { return Book.AuthorId; }
            set
            {
                if (Book.AuthorId != value)
                {
                    Book.AuthorId = value;
                    OnPropertyChanged("AuthorId");
                }
            }
        }

        public bool IsValid
        {
            get
            {
                return ((!string.IsNullOrEmpty(Book.Id)) ||
                    (Book.Year > 0) ||
                    (!string.IsNullOrEmpty(Name.Trim())) ||
                    (!string.IsNullOrEmpty(Book.Description)) ||
                    (!string.IsNullOrEmpty(Book.Id)));
            }
        }

        protected void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
