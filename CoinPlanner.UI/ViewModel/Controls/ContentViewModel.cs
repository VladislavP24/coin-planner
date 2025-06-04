using System.Collections.ObjectModel;
using System.Windows.Input;
using CoinPlanner.DataBase;
using CoinPlanner.LogService;
using CoinPlanner.UI.Model;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CoinPlanner.UI.ViewModel.Controls;

public class ContentViewModel : ObservableObject
{
    public ContentViewModel(DataService dataService, DiagramViewModel diagramViewModel) 
    {
        _dataService = dataService;
        _diagramViewModel = diagramViewModel;
    }

    private DataService _dataService;
    private DiagramViewModel _diagramViewModel;
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public PlanModel? Plan { get; set; }
    public string IsType { get; set; } = "Все операции";
    private const string logSender = "Content/Table";

    public bool IsVisibleContent
    {
        get => _isVisibleContent;
        set => SetProperty(ref _isVisibleContent, value, nameof(IsVisibleContent));
    }
    private bool _isVisibleContent = true;


    /// <summary>
    /// Обноавление данных в таблице в зависимости о параметров
    /// </summary>
    public void UpdateOperation()
    {
        Log.Send(EventLevel.Info, logSender, "Обновление таблицы");
        DynamicOperationCollection.Clear();
        if (StartDate == null || EndDate == null || Plan == null)
            return;

        int i = 1;
        foreach (var oper in _dataService.OperationsList.Where(x => x.Oper_Next_Date >= StartDate
                                                                 && x.Oper_Next_Date <= EndDate)
                                                        .Where(x => x.Oper_Plan_Id == Plan.PlanId)
                                                        .Where(x => x.Type_Name == IsType || IsType == "Все операции"))
        {       
            DynamicOperationCollection.Add(new OperationModel
            {
                OperIdTable = i,
                OperId = oper.Oper_Id,
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

        CalculationOfParameters();
        Log.Send(EventLevel.Info, logSender, "Таблицы и статистика обновлены");
    }


    /// <summary>
    /// Коллекция операций
    /// </summary>
    public ObservableCollection<OperationModel> DynamicOperationCollection
    {
        get => _dynamicOperationCollection;
        set
        {
            SetProperty(ref _dynamicOperationCollection, value, nameof(DynamicOperationCollection));
            _diagramViewModel.CreatDiagram(Plan.PlanId);
        }
    }
    private ObservableCollection<OperationModel> _dynamicOperationCollection = new();


    /// <summary>
    /// Выбранная строчка в плане
    /// </summary>
    public OperationModel? CurSelectedOperation
    {
        get => _curSelectedOperation;
        set => SetProperty(ref _curSelectedOperation, value, nameof(CurSelectedOperation));
    }
    private OperationModel? _curSelectedOperation;


    #region Нижняя панель контента

    /// <summary>
    /// Расчёт всех параметроы
    /// </summary>
    public void CalculationOfParameters()
    {
        SavingsAccountAllTime = _dataService.OperationsList.Where(x => x.Oper_Plan_Id == Plan.PlanId && x.Category_Name == "Накопления").Sum(x => x.Oper_Sum);
        LoansAllTime = _dataService.OperationsList.Where(x => x.Oper_Plan_Id == Plan.PlanId && x.Category_Name == "Кредит").Sum(x => x.Oper_Sum);
        EnrollmentsAllTime = _dataService.OperationsList.Where(x => x.Oper_Plan_Id == Plan.PlanId && x.Type_Name == "Зачисление").Sum(x => x.Oper_Sum);
        PaymentsAllTime = _dataService.OperationsList.Where(x => x.Oper_Plan_Id == Plan.PlanId && x.Type_Name == "Оплата").Sum(x => x.Oper_Sum);
        RemaindersAllTime = EnrollmentsAllTime - PaymentsAllTime;

        SavingsAccountSelectTime = DynamicOperationCollection.Where(x => x.OperCategory == "Накопления").Sum(x => x.OperSum);
        LoansSelectTime = DynamicOperationCollection.Where(x => x.OperCategory == "Кредит").Sum(x => x.OperSum);
        EnrollmentsSelectTime = DynamicOperationCollection.Where(x => x.OperType == "Зачисление").Sum(x => x.OperSum);
        PaymentsSelectTime = DynamicOperationCollection.Where(x => x.OperType == "Оплата").Sum(x => x.OperSum);
        RemaindersSelectTime = EnrollmentsSelectTime - PaymentsSelectTime;
    }


    // Накопительные счета
    public double SavingsAccountAllTime
    {
        get => _savingsAccountAllTime;
        set => SetProperty(ref _savingsAccountAllTime, value, nameof(SavingsAccountAllTime));
    }
    private double _savingsAccountAllTime = 0;

    public double SavingsAccountSelectTime
    {
        get => _savingsAccountSelectTime;
        set => SetProperty(ref _savingsAccountSelectTime, value, nameof(SavingsAccountSelectTime));
    }
    private double _savingsAccountSelectTime = 0;

    // Кредиты
    public double LoansAllTime
    {
        get => _loansAllTime;
        set => SetProperty(ref _loansAllTime, value, nameof(LoansAllTime));
    }
    private double _loansAllTime = 0;

    public double LoansSelectTime
    {
        get => _loansSelectTime;
        set => SetProperty(ref _loansSelectTime, value, nameof(LoansSelectTime));
    }
    private double _loansSelectTime = 0;

    // Зачисления
    public double EnrollmentsAllTime
    {
        get => _enrollmentsAllTime;
        set => SetProperty(ref _enrollmentsAllTime, value, nameof(EnrollmentsAllTime));
    }
    private double _enrollmentsAllTime = 0;

    public double EnrollmentsSelectTime
    {
        get => _enrollmentsSelectTime;
        set => SetProperty(ref _enrollmentsSelectTime, value, nameof(EnrollmentsSelectTime));
    }
    private double _enrollmentsSelectTime = 0;

    // Оплата
    public double PaymentsAllTime
    {
        get => _paymentsAllTime;
        set => SetProperty(ref _paymentsAllTime, value, nameof(PaymentsAllTime));
    }
    private double _paymentsAllTime = 0;

    public double PaymentsSelectTime
    {
        get => _paymentsSelectTime;
        set => SetProperty(ref _paymentsSelectTime, value, nameof(PaymentsSelectTime));
    }
    private double _paymentsSelectTime = 0;

    // Остаток
    public double RemaindersAllTime
    {
        get => _remaindersAllTime;
        set => SetProperty(ref _remaindersAllTime, value, nameof(RemaindersAllTime));
    }
    private double _remaindersAllTime = 0;

    public double RemaindersSelectTime
    {
        get => _remaindersSelectTime;
        set => SetProperty(ref _remaindersSelectTime, value, nameof(RemaindersSelectTime));
    }
    private double _remaindersSelectTime = 0;
    #endregion
}
