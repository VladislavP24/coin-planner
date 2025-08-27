using System.Windows;
using CoinPlanner.DataBase;
using CoinPlanner.UI.ViewModel.Controls;
using CoinPlanner.UI.ViewModel.Dialogs;

namespace CoinPlanner.UI.View.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для DeletePlanDialogs.xaml
    /// </summary>
    public partial class DeletePlanDialogs : Window
    {
        public DeletePlanDialogs(PanelViewModel panelViewModel, DataService dataService)
        {
            InitializeComponent();

            DataContext = new DeletePlanDialogsViewModel(panelViewModel, dataService);
        }
    }
}
