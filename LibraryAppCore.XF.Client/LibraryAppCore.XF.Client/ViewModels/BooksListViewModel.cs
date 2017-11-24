using LibraryAppCore.XF.Client.Models;
using LibraryAppCore.XF.Client.Pagination;
using LibraryAppCore.XF.Client.Services;
using LibraryAppCore.XF.Client.Views;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace LibraryAppCore.XF.Client.ViewModels
{
    public class BooksListViewModel : INotifyPropertyChanged
    {
        public bool initialized { get; set; }
        private bool isBusy;

        public ObservableCollection<Book> Books { get; set; }
        private BookService bookService;
        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand BackCommand { protected set; get; }
        public ICommand BackPageCommand { protected set; get; }
        public ICommand NextPageCommand { protected set; get; }

        public ICommand SortTableByIdCommand { protected set; get; }
        public ICommand SortTableByYearCommand { protected set; get; }
        public ICommand SortTableByNameCommand { protected set; get; }
        public ICommand SortTableByDescriptionCommand { protected set; get; }
        public ICommand SortTableByAuthorNameCommand { protected set; get; }

        Book selectedBook;
        public INavigation Navigation { get; set; }

        public static int currentPage { get; set; }
        public static string currentOrderBy { get; set; }
        public static bool currentAscending { get; set; }

        public BooksListViewModel()
        {
            Books = new ObservableCollection<Book>();
            bookService = new BookService();
            isBusy = false;
            initialized = false;
            currentPage = 1;
            currentAscending = true;
            currentOrderBy = "Id";
            SortTableByIdCommand = new Command(SortById);
            SortTableByYearCommand = new Command(SortByYear);
            SortTableByNameCommand = new Command(SortByName);
            SortTableByDescriptionCommand = new Command(SortByDescription);
            SortTableByAuthorNameCommand = new Command(SortByAuthorName);
            BackCommand = new Command(Back);
            BackPageCommand = new Command(BackPage);
            NextPageCommand = new Command(NextPage);
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
                        AuthorName = value.AuthorName,
                        AuthorId = value.AuthorId
                    };

                    selectedBook = null;
                    OnPropertyChanged("SelectedBook");
                    Navigation.PushAsync(new BookPage(tempBook, this));
                }
            }
        }

        public async void SortById()
        {
            initialized = false;
            currentOrderBy = "Id";
            currentAscending = currentAscending == true ? false : true;
            await LoadBooks();
        }

        public async void SortByYear()
        {
            initialized = false;
            currentOrderBy = "Year";
            currentAscending = currentAscending == true ? false : true;
            await LoadBooks();
        }

        public async void SortByName()
        {
            initialized = false;
            currentOrderBy = "Name";
            currentAscending = currentAscending == true ? false : true;
            await LoadBooks();
        }

        public async void SortByDescription()
        {
            initialized = false;
            currentOrderBy = "Description";
            currentAscending = currentAscending == true ? false : true;
            await LoadBooks();
        }

        public async void SortByAuthorName()
        {
            initialized = false;
            currentOrderBy = "AuthorName";
            currentAscending = currentAscending == true ? false : true;
            await LoadBooks();
        }

        public async Task LoadBooks()
        {
            if (initialized == true) return;
            isBusy = true;

            if (IsInConnection())
            {
                PagedResults<Book> books = await bookService.GetBooks(currentPage, currentOrderBy, currentAscending);

                while (Books.Any())
                    Books.RemoveAt(Books.Count - 1);

                for (int b = 0; b < books.Results.Count; b++)
                {
                    Book book = new Book
                    {
                        Id = books.Results[b].Id,
                        Year = books.Results[b].Year,
                        Name = books.Results[b].Name,
                        Description = books.Results[b].Description,
                        AuthorName = books.Results[b].AuthorName,
                        AuthorId = books.Results[b].AuthorId
                    };

                    Books.Add(book);
                    book = null;
                }

            }
            else
            {
                List<Entities.Book> books = App.BookDb.GetBooks().ToList();

                while (Books.Any())
                    Books.RemoveAt(Books.Count - 1);

                for (int b = 0; b < books.Count; b++)
                {
                    Book book = new Book
                    {
                        Id = books[b].Id.ToString(),
                        Year = books[b].Year,
                        Name = books[b].Name,
                        Description = books[b].Description,
                        //AuthorName = books[b].AuthorName,
                        AuthorId = books[b].AuthorId.ToString()
                    };

                    Books.Add(book);
                    book = null;
                }
            }

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

        public async void SaveBook(Book book)
        {
            if (book != null)
            {
                IsBusy = true;

                if (!string.IsNullOrEmpty(book.Id))
                {
                    bool isBookUpdated = await bookService.UpdateBook(book);

                    if (isBookUpdated)
                    {
                        initialized = false;
                        await LoadBooks();
                    }
                }
                else
                {
                    bool isBookCreated = await bookService.CreateBook(book);
                    if (isBookCreated)
                    {
                        initialized = false;
                        await LoadBooks();
                    }
                }
            }
        }

        public async void DeleteBook(Book book)
        {
            if (book != null)
            {
                IsBusy = true;
                bool isBookDeleted = await bookService.DeleteBook(book.Id);
                if (isBookDeleted)
                {
                    initialized = false;
                    await LoadBooks();
                }
            }
        }

        private async void NextPage()
        {
            initialized = false;
            currentPage = currentPage + 1;
            await LoadBooks();
        }

        private async void BackPage()
        {
            initialized = false;
            currentPage = currentPage - 1;
            if (currentPage >= 1)
                await LoadBooks();
            else
                currentPage = 1;
        }

        public bool IsInConnection()
        {
            string curConnection = App.ConnectionType;

            if (curConnection == "No Connection")
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
