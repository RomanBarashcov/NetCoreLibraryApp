using LibraryAppCore.XF.Client.Repositories;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace LibraryAppCore.XF.Client
{
    public partial class App : Application
    {

        public const string DATABASE_NAME = "Library";
        public static AuthorRepository authorDb;
        public static BookRepository bookDb;

        public static string connectionType;

        public static AuthorRepository AuthorDb
        {
            get
            {
                if (authorDb == null)
                {
                    try
                    {
                        authorDb = new AuthorRepository(DATABASE_NAME);
                    }
                    catch (Exception ex)
                    {
                        var expt = ex;
                    }

                }
                return authorDb;
            }
        }

        public static BookRepository BookDb
        {
            get
            {
                if (bookDb == null)
                {
                    bookDb = new BookRepository(DATABASE_NAME);
                }
                return bookDb;
            }
        }

        public static string ConnectionType
        {
            get
            {
                return CheckConnection();
            }
        }

        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        public static string CheckConnection()
        {

            connectionType = "No Connection";

            if (CrossConnectivity.Current != null &&
                CrossConnectivity.Current.ConnectionTypes != null &&
                CrossConnectivity.Current.IsConnected == true)
            {
                var connType = CrossConnectivity.Current.ConnectionTypes.FirstOrDefault();
                connectionType = connType.ToString();
            }

            return connectionType;
        }
    }
}
