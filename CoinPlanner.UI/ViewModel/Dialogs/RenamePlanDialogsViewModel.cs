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

        Ok = new RelayCommand(OkCommand);
        Cancel = new RelayCommand(CancelCommand);
    }

    private PanelViewModel _panelViewModel;
    private DataService _dataService;
    private RenamePlanDialogs _renamePlanDialogs;
    public ICommand Ok { get; set; }
    public ICommand Cancel { get; set; }
    public string InputName { get; set; }
    public ObservableCollection<string> Items { get; set; }
    public string SelectedItem
    {
        get => _selectedItem;
        set => SetProperty(ref _selectedItem, value, nameof(SelectedItem));
    }
    private string _selectedItem;

    private void OkCommand()
    {
        var plan = _dataService.PlansList.Where(x => x.Plan_Name == SelectedItem).First();

        if (InputName != _dataService.PlansList.Where(x => x.Plan_Name == InputName).Select(x => x.Plan_Name).First())
        {
            plan.Plan_Name = InputName;
            plan.Date_Update = DateTime.Now;
            _dataService.PlanCondition.Add(plan.Plan_Id, 2);
        }
        else
        {
            MessageBox.Show("Введённое имя уже используется среди других планов. Введите другое имя!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        _panelViewModel.PlanUpdate();
        _renamePlanDialogs.Close();
    }

    private void CancelCommand()
        => _renamePlanDialogs.Close();
}
