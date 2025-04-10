using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoinPlanner.UI.View.Controls;
using CoinPlanner.UI.ViewModel.Controls;

namespace CoinPlanner.UI.ViewModel;

public class MainWindowViewModel
{
    public MainWindowViewModel()
    {
        CalendarViewModel = new CalendarViewModel();
        PanelViewModel = new PanelViewModel(CalendarViewModel);
    }

    public CalendarViewModel CalendarViewModel { get; }
    public PanelViewModel PanelViewModel { get; }
}
