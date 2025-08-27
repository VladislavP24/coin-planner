using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;
using CoinPlanner.DataBase;
using CoinPlanner.DataBase.ModelsDB;
using CoinPlanner.LogService;
using CoinPlanner.UI.Interface;
using CoinPlanner.UI.View.Dialogs;
using CoinPlanner.UI.ViewModel.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CoinPlanner.UI.ViewModel.Dialogs;

public class CreatePlanDialogsViewModel : ObservableObject, IViewModelDialogs
{
    public CreatePlanDialogsViewModel(PanelViewModel panelViewModel, DataService dataService) 
    { 
        _dataService = dataService;
        _panelViewModel = panelViewModel;

        Ok = new RelayCommand<Window>(OkCommand);
        Cancel = new RelayCommand<Window>(CancelCommand);

        Log.Send(EventLevel.Info, logSender, "Открытие окна");
    }

    private PanelViewModel _panelViewModel;
    private DataService _dataService;
    public ICommand Ok { get; set; }
    public ICommand Cancel { get; set; }
    public string InputName { get; set; }

    private const string logSender = "Create Plan";

    public void OkCommand(Window window)
    {
        Guid guid = Guid.NewGuid();
        _dataService.PlanCondition.Add(guid, 1);

        _dataService.PlansList.Add(new Plans
        {
            Plan_Id = guid,
            Plan_Name = InputName,
            Date_Create = DateTime.Now,
            Date_Update = DateTime.Now
        });

        Log.Send(EventLevel.Info, logSender, "План добавлен");
        var saveSelectedPlan = _panelViewModel.SelectedItemPlan;
        _panelViewModel.PlanUpdate();
        _panelViewModel.SelectedItemPlan = saveSelectedPlan;

        window.Close();
    }

    public void CancelCommand(Window window)
    {
        Log.Send(EventLevel.Info, logSender, "Окно закрыто");
        window.Close();
    } 
}
