using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml;
using CoinPlanner.DataBase;
using CoinPlanner.DataBase.ModelsDB;
using CoinPlanner.FileService;
using CoinPlanner.FileService.DTO;
using CoinPlanner.UI.Model;
using CoinPlanner.UI.View.Dialogs;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;

namespace CoinPlanner.UI.ViewModel.Controls;

public class PanelViewModel : ObservableObject
{
    public PanelViewModel(CalendarViewModel calendarViewModel, ContentViewModel contentViewModel, DiagramViewModel diagramViewModel, DataService dataService) 
    {
        BindingCommandToButton();
        _calendarViewModel = calendarViewModel;
        _contentViewModel = contentViewModel;
        _diagramViewModel = diagramViewModel;
        _dataService = dataService;

        foreach (var category in _dataService.CategoriesList)
            Categories.Add(category.Category_Id, category.Category_Name);

        PlanUpdate();
    }

    private CalendarViewModel _calendarViewModel { get; set; }
    private ContentViewModel _contentViewModel { get; set; }
    private DiagramViewModel _diagramViewModel { get; set; }
    private DataService _dataService { get; set; }

    /// <summary>
    /// Факт включения Зачисления
    /// </summary>
    public bool IsCheckedEnroll
    {
        get => _isChekedEnroll;
        set => SetProperty(ref _isChekedEnroll, value, nameof(IsCheckedEnroll));
    }
    private bool _isChekedEnroll = false;

    /// <summary>
    /// Факт включения Расходов
    /// </summary>
    public bool IsCheckedExpenses
    {
        get => _isCheckedExpenses;
        set => SetProperty(ref _isCheckedExpenses, value, nameof(IsCheckedExpenses));
    }
    private bool _isCheckedExpenses = false;

    /// <summary>
    /// Факт включения Таблицы
    /// </summary>
    public bool IsCheckedTable
    {
        get => _isCheckedTable;
        set => SetProperty(ref _isCheckedTable, value, nameof(IsCheckedTable));
    }
    private bool _isCheckedTable = true;

    /// <summary>
    /// Факт включения Диаграммы
    /// </summary>
    public bool IsCheckedDiagram
    {
        get => _isCheckedDiagram;
        set => SetProperty(ref _isCheckedDiagram, value, nameof(IsCheckedDiagram));
    }
    private bool _isCheckedDiagram = false;


    public ObservableCollection<PlanModel> Items { get; set; } = new(); // Элементы комбобокс Планы
    public Dictionary<int, string> Categories { get; set; } = new();

    public PlanModel SelectedItemPlan  // Выбранный элемент из комбобокс Планы
    {
        get => _selectedItemPlan;
        set
        {
            SetProperty(ref _selectedItemPlan, value, nameof(SelectedItemPlan));
            _contentViewModel.Plan = value;
            _calendarViewModel.PlanId = value.PlanId;
            _diagramViewModel.CreatDiagram(SelectedItemPlan.PlanId);
            _contentViewModel.UpdateOperation();
        }
    }
    private PlanModel _selectedItemPlan;


