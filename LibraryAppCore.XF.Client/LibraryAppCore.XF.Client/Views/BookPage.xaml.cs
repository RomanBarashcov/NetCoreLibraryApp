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
            BindingContext = this;
        }

        private async void LoadAuthors()
        {
            await aViewModel.LoadAllAuthors();
            LoadFullAuthorName();
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
    }
}