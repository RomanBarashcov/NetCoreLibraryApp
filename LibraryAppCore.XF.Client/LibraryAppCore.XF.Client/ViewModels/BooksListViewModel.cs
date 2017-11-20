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
        public ObservableCollection<BookViewModel> Books { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand CreateBookCommand { protected set; get; }
        public ICommand DeleteBookCommand { protected set; get; }
        public ICommand SaveBookCommand { protected set; get; }
        public ICommand BackCommand { protected set; get; }

        BookViewModel selectedBook;
        public INavigation Navigation { get; set; }

        public BooksListViewModel()
        {
            Books = new ObservableCollection<BookViewModel>();
            CreateBookCommand = new Command(CreateBook);
            SaveBookCommand = new Command(SaveBook);
            DeleteBookCommand = new Command(DeleteBook);
            BackCommand = new Command(Back);
        }

        public BookViewModel SelectedBook
        {
            get { return selectedBook; }
            set
            {
                if (selectedBook != value)
                {
                    BookViewModel tempAuthor = value;
                    selectedBook = null;
                    OnPropertyChanged("SelectedFriend");
                    Navigation.PushAsync(new BookPage(tempAuthor));
                }
            }
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
            Navigation.PushAsync(new BookPage(new BookViewModel() { ListViewModel = this }));
        }

        private void SaveBook(object bookObject)
        {
            BookViewModel book = bookObject as BookViewModel;

            if (book != null && book.IsValid)
            {
                Books.Add(book);
            }

            Back();
        }

        private void DeleteBook(object bookObject)
        {
            BookViewModel book = bookObject as BookViewModel;

            if (book != null)
            {
                Books.Remove(book);
            }

            Back();
        }
    }
}
