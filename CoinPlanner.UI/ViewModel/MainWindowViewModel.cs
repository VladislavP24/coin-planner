﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using CoinPlanner.DataBase;
using CoinPlanner.UI.View.Controls;
using CoinPlanner.UI.ViewModel.Controls;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CoinPlanner.UI.ViewModel;

public class MainWindowViewModel
{
    public MainWindowViewModel(DataService dataService)
    {
        // Определение ViewModel`ей
        DiagramViewModel = new DiagramViewModel(dataService);
        ContentViewModel = new ContentViewModel(dataService, DiagramViewModel);   
        CalendarViewModel = new CalendarViewModel(DiagramViewModel, ContentViewModel, dataService);
        PanelViewModel = new PanelViewModel(CalendarViewModel, ContentViewModel, DiagramViewModel, dataService);
    }

    public CalendarViewModel CalendarViewModel { get; }
    public PanelViewModel PanelViewModel { get; }
    public ContentViewModel ContentViewModel { get; }
    public DiagramViewModel DiagramViewModel { get; }
}
