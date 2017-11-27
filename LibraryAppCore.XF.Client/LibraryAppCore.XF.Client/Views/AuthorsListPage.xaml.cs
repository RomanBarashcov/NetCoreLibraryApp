using LibraryAppCore.XF.Client.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LibraryAppCore.XF.Client.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AuthorsListPage : ContentPage
    {
        AuthorsListViewModel viewModel;

        public AuthorsListPage()
        {
            InitializeComponent();
            viewModel = new AuthorsListViewModel() { Navigation = this.Navigation };
            BindingContext = viewModel;
        }

        protected override async void OnAppearing()
        {
            await viewModel.LoadAuthors();
            base.OnAppearing();
        }

        private async Task CheckLocalDataSynced(object sender, EventArgs e)
        {
            viewModel.CheckLocalData();

            if (viewModel.AuhtorsLocalData.Count > 0 && App.ConnectionType != "No Connection")
            {
                bool answer = await DisplayAlert("Sync local data", "You are have " + viewModel.AuhtorsLocalData.Count + " not synced items", "Sync Now", "Sync Later");

                if (answer)
                {
                    viewModel.SyncedLocalData();
                }
            }
            else
            {
                await DisplayAlert("Save Data", "You don't have internet connection! Please try again leater", "OK");
            }
        }

    }
}