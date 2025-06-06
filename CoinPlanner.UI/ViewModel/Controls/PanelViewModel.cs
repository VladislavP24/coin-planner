﻿using System;
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
using CoinPlanner.DataBase.ModelsDb;
using CoinPlanner.DataBase.ModelsDB;
using CoinPlanner.FileService;
using CoinPlanner.FileService.DTO;
using CoinPlanner.LogService;
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
        _dataService.OnWarning += Choice_OnWarning;

        foreach (var category in _dataService.CategoriesList)
            Categories.Add(category.Category_Id, category.Category_Name);

        PlanUpdate();
    }

    private CalendarViewModel _calendarViewModel { get; set; }
    private ContentViewModel _contentViewModel { get; set; }
    private DiagramViewModel _diagramViewModel { get; set; }
    private DataService _dataService { get; set; }

    private const string logSender = "Panel";

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
    private bool _isCheckedTable = false;

    /// <summary>
    /// Факт включения Диаграммы
    /// </summary>
    public bool IsCheckedDiagram
    {
        get => _isCheckedDiagram;
        set => SetProperty(ref _isCheckedDiagram, value, nameof(IsCheckedDiagram));
    }
    private bool _isCheckedDiagram = true;

    public ObservableCollection<PlanModel> Items { get; set; } = new(); // Элементы комбобокс Планы
    public Dictionary<int, string> Categories { get; set; } = new();

    public PlanModel SelectedItemPlan  // Выбранный элемент из комбобокс Планы
    {
        get => _selectedItemPlan;
        set
        {
            SetProperty(ref _selectedItemPlan, value, nameof(SelectedItemPlan));
            if (value != null)
            {
                Log.Send(EventLevel.Info, logSender, "Отправка данных плана в Content/Table, Diagram, Calendar");
                _contentViewModel.Plan = value;
                _calendarViewModel.PlanId = value.PlanId;
                _diagramViewModel.CreatDiagram(SelectedItemPlan.PlanId);
                _contentViewModel.UpdateOperation();
            } 
        }
    }
    private PlanModel _selectedItemPlan;


    public void PlanUpdate()
    {
        Log.Send(EventLevel.Info, logSender, "Обновление планов");
        Items.Clear();
        foreach (var plan in _dataService.PlansList)
        {
            Items.Add(new PlanModel()
            {
                PlanId = plan.Plan_Id,
                PlanName = plan.Plan_Name,
                DateCreate = plan.Date_Create,
                DataUpdate = plan.Date_Update
            });
        }
        Log.Send(EventLevel.Info, logSender, "Обновление завершено");
    }

    public void UpdateDatePlan()
    {
        Log.Send(EventLevel.Info, logSender, "Обновление даты последнего изменения плана");
        var plan = _dataService.PlansList.Where(x => x.Plan_Id == SelectedItemPlan.PlanId).First();
        plan.Date_Update = DateTime.Now;

        if (_dataService.PlanCondition.Where(x => x.Key == plan.Plan_Id && x.Value == 1) == null)
        {
            _dataService.PlanCondition.Remove(plan.Plan_Id);
            _dataService.PlanCondition.Add(plan.Plan_Id, 2);
        }        
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
    public ICommand DownloadPlansDB { get; set; }
    public ICommand Synchronization { get; set; }

    public ICommand Interval { get; set; }
    public ICommand Type { get; set; }
    public ICommand Mark { get; set; }

    public ICommand Info { get; set; }

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
        DownloadPlansDB = new AsyncRelayCommand(DownloadPlansDBCommandAsync);
        Fixation = new RelayCommand(FixationCommand);
        Mark = new RelayCommand(MarkCommand);
        SavePlan = new RelayCommand(SavePlanCommand);
        OpenPlan = new RelayCommand(OpenPlanCommand);
        ExportPlan = new RelayCommand(ExportPlanCommand);
        Info = new RelayCommand(InfoCommand);
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
        if (SelectedItemPlan == null)
            return;
        
        if(_dataService.ExistsById(SelectedItemPlan.PlanId))
        {
            MessageBoxResult result = MessageBox.Show("Данный план с таким же ID уже есть в базе данных. Перезаписать план?\nЕсли нет, то синхронизируется, как новый!", 
                                                      "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.No)
            {
                Guid newGuid = Guid.NewGuid();

                _dataService.PlansList.First(x => x.Plan_Id == SelectedItemPlan.PlanId).Plan_Id = newGuid;

                foreach(var oper in _dataService.OperationsList.Where(x => x.Oper_Plan_Id == SelectedItemPlan.PlanId))
                    oper.Oper_Plan_Id = newGuid;

                foreach (var mark in _dataService.MarksList.Where(x => x.Mark_Plan_Id == SelectedItemPlan.PlanId))
                    mark.Mark_Plan_Id = newGuid;

                foreach (var fix in _dataService.FixationsList.Where(x => x.Fix_Plan_Id == SelectedItemPlan.PlanId))
                    fix.Fix_Plan_Id = newGuid;

                UpdateKeyInDictionary(_dataService.PlanCondition, SelectedItemPlan.PlanId, newGuid);
                UpdateKeyInDictionary(_dataService.OperCondition, SelectedItemPlan.PlanId, newGuid);
                UpdateKeyInDictionary(_dataService.MarkCondition, SelectedItemPlan.PlanId, newGuid);
                UpdateKeyInDictionary(_dataService.FixCondition, SelectedItemPlan.PlanId, newGuid);
            }
        }            

        if (_dataService.SaveDataToDatabaseAsync(SelectedItemPlan.PlanId))
            MessageBox.Show("Синхронизация данных прошла успешно!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        else
            MessageBox.Show("Не удалось провести синхронизацию данных. Проверьте подключение к БД.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    public void SortCommand()
    {
        if (IsCheckedEnroll && !IsCheckedExpenses)
            _contentViewModel.IsType = "Зачисление";
        else if (!IsCheckedEnroll && IsCheckedExpenses)
            _contentViewModel.IsType = "Оплата";
        else
            _contentViewModel.IsType = "Все операции";

        Log.Send(EventLevel.Info, logSender, $"Сортировка таблицы по параметру: {_contentViewModel.IsType}");
        _contentViewModel.UpdateOperation();
    }

    public void OpenDiagramCommand()
    {
        Log.Send(EventLevel.Info, logSender, $"Открытие таблицы");
        if (IsCheckedDiagram)
        {
            IsCheckedDiagram = false;
            IsCheckedTable = true;
            _contentViewModel.IsVisibleContent = true;
            _diagramViewModel.IsVisibleDiagram = false; 
        }
        else
            _diagramViewModel.IsVisibleDiagram = false;
    }

    public void OpenTableCommand()
    {
        Log.Send(EventLevel.Info, logSender, $"Открытие диаграммы");
        if (IsCheckedTable)
        {
            IsCheckedTable = false;
            IsCheckedDiagram = true;
            _contentViewModel.IsVisibleContent = false;
            _diagramViewModel.IsVisibleDiagram = true;
        }
        else
            _contentViewModel.IsVisibleContent = false;
    }

    public async Task DownloadPlansDBCommandAsync()
    {
        Log.Send(EventLevel.Info, logSender, $"Запуск выгрузки планов из БД");
        bool isConnected = await _dataService.CheckDatabaseConnectionAsync();

        if (isConnected)
        {
            bool isLoaded = await _dataService.LoadDataFromDatabaseAsync();
            if (!isLoaded)
                MessageBox.Show("Не удалось загрузить данные из базы данных", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {
                MessageBox.Show("Данные успешно выгружены из базы данных!", "Информации", MessageBoxButton.OK, MessageBoxImage.Information);
                PlanUpdate();
                SelectedItemPlan = null;
            }
        }
        else
        {
            MessageBox.Show("Не удалось подключиться к базе данных. Проверьте соединение.",
                            "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }
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
        if (SelectedItemPlan == null)
            return;

        DeletePlanDialogs deletePlanDialogs = new DeletePlanDialogs(this, _dataService);
        deletePlanDialogs.ShowDialog();
    }

    public void RenamePlanCommand()
    {
        if (SelectedItemPlan == null)
            return;

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
            try
            {
                XmlSerializationHelper.SerializeToXml(ConvertModelToDTO(), fullPath);
                
            }
            catch (Exception ex)
            {
                Log.Send(EventLevel.Info, logSender, "Сериализация в XML завершилась с ошибкой.");
                Log.Send(EventLevel.Error, logSender, ex.ToString());
            }   
        }        
    }

    public void OpenPlanCommand()
    {
        var openFileDialog = new OpenFileDialog
        {
            Filter = "XML Files (*.xml)|*.xml"
        };

        try
        {
            if (openFileDialog.ShowDialog() == true)
                ConvertDTOToModel(XmlSerializationHelper.DeserializeFromXml(openFileDialog.FileName));

            Log.Send(EventLevel.Info, logSender, "Десериализация из XML прошла успешно.");
        }
        catch (Exception ex)
        {
            Log.Send(EventLevel.Info, logSender, "Десериализация из XML завершилась с ошибкой.");
            Log.Send(EventLevel.Error, logSender, ex.ToString());
        }        
           
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
            Log.Send(EventLevel.Info, logSender, "Запущена конвертация плана в TXT");
            string selectedPath = openFolderDialog.FolderName;
            string fileName = $"{SelectedItemPlan.PlanName}.txt";
            string fullPath = Path.Combine(selectedPath, fileName);
            TxtExportHelper.ExportToTxt(ConvertModelToDTO(), fullPath);
        }

        Log.Send(EventLevel.Info, logSender, "Конвертация завершена");
    }

    public void InfoCommand()
    {
        if (SelectedItemPlan == null)
            return;

        MessageBox.Show($"ID: {SelectedItemPlan.PlanId}\nИмя плана: {SelectedItemPlan.PlanName}\nДата создания: {SelectedItemPlan.DateCreate}\nДата последнего изменения: {SelectedItemPlan.DataUpdate}",
                        "Информация о плане", MessageBoxButton.OK, MessageBoxImage.Information);

        
    }
    #endregion

    private static void UpdateKeyInDictionary<TKey, TValue>(Dictionary<TKey, TValue> dict, TKey oldKey, TKey newKey)
    {
        if (dict.TryGetValue(oldKey, out var value))
        {
            dict.Remove(oldKey);
            dict[newKey] = value;
        }
    }

    private void Choice_OnWarning(object sender, string message)
    {
        MessageBoxResult result = MessageBox.Show("Данный план, выгруженный из сети, уже имеется в приложении, но их дата изменения разная. " +
                                                  "Для загрузки плана из сети синхронизируйте план из приложения как новый. Затем выгрузите снова. " +
                                                  "Если выберите перезаписать план, то план, находящийся в приложении, перезапишется! Перезаписать план?\n\n" +
                                                  $"{message}", 
                                                  "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

        if (result == MessageBoxResult.Yes)
        {
            _dataService.IsDownLoad = true;
        }
        else
            _dataService.IsDownLoad = false;

        Log.Send(EventLevel.Info, logSender, $"Параметр для перезаписи планов устновлен - {_dataService.IsDownLoad}");
    }

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
            DataUpdate = plan.Date_Update
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
        Log.Send(EventLevel.Info, logSender, "Обработка данных из файла");
        // Проверяем, загружен ли такой же план
        bool planExists = _dataService.PlansList.Any(x => x.Plan_Id == data.Plan.PlanId);

        if (planExists)
        {
            MessageBoxResult result = MessageBox.Show("Данный план с таким же ID уже есть. Перезаписать план?\nЕсли нет, то откроется, как новый!", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                Log.Send(EventLevel.Info, logSender, "Перезапись данных");
                // Находим и удаляем все элементы плана
                var delPlan = _dataService.PlansList.First(x => x.Plan_Id == data.Plan.PlanId);      
                if (_dataService.PlanCondition.Any(x => x.Key == delPlan.Plan_Id && x.Value == 1))
                    _dataService.PlanCondition.Remove(delPlan.Plan_Id);
                else
                {
                    _dataService.PlanCondition.Remove(delPlan.Plan_Id);
                    _dataService.PlanCondition.Add(delPlan.Plan_Id, 2);
                }
                _dataService.PlansList.Remove(delPlan);


                foreach (var plan in _dataService.OperationsList.Where(x => x.Oper_Plan_Id == delPlan.Plan_Id))
                    _dataService.OperationsList.Remove(plan);

                foreach (var mark in _dataService.MarksList.Where(x => x.Mark_Plan_Id == delPlan.Plan_Id))
                    _dataService.MarksList.Remove(mark);

                foreach (var fix in _dataService.FixationsList.Where(x => x.Fix_Plan_Id == delPlan.Plan_Id))
                    _dataService.FixationsList.Remove(fix);
            }
            else
                _dataService.PlanCondition.Add(data.Plan.PlanId, 1);
        }
        else
            _dataService.PlanCondition.Add(data.Plan.PlanId, 1);


        // Добавляем в приложение данные из файла
        // Данные о плане
        _dataService.PlansList.Add(new Plans
        {
            Plan_Id = data.Plan.PlanId,
            Plan_Name = data.Plan.PlanName,
            Date_Create = data.Plan.DataCreate,
            Date_Update = data.Plan.DataUpdate
        });

        // Данные об операциях
        foreach (var oper in data.Operations)
            _dataService.OperationsList.Add(new Operations
            {
                Oper_Id = oper.OperId,
                Oper_Plan_Id = oper.OperPlanId,
                Oper_Name = oper.OperName,
                Type_Name = oper.OperType,
                Category_Name = oper.OperCategory,
                Oper_Sum = oper.OperSum,
                Oper_Completed = oper.OperCompleted,
                Oper_Next_Date = oper.OperNextDate
            });

        // Данные об отметках
        foreach(var mark in data.Marks)
            _dataService.MarksList.Add(new Marks
            {
                Mark_Id = mark.MarkId,
                Mark_Plan_Id = mark.MarkPlanId,
                Mark_Name = mark.MarkName,
                Mark_Date = mark.MarkDate  
            });


        // Данные о фкисациях
        foreach(var fix in data.Fixations)
            _dataService.FixationsList.Add(new Fixations
            {
                Fix_Id = fix.FixId,
                Fix_Plan_Id = fix.FixPlanId,
                Fix_Name = fix.FixName,
                Type_Name = fix.FixType,
                Category_Name = fix.FixCategory,
                Fix_Sum = fix.FixSum,
                Fix_Completed = fix.FixCompleted,
                Fix_Next_Date = fix.FixNextDate              
            });

        foreach(var cond in data.OperConditionPairs)
            _dataService.OperCondition.Add(cond.Key, cond.Value);

        foreach(var cond in data.MarkConditionPairs)
            _dataService.MarkCondition.Add(cond.Key, cond.Value);

        foreach(var cond in data.FixConditionPairs)
            _dataService.FixCondition.Add(cond.Key, cond.Value);

        Log.Send(EventLevel.Info, logSender, "Обработка завершена");
        PlanUpdate();
    }
}
