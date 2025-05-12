using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml;
using CoinPlanner.DataBase;
using CoinPlanner.DataBase.ModelsDB;
using CoinPlanner.UI.Model;
using CoinPlanner.UI.View.Dialogs;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CoinPlanner.UI.ViewModel.Controls;

public class PanelViewModel : ObservableObject
{
    public PanelViewModel(CalendarViewModel calendarViewModel, ContentViewModel contentViewModel, DataService dataService) 
    {
        BindingCommandToButton();
        _calendarViewModel = calendarViewModel;
        _contentViewModel = contentViewModel;
        _dataService = dataService;

        foreach (var category in _dataService.CategoriesList)
            Categories.Add(category.Category_Id, category.Category_Name);

        PlanUpdate();
    }

    private CalendarViewModel _calendarViewModel { get; set; }
    private ContentViewModel _contentViewModel { get; set; }
    private DataService _dataService { get; set; }

    public bool IsCheckedEnroll
    {
        get => _isChekedEnroll;
        set => SetProperty(ref _isChekedEnroll, value, nameof(IsCheckedEnroll));
    }
    private bool _isChekedEnroll = false;

    public bool IsCheckedExpenses
    {
        get => _isCheckedExpenses;
        set => SetProperty(ref _isCheckedExpenses, value, nameof(IsCheckedExpenses));
    }
    private bool _isCheckedExpenses = false;


    public ObservableCollection<PlanModel> Items { get; set; } = new(); // Элементы комбобокс Планы
    public Dictionary<int, string> Categories { get; set; } = new();

    public PlanModel SelectedItemPlan  // Выбранный элемент из комбобокс Планы
    {
        get => _selectedItemPlan;
        set
        {
            SetProperty(ref _selectedItemPlan, value, nameof(SelectedItemPlan));
            _contentViewModel.Plan = value;
            _contentViewModel.UpdateOperation();
        }
    }
    private PlanModel _selectedItemPlan;


    public void PlanUpdate()
    {
        foreach (var plan in _dataService.PlansList)
        {
            Items.Add(new PlanModel()
            {
                PlanId = plan.Plan_Id,
                PlanName = plan.Plan_Name,
                DataCreate = plan.Date_Create,
                DataUpdate = plan.Date_Update
            });
        }
    }


    #region Команды на панели

    public ICommand CreatePlan { get; set; }
    public ICommand DeletePlan { get; set; }
    public ICommand SavePlan { get; set; }
    public ICommand OpenPlan { get; set; }
    public ICommand RenamePlan { get; set; }
    public ICommand ConvertPlan { get; set; }

    public ICommand AddData { get; set; }
    public ICommand EditData { get; set; }
    public ICommand DeleteData { get; set; }
    public ICommand EnrollmentsSort { get; set; }
    public ICommand ExpensesSort { get; set; }
    public ICommand Fixation { get; set; }

    public ICommand OpenGraph { get; set; }
    public ICommand OpenTable { get; set; }
    public ICommand Synchronization { get; set; }

    public ICommand Interval { get; set; }
    public ICommand Type { get; set; }
    public ICommand Mark { get; set; }

    public void BindingCommandToButton()
    {
        Interval = new RelayCommand(IntervalCommand);
        Type = new RelayCommand(TypeCommand);
        AddData = new RelayCommand(AddDataCommand);
        DeleteData = new RelayCommand(DeleteDataCommand);
        EditData = new RelayCommand(EditDataCommand);
        Synchronization = new RelayCommand(SynchronizationCommand);
        EnrollmentsSort = new RelayCommand(SortCommand);
        ExpensesSort = new RelayCommand(SortCommand);
        CreatePlan = new RelayCommand(CreatePlanCommand);
        DeletePlan = new RelayCommand(DeletePlanCommand);
        RenamePlan = new RelayCommand(RenamePlanCommand);
    }

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

    public void AddDataCommand()
    {
        AddDataDialogs addDataDialogs = new AddDataDialogs(_dataService, this, _contentViewModel);
        addDataDialogs.ShowDialog();
    }

    public void EditDataCommand()
    {
        EditDataDialogs editDataDialogs = new EditDataDialogs(_dataService, _contentViewModel, this);
        editDataDialogs.ShowDialog();
    }

    public void DeleteDataCommand()
    {
        DeleteDataDialogs deleteDataDialogs = new DeleteDataDialogs(_dataService, _contentViewModel, this);
        deleteDataDialogs.ShowDialog();
    }


    public void SynchronizationCommand()
    {
        if (_dataService.SaveDataToDatabaseAsync())
        {
            MessageBox.Show("Синхронизация данных прошла успешно!",
                            "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        else
        {
            MessageBox.Show("Не удалось провести синхронизацию данных. Проверьте подключение к БД.",
                             "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }

    public void SortCommand()
    {
        if (IsCheckedEnroll && !IsCheckedExpenses)
            _contentViewModel.IsType = "Зачисление";
        else if (!IsCheckedEnroll && IsCheckedExpenses)
            _contentViewModel.IsType = "Оплата";
        else
            _contentViewModel.IsType = "Все операции";

        _contentViewModel.UpdateOperation();
    }

    public void CreatePlanCommand()
    {
        CreatePlanDialogs createPlanDialogs = new CreatePlanDialogs(this, _dataService);
        createPlanDialogs.ShowDialog();
    }

    public void DeletePlanCommand()
    {
        DeletePlanDialogs deletePlanDialogs = new DeletePlanDialogs(this, _dataService);
        deletePlanDialogs.ShowDialog();
    }

    public void RenamePlanCommand()
    {
        RenamePlanDialogs renamePlanDialogs = new RenamePlanDialogs(this, _dataService);
        renamePlanDialogs.ShowDialog();
    }
    #endregion
}
