using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CoinPlanner.DataBase;
using CoinPlanner.DataBase.ModelsDB;
using CoinPlanner.LogService;
using CoinPlanner.UI.Interface;
using CoinPlanner.UI.View.Dialogs;
using CoinPlanner.UI.ViewModel.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CoinPlanner.UI.ViewModel.Dialogs;

public class RenamePlanDialogsViewModel : ObservableObject, IViewModelDialogs
{
    public RenamePlanDialogsViewModel(PanelViewModel panelViewModel, DataService dataService)
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
    public string InputName { get; set; }
    public ObservableCollection<string> Items { get; set; } = new();
    private const string logSender = "Rename Plan";

    public string SelectedItem
    {
        get => _selectedItem;
        set => SetProperty(ref _selectedItem, value, nameof(SelectedItem));
    }
    private string _selectedItem;

    public void OkCommand(Window window)
    {
        var plan = _dataService.PlansList.Where(x => x.Plan_Name == SelectedItem).First();

        if (InputName != _dataService.PlansList.Where(x => x.Plan_Name == InputName).Select(x => x.Plan_Name).FirstOrDefault())
        {
            Log.Send(EventLevel.Info, logSender, $"План {plan.Plan_Name} переименован на {InputName}");
            plan.Plan_Name = InputName;
            plan.Date_Update = DateTime.Now;

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

        if (SelectedItem == _panelViewModel.SelectedItemPlan.PlanName)
        {
            _panelViewModel.PlanUpdate();
            _panelViewModel.SelectedItemPlan = new Model.PlanModel {PlanId = plan.Plan_Id, 
                                                                    PlanName = plan.Plan_Name, 
                                                                    DateCreate = plan.Date_Create, 
                                                                    DataUpdate = plan.Date_Update};
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
