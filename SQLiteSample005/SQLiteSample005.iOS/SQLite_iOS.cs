using System;
using System.IO;
using SQLite.Net;
using SQLite.Net.Platform.XamarinIOS;
using SQLiteSample005.iOS;
using Xamarin.Forms;

[assembly: Dependency(typeof(SQLite_iOS))] // <-1
namespace SQLiteSample005.iOS
{
    public class SQLite_iOS : ISQLite
    {
        public SQLiteConnection GetConnection()
        {
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal); 
            var libraryPath = Path.Combine(documentsPath, "..", "Library"); 
            var path = Path.Combine(libraryPath, "TodoSQLite.db3");         
            return new SQLiteConnection(new SQLitePlatformIOS(), path);     
        }
    }
}