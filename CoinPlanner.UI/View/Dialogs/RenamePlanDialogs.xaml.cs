using System.Windows;
using CoinPlanner.DataBase;
using CoinPlanner.UI.ViewModel.Controls;
using CoinPlanner.UI.ViewModel.Dialogs;

namespace CoinPlanner.UI.View.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для RenamePlanDialogs.xaml
    /// </summary>
    public partial class RenamePlanDialogs : Window
    {
        public RenamePlanDialogs(PanelViewModel panelViewModel, DataService dataService)
        {
            InitializeComponent();

            DataContext = new RenamePlanDialogsViewModel(panelViewModel, dataService);
        }
    }
}
