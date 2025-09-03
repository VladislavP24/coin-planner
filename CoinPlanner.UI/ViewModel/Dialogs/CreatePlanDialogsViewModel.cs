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

public class CreatePlanDialogsViewModel : ObservableObject, IViewModelDialogs
{
    public CreatePlanDialogsViewModel(IPanelControls panel, IDataService dataService)
    {
        _dataService = dataService;
        _panel = panel;

        Ok = new RelayCommand<Window>(OkCommand);
        Cancel = new RelayCommand<Window>(CancelCommand);

        Log.Send(EventLevel.Info, logSender, "Открытие окна");
    }

    private readonly IPanelControls _panel;
    private readonly IDataService _dataService;
    public ICommand Ok { get; set; }
    public ICommand Cancel { get; set; }
    public string InputName { get; set; }

    private const string logSender = "Create Plan";

    public void OkCommand(object currWindow)
    {
        Guid guid = Guid.NewGuid();
        _dataService.PlanCondition.Add(guid, 1);

        _dataService.AddPlanList(new PlansDTO
        {
            Plan_Id = guid,
            Plan_Name = InputName,
            Date_Create = DateTime.Now,
            Date_Update = DateTime.Now
        });

        Log.Send(EventLevel.Info, logSender, "План добавлен");
        var saveSelectedPlan = _panel.SelectedItemPlan;
        _panel.PlanUpdate();
        _panel.SelectedItemPlan = saveSelectedPlan;

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
