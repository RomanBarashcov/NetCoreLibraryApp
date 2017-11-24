using LibraryAppCore.XF.Client.Interfaces;
using LibraryAppCore.XF.Client.UWP.DbConnection;
using System.IO;
using Windows.Storage;
using Xamarin.Forms;

[assembly: Dependency(typeof(SqlLiteUwpConfig))]
namespace LibraryAppCore.XF.Client.UWP.DbConnection
{
    public class SqlLiteUwpConfig : ISqlLite
    {
        public SqlLiteUwpConfig() { }

        public string GetDatabasePath(string sqliteFilename)
        {
            string path = Path.Combine(ApplicationData.Current.LocalFolder.Path, sqliteFilename);
            return path;
        }
    }
}
