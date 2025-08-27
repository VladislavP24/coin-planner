using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CoinPlanner.DataBase;
using CoinPlanner.DataBase.ModelsDb;
using CoinPlanner.LogService;
using CoinPlanner.UI.Interface;
using CoinPlanner.UI.Model;
using CoinPlanner.UI.View.Dialogs;
using CoinPlanner.UI.ViewModel.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CoinPlanner.UI.ViewModel.Dialogs;

public class MarkDialogsViewModel : ObservableObject, IViewModelDialogs
{
    public MarkDialogsViewModel(DataService dataService, CalendarViewModel calendarViewModel, PanelViewModel panelViewModel)
    {
        _calendarViewModel = calendarViewModel;
        _dataService = dataService;
        _panelViewModel = panelViewModel;

        foreach (var item in _dataService.MarksList.Where(x => x.Mark_Plan_Id == _panelViewModel.SelectedItemPlan.PlanId))
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

    private DataService _dataService { get; }
    private CalendarViewModel _calendarViewModel { get; }
    private PanelViewModel _panelViewModel { get; }


    public ICommand Ok { get; set; }
    public ICommand Cancel { get; set; }
    public ICommand AddItem { get; set; }
    public ICommand DeleteItem { get; set; }

    public ObservableCollection<MarkModel> Items { get; set; } = new();
    private const string logSender = "Mark";

    private void AddItemCommand()
    {
        Guid guid = Guid.NewGuid();

        _dataService.MarkCondition.Add(guid, 1);
        Items.Add(new MarkModel()
        {
            MarkId = guid,
            MarkName = " ",
            MarkDate = DateTime.Now,
            MarkPlanId = _panelViewModel.SelectedItemPlan.PlanId
        });

        Log.Send(EventLevel.Info, logSender, "Добавлена новая отметка");
    }

    private void DeleteItemCommand(MarkModel markModel)
    {
        if (_dataService.MarkCondition.Any(x => x.Key == markModel.MarkId && x.Value == 1))
        {
            _dataService.MarkCondition.Remove(markModel.MarkId);
            _dataService.MarksList.RemoveAll(x => x.Mark_Id == markModel.MarkId);
            Items.Remove(markModel);
        }
        else
        {
            _dataService.MarksList.RemoveAll(x => x.Mark_Id == markModel.MarkId);
            _dataService.MarkCondition.Add(markModel.MarkId, 3);
        }

        Items.Remove(markModel);
        Log.Send(EventLevel.Info, logSender, $"Удалена ометка : {markModel.MarkName}");
    }

    public void OkCommand(Window window)
    {
        foreach (MarkModel mark in Items)
            SaveMarks(mark);

        _panelViewModel.UpdateDatePlan();
        _calendarViewModel.UpdateButtons();
        window.Close();
    }

    public void CancelCommand(Window window)
    {
        foreach (MarkModel mark in Items)
            SaveMarks(mark);

        Log.Send(EventLevel.Info, logSender, "Окно закрыто");
        window.Close();
    }

    /// <summary>
    /// Сохранение и проверка фиксаций на состояние
    /// </summary>
    private void SaveMarks(MarkModel markModel)
    {
        Log.Send(EventLevel.Info, logSender, "Сохранение отметок и проверка их");
        var newMark = new DataBase.ModelsDb.Marks
        {
            Mark_Id = markModel.MarkId,
            Mark_Name = markModel.MarkName,
            Mark_Date = markModel.MarkDate,
            Mark_Plan_Id = markModel.MarkPlanId
        };

        if (!_dataService.MarksList.Any(x => x.Mark_Id == markModel.MarkId))
            _dataService.MarksList.Add(newMark);
        else if (_dataService.MarksList.Where(x => x.Mark_Id == markModel.MarkId)
                                       .Where(x => x.Mark_Name == markModel.MarkName)
                                       .Where(x => x.Mark_Date == markModel.MarkDate)
                                       .FirstOrDefault() == null)
        {
            _dataService.MarkCondition.Remove(markModel.MarkId);
            _dataService.MarkCondition.Add(markModel.MarkId, 2);
            _dataService.MarksList.Add(newMark);
        }

        Log.Send(EventLevel.Info, logSender, "Отметки сохранены");
    }
}
