using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CoinPlanner.UI.ViewModel.Controls;

public class DiagramViewModel : ObservableObject
{
    public DiagramViewModel() { }

    public bool IsVisibleDiagram
    {
        get => _isVisibleDiagram;
        set => SetProperty(ref _isVisibleDiagram, value, nameof(IsVisibleDiagram));
    }
    private bool _isVisibleDiagram = false;
}
