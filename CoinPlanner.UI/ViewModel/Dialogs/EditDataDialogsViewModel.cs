using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using CoinPlanner.Contracts.Abstractions.DataBase;
using CoinPlanner.Contracts.Abstractions.ViewModel;
using CoinPlanner.Contracts.Abstractions.ViewModel.Controls;
using CoinPlanner.Contracts.DTO.DataServieDTO;
using CoinPlanner.LogService;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CoinPlanner.UI.ViewModel.Dialogs;

public class EditDataDialogsViewModel : ObservableObject, IViewModelDialogs
{
    public EditDataDialogsViewModel(IDataService dataService, IContentControls content, IPanelControls panel)
    {
        _dataService = dataService;
        _panel = panel;
        _content = content;

        Ok = new RelayCommand<Window>(OkCommand);
        Cancel = new RelayCommand<Window>(CancelCommand);

        foreach (var category in _panel.Categories)
            CategoryItems.Add(category.Value);

        Log.Send(EventLevel.Info, logSender, "Открытие окна");
    }

    private readonly IDataService _dataService;
    private readonly IContentControls _content;
    private readonly IPanelControls _panel;
    public ICommand Ok { get; set; }
    public ICommand Cancel { get; set; }

    private const string logSender = "Edit Data";

    public int NumberRow
    {
        get => _numberRow;
        set
        {
            SetProperty(ref _numberRow, value, nameof(NumberRow));
            Log.Send(EventLevel.Info, logSender, $"Установлен номер операции: {value}");
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
    public void ShowData()
    {
        if (NumberRow != 0 && NumberRow != null && _panel.SelectedItemPlan != null)
        {
            OperationsDTO? operationModel = _content.DynamicOperationCollection.Where(x => x.Oper_Id_Table == NumberRow).FirstOrDefault();

            if (operationModel == null)
                return;

            Name = operationModel.Oper_Name;
            TypeSelected = operationModel.Type_Name;
            CategorySelected = operationModel.Category_Name;
            Sum = operationModel.Oper_Sum;
            Completed = operationModel.Oper_Completed;
            Date = operationModel.Oper_Next_Date;
        }
    }

    public void OkCommand(object currWindow)
    {
        int row = 0;
        foreach (var oper in _dataService.GetOperationsList().Where(x => x.Oper_Next_Date >= _content.StartDate && x.Oper_Next_Date <= _content.EndDate).Where(x => x.Oper_Plan_Id == _content.Plan.Plan_Id))
        {
            row++;
            if (row == NumberRow)
            {
                oper.Oper_Name = Name;
                oper.Type_Name = TypeSelected;
                oper.Category_Name = CategorySelected;
                oper.Oper_Sum = Sum;
                oper.Oper_Completed = Completed;
                oper.Oper_Next_Date = Date;
                oper.Oper_Plan_Id = _panel.SelectedItemPlan.Plan_Id;

                if (_dataService.OperCondition.Where(x => x.Key == oper.Oper_Id && x.Value == 1) == null)
                {
                    _dataService.OperCondition.Remove(oper.Oper_Id);
                    _dataService.OperCondition.Add(oper.Oper_Id, 2);
                }
                else
                    _dataService.OperCondition.Add(oper.Oper_Id, 2);

                Log.Send(EventLevel.Info, logSender, "Изменения операции применены");
                break;
            }
        }

        _panel.UpdateDatePlan();
        _content.UpdateOperation();

        Window window = currWindow as Window;
        window.Close();
    }

    public void CancelCommand(object currWindow)
    {
        Log.Send(EventLevel.Info, logSender, "Открытие окна");

        Window window = currWindow as Window;
        window.Close();
    }
}
