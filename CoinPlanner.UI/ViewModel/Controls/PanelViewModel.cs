using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CoinPlanner.DataBase;
using CoinPlanner.DataBase.ModelsDB;
using CoinPlanner.UI.Model;
using CoinPlanner.UI.View.Dialogs;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CoinPlanner.UI.ViewModel.Controls;

public class PanelViewModel : ObservableObject
{
    public PanelViewModel(CalendarViewModel calendarViewModel, DBProcessing dBProcessing) 
    {
        Interval = new RelayCommand(IntervalCommand);
        Type = new RelayCommand(TypeCommand);
        _calendarViewModel = calendarViewModel;
        _dBProcessing = dBProcessing;

        foreach(var plan in dBProcessing.PlansList)
        {
            Items.Add(new PlanModel() 
            {
                PlanId = plan.Plan_Id,
                PlanName = plan.Plan_Name,
                DataCreate = plan.Data_Create,
                DataUpdate = plan.Data_Update
            });
        }
    }

    private CalendarViewModel _calendarViewModel { get; set; }
    private DBProcessing _dBProcessing { get; set; }
    public ICommand Interval { get; set; }
    public ICommand Type { get; set; }

    public ObservableCollection<PlanModel> Items { get; set; } = new(); // Элементы комбобокс Планы

    public PlanModel SelectedItemPlan  // Выбранный элемент из комбобокс Планы
    {
        get => _selectedItemPlan;
        set => SetProperty(ref _selectedItemPlan, value, nameof(SelectedItemPlan));
    }
    private PlanModel _selectedItemPlan;

    public void IntervalCommand()
    {
        IntervalDialogs dialog = new IntervalDialogs(_calendarViewModel);
        dialog.ShowDialog();
    }

    public void TypeCommand() 
    {
        TypeDialogs typeDialogs = new TypeDialogs(_calendarViewModel);
        typeDialogs.ShowDialog();
    }
}
