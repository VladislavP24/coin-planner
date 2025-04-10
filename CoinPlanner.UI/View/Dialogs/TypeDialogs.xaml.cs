using System.Windows;
using CoinPlanner.UI.ViewModel.Controls;
using CoinPlanner.UI.ViewModel.Dialogs;

namespace CoinPlanner.UI.View.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для TypeDialogs.xaml
    /// </summary>
    public partial class TypeDialogs : Window
    {
        public TypeDialogs(CalendarViewModel calendarViewModel)
        {
            InitializeComponent();

            DataContext = new TypeDialogsViewModel(this, calendarViewModel);
        }
    }
}
