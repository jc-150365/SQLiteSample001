using System.Collections.Generic;
using SQLite.Net;
using Xamarin.Forms;

namespace SQLiteSample005
{
    class TodoRepository
    {

        static readonly object Locker = new object(); 
        readonly SQLiteConnection _db; 

        public TodoRepository()
        {
            _db = DependencyService.Get<ISQLite>().GetConnection(); 
            _db.CreateTable<TodoItem>(); 
        }

        public IEnumerable<TodoItem> GetItems()
        { 
            lock (Locker)
            { 
              // Delete==falseの一覧を取得する（削除フラグが立っているものは対象外）
                return _db.Table<TodoItem>().Where(m => m.Delete == false); // <-7
            }
        }

        public int SaveItem(TodoItem item)
        { 
            lock (Locker)
            { 
                if (item.Id != 0)
                { // Idが0でない場合は、更新
                    _db.Update(item); 
                    return item.Id;
                }
                return _db.Insert(item);
            }
        }
    }
}