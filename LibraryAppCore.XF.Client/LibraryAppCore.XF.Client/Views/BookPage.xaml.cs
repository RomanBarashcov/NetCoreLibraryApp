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
        public AuthorsListViewModel aViewModel { get; private set; }
        public List<Author> Authors { get; set; }

        public BookPage(Book model, BooksListViewModel blvm)
        {
            InitializeComponent();
            Model = model;
            ViewModel = blvm;
            aViewModel = new AuthorsListViewModel();
            LoadAuthors();
            CheckConnType();
            BindingContext = this;
        }

        private async void LoadAuthors()
        {
            if (App.ConnectionType != "No Connection")
            {
                await aViewModel.LoadAllAuthors();
                LoadFullAuthorName();
            }
            else
            {
                await aViewModel.LoadAuthors();
            }
        }

        private void LoadFullAuthorName()
        {
            List<Author> author = aViewModel.Authors.Where(x => x.Id == Model.AuthorId).ToList();

            for (int i = 0; i <= author.Count; i++)
            {
                picker.Title = "Selected Author: " + author[i].FullName;
                break;
            }
        }

        void OnPickerSelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            int selectedIndex = picker.SelectedIndex;

            if (selectedIndex != -1)
            {
                var selectedItem = picker.SelectedItem;
                Author selectedAuthor = selectedItem as Author;
                Model.AuthorId = selectedAuthor.Id;
                picker.Title = "Selected Author: " + selectedAuthor.FullName;
            }
        }

        public void CheckConnType()
        {
            string conType = App.ConnectionType;
            ConnType.Text = "Connection type:" + conType;
        }

        private async Task SaveBook(object sender, EventArgs e)
        {
            Book book = Model;

            if (!String.IsNullOrEmpty(book.Name) && !String.IsNullOrEmpty(book.Description) && book.Year > 0)
            {
                if (App.ConnectionType != "No Connection")
                {
                    ViewModel.SaveBook(book);
                    BooksListViewModel.initialized = false;
                    await this.Navigation.PopAsync();
                }
                else
                {
                    var answer = await DisplayAlert("Save Data", "You don't have internet connection, you can save data in local storage!", "Save local", "Try agin");

                    Entities.Book newBook = new Entities.Book
                    {
                        Id = Convert.ToInt32(Model.Id),
                        Year = Model.Year,
                        Name = Model.Name,
                        Description = Model.Description,
                        AuthorId = Convert.ToInt32(Model.AuthorId)
                        
                    };

                    if (answer)
                    {
                        App.BookDb.SaveBook(newBook);
                        BooksListViewModel.initialized = false;
                        await this.Navigation.PopAsync();
                    }
                }
            }
        }

        private async Task DeleteBook(object sender, EventArgs e)
        {
            var book = Model;
            if (App.ConnectionType != "No Connection")
            {
                ViewModel.DeleteBook(book);
                await this.Navigation.PopAsync();
            }
            else
            {
                var answer = await DisplayAlert("Save Data", "You don't have internet connection, you can delete data local!", "Delete local", "Try agin");

                if (answer)
                {
                    App.BookDb.DeleteBook(Convert.ToInt32(book.Id));
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