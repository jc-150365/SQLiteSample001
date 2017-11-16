using System;
using Xamarin.Forms;

namespace SQLiteSample005
{
    public class App : Application
    {
        public App()
        {
            MainPage = new MyPage();
        }
    }

    class MyPage : ContentPage
    {

        readonly TodoRepository _db = new TodoRepository(); // <-1

        public MyPage()
        {

            var listView = new ListView
            { // <-2
                ItemsSource = _db.GetItems(), // <-3
                ItemTemplate = new DataTemplate(typeof(TextCell)) // <-4
            };
            listView.ItemTemplate.SetBinding(TextCell.TextProperty, "Text");
            listView.ItemTemplate.SetBinding(TextCell.DetailProperty, new Binding("CreatedAt", stringFormat: "{0:yyy/MM/dd hh:mm}"));
            listView.ItemTapped += async (s, a) => { // <-5
                var item = (TodoItem)a.Item;
                if (await DisplayAlert("削除してい宜しいですか", item.Text, "OK", "キャンセル"))
                {
                    item.Delete = true; // 削除フラグを有効にして
                    _db.SaveItem(item); // データベースの更新
                    listView.ItemsSource = _db.GetItems(); // リスト更新
                }
            };
            var entry = new Entry
            { // <-6
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            var buttonAdd = new Button
            { // <-7
                WidthRequest = 60,
                TextColor = Color.White,
                Text = "Add"
            };
            buttonAdd.Clicked += (s, a) => { // <-8
                if (!String.IsNullOrEmpty(entry.Text))
                { // Entryに文字列が入力されている場合に処理する
                    var item = new TodoItem { Text = entry.Text, CreatedAt = DateTime.Now, Delete = false };
                    _db.SaveItem(item);
                    listView.ItemsSource = _db.GetItems(); //リスト更新
                    entry.Text = ""; // 入力コントロールをクリアする
                }
            };

            Content = new StackLayout
            { // <-9
                Padding = new Thickness(0, Device.OnPlatform(20, 0, 0), 0, 0),
                Children = {
          new StackLayout {
            BackgroundColor = Color.Navy, // 入力部の背景色はネイビー
            Padding = 5,
            Orientation = StackOrientation.Horizontal,
            Children = {entry, buttonAdd} // Entryコントロールとボタンコントロールを横に並べる
          },
          listView // その下にリストボックスを置く
        }
            };
        }

    }
}
