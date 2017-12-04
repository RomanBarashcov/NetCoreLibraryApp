using LibraryAppCore.XF.Client.Services;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace LibraryAppCore.XF.Client.ViewModels
{
    public class ChooseDbViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand BackCommand { protected set; get; }

        public string dbConnection;
        private ConnectionDbService cDbService;
        public static bool LocalDb { get; set; }

        public bool IsVisible { get; set; }
        public INavigation Navigation { get; set; }

        public ChooseDbViewModel()
        {
            cDbService = new ConnectionDbService();
            BackCommand = new Command(Back);
        }

        private async void Back()
        {
            await Navigation.PopAsync();
        }

        public bool IsVisibleData
        {
            get
            {
                return IsVisible;
            }
            set
            {
                IsVisible = value;
                OnPropertyChanged("IsVisibleData");
            }
        }

        public async void SelectLocalStorage()
        {
            LocalDb = true; 
            AuthorsListViewModel.initialized = false;
            BooksListViewModel.initialized = false;
        }

        public async Task<bool> SelectPostgreSqlConnection()
        {
            LocalDb = false;
            AuthorsListViewModel.initialized = false;
            BooksListViewModel.initialized = false;
            dbConnection = "DefaultConnection";
            bool result = await cDbService.SetConnectionString(dbConnection);
            return result;
        }

        public async Task<bool> SelectMongoDbConnection()
        {
            LocalDb = false;
            AuthorsListViewModel.initialized = false;
            BooksListViewModel.initialized = false;
            dbConnection = "MondoDbConnection";
            bool result = await cDbService.SetConnectionString(dbConnection);
            return result;
        }

        protected void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
