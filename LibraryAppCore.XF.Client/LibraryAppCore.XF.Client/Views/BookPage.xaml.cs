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
	public partial class BookPage : ContentPage
	{
        public BookViewModel ViewModel { get; set; }

        public BookPage(BookViewModel bvm)
        {
            InitializeComponent();
            ViewModel = bvm;
            BindingContext = ViewModel;
        }
    }
}