using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CoinPlanner.DataBase;
using CoinPlanner.UI.Model;
using CoinPlanner.UI.View.Dialogs;
using CoinPlanner.UI.ViewModel.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CoinPlanner.UI.ViewModel.Dialogs;

public class FixationDialogsViewModel : ObservableObject
{

    public FixationDialogsViewModel(FixationDialogs fixationDialogs, PanelViewModel panelViewModel, DataService dataService)
    {
        _fixationDialogs = fixationDialogs;
        _panelViewModel = panelViewModel;
        _dataService = dataService;

        foreach (var item in _dataService.FixationsList)
            Items.Add(new FixationModel()
            {
                FixId = item.Fix_Id,
                FixName = item.Fix_Name,
                FixType = item.Type_Name,
                FixCategory = item.Category_Name,
                FixSum = item.Fix_Sum,
                FixCompleted = item.Fix_Completed,
                FixNextDate = item.Fix_Next_Date.ToString(),
                FixPlanId = item.Fix_Plan_Id,
                IsCheckFix = false,
            });

        foreach (var category in _panelViewModel.Categories)
            CategoryItems.Add(category.Value);

        AddItem = new RelayCommand(AddItemCommand);
        DeleteItem = new RelayCommand<FixationModel>(DeleteItemCommand);
        Ok = new RelayCommand(OkCommand);
        Cancel = new RelayCommand(CancelCommand);
    }

    private FixationDialogs _fixationDialogs { get; }
    private PanelViewModel _panelViewModel { get; }
    private DataService _dataService { get; }

    public ICommand Ok { get; set; }
    public ICommand Cancel { get; set; }
    public ICommand AddItem { get; set; }
    public ICommand DeleteItem { get; set; }

    public ObservableCollection<FixationModel> Items { get; set; } = new();
    public ObservableCollection<string> TypeItems { get; set; } = new ObservableCollection<string> { "Зачисление", "Оплата" };
    public ObservableCollection<string> CategoryItems { get; set; } = new();

    public void AddItemCommand()
        => Items.Add(new FixationModel()
        {
            FixId = 1,
            FixName = "Новый",
            FixType = "-",
            FixCategory = "-",
            FixSum = 0,
            FixCompleted = false,
            FixNextDate = "2025-01-01 00:00",
        });

    public void DeleteItemCommand(FixationModel fixation)
        => Items.Remove(fixation);

    private void OkCommand()
    {
        _fixationDialogs.Close();
    }

    private void CancelCommand()
        => _fixationDialogs.Close();
}
