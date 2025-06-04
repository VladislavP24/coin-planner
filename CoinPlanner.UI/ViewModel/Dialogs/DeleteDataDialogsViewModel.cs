using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CoinPlanner.DataBase;
using CoinPlanner.UI.View.Dialogs;
using CoinPlanner.UI.ViewModel.Controls;
using System.Xml.Linq;
using CommunityToolkit.Mvvm.Input;
using static System.Runtime.InteropServices.JavaScript.JSType;
using CoinPlanner.LogService;

namespace CoinPlanner.UI.ViewModel.Dialogs;

public class DeleteDataDialogsViewModel
{
    public DeleteDataDialogsViewModel(DeleteDataDialogs deleteDataDialogs, DataService dataService, ContentViewModel contentViewModel, PanelViewModel panelViewModel) 
    {
        _deleteDataDialogs = deleteDataDialogs;
        _contentViewModel = contentViewModel;
        _dataService = dataService;
        _panelViewModel = panelViewModel;
        Ok = new RelayCommand(OkCommand);
        Cancel = new RelayCommand(CancelCommand);

        Log.Send(EventLevel.Info, logSender, "Открытие окна");
    }

    private DataService _dataService;
    private ContentViewModel _contentViewModel;
    private DeleteDataDialogs _deleteDataDialogs;
    private PanelViewModel _panelViewModel;
    public ICommand Ok { get; set; }
    public ICommand Cancel { get; set; }
    public int NumberRow { get; set; }

    private const string logSender = "Delete Data";

    private void OkCommand()
    {
        if (_panelViewModel.SelectedItemPlan == null)
        {
            _deleteDataDialogs.Close();
            return;
        }

        int row = 0;
        foreach (var oper in _dataService.OperationsList.Where(x => x.Oper_Next_Date >= _contentViewModel.StartDate && x.Oper_Next_Date <= _contentViewModel.EndDate)
                                                         .Where(x => x.Oper_Plan_Id == _contentViewModel.Plan.PlanId))
        {
            row++;
            if (row == NumberRow)
            {
                if (!_dataService.OperCondition.Any(x => x.Key == oper.Oper_Id && x.Value == 1))
                {
                    _dataService.OperCondition.Remove(oper.Oper_Id);
                    _dataService.OperCondition.Add(oper.Oper_Id, 3);
                }

                Log.Send(EventLevel.Info, logSender, $"Операция {oper.Oper_Name} удалена");
                _dataService.OperCondition.Remove(oper.Oper_Id);
                _dataService.OperationsList.Remove(oper);
                break;
            }
        }

        _panelViewModel.UpdateDatePlan();
        _contentViewModel.UpdateOperation();
        _deleteDataDialogs.Close();
    }

    private void CancelCommand()
    {
        Log.Send(EventLevel.Info, logSender, "Окно закрыто");
        _deleteDataDialogs.Close();
    }
}
