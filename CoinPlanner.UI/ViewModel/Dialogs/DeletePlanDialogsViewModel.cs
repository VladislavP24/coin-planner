using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using CoinPlanner.Contracts.Abstractions.DataBase;
using CoinPlanner.Contracts.Abstractions.ViewModel;
using CoinPlanner.Contracts.Abstractions.ViewModel.Controls;
using CoinPlanner.LogService;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CoinPlanner.UI.ViewModel.Dialogs;

public class DeletePlanDialogsViewModel : ObservableObject, IViewModelDialogs
{
    public DeletePlanDialogsViewModel(IPanelControls panel, IDataService dataService)
    {
        _dataService = dataService;
        _panel = panel;

        foreach (var plan in _dataService.GetPlanList().Select(x => x.Plan_Name))
            Items.Add(plan);

        Ok = new RelayCommand<Window>(OkCommand);
        Cancel = new RelayCommand<Window>(CancelCommand);

        Log.Send(EventLevel.Info, logSender, "Открытие окна");
    }

    private readonly IPanelControls _panel;
    private readonly IDataService _dataService;
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

    public void OkCommand(object currWindow)
    {
        var plan = _dataService.GetPlanList().Where(x => x.Plan_Name == SelectedItem).First();

        if (_dataService.PlanCondition.Any(x => x.Key == plan.Plan_Id && x.Value == 1))
            _dataService.PlanCondition.Remove(plan.Plan_Id);
        else
        {
            _dataService.PlanCondition.Remove(plan.Plan_Id);
            _dataService.PlanCondition.Add(plan.Plan_Id, 3);
        }


        _dataService.RemovePlanList(plan);
        Log.Send(EventLevel.Info, logSender, $"План {plan.Plan_Name} удалён");

        if (_panel.SelectedItemPlan != null && plan.Plan_Name == _panel.SelectedItemPlan.Plan_Name)
        {
            _panel.SelectedItemPlan = null;
            _panel.PlanUpdate();
        }
        else
        {
            var saveSelectedPlan = _panel.SelectedItemPlan;
            _panel.PlanUpdate();
            _panel.SelectedItemPlan = saveSelectedPlan;
        }

        Window window = currWindow as Window;
        window.Close();
    }

    public void CancelCommand(object currWindow)
    {
        Log.Send(EventLevel.Info, logSender, "Окно закрыто");

        Window window = currWindow as Window;
        window.Close();
    }
}
