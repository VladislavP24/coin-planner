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
    public PanelViewModel() 
    {
        Interval = new RelayCommand(IntervalCommand);
    }

    public ICommand Interval { get; set; }

    public void IntervalCommand()
    {
        IntervalDialogs dialog = new IntervalDialogs();
        dialog.Show();
    }
}
