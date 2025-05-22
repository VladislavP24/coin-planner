using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using CoinPlanner.DataBase;
using CoinPlanner.UI.ViewModel.Controls;
using CoinPlanner.UI.ViewModel.Dialogs;

namespace CoinPlanner.UI.View.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для MarkDialogs.xaml
    /// </summary>
    public partial class MarkDialogs : Window
    {
        public MarkDialogs(PanelViewModel panelViewModel, DataService dataService, CalendarViewModel calendarViewModel)
        {
            InitializeComponent();

            DataContext = new MarkDialogsViewModel(this, dataService, calendarViewModel, panelViewModel);
        }
    }
}
