using LibraryAppCore.XF.Client.Models;
using LibraryAppCore.XF.Client.Pagination;
using LibraryAppCore.XF.Client.Services;
using LibraryAppCore.XF.Client.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace LibraryAppCore.XF.Client.ViewModels
{
    public class BooksListViewModel : INotifyPropertyChanged
    {
        bool initialized = false;
        private bool isBusy;

        public ObservableCollection<Book> Books { get; set; }
        private BookService bookService;
        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand CreateBookCommand { protected set; get; }
        public ICommand DeleteBookCommand { protected set; get; }
        public ICommand SaveBookCommand { protected set; get; }
        public ICommand BackCommand { protected set; get; }

        Book selectedBook;
        public INavigation Navigation { get; set; }

        public BooksListViewModel()
        {
            Books = new ObservableCollection<Book>();
            bookService = new BookService();
            isBusy = false;
            CreateBookCommand = new Command(CreateBook);
            SaveBookCommand = new Command(SaveBook);
            DeleteBookCommand = new Command(DeleteBook);
            BackCommand = new Command(Back);
        }


        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                OnPropertyChanged("IsBusy");
                OnPropertyChanged("IsLoaded");
            }
        }

        public bool IsLoaded
        {
            get { return !isBusy; }
        }

        public Book SelectedBook
        {
            get { return selectedBook; }
            set
            {
                if (selectedBook != value)
                {
                    Book tempBook = new Book
                    {
                        Id = value.Id,
                        Year = value.Year,
                        Name = value.Name,
                        Description = value.Description,
                        AuthorId = value.AuthorId
                    };

                    selectedBook = null;
                    OnPropertyChanged("SelectedBook");
                    Navigation.PushAsync(new BookPage(tempBook, this));
                }
            }
        }

        public async Task GetBooks()
        {
            if (initialized == true) return;
            isBusy = true;

            PagedResults<Book> bookReuslt = await bookService.GetBooks();
            Book bModel = new Book();

            while (Books.Any())
                Books.RemoveAt(Books.Count - 1);

            foreach (var a in bookReuslt.Results)
                Books.Add(a);

            IsBusy = false;
            initialized = true;
        }

        private void Back()
        {
            Navigation.PopAsync();
        }

        protected void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        private void CreateBook()
        {
            Navigation.PushAsync(new BookPage(new Book(), this));
        }

        private async void SaveBook(object bookObject)
        {
            Book book = bookObject as Book;

            if (book != null)
            {
                IsBusy = true;

                if (!string.IsNullOrEmpty(book.Id))
                {
                    bool isBookUpdated = await bookService.UpdateBook(book);

                    if (isBookUpdated)
                    {
                        initialized = false;
                        await GetBooks();
                    }
                }
                else
                {
                    bool isBookCreated = await bookService.CreateBook(book);
                    if (isBookCreated)
                    {
                        initialized = false;
                        await GetBooks();
                    }
                }
                initialized = true;
                IsBusy = false;
            }
            Back();
        }

        private async void DeleteBook(object bookObject)
        {
            Book book = bookObject as Book;
            if (book != null)
            {
                IsBusy = true;
                bool isBookDeleted = await bookService.DeleteBook(book.Id);
                if (isBookDeleted)
                {
                    initialized = false;
                    await GetBooks();
                }
                initialized = true;
                IsBusy = false;
            }
            Back();
        }
    }
}
