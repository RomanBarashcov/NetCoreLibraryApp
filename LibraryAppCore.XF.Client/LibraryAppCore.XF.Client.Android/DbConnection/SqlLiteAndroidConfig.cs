using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

[assembly: Dependency(typeof(SqlLiteAndroidConfig))]
namespace LibraryAppCore.XF.Client.Droid.DbConnection
{
    public class SqlLiteAndroidConfig : ISqlLite
    {
        public SqlLiteAndroidConfig() { }

        public string GetDatabasePath(string sqliteFilename)
        {
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var path = Path.Combine(documentsPath, sqliteFilename);

            return path;
        }
    }
}