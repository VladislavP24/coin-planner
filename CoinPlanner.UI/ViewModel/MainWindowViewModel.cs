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
        PanelViewModel = new PanelViewModel();
    }

    public PanelViewModel PanelViewModel { get; }
}
