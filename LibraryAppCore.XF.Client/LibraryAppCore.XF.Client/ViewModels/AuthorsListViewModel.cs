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
    public class AuthorsListViewModel : INotifyPropertyChanged
    {
        public bool initialized { get; set; }
        private bool isBusy;

        public ObservableCollection<Author> Authors { get; set; }
        public ObservableCollection<Pages> Pages { get; set; }

        private AuthorService authorService;
        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand BackCommand { protected set; get; }
        public ICommand BackPageCommand { protected set; get; }
        public ICommand NextPageCommand { protected set; get; }

        public ICommand SortTableByIdCommand { protected set; get; }
        public ICommand SortTableByNameCommand { protected set; get; }
        public ICommand SortTableBySurnameCommand { protected set; get; }

        Author selectedAuthor;
        public INavigation Navigation { get; set; }
        public static int currentPage { get;set; }
        public static string currentOrderBy { get; set; }
        public static bool currentAscending { get; set; }

        public AuthorsListViewModel()
        {
            Authors = new ObservableCollection<Author>();
            authorService = new AuthorService();
            currentPage = 1;
            currentAscending = true;
            currentOrderBy = "Id";
            isBusy = false;
            initialized = false;
            SortTableByIdCommand = new Command(SortById);
            SortTableByNameCommand = new Command(SortByName);
            SortTableBySurnameCommand = new Command(SortBySurname);
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
                        Surname = value.Surname,
                    };

                    selectedAuthor = null;
                    OnPropertyChanged("SelectedAuthor");
                    Navigation.PushAsync(new AuthorPage(tempAuthor, this));
                }
            }
        }

        public async void SortById()
        {
            initialized = false;
            currentPage = currentPage == 0 ? 1 : currentPage;
            currentOrderBy = "Id";
            currentAscending = currentAscending == true ? false : true;
            await LoadAuthors();
        }

        public async void SortByName()
        {
            initialized = false;
            currentOrderBy = "Name";
            currentAscending = currentAscending == true ? false : true;
            await LoadAuthors();
        }

        public async void SortBySurname()
        {
            initialized = false;
            currentOrderBy = "Surname";
            currentAscending = currentAscending == true ? false : true;
            await LoadAuthors();
        }

        public async Task LoadAuthors()
        {
            if (initialized == true) return;
            isBusy = true;

            if (IsInConnection())
            {
                PagedResults<Author> authors = await authorService.GetAuthors(currentPage, currentOrderBy, currentAscending);

                while (Authors.Any())
                    Authors.RemoveAt(Authors.Count - 1);

                for (int a = 0; a < authors.Results.Count; a++)
                {
                    Author author = new Author
                    {
                        Id = authors.Results[a].Id.ToString(),
                        Name = authors.Results[a].Name,
                        Surname = authors.Results[a].Surname
                    };

                    Authors.Add(author);
                    author = null;
                }
            }
            else
            {
                List<Entities.Author> authors = App.authorDb.GetAuthors().ToList();

                while (Authors.Any())
                    Authors.RemoveAt(Authors.Count - 1);


                for(int a = 0; a < authors.Count(); a++)
                {
                    Author author = new Author
                    {
                        Id = authors[a].Id.ToString(),
                        Name = authors[a].Name,
                        Surname = authors[a].Surname
                    };

                    Authors.Add(author);
                    author = null;
                }

            }

            IsBusy = false;
            initialized = true;
        }

        public async Task LoadAllAuthors()
        {
            if (initialized == true) return;
            isBusy = true;

            List<Author> authors = await authorService.GetAllAuthors();
           
            while (Authors.Any())
                Authors.RemoveAt(Authors.Count - 1);

            for (int a = 0; a < authors.Count; a++)
            {
                Author author = new Author
                {
                    Id = authors[a].Id.ToString(),
                    Name = authors[a].Name,
                    Surname = authors[a].Surname,
                    FullName = authors[a].Name + " " + authors[a].Surname
                };

                Authors.Add(author);
                author = null;
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

        private void CreateAuthor()
        {
            Navigation.PushAsync(new AuthorPage(new Author(), this));
        }

        public async void SaveAuthor(Author author)
        { 
            if (author != null)
            {
                IsBusy = true;

                if (!string.IsNullOrEmpty(author.Id))
                {
                    bool isAuthorUpdated = await authorService.UpdateAuthor(author);

                    if (isAuthorUpdated)
                    {
                        initialized = false;
                        await LoadAuthors();
                    }
                }
                else
                {
                    bool isAuthorCreated = await authorService.CreateAuthor(author);
                    if (isAuthorCreated)
                    {
                        initialized = false;
                        await LoadAuthors();
                    }
                }
            }
        }

        public async void DeleteAuthor(Author author)
        {
            if (author != null)
            {
                IsBusy = true;
                bool isAuthorDeleted = await authorService.DeleteAuthor(author.Id);
                if (isAuthorDeleted)
                {
                    initialized = false;
                    await LoadAuthors();
                }
            }
        }

        private async void NextPage()
        {
            initialized = false;
            currentPage = currentPage + 1;
            await LoadAuthors();
        }

        private async void BackPage()
        {
            initialized = false;
            currentPage = currentPage - 1;
            if (currentPage >= 1)
                await LoadAuthors();
            else
                currentPage = 1;
        }

        public bool IsInConnection()
        {
            string curConnection = App.ConnectionType;

            if(curConnection == "No Connection")
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
