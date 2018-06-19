using Browser.Models;
using System.Collections.ObjectModel;

namespace Browser
{
    class ViewModel
    {
        public ObservableCollection<Tab> TabList { get; set; }

        public ViewModel()
        {
            TabList = new ObservableCollection<Tab>();
            TabList.Add(new Tab());
        }
    }
}