    public void PlanUpdate()
    {
        Items.Clear();
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

    public void UpdateDatePlan()
    {
        // Обновление даты последнего изменения плана
        var plan = _dataService.PlansList.Where(x => x.Plan_Id == SelectedItemPlan.PlanId).First();
        plan.Date_Update = DateTime.Now;

        if (_dataService.PlanCondition.Where(x => x.Key == plan.Plan_Id && x.Value == 1) == null)
        {
            _dataService.PlanCondition.Remove(plan.Plan_Id);
            _dataService.PlanCondition.Add(plan.Plan_Id, 2);
        }        
    }

    /// <summary>
    /// Получение первого свободного ID из операций
    /// </summary>
    public int GetOperFirstFreeID()
    {
        int result = 1;

        for (int i = 0; i < _dataService.OperationsList.Count; i++)
        {
            result = _dataService.OperationsList[i].Oper_Id + 1;
            if (!_dataService.OperationsList.Any(x => x.Oper_Id == result))
                return result;
        }

        return result;
    }


    /// <summary>
    /// Получение первого свободного ID из плана
    /// </summary>
    public int GetPlanFirstFreeID()
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


    #region Команды на панели

    public ICommand CreatePlan { get; set; }
    public ICommand DeletePlan { get; set; }
    public ICommand SavePlan { get; set; }
    public ICommand OpenPlan { get; set; }
    public ICommand RenamePlan { get; set; }
    public ICommand ExportPlan { get; set; }

    public ICommand AddData { get; set; }
    public ICommand EditData { get; set; }
    public ICommand DeleteData { get; set; }
    public ICommand EnrollmentsSort { get; set; }
    public ICommand ExpensesSort { get; set; }
    public ICommand Fixation { get; set; }

    public ICommand OpenDiagram { get; set; }
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
        OpenTable = new RelayCommand(OpenTableCommand);
        OpenDiagram = new RelayCommand(OpenDiagramCommand);
        Fixation = new RelayCommand(FixationCommand);
        Mark = new RelayCommand(MarkCommand);
        SavePlan = new RelayCommand(SavePlanCommand);
        OpenPlan = new RelayCommand(OpenPlanCommand);
        ExportPlan = new RelayCommand(ExportPlanCommand);
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
        if (SelectedItemPlan == null)
            return;

        AddDataDialogs addDataDialogs = new AddDataDialogs(this, _dataService,  _contentViewModel);
        addDataDialogs.ShowDialog();
    }

    public void EditDataCommand()
    {
        if (SelectedItemPlan == null)
            return;

        EditDataDialogs editDataDialogs = new EditDataDialogs(this, _dataService, _contentViewModel);
        editDataDialogs.ShowDialog();
    }

