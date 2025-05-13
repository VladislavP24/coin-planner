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
using CoinPlanner.UI.View.Dialogs;
using CoinPlanner.UI.ViewModel.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CoinPlanner.UI.ViewModel.Dialogs;

public class RenamePlanDialogsViewModel : ObservableObject
{
    public RenamePlanDialogsViewModel(PanelViewModel panelViewModel, DataService dataService, RenamePlanDialogs renamePlanDialogs)
    {
        _dataService = dataService;
        _panelViewModel = panelViewModel;
        _renamePlanDialogs = renamePlanDialogs;
        foreach (var plan in _dataService.PlansList.Select(x => x.Plan_Name))
            Items.Add(plan);

        Ok = new RelayCommand(OkCommand);
        Cancel = new RelayCommand(CancelCommand);
    }

    private PanelViewModel _panelViewModel;
    private DataService _dataService;
    private RenamePlanDialogs _renamePlanDialogs;
    public ICommand Ok { get; set; }
    public ICommand Cancel { get; set; }
    public string InputName { get; set; }
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

        if (InputName != _dataService.PlansList.Where(x => x.Plan_Name == InputName).Select(x => x.Plan_Name).FirstOrDefault())
        {
            plan.Plan_Name = InputName;
            plan.Date_Update = DateTime.Now;

            if (_dataService.PlanCondition.Where(x => x.Key == plan.Plan_Id && x.Value == 1) == null)
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
                                                                    DataCreate = plan.Date_Create, 
                                                                    DataUpdate = plan.Date_Update};
        }   
        else
        {
            var saveSelectedPlan = _panelViewModel.SelectedItemPlan;
            _panelViewModel.PlanUpdate();
            _panelViewModel.SelectedItemPlan = saveSelectedPlan;
        }

        _renamePlanDialogs.Close();
    }

    private void CancelCommand()
        => _renamePlanDialogs.Close();
}
