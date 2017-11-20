using LibraryAppCore.XF.Client.Models;
using LibraryAppCore.XF.Client.ViewModels;
using LibraryAppCore.XF.Client.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LibraryAppCore.XF.Client
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
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
