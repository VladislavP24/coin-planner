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
        ContentViewModel = new ContentViewModel(dbProcessing);
        CalendarViewModel = new CalendarViewModel(ContentViewModel);
        PanelViewModel = new PanelViewModel(CalendarViewModel, ContentViewModel, dbProcessing); 
    }

    public CalendarViewModel CalendarViewModel { get; }
    public PanelViewModel PanelViewModel { get; }
    public ContentViewModel ContentViewModel { get; }
}
