using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;
using CoinPlanner.DataBase;
using CoinPlanner.DataBase.ModelsDb;
using CoinPlanner.UI.Model;
using CoinPlanner.UI.View.Dialogs;
using CoinPlanner.UI.ViewModel.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CoinPlanner.UI.ViewModel.Dialogs;

public class FixationDialogsViewModel : ObservableObject
{

    public FixationDialogsViewModel(FixationDialogs fixationDialogs, PanelViewModel panelViewModel, DataService dataService, ContentViewModel contentViewModel)
    {
        _fixationDialogs = fixationDialogs;
        _panelViewModel = panelViewModel;
        _dataService = dataService;
        _contentViewModel = contentViewModel;

        foreach (var item in _dataService.FixationsList.Where(x => x.Fix_Plan_Id == _panelViewModel.SelectedItemPlan.PlanId))
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

        foreach (var category in _panelViewModel.Categories)
            CategoryItems.Add(category.Value);

        AddItem = new RelayCommand(AddItemCommand);
        DeleteItem = new RelayCommand<FixationModel>(DeleteItemCommand);
        Ok = new RelayCommand(OkCommand);
        Cancel = new RelayCommand(CancelCommand);
    }

    private  FixationDialogs _fixationDialogs { get; }
    private PanelViewModel _panelViewModel { get; }
    private DataService _dataService { get; }
    private ContentViewModel _contentViewModel { get; }

    public ICommand Ok { get; set; }
    public ICommand Cancel { get; set; }
    public ICommand AddItem { get; set; }
    public ICommand DeleteItem { get; set; }

    public ObservableCollection<FixationModel> Items { get; set; } = new();
    public ObservableCollection<string> TypeItems { get; set; } = new ObservableCollection<string> { "Зачисление", "Оплата" };
    public ObservableCollection<string> CategoryItems { get; set; } = new();
    private IList<int> usedIdList = new List<int>();

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
            FixPlanId = _panelViewModel.SelectedItemPlan.PlanId,
            IsCheckFix = false
        });
        
    }

    public void DeleteItemCommand(FixationModel fixation)
    {
        if (_dataService.FixCondition.Any(x => x.Key == fixation.FixId && x.Value == 1))
        {
            _dataService.FixCondition.Remove(fixation.FixId);
            _dataService.FixationsList.RemoveAll(x => x.Fix_Id == fixation.FixId);
            Items.Remove(fixation);
        }            
        else
        {
            _dataService.FixationsList.RemoveAll(x => x.Fix_Id == fixation.FixId);
            _dataService.FixCondition.Add(fixation.FixId, 3);
        }

        Items.Remove(fixation);
    }

    private void OkCommand()
    {
        foreach (FixationModel fixation in Items)
        {
            SaveFixations(fixation);
            if (fixation.IsCheckFix == true)
                AddDataOperations(fixation);
        }

        _panelViewModel.UpdateDatePlan();
        _contentViewModel.UpdateOperation();
        _fixationDialogs.Close();
    }

    private void CancelCommand()
    {
        foreach (FixationModel fixation in Items)
            SaveFixations(fixation);

        _fixationDialogs.Close();
    }


    /// <summary>
    /// Сохранение и проверка фиксаций на состояние
    /// </summary>
    private void SaveFixations(FixationModel fixation)
    {
        var newFixation = new DataBase.ModelsDb.Fixations
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

        if (!_dataService.FixationsList.Any(x => x.Fix_Id == fixation.FixId))
            _dataService.FixationsList.Add(newFixation);
        else if (_dataService.FixationsList.Where(x => x.Fix_Id == fixation.FixId)
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
            _dataService.FixationsList.Add(newFixation);
        }
    }

    /// <summary>
    /// Добавление операций из фиксаций, выбранные из окна
    /// </summary>
    private void AddDataOperations(FixationModel fixation)
    {
        Guid guid = Guid.NewGuid();

        _dataService.OperCondition.Add(guid, 1);

        _dataService.OperationsList.Add(new DataBase.ModelsDB.Operations
        {
            Oper_Id = guid,
            Oper_Name = fixation.FixName,
            Type_Name = fixation.FixType,
            Category_Name = fixation.FixCategory,
            Oper_Sum = fixation.FixSum,
            Oper_Completed = fixation.FixCompleted,
            Oper_Next_Date = fixation.FixNextDate,
            Oper_Plan_Id = _panelViewModel.SelectedItemPlan.PlanId,
        });

        _panelViewModel.UpdateDatePlan();
        _contentViewModel.UpdateOperation();
    }
}
