using System.Collections.ObjectModel;
using System.Windows.Input;
using CoinPlanner.DataBase;
using CoinPlanner.UI.Model;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CoinPlanner.UI.ViewModel.Controls;

public class ContentViewModel : ObservableObject
{
    public ContentViewModel(DataService dataService) 
    {
        _dataService = dataService;
    }

    private DataService _dataService;
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public PlanModel? Plan { get; set; }

    /// <summary>
    /// Обноавление данных в таблице в зависимости о параметров
    /// </summary>
    public void UpdateOperation()
    {
        DynamicOperationCollection.Clear();
        if (StartDate == null || EndDate == null || Plan == null)
            return;

        int i = 1;
        foreach (var oper in _dataService.OperationsList.Where(x => x.Oper_Next_Date >= StartDate
                                                                  && x.Oper_Next_Date <= EndDate)
                                                         .Where(x => x.Oper_Plan_Id == Plan.PlanId))
        {       
            DynamicOperationCollection.Add(new OperationModel
            {
                OperId = i,
                OperName = oper.Oper_Name,
                OperType = oper.Type_Name,
                OperCategory = oper.Category_Name,
                OperSum = oper.Oper_Sum,
                OperCompleted = oper.Oper_Completed == true ? "Да" : "Нет",
                OperNextDate = oper.Oper_Next_Date.ToString("HH:mm  dd-MM-yyyy 'г.'"),
                OperPlanId = oper.Oper_Plan_Id
            });

            i++;
        }
    }


    /// <summary>
    /// Коллекция записной книжки
    /// </summary>
    public ObservableCollection<OperationModel> DynamicOperationCollection
    {
        get => _dynamicOperationCollection;
        set => SetProperty(ref _dynamicOperationCollection, value, nameof(DynamicOperationCollection));
    }
    private ObservableCollection<OperationModel> _dynamicOperationCollection = new();


    /// <summary>
    /// Выбранная строчка в записной книжке
    /// </summary>
    public OperationModel? CurSelectedOperation
    {
        get => _curSelectedOperation;
        set => SetProperty(ref _curSelectedOperation, value, nameof(CurSelectedOperation));
    }
    private OperationModel? _curSelectedOperation;
}
