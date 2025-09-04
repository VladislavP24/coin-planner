using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using CoinPlanner.Contracts.Abstractions.DataBase;
using CoinPlanner.Contracts.Abstractions.ViewModel;
using CoinPlanner.Contracts.Abstractions.ViewModel.Controls;
using CoinPlanner.Contracts.DTO.DataServieDTO;
using CoinPlanner.LogService;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CoinPlanner.UI.ViewModel.Dialogs;

public class RenamePlanDialogsViewModel : ObservableObject, IViewModelDialogs
{
    public RenamePlanDialogsViewModel(IPanelControls panel, IDataService dataService)
    {
        _dataService = dataService;
        _panel = panel;

        foreach (var plan in _dataService.GetPlanList().Select(x => x.Plan_Name))
            Items.Add(plan);

        Ok = new RelayCommand<Window>(OkCommand);
        Cancel = new RelayCommand<Window>(CancelCommand);

        Log.Send(EventLevel.Info, logSender, "Открытие окна");
    }

    private readonly IDataService _dataService;
    private readonly IPanelControls _panel;
    public ICommand Ok { get; set; }
    public ICommand Cancel { get; set; }
    public string InputName { get; set; }
    public ObservableCollection<string> Items { get; set; } = new();
    private const string logSender = "Rename Plan";

    public string SelectedItem
    {
        get => _selectedItem;
        set => SetProperty(ref _selectedItem, value, nameof(SelectedItem));
    }
    private string _selectedItem;

    public void OkCommand(object currWindow)
    {
        var plan = _dataService.GetPlanList().Where(x => x.Plan_Name == SelectedItem).First();
        _dataService.RemovePlanList(plan);

        if (InputName != _dataService.GetPlanList().Where(x => x.Plan_Name == InputName).Select(x => x.Plan_Name).FirstOrDefault())
        {
            Log.Send(EventLevel.Info, logSender, $"План {plan.Plan_Name} переименован на {InputName}");
            plan.Plan_Name = InputName;
            plan.Date_Update = DateTime.Now;

            _dataService.AddPlanList(plan);

            if (!_dataService.PlanCondition.Any(x => x.Key == plan.Plan_Id && x.Value == 1))
            {
                _dataService.PlanCondition.Remove(plan.Plan_Id);
                _dataService.PlanCondition.Add(plan.Plan_Id, 2);
            }
        }
        else
        {
            MessageBox.Show("Введённое имя уже используется среди других планов. Введите другое имя!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        if (SelectedItem == _panel.SelectedItemPlan.Plan_Name)
        {
            _panel.PlanUpdate();
            _panel.SelectedItemPlan = new PlansDTO
            {
                Plan_Id = plan.Plan_Id,
                Plan_Name = plan.Plan_Name,
                Date_Create = plan.Date_Create,
                Date_Update = plan.Date_Update
            };
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
