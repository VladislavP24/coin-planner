using System.Windows;
using CoinPlanner.UI.ViewModel.Dialogs;

namespace CoinPlanner.UI.View.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для RenamePlanDialogs.xaml
    /// </summary>
    public partial class RenamePlanDialogs : Window
    {
        public RenamePlanDialogs()
        {
            InitializeComponent();

            DataContext = new RenamePlanDialogsViewModel();
        }
    }
}
