using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoinPlanner.DataBase;
using CoinPlanner.UI.View.Dialogs;
using CoinPlanner.UI.ViewModel.Controls;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CoinPlanner.UI.ViewModel.Dialogs;

public class MarkDialogsViewModel : ObservableObject
{
    public MarkDialogsViewModel(MarkDialogs markDialogs, DataService dataService, CalendarViewModel calendarViewModel) { }
}
