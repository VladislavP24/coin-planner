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
using CoinPlanner.LogService;
using CoinPlanner.UI.Interface;

namespace CoinPlanner.UI.ViewModel.Dialogs;

public class DeletePlanDialogsViewModel : ObservableObject, IViewModelDialogs
{
    public DeletePlanDialogsViewModel(PanelViewModel panelViewModel, DataService dataService)
    {
        _dataService = dataService;
        _panelViewModel = panelViewModel;
        foreach (var plan in _dataService.PlansList.Select(x => x.Plan_Name))
            Items.Add(plan);

        Ok = new RelayCommand<Window>(OkCommand);
        Cancel = new RelayCommand<Window>(CancelCommand);

        Log.Send(EventLevel.Info, logSender, "Открытие окна");
    }

    private PanelViewModel _panelViewModel;
    private DataService _dataService;
    public ICommand Ok { get; set; }
    public ICommand Cancel { get; set; }
    public ObservableCollection<string> Items { get; set; } = new();
    private const string logSender = "Delete Plan";

    public string SelectedItem
    {
        get => _selectedItem;
        set => SetProperty(ref _selectedItem, value, nameof(SelectedItem));
    }
    private string _selectedItem;

    public void OkCommand(Window window)
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
        Log.Send(EventLevel.Info, logSender, $"План {plan.Plan_Name} удалён");

        if (_panelViewModel.SelectedItemPlan != null && plan.Plan_Name == _panelViewModel.SelectedItemPlan.PlanName)
        {
            _panelViewModel.SelectedItemPlan = null;
            _panelViewModel.PlanUpdate();
        }       
        else
        {
            var saveSelectedPlan = _panelViewModel.SelectedItemPlan;
            _panelViewModel.PlanUpdate();
            _panelViewModel.SelectedItemPlan = saveSelectedPlan;
        }

        window.Close();
    }

    public void CancelCommand(Window window)
    {
        Log.Send(EventLevel.Info, logSender, "Окно закрыто");
        window.Close();
    }
}
