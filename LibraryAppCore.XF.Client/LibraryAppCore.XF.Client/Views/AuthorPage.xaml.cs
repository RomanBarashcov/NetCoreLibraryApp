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
            CheckConnType();
            Model = model;
            ViewModel = alvm;
            BindingContext = this;
        }

        public void CheckConnType()
        {
            string conType = App.ConnectionType;
            ConnType.Text = "Connection type:" + conType;
        }

        private async Task SaveAuthor(object sender, EventArgs e)
        {
            Author author = Model;

            if (!String.IsNullOrEmpty(author.Name) && !String.IsNullOrEmpty(author.Surname))
            {
                if(App.ConnectionType != "No Connection")
                {
                    ViewModel.SaveAuthor(author);
                    AuthorsListViewModel.initialized = false;
                    await this.Navigation.PopAsync();
                }
                else
                {
                    bool answer = await DisplayAlert("Save Data", "You don't have internet connection, you can save data in local storage!", "Save local", "Try agin");

                    Entities.Author newAuthor = new Entities.Author
                    {
                        Id = Convert.ToInt32(Model.Id),
                        Name = Model.Name,
                        Surname = Model.Surname
                    };

                    if (answer)
                    {
                        App.AuthorDb.SaveAuthor(newAuthor);
                        AuthorsListViewModel.initialized = false;
                        await this.Navigation.PopAsync();
                    }
                }
            }
        }

        private async Task DeleteAuthor(object sender, EventArgs e)
        {
            var author = Model;
            if (App.ConnectionType != "No Connection")
            {
                ViewModel.DeleteAuthor(author);
                await this.Navigation.PopAsync();
            }
            else
            {
                bool answer = await DisplayAlert("Save Data", "You don't have internet connection, you can delete data local!", "Delete local", "Try agin");

                if (answer)
                {
                    App.AuthorDb.DeleteAuthor(Convert.ToInt32(author.Id));
                    await this.Navigation.PopAsync();
                }
            }
        }

        private async void Cancel(object sender, EventArgs e)
        {
           await this.Navigation.PopAsync();
        }
    }
}