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
	public partial class ChooseDbPage : ContentPage
	{
        ChooseDbViewModel viewModel;

        public ChooseDbPage ()
		{
			InitializeComponent ();
            viewModel = new ChooseDbViewModel() { Navigation = this.Navigation };
            BindingContext = viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        private async Task ChooseLocalDb(object sender, EventArgs e)
        {
            AuthorsListViewModel.initialized = false;
            BooksListViewModel.initialized = false;
            ChooseDbViewModel.LocalDb = ChooseDbViewModel.LocalDb == true ? false : true;
        }
        

    }
}