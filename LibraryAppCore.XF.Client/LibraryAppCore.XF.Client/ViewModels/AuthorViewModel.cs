using LibraryAppCore.XF.Client.Models;
using LibraryAppCore.XF.Client.Pagination;
using LibraryAppCore.XF.Client.Services;
using System.ComponentModel;

namespace LibraryAppCore.XF.Client.ViewModels
{
    public class AuthorViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public Author Author { get; private set; }
        AuthorsListViewModel Alvm;

        public AuthorViewModel()
        {
            Author = new Author();
        }
        
        public AuthorsListViewModel ListViewModel
        {
            get { return Alvm; }
            set
            {
                if(Alvm != value)
                {
                    Alvm = value;
                    OnPropertyChanged("ListViewModel");
                }
            }
        }

        public string Id
        {
            get { return Author.Id; }
            set
            {
                if (Author.Id != value)
                {
                    Author.Id = value;
                    OnPropertyChanged("AuthorId");
                }

            }   
        }

        public string Name
        {
            get { return Author.Name; }
            set
            {
                if(Author.Name != value)
                {
                    Author.Name = value;
                    OnPropertyChanged("Name");
                }
            }
        }

        public string Surname
        {
            get { return Author.Surname; }
            set
            {
                if(Author.Surname != value)
                {
                    Author.Surname = value;
                    OnPropertyChanged("Surname");
                }
            }
        }
        
        public bool IsValid
        {
            get
            {
                return ((!string.IsNullOrEmpty(Author.Id)) ||
                    (!string.IsNullOrEmpty(Name.Trim())) ||
                    (!string.IsNullOrEmpty(Author.Surname)));
            }
        }

        protected void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

    }
}
