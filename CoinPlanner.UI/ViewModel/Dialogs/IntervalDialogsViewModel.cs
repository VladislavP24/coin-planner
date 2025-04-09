using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CoinPlanner.UI.ViewModel.Dialogs;

public class IntervalDialogsViewModel : ObservableObject
{
    public IntervalDialogsViewModel() 
    {
        Ok = new RelayCommand(OkCommand);
        Cancel = new RelayCommand(CancelCommand);
    }

    public ICommand Ok { get; set; }
    public ICommand Cancel { get; set; }

    public DateTime StartDate
    {
        get => _startDate;
        set => SetProperty(ref _startDate, value, nameof(StartDate));
    }
    public DateTime _startDate;

    public DateTime EndDate
    {
        get => _endDate;
        set => SetProperty(ref _endDate, value, nameof(EndDate));
    }
    public DateTime _endDate;

    private void OkCommand() { }

    private void CancelCommand() { }
}
