using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoinPlanner.DataBase;
using CoinPlanner.UI.ViewModel.Controls;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CoinPlanner.UI.ViewModel.Dialogs;

public class CreatePlanDialogsViewModel : ObservableObject
{
    public CreatePlanDialogsViewModel(PanelViewModel panelViewModel, DataService dataService) { }
}
