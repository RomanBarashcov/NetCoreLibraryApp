using LibraryAppCore.XF.Client.Services;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace LibraryAppCore.XF.Client.ViewModels
{
    public class ChooseDbViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand DefaultConnectionCommand { protected set; get; }
        public ICommand MongoDbConnectionCommand { protected set; get; }
        public ICommand BackCommand { protected set; get; }

        public string dbConnection;
        private ConnectionDbService cDbService;

        public bool IsVisible { get; set; }
        public INavigation Navigation { get; set; }

        public ChooseDbViewModel()
        {
            cDbService = new ConnectionDbService();
            DefaultConnectionCommand = new Command(SelectPostgreSqlConnection);
            MongoDbConnectionCommand = new Command(SelectMongoDbConnection);
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

        private async void SelectPostgreSqlConnection()
        {
            dbConnection = "DefaultConnection";
            bool result = await cDbService.SetConnectionString(dbConnection);
            IsVisible = result;
        }

        private async void SelectMongoDbConnection()
        {
            dbConnection = "MondoDbConnection";
            bool result = await cDbService.SetConnectionString(dbConnection);
            IsVisible = result;

        }

        protected void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