    public void DeleteDataCommand()
    {
        if (SelectedItemPlan == null)
            return;

        DeleteDataDialogs deleteDataDialogs = new DeleteDataDialogs(this, _dataService, _contentViewModel);
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

    public void OpenDiagramCommand()
    {
        if (IsCheckedDiagram)
        {
            IsCheckedTable = false;
            _contentViewModel.IsVisibleContent = false;
            _diagramViewModel.IsVisibleDiagram = true;
        }
        else
            _diagramViewModel.IsVisibleDiagram = false;
    }

    public void OpenTableCommand()
    {

        if (IsCheckedTable)
        {
            IsCheckedDiagram = false;
            _contentViewModel.IsVisibleContent = true;
            _diagramViewModel.IsVisibleDiagram = false;
        }
        else
            _contentViewModel.IsVisibleContent = false;
    }

    public void FixationCommand()
    {
        if (SelectedItemPlan == null)
            return;

        FixationDialogs fixationDialogs = new FixationDialogs(this, _dataService, _contentViewModel);
        fixationDialogs.ShowDialog();
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

    public void MarkCommand()
    {
        if(SelectedItemPlan == null)
            return;

        MarkDialogs markDialogs = new MarkDialogs(this, _dataService, _calendarViewModel);
        markDialogs.ShowDialog();
    }

    public void SavePlanCommand()
    {
        if (SelectedItemPlan == null)
            return;

        var openFolderDialog = new OpenFolderDialog
        {
            Title = "Выберите папку для сохранения файла"
        };

        if (openFolderDialog.ShowDialog() == true)
        {
            string selectedPath = openFolderDialog.FolderName;
            string fileName = $"{SelectedItemPlan.PlanName}.xml";
            string fullPath = Path.Combine(selectedPath, fileName);
            XmlSerializationHelper.SerializeToXml(ConvertModelToDTO(), fullPath);
        }
            
    }

    public void OpenPlanCommand()
    {
        var openFileDialog = new OpenFileDialog
        {
            Filter = "XML Files (*.xml)|*.xml"
        };

        if (openFileDialog.ShowDialog() == true)
            ConvertDTOToModel(XmlSerializationHelper.DeserializeFromXml(openFileDialog.FileName));
    }

    public void ExportPlanCommand()
    {
        if (SelectedItemPlan == null)
            return;

        var openFolderDialog = new OpenFolderDialog
        {
            Title = "Выберите папку для сохранения экспорта"
        };

        if (openFolderDialog.ShowDialog() == true)
        {
            string selectedPath = openFolderDialog.FolderName;
            string fileName = $"{SelectedItemPlan.PlanName}.txt";
            string fullPath = Path.Combine(selectedPath, fileName);
            TxtExportHelper.ExportToTxt(ConvertModelToDTO(), fullPath);
        }
    }
    #endregion

    /// <summary>
    /// Подготовка данных для хранения файлов
    /// </summary>
    private DataCollection ConvertModelToDTO()
    {
        DataCollection dataCollection = new();

        //Данные о плане
        Plans plan = _dataService.PlansList.First(x => x.Plan_Id == SelectedItemPlan.PlanId);
        dataCollection.Plan = new PlanDTO
        {
            PlanId = plan.Plan_Id,
            PlanName = plan.Plan_Name,
            DataCreate = plan.Date_Create,
            DataUpdate = plan.Date_Update,
            IsSynchro = plan.Is_Synchro
        };

        // Данные об операциях
        foreach (var oper in _dataService.OperationsList.Where(x => x.Oper_Plan_Id == SelectedItemPlan.PlanId))
            dataCollection.Operations.Add(new OperationDTO
            {
                OperId = oper.Oper_Id,
                OperPlanId = oper.Oper_Plan_Id,
                OperName = oper.Oper_Name,
                OperType = oper.Type_Name,
                OperCategory = oper.Category_Name,
                OperSum = oper.Oper_Sum,
                OperCompleted = oper.Oper_Completed,
                OperNextDate = oper.Oper_Next_Date
            });

        // Данные о фиксациях
        foreach (var fix in _dataService.FixationsList.Where(x => x.Fix_Plan_Id == SelectedItemPlan.PlanId))
            dataCollection.Fixations.Add(new FixationDTO
            {
                FixId = fix.Fix_Id,
                FixPlanId = fix.Fix_Plan_Id,
                FixName = fix.Fix_Name,
                FixType = fix.Type_Name,
                FixCategory = fix.Category_Name,
                FixSum = fix.Fix_Sum,
                FixCompleted = fix.Fix_Completed,
                FixNextDate = fix.Fix_Next_Date
            });

        // Данные об отметках
        foreach (var mark in _dataService.MarksList.Where(x => x.Mark_Plan_Id == SelectedItemPlan.PlanId))
            dataCollection.Marks.Add(new MarkDTO
            {
                MarkId = mark.Mark_Id,
                MarkName = mark.Mark_Name,
                MarkDate = mark.Mark_Date,
                MarkPlanId = mark.Mark_Plan_Id
            });

        // Далее будет конвертация данных о состояниях, т.к. мы можем работать в offline-режиме и в дальнейшем эти данные будут нужны для синхронизации, как будет сеть
        // Данные о состоянии плана
        dataCollection.PlanConditionPairs = new KeyValuePairDTO {Key = SelectedItemPlan.PlanId, 
                                                                 Value = _dataService.PlanCondition.Where(x => x.Key == SelectedItemPlan.PlanId)
                                                                                                   .Select(x => x.Value)
                                                                                                   .FirstOrDefault() };

        // Данные о состоянии операций
        foreach (var cond in _dataService.OperCondition)
        {
            if(_dataService.OperationsList.Where(x => x.Oper_Id == cond.Key && x.Oper_Plan_Id == SelectedItemPlan.PlanId).Any())
                dataCollection.OperConditionPairs.Add(new KeyValuePairDTO {Key = cond.Key, Value = cond.Value});
        }

        // Данные о состоянии фиксаций
        foreach (var cond in _dataService.FixCondition)
        {
            if (_dataService.FixationsList.Where(x => x.Fix_Id == cond.Key && x.Fix_Plan_Id == SelectedItemPlan.PlanId).Any())
                dataCollection.FixConditionPairs.Add(new KeyValuePairDTO { Key = cond.Key, Value = cond.Value });
        }

        // Данные о состоянии отметок
        foreach (var cond in _dataService.MarkCondition)
        {
            if (_dataService.MarksList.Where(x => x.Mark_Id == cond.Key && x.Mark_Plan_Id == SelectedItemPlan.PlanId).Any())
                dataCollection.MarkConditionPairs.Add(new KeyValuePairDTO { Key = cond.Key, Value = cond.Value });
        }

        return dataCollection;
    }

    /// <summary>
    /// Обработка данных из файла
    /// </summary>
    private void ConvertDTOToModel(DataCollection data)
    {
        
    }
}
