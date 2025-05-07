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

public class EditDataDialogsViewModel : ObservableObject
{
    public EditDataDialogsViewModel(EditDataDialogs editDataDialogs, DataService dataService, ContentViewModel contentViewModel, PanelViewModel panelViewModel) 
    {
        _editDataDialogs = editDataDialogs;
        _dataService = dataService;
        _contentViewModel = contentViewModel;
        _panelViewModel = panelViewModel;
        Ok = new RelayCommand(OkCommand);
        Cancel = new RelayCommand(CancelCommand);

        foreach (var category in _panelViewModel.Categories)
            CategoryItems.Add(category.Value);
    }

    private DataService _dataService;
    private ContentViewModel _contentViewModel;
    private EditDataDialogs _editDataDialogs;
    private PanelViewModel _panelViewModel;
    public ICommand Ok { get; set; }
    public ICommand Cancel { get; set; }

    public int NumberRow
    {
        get => _numberRow;
        set
        {
            SetProperty(ref _numberRow, value, nameof(NumberRow));
            ShowData();
        }
    }
    private int _numberRow;

    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value, nameof(Name));
    }
    private string _name;


    public double Sum
    {
        get => _sum;
        set => SetProperty(ref _sum, value, nameof(Sum));
    }
    private double _sum;


    public bool Completed
    {
        get => _completed;
        set => SetProperty(ref _completed, value, nameof(Completed));
    }
    private bool _completed;


    public DateTime Date
    {
        get => _date;
        set => SetProperty(ref _date, value, nameof(Date));
    }
    private DateTime _date = new DateTime(2025, 1, 01, 0, 0, 0);


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
    /// Показ данных при вводе числа
    /// </summary>
    private void ShowData()
    {
        if (NumberRow != 0 && NumberRow != null && _panelViewModel.SelectedItemPlan != null)
        {
            OperationModel? operationModel =  _contentViewModel.DynamicOperationCollection.Where(x => x.OperId == NumberRow).FirstOrDefault();

            if (operationModel == null)
                return;

            Name = operationModel.OperName;
            TypeSelected = operationModel.OperType;
            CategorySelected = operationModel.OperCategory;
            Sum = operationModel.OperSum;
            Completed = operationModel.OperCompleted == "Да" ? true : false;
            Date = DateTime.ParseExact(operationModel.OperNextDate, "HH:mm:ss dd MMMM yyyy 'г.'", System.Globalization.CultureInfo.GetCultureInfo("ru-RU"));
        }
    }

    private void OkCommand()
    {
        if (_panelViewModel.SelectedItemPlan == null)
        {
            _editDataDialogs.Close();
            return;
        }   
        
        int row = 0;
        foreach (var oper in _dataService.OperationsList.Where(x => x.Oper_Next_Date >= _contentViewModel.StartDate && x.Oper_Next_Date <= _contentViewModel.EndDate) .Where(x => x.Oper_Plan_Id == _contentViewModel.Plan.PlanId))
        {
            row++;
            if (row == NumberRow)
            {
                oper.Oper_Id = _dataService.OperationsList.Count + 1;
                oper.Oper_Name = Name;
                oper.Type_Name = TypeSelected;
                oper.Category_Name = CategorySelected;
                oper.Oper_Sum = Sum;
                oper.Oper_Completed = Completed;
                oper.Oper_Next_Date = Date;
                oper.Oper_Plan_Id = _panelViewModel.SelectedItemPlan.PlanId;

                _dataService.OperCondition.Add(oper.Oper_Id, 2);
                _dataService.OperationsList.Remove(oper);
                break;
            }
        }

        _contentViewModel.UpdateOperation();
        _editDataDialogs.Close();
    }

    private void CancelCommand()
        => _editDataDialogs.Close();
}
