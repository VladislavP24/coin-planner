using System.Windows;
using CoinPlanner.Contracts.Abstractions.DataBase;
using CoinPlanner.Contracts.Abstractions.ViewModel.Controls;
using CoinPlanner.UI.ViewModel.Dialogs;

namespace CoinPlanner.UI.View.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для MarkDialogs.xaml
    /// </summary>
    public partial class MarkDialogs : Window
    {
        public MarkDialogs(IPanelControls panel, IDataService dataService, ICalendarControls calendar)
        {
            InitializeComponent();

            DataContext = new MarkDialogsViewModel(dataService, calendar, panel);
        }
    }
}
