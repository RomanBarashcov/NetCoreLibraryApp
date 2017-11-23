using LibraryAppCore.XF.Client.Models;
using LibraryAppCore.XF.Client.Pagination;
using LibraryAppCore.XF.Client.Services;
using LibraryAppCore.XF.Client.Views;
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

        public ICommand CreateBookCommand { protected set; get; }
        public ICommand DeleteBookCommand { protected set; get; }
        public ICommand SaveBookCommand { protected set; get; }
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
            CreateBookCommand = new Command(CreateBook);
            SaveBookCommand = new Command(SaveBook);
            DeleteBookCommand = new Command(DeleteBook);
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

            PagedResults<Book> bookReuslt = await bookService.GetBooks(currentPage, currentOrderBy, currentAscending);
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
                    await LoadBooks();
                }
                initialized = true;
                IsBusy = false;
            }
            Back();
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
    }
}
