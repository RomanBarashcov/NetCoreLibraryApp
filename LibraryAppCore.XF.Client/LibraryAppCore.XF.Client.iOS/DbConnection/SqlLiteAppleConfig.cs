using System;
using LibraryAppCore.XF.Client.iOS.DbConnection;
using LibraryAppCore.XF.Client.Interfaces;
using System.IO;

[assembly: Xamarin.Forms.Dependency(typeof(SqlLiteAppleConfig))]
namespace LibraryAppCore.XF.Client.iOS.DbConnection
{
    public class SqlLiteAppleConfig : ISqlLite
    {
        public SqlLiteAppleConfig() { }

        public string GetDatabasePath(string sqliteFilename)
        {
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string libraryPath = Path.Combine(documentsPath, "..", "Library"); // папка библиотеки
            var path = Path.Combine(libraryPath, sqliteFilename);

            return path;
        }
    }
}