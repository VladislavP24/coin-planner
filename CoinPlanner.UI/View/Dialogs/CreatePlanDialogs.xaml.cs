using System.Windows;
using CoinPlanner.DataBase;
using CoinPlanner.UI.ViewModel.Controls;
using CoinPlanner.UI.ViewModel.Dialogs;

namespace CoinPlanner.UI.View.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для CreatePlanDialogs.xaml
    /// </summary>
    public partial class CreatePlanDialogs : Window
    {
        public CreatePlanDialogs(PanelViewModel panelViewModel, DataService dataService)
        {
            InitializeComponent();

            DataContext = new CreatePlanDialogsViewModel(panelViewModel, dataService);
        }
    }
}
