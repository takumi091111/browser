using Microsoft.Toolkit.Win32.UI.Controls.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Browser
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        const string URL_REGEXP = @"^https?:\/\/[\w\/:%#\$&\?\(\)~\.=\+\-]+";
        WebBrowser webView;

        public MainWindow()
        {
            InitializeComponent();

            webView = new WebBrowser();
            webView.SetValue(Grid.RowProperty, 1);
            webView.SetValue(Grid.ColumnProperty, 2);
            mGrid.Children.Add(webView);
        }

        private void AddressBar_KeyDown(object sender, KeyEventArgs e)
        {
            string addrText = AddressBar.Text;

            if (e.Key == Key.Enter)
            {
                // 0文字ならreturn
                if (addrText.Count() <= 0) return;

                // URLパターンにマッチしている場合
                if (isURL(addrText))
                {
                    // URL先を表示
                    webView.Navigate(addrText);
                }
                else
                {
                    // Googleでの検索結果を表示
                    webView.Navigate(getSearchURL(addrText));
                }
            }
        }

        private Boolean isURL(string text)
        {
            return (Regex.IsMatch(text, URL_REGEXP)) ? true : false;
        }

        private string getSearchURL(string text)
        {
            // 文字列をURLエンコード
            string encoded = HttpUtility.HtmlEncode(text);
            return $"https://www.google.com/search?q={encoded}";
        }
    }
}
