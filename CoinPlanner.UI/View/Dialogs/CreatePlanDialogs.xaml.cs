using System.Windows;
using CoinPlanner.UI.ViewModel.Dialogs;

namespace CoinPlanner.UI.View.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для CreatePlanDialogs.xaml
    /// </summary>
    public partial class CreatePlanDialogs : Window
    {
        public CreatePlanDialogs()
        {
            InitializeComponent();

            DataContext = new CreatePlanDialogsViewModel();
        }
    }
}
