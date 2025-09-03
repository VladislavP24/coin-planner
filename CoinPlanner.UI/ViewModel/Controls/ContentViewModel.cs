using System.Collections.ObjectModel;
using CoinPlanner.Contracts.Abstractions.DataBase;
using CoinPlanner.Contracts.Abstractions.ViewModel.Controls;
using CoinPlanner.Contracts.DTO.DataServieDTO;
using CoinPlanner.LogService;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CoinPlanner.UI.ViewModel.Controls;

public class ContentViewModel : ObservableObject, IContentControls
{
    public ContentViewModel(IDataService dataService)
    {
        _dataService = dataService;
    }

    private readonly IDataService _dataService;

    public delegate void EventHandler(object sender, Guid guid);
    public event EventHandler OnCreateDiagram;

    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public PlansDTO? Plan { get; set; }
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
        foreach (var oper in _dataService.GetOperationsList().Where(x => x.Oper_Next_Date >= StartDate && x.Oper_Next_Date <= EndDate)
                                                             .Where(x => x.Oper_Plan_Id == Plan.Plan_Id)
                                                             .Where(x => x.Type_Name == IsType || IsType == "Все операции"))
        {
            DynamicOperationCollection.Add(new OperationsDTO
            {
                Oper_Id_Table = i,
                Oper_Id = oper.Oper_Id,
                Oper_Name = oper.Oper_Name,
                Type_Name = oper.Type_Name,
                Category_Name = oper.Category_Name,
                Oper_Sum = oper.Oper_Sum,
                Oper_Completed = oper.Oper_Completed,
                Oper_Next_Date = oper.Oper_Next_Date,
                Oper_Plan_Id = oper.Oper_Plan_Id
            });

            i++;
        }

        CalculationOfParameters();
        Log.Send(EventLevel.Info, logSender, "Таблицы и статистика обновлены");
    }


    /// <summary>
    /// Коллекция операций
    /// </summary>
    public ObservableCollection<OperationsDTO> DynamicOperationCollection
    {
        get => _dynamicOperationCollection;
        set
        {
            SetProperty(ref _dynamicOperationCollection, value, nameof(DynamicOperationCollection));
            OnCreateDiagram.Invoke(this, Plan.Plan_Id);
        }
    }
    private ObservableCollection<OperationsDTO> _dynamicOperationCollection = new();


    /// <summary>
    /// Выбранная строчка в плане
    /// </summary>
    public OperationsDTO? CurSelectedOperation
    {
        get => _curSelectedOperation;
        set => SetProperty(ref _curSelectedOperation, value, nameof(CurSelectedOperation));
    }
    private OperationsDTO? _curSelectedOperation;


    #region Нижняя панель контента

    /// <summary>
    /// Расчёт всех параметроы
    /// </summary>
    public void CalculationOfParameters()
    {
        SavingsAccountAllTime = _dataService.GetOperationsList().Where(x => x.Oper_Plan_Id == Plan.Plan_Id && x.Category_Name == "Накопления").Sum(x => x.Oper_Sum);
        LoansAllTime = _dataService.GetOperationsList().Where(x => x.Oper_Plan_Id == Plan.Plan_Id && x.Category_Name == "Кредит").Sum(x => x.Oper_Sum);
        EnrollmentsAllTime = _dataService.GetOperationsList().Where(x => x.Oper_Plan_Id == Plan.Plan_Id && x.Type_Name == "Зачисление").Sum(x => x.Oper_Sum);
        PaymentsAllTime = _dataService.GetOperationsList().Where(x => x.Oper_Plan_Id == Plan.Plan_Id && x.Type_Name == "Оплата").Sum(x => x.Oper_Sum);
        RemaindersAllTime = EnrollmentsAllTime - PaymentsAllTime;

        SavingsAccountSelectTime = DynamicOperationCollection.Where(x => x.Category_Name == "Накопления").Sum(x => x.Oper_Sum);
        LoansSelectTime = DynamicOperationCollection.Where(x => x.Category_Name == "Кредит").Sum(x => x.Oper_Sum);
        EnrollmentsSelectTime = DynamicOperationCollection.Where(x => x.Category_Name == "Зачисление").Sum(x => x.Oper_Sum);
        PaymentsSelectTime = DynamicOperationCollection.Where(x => x.Category_Name == "Оплата").Sum(x => x.Oper_Sum);
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
