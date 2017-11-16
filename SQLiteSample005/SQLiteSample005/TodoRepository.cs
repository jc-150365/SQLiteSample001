using System.Collections.Generic;
using SQLite.Net;
using Xamarin.Forms;

namespace SQLiteSample005
{
    class TodoRepository
    {

        static readonly object Locker = new object(); // <-1
        readonly SQLiteConnection _db; // <-2

        public TodoRepository()
        {
            _db = DependencyService.Get<ISQLite>().GetConnection(); // <-3
            _db.CreateTable<TodoItem>(); // <-4
        }

        public IEnumerable<TodoItem> GetItems()
        { // <-5
            lock (Locker)
            { // <-6
              // Delete==falseの一覧を取得する（削除フラグが立っているものは対象外）
                return _db.Table<TodoItem>().Where(m => m.Delete == false); // <-7
            }
        }

        public int SaveItem(TodoItem item)
        { // <-8
            lock (Locker)
            { // <-9
                if (item.Id != 0)
                { // Idが0でない場合は、更新
                    _db.Update(item); // <-10
                    return item.Id;
                }
                return _db.Insert(item); // <-11
            }
        }
    }
}