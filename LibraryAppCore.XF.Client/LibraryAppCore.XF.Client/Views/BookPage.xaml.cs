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
	public partial class BookPage : ContentPage
	{
        public Book Model { get; set; }
        public BooksListViewModel ViewModel { get; private set; }

        public BookPage(Book model, BooksListViewModel blvm)
        {
            InitializeComponent();
            Model = model;
            ViewModel = blvm;
            BindingContext = this;
        }
    }
}