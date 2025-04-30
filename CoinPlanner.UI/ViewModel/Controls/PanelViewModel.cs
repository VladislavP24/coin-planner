using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    public PanelViewModel(CalendarViewModel calendarViewModel, ContentViewModel contentViewModel, DBProcessing dBProcessing) 
    {
        BindingCommandToButton();
        _calendarViewModel = calendarViewModel;
        _contentViewModel = contentViewModel;
        _dBProcessing = dBProcessing;
        ModelConvert();
    }

    private CalendarViewModel _calendarViewModel { get; set; }
    private ContentViewModel _contentViewModel { get; set; }
    private DBProcessing _dBProcessing { get; set; }

    public ICommand CreateFile { get; set; }
    public ICommand OpenFile { get; set; }
    public ICommand SaveFile { get; set; }

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


    public void ModelConvert()
    {
        foreach (var plan in _dBProcessing.PlansList)
        {
            Items.Add(new PlanModel()
            {
                PlanId = plan.Plan_Id,
                PlanName = plan.Plan_Name,
                DataCreate = plan.Data_Create,
                DataUpdate = plan.Data_Update
            });
        }

        foreach (var category in _dBProcessing.CategoriesList)
            Categories.Add(category.Category_Id, category.Category_Name);
    }


    #region Команды на панели

    public void BindingCommandToButton()
    {
        Interval = new RelayCommand(IntervalCommand);
        Type = new RelayCommand(TypeCommand);
        AddData = new RelayCommand(AddDataCommand);
        DeleteData = new RelayCommand(DeleteDataCommand);
        EditData = new RelayCommand(EditDataCommand);
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
        AddDataDialogs addDataDialogs = new AddDataDialogs(_dBProcessing, this, _contentViewModel);
        addDataDialogs.ShowDialog();
    }

    public void EditDataCommand()
    {
        EditDataDialogs editDataDialogs = new EditDataDialogs(_dBProcessing, _contentViewModel, this);
        editDataDialogs.ShowDialog();
    }

    public void DeleteDataCommand()
    {
        DeleteDataDialogs deleteDataDialogs = new DeleteDataDialogs(_dBProcessing, _contentViewModel, this);
        deleteDataDialogs.ShowDialog();
    }

    #endregion
}
