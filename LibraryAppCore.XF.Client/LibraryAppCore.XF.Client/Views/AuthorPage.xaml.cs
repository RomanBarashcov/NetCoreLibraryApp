using LibraryAppCore.XF.Client.Models;
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
        public Author Model { get; set; }
        public AuthorsListViewModel ViewModel { get; private set; }

        public AuthorPage(Author model, AuthorsListViewModel alvm)
        {
            InitializeComponent();
            Model = model;
            ViewModel = alvm;
            BindingContext = this;
        }
    }
}