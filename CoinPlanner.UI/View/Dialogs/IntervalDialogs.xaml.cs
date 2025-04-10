using System.Windows;
using CoinPlanner.UI.ViewModel.Controls;
using CoinPlanner.UI.ViewModel.Dialogs;


namespace CoinPlanner.UI.View.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для IntervalDIalogs.xaml
    /// </summary>
    public partial class IntervalDialogs : Window
    {
        public IntervalDialogs(CalendarViewModel calendarViewModel)
        {
            InitializeComponent();

            DataContext = new IntervalDialogsViewModel(this, calendarViewModel);
        }
    }
}
