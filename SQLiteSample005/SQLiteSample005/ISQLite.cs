using SQLite.Net;
namespace SQLiteSample005
{
    public interface ISQLite
    {
        SQLiteConnection GetConnection(); 
    }
}