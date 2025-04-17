using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using CoinPlanner.DataBase;
using CoinPlanner.UI.View.Controls;
using CoinPlanner.UI.ViewModel.Controls;

namespace CoinPlanner.UI.ViewModel;

public class MainWindowViewModel
{
    public MainWindowViewModel(DBProcessing dbProcessing)
    {
        // Определение ViewModel`ей
        CalendarViewModel = new CalendarViewModel();
        PanelViewModel = new PanelViewModel(CalendarViewModel, dbProcessing);
        ContentViewModel = new ContentViewModel(CalendarViewModel, PanelViewModel, dbProcessing);

        CalendarViewModel.OnButtonPressed += ContentViewModel.UpdateOperation;
        PanelViewModel.OnButtonPressed += ContentViewModel.UpdateOperation;
    }

    public CalendarViewModel CalendarViewModel { get; }
    public PanelViewModel PanelViewModel { get; }
    public ContentViewModel ContentViewModel { get; }
}
