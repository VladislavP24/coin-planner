using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;
using CoinPlanner.DataBase;
using CoinPlanner.DataBase.ModelsDB;
using CoinPlanner.UI.View.Dialogs;
using CoinPlanner.UI.ViewModel.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CoinPlanner.UI.ViewModel.Dialogs;

public class CreatePlanDialogsViewModel : ObservableObject
{
    public CreatePlanDialogsViewModel(PanelViewModel panelViewModel, DataService dataService, CreatePlanDialogs createPlanDialogs) 
    { 
        _dataService = dataService;
        _panelViewModel = panelViewModel;
        _createPlanDialogs = createPlanDialogs;

        Ok = new RelayCommand(OkCommand);
        Cancel = new RelayCommand(CancelCommand);
    }

    private PanelViewModel _panelViewModel;
    private DataService _dataService;
    private CreatePlanDialogs _createPlanDialogs;
    public ICommand Ok { get; set; }
    public ICommand Cancel { get; set; }
    public string InputName { get; set; }

    private void OkCommand()
    {
        Guid guid = Guid.NewGuid();
        _dataService.PlanCondition.Add(guid, 1);

        _dataService.PlansList.Add(new Plans
        {
            Plan_Id = guid,
            Plan_Name = InputName,
            Date_Create = DateTime.Now,
            Date_Update = DateTime.Now,
            Is_Synchro = false
        });

        var saveSelectedPlan = _panelViewModel.SelectedItemPlan;
        _panelViewModel.PlanUpdate();
        _panelViewModel.SelectedItemPlan = saveSelectedPlan;

        _createPlanDialogs.Close();
    }

    private void CancelCommand()
        => _createPlanDialogs.Close();
}
