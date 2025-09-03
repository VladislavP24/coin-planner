using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using CoinPlanner.Contracts.Abstractions.DataBase;
using CoinPlanner.Contracts.Abstractions.ViewModel;
using CoinPlanner.Contracts.Abstractions.ViewModel.Controls;
using CoinPlanner.LogService;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CoinPlanner.UI.ViewModel.Dialogs;

public class AddDataDialogsViewmodel : ObservableObject, IViewModelDialogs
{
    public AddDataDialogsViewmodel(IDataService dataService, IPanelControls panel, IContentControls content)
    {
        _dataService = dataService;
        _panel = panel;
        _content = content;

        foreach (var category in _panel.Categories)
            CategoryItems.Add(category.Value);

        Ok = new RelayCommand<Window>(OkCommand);
        Cancel = new RelayCommand<Window>(CancelCommand);

        Log.Send(EventLevel.Info, logSender, "Открытие окна");
        _panel = panel;
    }

    private readonly IDataService _dataService;
    private readonly IContentControls _content;
    private readonly IPanelControls _panel;

    public ICommand Ok { get; set; }
    public ICommand Cancel { get; set; }

    public string Name { get; set; }
    public double Sum { get; set; }
    public bool Completed { get; set; }
    public DateTime Date { get; set; } = DateTime.Now;

    private const string logSender = "Add Data";


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


    public void OkCommand(object currWindow)
    {
        Guid newGuid = Guid.NewGuid();
        _dataService.OperCondition.Add(newGuid, 1);

        _dataService.AddOperationsList(new Contracts.DTO.DataServieDTO.OperationsDTO
        {
            Oper_Id = newGuid,
            Oper_Name = Name,
            Type_Name = TypeSelected,
            Category_Name = CategorySelected,
            Oper_Sum = Sum,
            Oper_Completed = Completed,
            Oper_Next_Date = Date,
            Oper_Plan_Id = _panel.SelectedItemPlan.Plan_Id,
        });

        Log.Send(EventLevel.Info, logSender, "Операция добавлена");

        _panel.UpdateDatePlan();
        _content.UpdateOperation();

        Window window = currWindow as Window;
        window.Close();
    }

    public void CancelCommand(object currWindow)
    {
        Log.Send(EventLevel.Info, logSender, "Окно закрыто");

        Window window = currWindow as Window;
        window.Close();
    }
}
