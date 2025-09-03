using System.Windows;
using CoinPlanner.Contracts.Abstractions.DataBase;
using CoinPlanner.Contracts.Abstractions.ViewModel.Controls;
using CoinPlanner.UI.ViewModel.Dialogs;

namespace CoinPlanner.UI.View.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для CreatePlanDialogs.xaml
    /// </summary>
    public partial class CreatePlanDialogs : Window
    {
        public CreatePlanDialogs(IPanelControls panel, IDataService dataService)
        {
            InitializeComponent();

            DataContext = new CreatePlanDialogsViewModel(panel, dataService);
        }
    }
}
