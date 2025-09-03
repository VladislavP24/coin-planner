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

public class MarkDialogsViewModel : ObservableObject, IViewModelDialogs
{
    public MarkDialogsViewModel(IDataService dataService, ICalendarControls calendar, IPanelControls panel)
    {
        _dataService = dataService;
        _calendar = calendar;
        _panel = panel;

        foreach (var item in _dataService.GetMarkList().Where(x => x.Mark_Plan_Id == _panel.SelectedItemPlan.Plan_Id))
            Items.Add(new MarkModel()
            {
                MarkId = item.Mark_Id,
                MarkName = item.Mark_Name,
                MarkDate = item.Mark_Date,
                MarkPlanId = item.Mark_Plan_Id,
            });

        Ok = new RelayCommand<Window>(OkCommand);
        Cancel = new RelayCommand<Window>(CancelCommand);
        AddItem = new RelayCommand(AddItemCommand);
        DeleteItem = new RelayCommand<MarkModel>(DeleteItemCommand);

        Log.Send(EventLevel.Info, logSender, "Открытие окна");
    }

    private readonly IDataService _dataService;
    private readonly ICalendarControls _calendar;
    private readonly IPanelControls _panel;


    public ICommand Ok { get; set; }
    public ICommand Cancel { get; set; }
    public ICommand AddItem { get; set; }
    public ICommand DeleteItem { get; set; }

    public ObservableCollection<MarkModel> Items { get; set; } = new();
    private const string logSender = "Mark";

    public void AddItemCommand()
    {
        Guid guid = Guid.NewGuid();

        _dataService.MarkCondition.Add(guid, 1);
        Items.Add(new MarkModel()
        {
            MarkId = guid,
            MarkName = " ",
            MarkDate = DateTime.Now,
            MarkPlanId = _panel.SelectedItemPlan.Plan_Id
        });

        Log.Send(EventLevel.Info, logSender, "Добавлена новая отметка");
    }

    private void DeleteItemCommand(MarkModel markModel)
    {
        if (_dataService.MarkCondition.Any(x => x.Key == markModel.MarkId && x.Value == 1))
        {
            _dataService.MarkCondition.Remove(markModel.MarkId);
            _dataService.RemoveAllMarkList(markModel.MarkId);
            Items.Remove(markModel);
        }
        else
        {
            _dataService.RemoveAllMarkList(markModel.MarkId);
            _dataService.MarkCondition.Add(markModel.MarkId, 3);
        }

        Items.Remove(markModel);
        Log.Send(EventLevel.Info, logSender, $"Удалена ометка : {markModel.MarkName}");
    }

    public void OkCommand(object currWindow)
    {
        foreach (MarkModel mark in Items)
            SaveMarks(mark);

        _panel.UpdateDatePlan();
        _calendar.UpdateButtons();

        Window window = currWindow as Window;
        window.Close();
    }

    public void CancelCommand(object currWindow)
    {
        foreach (MarkModel mark in Items)
            SaveMarks(mark);

        Log.Send(EventLevel.Info, logSender, "Окно закрыто");

        Window window = currWindow as Window;
        window.Close();
    }

    /// <summary>
    /// Сохранение и проверка фиксаций на состояние
    /// </summary>
    private void SaveMarks(MarkModel markModel)
    {
        Log.Send(EventLevel.Info, logSender, "Сохранение отметок и проверка их");
        var newMark = new MarksDTO
        {
            Mark_Id = markModel.MarkId,
            Mark_Name = markModel.MarkName,
            Mark_Date = markModel.MarkDate,
            Mark_Plan_Id = markModel.MarkPlanId
        };

        if (!_dataService.GetMarkList().Any(x => x.Mark_Id == markModel.MarkId))
            _dataService.AddMarkList(newMark);
        else if (_dataService.GetMarkList().Where(x => x.Mark_Id == markModel.MarkId)
                                           .Where(x => x.Mark_Name == markModel.MarkName)
                                           .Where(x => x.Mark_Date == markModel.MarkDate)
                                           .FirstOrDefault() == null)
        {
            _dataService.MarkCondition.Remove(markModel.MarkId);
            _dataService.MarkCondition.Add(markModel.MarkId, 2);
            _dataService.AddMarkList(newMark);
        }

        Log.Send(EventLevel.Info, logSender, "Отметки сохранены");
    }
}
