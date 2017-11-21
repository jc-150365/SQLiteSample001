using System;
using Xamarin.Forms;
using System.Text;
using System.Collections.Generic;
using System.Linq;

namespace SQLiteSample005
{
    public partial class App : Application //partial付けた
    {
        public App()
        {
            MainPage = new MyPage();
        }
    }

    class MyPage : ContentPage
    {

        readonly TodoRepository _db = new TodoRepository(); 

        public MyPage()
        {

            var listView = new ListView
            { 
                ItemsSource = _db.GetItems(), 
                ItemTemplate = new DataTemplate(typeof(TextCell)) 
            };

            listView.ItemTemplate.SetBinding(TextCell.TextProperty, "Text");
            //listView.ItemTemplate.SetBinding(TextCell.DetailProperty, new Binding("CreatedAt", stringFormat: "{0:yyy/MM/dd hh:mm}"));
            listView.ItemTemplate.SetBinding(TextCell.DetailProperty, new Binding("CreatedAt", stringFormat:"{yyyy/mm/dd}"));
            listView.ItemTemplate.SetBinding(TextCell.TextProperty, "Text");//画像のバイナリデータを入れたいけども

            listView.ItemTapped += async (s, a) => { 
                var item = (TodoItem)a.Item;
                if (await DisplayAlert("削除してい宜しいですか", item.Text, "OK", "キャンセル"))
                {
                    item.Delete = true; // 削除フラグを有効にして
                    _db.SaveItem(item); // データベースの更新
                    listView.ItemsSource = _db.GetItems(); // リスト更新
                }
            };

            var entry = new Entry
            { 
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            var entry2 = new Entry//
            {
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            var buttonAdd = new Button
            { 
                WidthRequest = 60,
                TextColor = Color.White,
                Text = "Add"
            };

            var buttonAdd2 = new Button//
            {
                WidthRequest = 60,
                TextColor = Color.White,
                Text = "Add"
            };

            buttonAdd.Clicked += (s, a) => { 
                if (!String.IsNullOrEmpty(entry.Text))
                { // Entryに文字列が入力されている場合に処理する
                    var item = new TodoItem { Text = entry.Text, CreatedAt = DateTime.Now, Delete = false };
                    _db.SaveItem(item);
                    listView.ItemsSource = _db.GetItems(); //リスト更新
                    entry.Text = ""; // 入力コントロールをクリアする
                }
            };

            buttonAdd2.Clicked += (s, a) => {//追加分
                if (!String.IsNullOrEmpty(entry.Text))
                { // Entryに文字列が入力されている場合に処理する
                    var item = new TodoItem { Text = entry.Text, CreatedAt = DateTime.Now, Delete = false };
                    _db.SaveItem(item);
                    listView.ItemsSource = _db.GetItems(); //リスト更新
                    entry.Text = ""; // 入力コントロールをクリアする
                }
            };

            Content = new StackLayout
            { 
                Padding = new Thickness(0, Device.OnPlatform(20, 0, 0), 0, 0),
                Children = {
          new StackLayout {
            BackgroundColor = Color.Red, // 入力部の背景色は赤で
            Padding = 5,
            Orientation = StackOrientation.Horizontal,
            Children = {entry, buttonAdd,entry2,buttonAdd2} // Entryコントロールとボタンコントロールを横に並べる(余計な事したぁー)
          },
          listView // その下にリストボックスを置く
        }
            };
        }

    }
}
