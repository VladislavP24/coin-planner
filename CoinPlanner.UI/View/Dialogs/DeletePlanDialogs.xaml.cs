using System.Windows;
using CoinPlanner.UI.ViewModel.Dialogs;

namespace CoinPlanner.UI.View.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для DeletePlanDialogs.xaml
    /// </summary>
    public partial class DeletePlanDialogs : Window
    {
        public DeletePlanDialogs()
        {
            InitializeComponent();

            DataContext = new DeletePlanDialogsViewModel();
        }
    }
}
