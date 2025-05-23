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


    /// <summary>
    /// Получение первого свободного ID из плана
    /// </summary>
    private int GetPlanFirstFreeID()
    {
        int result = 0;

        for (int i = 0; i < _dataService.PlansList.Count; i++)
        {
            result = _dataService.PlansList[i].Plan_Id + 1;
            if (!_dataService.PlansList.Any(x => x.Plan_Id == result))
                return result;
        }

        return result;
    }


    private void OkCommand()
    {
        int id = GetPlanFirstFreeID();

        if (_dataService.PlanCondition.Any(x => x.Key == id))
        {
            _dataService.PlanCondition.Remove(id);
            _dataService.PlanCondition.Add(id, 2);
        }
        else
            _dataService.PlanCondition.Add(id, 1);

        _dataService.PlansList.Add(new Plans
        {
            Plan_Id = id,
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
