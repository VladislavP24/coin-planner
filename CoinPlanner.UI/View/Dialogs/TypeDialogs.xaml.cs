using System.Windows;
using CoinPlanner.Contracts.Abstractions.ViewModel.Controls;
using CoinPlanner.UI.ViewModel.Dialogs;

namespace CoinPlanner.UI.View.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для TypeDialogs.xaml
    /// </summary>
    public partial class TypeDialogs : Window
    {
        public TypeDialogs(ICalendarControls calendar)
        {
            InitializeComponent();

            DataContext = new TypeDialogsViewModel(calendar);
        }
    }
}
