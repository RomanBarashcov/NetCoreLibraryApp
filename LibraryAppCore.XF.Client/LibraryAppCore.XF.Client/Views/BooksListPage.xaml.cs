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
	public partial class BooksListPage : ContentPage
	{
        BooksListViewModel viewModel;

        public BooksListPage()
        {
            InitializeComponent();
            viewModel = new BooksListViewModel() { Navigation = this.Navigation };
            BindingContext = viewModel;
        }

        protected override async void OnAppearing()
        {
            await viewModel.GetBooks();
            base.OnAppearing();
        }

        private void Move_To_Home(object sender, EventArgs e)
        {
            Navigation.PushAsync(new MainPage());
        }

        private void Move_To_Authors(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AuthorsListPage());
        }

        private void Move_To_Books(object sender, EventArgs e)
        {
            Navigation.PushAsync(new BooksListPage());
        }

        private void Move_To_ChooseDb(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ChooseDbPage());
        }
    }
}