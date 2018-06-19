using Reactive.Bindings;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Browser.Models
{
    class Tab
    {
        public ReactiveProperty<string> Title { get; set; }
        public ReactiveProperty<string> URL { get; set; }
        public WebBrowser View { get; set; }

        public Tab()
        {
            Title = new ReactiveProperty<string>();
            Title.Value = "新しいタブ";

            URL = new ReactiveProperty<string>();
            URL.Value = "about:blank";

            View = new WebBrowser();
            View.SetValue(Grid.RowProperty, 1);
            View.SetValue(Grid.ColumnProperty, 2);

            View.Navigated += View_Navigated;
            View.LoadCompleted += View_LoadCompleted;
        }

        private void View_Navigated(object sender, NavigationEventArgs e)
        {
            URL.Value = e.Uri.ToString();
        }

        private void View_LoadCompleted(object sender, NavigationEventArgs e)
        {
            if (View.Document == null) return;

            // ページタイトルの取得・更新
            mshtml.IHTMLDocument2 doc = View.Document as mshtml.IHTMLDocument2;
            Title.Value = doc.title;
        }
    }
}
