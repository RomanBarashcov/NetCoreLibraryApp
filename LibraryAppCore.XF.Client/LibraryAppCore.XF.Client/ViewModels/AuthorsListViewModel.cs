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
    public class AuthorsListViewModel : INotifyPropertyChanged
    {
        bool initialized = false;
        private bool isBusy;

        public ObservableCollection<Author> Authors { get; set; }
        private AuthorService authorService;
        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand CreateAuthorCommand { protected set; get; }
        public ICommand DeleteAuthorCommand { protected set; get; }
        public ICommand SaveAuthorCommand { protected set; get; }
        public ICommand BackCommand { protected set; get; }

        Author selectedAuthor;
        public INavigation Navigation { get; set; }

        public AuthorsListViewModel()
        {
            Authors = new ObservableCollection<Author>();
            authorService = new AuthorService();
            isBusy = false;
            CreateAuthorCommand = new Command(CreateAuthor);
            SaveAuthorCommand = new Command(SaveAuthor);
            DeleteAuthorCommand = new Command(DeleteAuthor);
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

        public Author SelectedAuthor
        {
            get { return selectedAuthor; }
            set
            {
                if(selectedAuthor != value)
                {
                    Author tempAuthor = new Author
                    {
                        Id = value.Id,
                        Name = value.Name,
                        Surname = value.Surname
                        
                    };

                    selectedAuthor = null;
                    OnPropertyChanged("SelectedAuthor");
                    Navigation.PushAsync(new AuthorPage(tempAuthor, this));
                }
            }
        }

        public async Task GetAuthors()
        {
            if (initialized == true) return;
            isBusy = true;

            PagedResults<Author> authorReuslt = await authorService.GetAuthors();
            Author aModel = new Author();

            while (Authors.Any())
                Authors.RemoveAt(Authors.Count - 1);

            foreach (var a in authorReuslt.Results)
                Authors.Add(a);

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

        private void CreateAuthor()
        {
            Navigation.PushAsync(new AuthorPage(new Author(), this));
        }

        private async void SaveAuthor(object authorObject)
        {
            Author author = authorObject as Author;

            if (author != null)
            {
                IsBusy = true;

                if (!string.IsNullOrEmpty(author.Id))
                {
                    bool isAuthorUpdated = await authorService.UpdateAuthor(author);

                    if (isAuthorUpdated)
                    {
                        initialized = false;
                        await GetAuthors();
                    }
                }
                else
                {
                    bool isAuthorCreated = await authorService.CreateAuthor(author);
                    if (isAuthorCreated)
                    {
                        initialized = false;
                        await GetAuthors();
                    }
                }
                initialized = true;
                IsBusy = false;
            }
            Back();
        }

        private async void DeleteAuthor(object authorObject)
        {
            Author author = authorObject as Author;
            if (author != null)
            {
                IsBusy = true;
                bool isAuthorDeleted = await authorService.DeleteAuthor(author.Id);
                if (isAuthorDeleted)
                {
                    initialized = false;
                    await GetAuthors();
                }
                initialized = true;
                IsBusy = false;
            }
            Back();
        }

    }
}
