using System.Windows;
using System.Windows.Input;
using CoinPlanner.LogService;
using CoinPlanner.UI.View.Dialogs;
using CoinPlanner.UI.ViewModel.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CoinPlanner.UI.ViewModel.Dialogs;

public class IntervalDialogsViewModel : ObservableObject
{
    public IntervalDialogsViewModel(IntervalDialogs intervalDialogs, CalendarViewModel calendarViewModel) 
    {
        Ok = new RelayCommand(OkCommand);
        Cancel = new RelayCommand(CancelCommand);
        _intervalDialogs = intervalDialogs;
        _calendarViewModel = calendarViewModel;
        StartDate = calendarViewModel.Start;
        EndDate = calendarViewModel.End;

        Log.Send(EventLevel.Info, logSender, "Открытие окна");
    }

    private CalendarViewModel _calendarViewModel { get; }
    private IntervalDialogs _intervalDialogs { get; }
    public ICommand Ok { get; set; }
    public ICommand Cancel { get; set; }

    private const string logSender = "Interval Calendar";

    public DateTime StartDate
    {
        get => _startDate;
        set => SetProperty(ref _startDate, value, nameof(StartDate));
    }
    private DateTime _startDate;

    public DateTime EndDate
    {
        get => _endDate;
        set => SetProperty(ref _endDate, value, nameof(EndDate));
    }
    private DateTime _endDate;

    private void OkCommand() 
    {

        StartDate = StartDate.Date;
        EndDate = EndDate.Date;

        if (StartDate > EndDate)
        {
            MessageBox.Show("Начальная дата больше конечной даты. Измените интервал снова!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        Log.Send(EventLevel.Info, logSender, "Установлен интервал");

        _calendarViewModel.Start = StartDate;
        _calendarViewModel.End = EndDate;
        _calendarViewModel.UpdateButtons();
        _intervalDialogs.Close();
    }

    private void CancelCommand()
    {
        Log.Send(EventLevel.Info, logSender, "Окно закрыто");
        _intervalDialogs.Close();
    }
}
