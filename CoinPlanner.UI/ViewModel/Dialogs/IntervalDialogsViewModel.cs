using System.Windows;
using System.Windows.Input;
using CoinPlanner.Contracts.Abstractions.ViewModel;
using CoinPlanner.Contracts.Abstractions.ViewModel.Controls;
using CoinPlanner.LogService;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CoinPlanner.UI.ViewModel.Dialogs;

public class IntervalDialogsViewModel : ObservableObject, IViewModelDialogs
{
    public IntervalDialogsViewModel(ICalendarControls calendar)
    {
        Ok = new RelayCommand<Window>(OkCommand);
        Cancel = new RelayCommand<Window>(CancelCommand);

        _calendar = calendar;
        StartDate = _calendar.Start;
        EndDate = _calendar.End;

        Log.Send(EventLevel.Info, logSender, "Открытие окна");
    }

    private readonly ICalendarControls _calendar;
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

    public void OkCommand(object currWindow)
    {

        StartDate = StartDate.Date;
        EndDate = EndDate.Date;

        if (StartDate > EndDate)
        {
            MessageBox.Show("Начальная дата больше конечной даты. Измените интервал снова!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        Log.Send(EventLevel.Info, logSender, "Установлен интервал");

        _calendar.Start = StartDate;
        _calendar.End = EndDate;
        _calendar.UpdateButtons();

        Window window = currWindow as Window;
        window.Close();
    }

    public void CancelCommand(object currWindow)
    {
        Log.Send(EventLevel.Info, logSender, "Окно закрыто");

        Window window = currWindow as Window;
        window.Close();
    }
}
