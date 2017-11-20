using LibraryAppCore.XF.Client.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace LibraryAppCore.XF.Client.ViewModels
{
    public class AuthorsListViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<AuthorViewModel> Authors { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand CreateAuthorCommand { protected set; get; }
        public ICommand DeleteAuthorCommand { protected set; get; }
        public ICommand SaveAuthorCommand { protected set; get; }
        public ICommand BackCommand { protected set; get; }

        AuthorViewModel selectedAuthor;
        public INavigation Navigation { get; set; }

        public AuthorsListViewModel()
        {
            Authors = new ObservableCollection<AuthorViewModel>();
            CreateAuthorCommand = new Command(CreateAuthor);
            SaveAuthorCommand = new Command(SaveAuthor);
            DeleteAuthorCommand = new Command(DeleteAuthor);
            BackCommand = new Command(Back);
        }

        public AuthorViewModel SelectedAuthor
        {
            get { return selectedAuthor; }
            set
            {
                if(selectedAuthor != value)
                {
                    AuthorViewModel tempAuthor = value;
                    selectedAuthor = null;
                    OnPropertyChanged("SelectedFriend");
                    Navigation.PushAsync(new AuthorPage(tempAuthor));
                }
            }
        }

        private void Back()
        {
            Navigation.PopAsync();
        }

        protected void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        private void CreateAuthor()
        {
            Navigation.PushAsync(new AuthorPage(new AuthorViewModel() { ListViewModel = this }));
        }

        private void SaveAuthor(object authorObject)
        {
            AuthorViewModel author = authorObject as AuthorViewModel;

            if(author != null && author.IsValid)
            {
                Authors.Add(author);
            }

            Back();
        }

        private void DeleteAuthor(object authorObject)
        {
            AuthorViewModel author = authorObject as AuthorViewModel;

            if(author != null)
            {
                Authors.Remove(author);
            }

            Back();
        }

    }
}
