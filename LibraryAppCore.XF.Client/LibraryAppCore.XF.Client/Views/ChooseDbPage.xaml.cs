using LibraryAppCore.XF.Client.ViewModels;
using System;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LibraryAppCore.XF.Client.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ChooseDbPage : ContentPage
	{
        ChooseDbViewModel viewModel;

        public ChooseDbPage()
		{
			InitializeComponent ();
            viewModel = new ChooseDbViewModel() { Navigation = this.Navigation };
            BindingContext = viewModel;
            Result.Text = "";
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        private async Task ChooseLocalDb(object sender, EventArgs e)
        {
            viewModel.SelectLocalStorage();
            Result.Text = "local storage is connected";
        }

        private async Task ChoosePostgreSqlDb(object sender, EventArgs e)
        {
            bool result =  await viewModel.SelectPostgreSqlConnection();
            if (result)
            {
                Result.Text = "PostgreSql is connected";
            }
            else
            {
                Result.Text = "Error, check your Internet connection.";
            }

        }

        private async Task ChooseMongoDb(object sender, EventArgs e)
        {
            bool result = await viewModel.SelectMongoDbConnection();
            if (result)
            {
                Result.Text = "MongoDb is connected";
            }
            else
            {
                Result.Text = "Error, check your Internet connection.";
            }
        }
    }
}