using System.Windows;
using System.Windows.Input;
using CoinPlanner.LogService;
using CoinPlanner.UI.Interface;
using CoinPlanner.UI.View.Dialogs;
using CoinPlanner.UI.ViewModel.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CoinPlanner.UI.ViewModel.Dialogs;

public class IntervalDialogsViewModel : ObservableObject, IViewModelDialogs
{
    public IntervalDialogsViewModel(CalendarViewModel calendarViewModel) 
    {
        Ok = new RelayCommand<Window>(OkCommand);
        Cancel = new RelayCommand<Window>(CancelCommand);
        _calendarViewModel = calendarViewModel;
        StartDate = calendarViewModel.Start;
        EndDate = calendarViewModel.End;

        Log.Send(EventLevel.Info, logSender, "Открытие окна");
    }

    private CalendarViewModel _calendarViewModel { get; }
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

    public void OkCommand(Window window) 
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
        window.Close();
    }

    public void CancelCommand(Window window)
    {
        Log.Send(EventLevel.Info, logSender, "Окно закрыто");
        window.Close();
    }
}
