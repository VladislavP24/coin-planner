using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CoinPlanner.DataBase;
using CoinPlanner.DataBase.ModelsDb;
using CoinPlanner.UI.Model;
using CoinPlanner.UI.View.Dialogs;
using CoinPlanner.UI.ViewModel.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CoinPlanner.UI.ViewModel.Dialogs;

public class MarkDialogsViewModel : ObservableObject
{
    public MarkDialogsViewModel(MarkDialogs markDialogs, DataService dataService, CalendarViewModel calendarViewModel, PanelViewModel panelViewModel)
    {
        _calendarViewModel = calendarViewModel;
        _markDialogs = markDialogs;
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

        Ok = new RelayCommand(OkCommand);
        Cancel = new RelayCommand(CancelCommand);
        AddItem = new RelayCommand(AddItemCommand);
        DeleteItem = new RelayCommand<MarkModel>(DeleteItemCommand);
    }

    private MarkDialogs _markDialogs { get; }
    private DataService _dataService { get; }
    private CalendarViewModel _calendarViewModel { get; }
    private PanelViewModel _panelViewModel { get; }


    public ICommand Ok { get; set; }
    public ICommand Cancel { get; set; }
    public ICommand AddItem { get; set; }
    public ICommand DeleteItem { get; set; }

    public ObservableCollection<MarkModel> Items { get; set; } = new();
    private IList<int> usedIdList = new List<int>();

    private void AddItemCommand()
    {
        int id = GetMarkFirstFreeID(usedIdList);
        usedIdList.Add(id);

        if (_dataService.MarkCondition.Any(x => x.Key == id && x.Value == 3))
            _dataService.MarkCondition.Remove(id);

        _dataService.MarkCondition.Add(id, 1);
        Items.Add(new MarkModel()
        {
            MarkId = id,
            MarkName = " ",
            MarkDate = DateTime.Now,
            MarkPlanId = _panelViewModel.SelectedItemPlan.PlanId
        });
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
    }

    private void OkCommand()
    {
        foreach (MarkModel mark in Items)
            SaveMarks(mark);

        _panelViewModel.UpdateDatePlan();
        _calendarViewModel.UpdateButtons();
        _markDialogs.Close();
    }

    private void CancelCommand()
    {
        foreach (MarkModel mark in Items)
            SaveMarks(mark);

        _markDialogs.Close();
    }


    /// <summary>
    /// Получение первого свободного ID из отметок
    /// </summary>
    private int GetMarkFirstFreeID(in IList<int> usedIdList)
    {
        int result = 1;

        for (int i = 0; i < _dataService.MarksList.Count; i++)
        {
            result = _dataService.MarksList[i].Mark_Id + 1;
            if (!_dataService.MarksList.Any(x => x.Mark_Id == result) && !usedIdList.Contains(result))
                return result;
        }

        while (usedIdList.Contains(result))
            ++result;

        return result;
    }


    /// <summary>
    /// Сохранение и проверка фиксаций на состояние
    /// </summary>
    private void SaveMarks(MarkModel markModel)
    {
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
    }
}
