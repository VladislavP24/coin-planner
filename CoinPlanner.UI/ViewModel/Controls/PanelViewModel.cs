using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CoinPlanner.UI.View.Dialogs;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CoinPlanner.UI.ViewModel.Controls;

public class PanelViewModel : ObservableObject
{
    public PanelViewModel(CalendarViewModel calendarViewModel) 
    {
        Interval = new RelayCommand(IntervalCommand);
        Type = new RelayCommand(TypeCommand);
        _calendarViewModel = calendarViewModel;
    }

    private CalendarViewModel _calendarViewModel { get; set; }
    public ICommand Interval { get; set; }
    public ICommand Type { get; set; }

    public void IntervalCommand()
    {
        IntervalDialogs dialog = new IntervalDialogs(_calendarViewModel);
        dialog.ShowDialog();
    }

    public void TypeCommand() 
    {
        TypeDialogs typeDialogs = new TypeDialogs(_calendarViewModel);
        typeDialogs.ShowDialog();
    }
}
