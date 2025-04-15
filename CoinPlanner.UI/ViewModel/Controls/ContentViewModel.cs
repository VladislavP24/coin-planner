using System.Collections.ObjectModel;
using CoinPlanner.DataBase;
using CoinPlanner.UI.Model;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CoinPlanner.UI.ViewModel.Controls;

public class ContentViewModel : ObservableObject
{
    public ContentViewModel(CalendarViewModel calendarViewModel, PanelViewModel panelViewModel, DBProcessing dBProcessing) 
    {
        _dBProcessing = dBProcessing;
        _calendarViewModel = calendarViewModel;
        _panelViewModel = panelViewModel;
        UpdateOperation(this, EventArgs.Empty);
    }

    private DBProcessing _dBProcessing;
    private CalendarViewModel _calendarViewModel;
    private PanelViewModel _panelViewModel;

    public void UpdateOperation(object sender, EventArgs e)
    {
        if (_calendarViewModel.SelectedStart == null || _calendarViewModel.SelectedEnd == null || _panelViewModel.SelectedItemPlan == null)
            return;
        
        foreach (var oper in _dBProcessing.OperationsList)
        {
            int i = 1;
            DynamicOperationCollection.Add(new OperationModel
            {
                OperId = i,
                OperName = 
            });

            i++;
        }
    }

    /// <summary>
    /// Коллекция записной книжки
    /// </summary>
    public ObservableCollection<OperationModel> DynamicOperationCollection
    {
        get => _dynamicOperationCollection;
        set => SetProperty(ref _dynamicOperationCollection, value, nameof(DynamicOperationCollection));
    }
    private ObservableCollection<OperationModel> _dynamicOperationCollection = new();


    /// <summary>
    /// Выбранная строчка в записной книжке
    /// </summary>
    public OperationModel CurSelectedOperation
    {
        get => _curSelectedOperation;
        set => SetProperty(ref _curSelectedOperation, value, nameof(CurSelectedOperation));
    }
    private OperationModel _curSelectedOperation;


}
