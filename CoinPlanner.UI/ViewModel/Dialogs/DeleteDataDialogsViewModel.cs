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

namespace CoinPlanner.UI.ViewModel.Dialogs;

public class DeleteDataDialogsViewModel
{
    public DeleteDataDialogsViewModel(DeleteDataDialogs deleteDataDialogs, DBProcessing dBProcessing, ContentViewModel contentViewModel, PanelViewModel panelViewModel) 
    {
        _deleteDataDialogs = deleteDataDialogs;
        _contentViewModel = contentViewModel;
        _dBProcessing = dBProcessing;
        _panelViewModel = panelViewModel;
        Ok = new RelayCommand(OkCommand);
        Cancel = new RelayCommand(CancelCommand);
    }

    private DBProcessing _dBProcessing;
    private ContentViewModel _contentViewModel;
    private DeleteDataDialogs _deleteDataDialogs;
    private PanelViewModel _panelViewModel;
    public ICommand Ok { get; set; }
    public ICommand Cancel { get; set; }
    public int NumberRow { get; set; }


    private void OkCommand()
    {
        if (_panelViewModel.SelectedItemPlan != null)
        {
            int row = 0;

            foreach (var oper in _dBProcessing.OperationsList.Where(x => x.Oper_Next_Date >= _contentViewModel.StartDate && x.Oper_Next_Date <= _contentViewModel.EndDate) 
                                                             .Where(x => x.Oper_Plan_Id == _contentViewModel.Plan.PlanId))
            {
                row++;

                if (row == NumberRow)
                {
                    _dBProcessing.OperationsList.Remove(oper);
                    break;
                }             
            }
        }

        _contentViewModel.UpdateOperation();
        _deleteDataDialogs.Close();
    }

    private void CancelCommand()
        => _deleteDataDialogs.Close();
}
