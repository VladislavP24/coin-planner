using System.Windows;
using CoinPlanner.Contracts.Abstractions.DataBase;
using CoinPlanner.Contracts.Abstractions.ViewModel.Controls;
using CoinPlanner.UI.ViewModel.Dialogs;

namespace CoinPlanner.UI.View.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для DeletePlanDialogs.xaml
    /// </summary>
    public partial class DeletePlanDialogs : Window
    {
        public DeletePlanDialogs(IPanelControls panel, IDataService dataService)
        {
            InitializeComponent();

            DataContext = new DeletePlanDialogsViewModel(panel, dataService);
        }
    }
}
