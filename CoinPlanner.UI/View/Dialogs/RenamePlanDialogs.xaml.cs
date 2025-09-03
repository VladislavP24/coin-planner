using System.Windows;
using CoinPlanner.Contracts.Abstractions.DataBase;
using CoinPlanner.Contracts.Abstractions.ViewModel.Controls;
using CoinPlanner.UI.ViewModel.Dialogs;

namespace CoinPlanner.UI.View.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для RenamePlanDialogs.xaml
    /// </summary>
    public partial class RenamePlanDialogs : Window
    {
        public RenamePlanDialogs(IPanelControls panel, IDataService dataService)
        {
            InitializeComponent();

            DataContext = new RenamePlanDialogsViewModel(panel, dataService);
        }
    }
}
