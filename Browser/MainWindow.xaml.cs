using Browser.Models;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace Browser
{
    public partial class MainWindow : Window
    {
        const string URL_REGEXP = @"^https?:\/\/[\w\/:%#\$&\?\(\)~\.=\+\-]+";
        WebBrowser View;

        public MainWindow()
        {
            InitializeComponent();

            // タブ初期化
            ViewModel viewModel = (ViewModel)DataContext;
            ReplaceView(viewModel.TabList[0].View);

            // 最初のタブを選択状態にする
            TabView.SelectedIndex = 0;
        }

        private void AddressBar_KeyDown(object sender, KeyEventArgs e)
        {
            string addrText = AddressBar.Text;

            if (e.Key == Key.Enter)
            {
                // 0文字ならreturn
                if (addrText.Count() <= 0) return;

                // URLパターンにマッチしている場合
                if (IsURL(addrText))
                {
                    // URL先を表示
                    View.Navigate(addrText);
                }
                else
                {
                    // Googleでの検索結果を表示
                    View.Navigate(GetSearchURL(addrText));
                }

                ViewModel viewModel = (ViewModel)DataContext;
                string url = viewModel.TabList[TabView.SelectedIndex].URL.Value;
                AddressBar.Text = url;
            }
        }

        private Boolean IsURL(string text)
        {
            return (Regex.IsMatch(text, URL_REGEXP)) ? true : false;
        }

        private string GetSearchURL(string text)
        {
            // 文字列をURLエンコード
            string encoded = HttpUtility.HtmlEncode(text);
            return $"https://www.google.com/search?q={encoded}";
        }

        private void NewTab_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            // 新しいタブを作成
            ViewModel viewModel = (ViewModel)DataContext;
            viewModel.TabList.Add(new Tab());

            TabView.SelectedIndex++;
        }

        private void CloseTab_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ViewModel viewModel = (ViewModel)DataContext;

            // タブ数が2つ以上の場合
            if (viewModel.TabList.Count() > 1)
            {
                // 現在のタブを削除
                viewModel.TabList.RemoveAt(TabView.SelectedIndex);

                // インデックスが0の場合
                if (TabView.SelectedIndex <= 0)
                {
                    TabView.SelectedIndex++;
                }
                // インデックスが0以外の場合
                else
                {
                    TabView.SelectedIndex--;
                }
            }
        }

        private void View_Navigated(object sender, NavigationEventArgs e)
        {
            // アドレスバーにURLをセット
            ViewModel viewModel = (ViewModel)DataContext;
            string url = viewModel.TabList[TabView.SelectedIndex].URL.Value;
            AddressBar.Text = url;
        }

        private void TabView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListView listView = (ListView)sender;

            // クリックしたタブのデータを取得
            if (listView.SelectedItem == null) return;
            Tab itemContent = (Tab)listView.SelectedItem;

            // 現在のViewを破棄して
            // クリックしたタブのViewに差し替える
            ReplaceView(itemContent.View);

            // アドレスバーにURLをセット
            AddressBar.Text = itemContent.URL.Value;
        }

        private void ReplaceView(WebBrowser browser)
        {
            // Viewの差し替え
            mGrid.Children.Remove(View);
            View = browser;
            View.Navigated += View_Navigated;
            mGrid.Children.Add(View);
        }
    }
}
