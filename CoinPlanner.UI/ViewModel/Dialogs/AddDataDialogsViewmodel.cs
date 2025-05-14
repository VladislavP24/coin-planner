using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CoinPlanner.DataBase;
using CoinPlanner.UI.Model;
using CoinPlanner.UI.View.Dialogs;
using CoinPlanner.UI.ViewModel.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CoinPlanner.UI.ViewModel.Dialogs;

public class AddDataDialogsViewmodel : ObservableObject
{
    public AddDataDialogsViewmodel(AddDataDialogs addDataDialogs, DataService dataService, PanelViewModel panelViewModel, ContentViewModel contentViewModel) 
    {
        _panelViewModel = panelViewModel;
        _dataService = dataService;
        _addDataDialogs = addDataDialogs;
        _contentViewModel = contentViewModel;

        foreach (var category in _panelViewModel.Categories)
            CategoryItems.Add(category.Value);

        Ok = new RelayCommand(OkCommand);
        Cancel = new RelayCommand(CancelCommand);
    }

    private AddDataDialogs _addDataDialogs;
    private DataService _dataService;
    private PanelViewModel _panelViewModel;
    private ContentViewModel _contentViewModel;

    public ICommand Ok { get; set; }
    public ICommand Cancel { get; set; }

    public string Name { get; set; }
    public double Sum { get; set; }
    public bool Completed { get; set; }
    public DateTime Date { get; set; } = new DateTime(2025, 1, 01, 0, 0, 0);


    //ComboBox Items and Selected
    public ObservableCollection<string> TypeItems { get; set; } = new ObservableCollection<string> { "Зачисление", "Оплата" };

    public string TypeSelected
    {
        get => _typeSelected;
        set => SetProperty(ref _typeSelected, value, nameof(TypeSelected));
    }
    private string _typeSelected;

    public ObservableCollection<string> CategoryItems { get; set; } = new();

    public string CategorySelected
    {
        get => _categorySelected;
        set => SetProperty(ref _categorySelected, value, nameof(CategorySelected));
    }
    private string _categorySelected;


    /// <summary>
    /// Получение первого свободного ID из операций
    /// </summary>
    private int GetOperFirstFreeID()
    {
        int result = 0;

        for (int i = 0; i < _dataService.OperationsList.Count - 1; i++)
        {
            result = _dataService.OperationsList[i].Oper_Id + 1;
            if (!_dataService.OperationsList.Any(x => x.Oper_Id == result))
                return result;
        }

        return result;
    }


    private void OkCommand()
    {
        if (_panelViewModel.SelectedItemPlan == null)
        {
            _addDataDialogs.Close();
            return;
        }

        if(_dataService.OperCondition.Any(x => x.Key == GetOperFirstFreeID()))
        {
            _dataService.OperCondition.Remove(GetOperFirstFreeID());
            _dataService.OperCondition.Add(GetOperFirstFreeID(), 2);
        }
        else
            _dataService.OperCondition.Add(GetOperFirstFreeID(), 1);

        _dataService.OperationsList.Add(new DataBase.ModelsDB.Operations
        {
            Oper_Id = GetOperFirstFreeID(),
            Oper_Name = Name,
            Type_Name = TypeSelected,
            Category_Name = CategorySelected,
            Oper_Sum = Sum,
            Oper_Completed = Completed,
            Oper_Next_Date = Date,
            Oper_Plan_Id = _panelViewModel.SelectedItemPlan.PlanId,
        });

        _panelViewModel.UpdateDatePlan();
        _contentViewModel.UpdateOperation();
        _addDataDialogs.Close();
    }

    private void CancelCommand()
        => _addDataDialogs.Close();
}
