using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Browser.Models;

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
