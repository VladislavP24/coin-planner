using System.Windows;
using System.Windows.Input;
using CoinPlanner.Contracts.Abstractions.DataBase;
using CoinPlanner.Contracts.Abstractions.ViewModel;
using CoinPlanner.Contracts.Abstractions.ViewModel.Controls;
using CoinPlanner.LogService;
using CommunityToolkit.Mvvm.Input;

namespace CoinPlanner.UI.ViewModel.Dialogs;

public class DeleteDataDialogsViewModel : IViewModelDialogs
{
    public DeleteDataDialogsViewModel(IPanelControls panel, IDataService dataService, IContentControls content)
    {
        _dataService = dataService;
        _content = content;
        _panel = panel;

        Ok = new RelayCommand<Window>(OkCommand);
        Cancel = new RelayCommand<Window>(CancelCommand);

        Log.Send(EventLevel.Info, logSender, "Открытие окна");
    }

    private readonly IDataService _dataService;
    private readonly IContentControls _content;
    private readonly IPanelControls _panel;
    public ICommand Ok { get; set; }
    public ICommand Cancel { get; set; }
    public int NumberRow { get; set; }

    private const string logSender = "Delete Data";

    public void OkCommand(object currWindow)
    {
        Window window = currWindow as Window;

        if (_panel.SelectedItemPlan == null)
        {
            window.Close();
            return;
        }

        int row = 0;
        foreach (var oper in _dataService.GetOperationsList().Where(x => x.Oper_Next_Date >= _content.StartDate && x.Oper_Next_Date <= _content.EndDate)
                                                             .Where(x => x.Oper_Plan_Id == _content.Plan.Plan_Id))
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
                _dataService.RemoveOperationsList(oper);
                break;
            }
        }

        _panel.UpdateDatePlan();
        _content.UpdateOperation();

        window.Close();
    }

    public void CancelCommand(object currWindow)
    {
        Log.Send(EventLevel.Info, logSender, "Окно закрыто");

        Window window = currWindow as Window;
        window.Close();
    }
}
