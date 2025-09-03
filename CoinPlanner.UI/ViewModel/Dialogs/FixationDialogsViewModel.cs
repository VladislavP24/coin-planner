using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using CoinPlanner.Contracts.Abstractions.DataBase;
using CoinPlanner.Contracts.Abstractions.ViewModel;
using CoinPlanner.Contracts.Abstractions.ViewModel.Controls;
using CoinPlanner.Contracts.DTO.DataServieDTO;
using CoinPlanner.LogService;
using CoinPlanner.UI.Model;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CoinPlanner.UI.ViewModel.Dialogs;

public class FixationDialogsViewModel : ObservableObject, IViewModelDialogs
{

    public FixationDialogsViewModel(IPanelControls panel, IDataService dataService, IContentControls content)
    {
        _dataService = dataService;
        _content = content;
        _panel = panel;

        foreach (var item in _dataService.GetFixationList().Where(x => x.Fix_Plan_Id == _panel.SelectedItemPlan.Plan_Id))
            Items.Add(new FixationModel()
            {
                FixId = item.Fix_Id,
                FixName = item.Fix_Name,
                FixType = item.Type_Name,
                FixCategory = item.Category_Name,
                FixSum = item.Fix_Sum,
                FixCompleted = item.Fix_Completed,
                FixNextDate = item.Fix_Next_Date,
                FixPlanId = item.Fix_Plan_Id,
                IsCheckFix = false,
            });

        foreach (var category in _panel.Categories)
            CategoryItems.Add(category.Value);

        AddItem = new RelayCommand(AddItemCommand);
        DeleteItem = new RelayCommand<FixationModel>(DeleteItemCommand);
        Ok = new RelayCommand<Window>(OkCommand);
        Cancel = new RelayCommand<Window>(CancelCommand);

        Log.Send(EventLevel.Info, logSender, "Открытие окна");
    }

    private readonly IDataService _dataService;
    private readonly IPanelControls _panel;
    private readonly IContentControls _content;

    private const string logSender = "Fixation";

    public ICommand Ok { get; set; }
    public ICommand Cancel { get; set; }
    public ICommand AddItem { get; set; }
    public ICommand DeleteItem { get; set; }

    public ObservableCollection<FixationModel> Items { get; set; } = new();
    public ObservableCollection<string> TypeItems { get; set; } = new ObservableCollection<string> { "Зачисление", "Оплата" };
    public ObservableCollection<string> CategoryItems { get; set; } = new();

    public void AddItemCommand()
    {
        Guid newGuid = Guid.NewGuid();

        _dataService.FixCondition.Add(newGuid, 1);
        Items.Add(new FixationModel()
        {
            FixId = newGuid,
            FixName = "Новый",
            FixType = "-",
            FixCategory = "-",
            FixSum = 0,
            FixCompleted = false,
            FixNextDate = DateTime.Now,
            FixPlanId = _panel.SelectedItemPlan.Plan_Id,
            IsCheckFix = false
        });

        Log.Send(EventLevel.Info, logSender, "Добавлена новая фиксация");
    }

    public void DeleteItemCommand(FixationModel fixation)
    {
        if (_dataService.FixCondition.Any(x => x.Key == fixation.FixId && x.Value == 1))
        {
            _dataService.FixCondition.Remove(fixation.FixId);
            _dataService.RemoveAllFixationList(fixation.FixId);
            Items.Remove(fixation);
        }
        else
        {
            _dataService.RemoveAllFixationList(fixation.FixId);
            _dataService.FixCondition.Add(fixation.FixId, 3);
        }

        Items.Remove(fixation);
        Log.Send(EventLevel.Info, logSender, $"Удалена фиксация: {fixation.FixName}");
    }

    public void OkCommand(object currWindow)
    {
        foreach (FixationModel fixation in Items)
        {
            SaveFixations(fixation);
            if (fixation.IsCheckFix == true)
                AddDataOperations(fixation);
        }

        _panel.UpdateDatePlan();
        _content.UpdateOperation();

        Window window = currWindow as Window;
        window.Close();
    }

    public void CancelCommand(object currWindow)
    {
        foreach (FixationModel fixation in Items)
            SaveFixations(fixation);

        Log.Send(EventLevel.Info, logSender, "Окно закрыто");

        Window window = currWindow as Window;
        window.Close();
    }


    /// <summary>
    /// Сохранение и проверка фиксаций на состояние
    /// </summary>
    private void SaveFixations(FixationModel fixation)
    {
        Log.Send(EventLevel.Info, logSender, "Сохранение фиксаций и их состояний");

        var newFixation = new Contracts.DTO.DataServieDTO.FixationsDTO
        {
            Fix_Id = fixation.FixId,
            Fix_Name = fixation.FixName,
            Type_Name = fixation.FixType,
            Category_Name = fixation.FixCategory,
            Fix_Sum = fixation.FixSum,
            Fix_Completed = fixation.FixCompleted,
            Fix_Next_Date = fixation.FixNextDate,
            Fix_Plan_Id = fixation.FixPlanId
        };

        if (!_dataService.GetFixationList().Any(x => x.Fix_Id == fixation.FixId))
            _dataService.AddFixationList(newFixation);
        else if (_dataService.GetFixationList().Where(x => x.Fix_Id == fixation.FixId)
                                               .Where(x => x.Fix_Name == fixation.FixName)
                                               .Where(x => x.Type_Name == fixation.FixType)
                                               .Where(x => x.Category_Name == fixation.FixCategory)
                                               .Where(x => x.Fix_Sum == fixation.FixSum)
                                               .Where(x => x.Fix_Completed == fixation.FixCompleted)
                                               .Where(x => x.Fix_Next_Date == fixation.FixNextDate)
                                               .FirstOrDefault() == null)
        {
            _dataService.FixCondition.Remove(fixation.FixId);
            _dataService.FixCondition.Add(fixation.FixId, 2);
            _dataService.AddFixationList(newFixation);
        }

        Log.Send(EventLevel.Info, logSender, "Фиксации сохранены");
    }

    /// <summary>
    /// Добавление операций из фиксаций, выбранные из окна
    /// </summary>
    private void AddDataOperations(FixationModel fixation)
    {
        Guid guid = Guid.NewGuid();

        _dataService.OperCondition.Add(guid, 1);

        _dataService.AddOperationsList(new OperationsDTO
        {
            Oper_Id = guid,
            Oper_Name = fixation.FixName,
            Type_Name = fixation.FixType,
            Category_Name = fixation.FixCategory,
            Oper_Sum = fixation.FixSum,
            Oper_Completed = fixation.FixCompleted,
            Oper_Next_Date = fixation.FixNextDate,
            Oper_Plan_Id = _panel.SelectedItemPlan.Plan_Id,
        });

        Log.Send(EventLevel.Info, logSender, "Добавлены фиксции в операции");
        _panel.UpdateDatePlan();
        _content.UpdateOperation();
    }
}
