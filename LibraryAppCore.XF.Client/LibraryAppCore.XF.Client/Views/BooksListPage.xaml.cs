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
            await viewModel.LoadBooks();
            base.OnAppearing();
        }

    }
}