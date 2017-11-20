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
	public partial class AuthorPage : ContentPage
	{
        public AuthorViewModel ViewModel { get; set; }

        public AuthorPage(AuthorViewModel avm)
        {
            InitializeComponent();
            ViewModel = avm;
            BindingContext = ViewModel;
        }
    }
}