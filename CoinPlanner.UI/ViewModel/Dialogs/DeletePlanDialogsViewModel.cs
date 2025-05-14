using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using CoinPlanner.DataBase;
using CoinPlanner.UI.View.Dialogs;
using CoinPlanner.UI.ViewModel.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CoinPlanner.UI.ViewModel.Dialogs;

public class DeletePlanDialogsViewModel : ObservableObject
{
    public DeletePlanDialogsViewModel(PanelViewModel panelViewModel, DataService dataService, DeletePlanDialogs deletePlanDialogs)
    {
        _dataService = dataService;
        _panelViewModel = panelViewModel;
        _deletePlanDialogs = deletePlanDialogs;
        foreach (var plan in _dataService.PlansList.Select(x => x.Plan_Name))
            Items.Add(plan);

        Ok = new RelayCommand(OkCommand);
        Cancel = new RelayCommand(CancelCommand);
    }

    private PanelViewModel _panelViewModel;
    private DataService _dataService;
    private DeletePlanDialogs _deletePlanDialogs;
    public ICommand Ok { get; set; }
    public ICommand Cancel { get; set; }
    public ObservableCollection<string> Items { get; set; } = new();
    public string SelectedItem
    {
        get => _selectedItem;
        set => SetProperty(ref _selectedItem, value, nameof(SelectedItem));
    }
    private string _selectedItem;

    private void OkCommand()
    {
        var plan = _dataService.PlansList.Where(x => x.Plan_Name == SelectedItem).First();

        if (_dataService.PlanCondition.Any(x => x.Key == plan.Plan_Id && x.Value == 1))
            _dataService.PlanCondition.Remove(plan.Plan_Id);
        else
        {
            _dataService.PlanCondition.Remove(plan.Plan_Id);
            _dataService.PlanCondition.Add(plan.Plan_Id, 3);
        }
            

        _dataService.PlansList.Remove(plan);

        if (_panelViewModel.SelectedItemPlan != null && plan.Plan_Name == _panelViewModel.SelectedItemPlan.PlanName)
            _panelViewModel.PlanUpdate();
        else
        {
            var saveSelectedPlan = _panelViewModel.SelectedItemPlan;
            _panelViewModel.PlanUpdate();
            _panelViewModel.SelectedItemPlan = saveSelectedPlan;
        }

            _deletePlanDialogs.Close();
    }

    private void CancelCommand()
        => _deletePlanDialogs.Close();
}
