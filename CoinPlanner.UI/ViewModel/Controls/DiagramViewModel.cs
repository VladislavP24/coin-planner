using System.Collections.ObjectModel;
using System.Numerics;
using System.Windows.Input;
using CoinPlanner.DataBase;
using CoinPlanner.DataBase.ModelsDB;
using CoinPlanner.UI.Model;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiveCharts;
using LiveCharts.Wpf;

namespace CoinPlanner.UI.ViewModel.Controls;

public class DiagramViewModel : ObservableObject
{
    public DiagramViewModel(DataService dataService)
    {
        _dataService = dataService;

        AllTime = new RelayCommand(AllTimeCommand);
        SelectTime = new RelayCommand(SelectTimeCommand);
    }

    private DataService _dataService { get; set; }
    public ICommand AllTime { get; set; }
    public ICommand SelectTime { get; set; }
    private int selectedPlanId;
    public DateTime? Start { get; set; }
    public DateTime? End { get; set; }

    public bool IsVisibleDiagram
    {
        get => _isVisibleDiagram;
        set => SetProperty(ref _isVisibleDiagram, value, nameof(IsVisibleDiagram));
    }
    private bool _isVisibleDiagram = false;


    public SeriesCollection PieSeriesExpenses
    {
        get => _pieSeriesExpenses;
        set => SetProperty(ref _pieSeriesExpenses, value, nameof(PieSeriesExpenses));
    }
    private SeriesCollection _pieSeriesExpenses;


    public SeriesCollection PieSeriesEnrollment
    {
        get => _pieSeriesEnrollment;
        set => SetProperty(ref _pieSeriesEnrollment, value, nameof(PieSeriesEnrollment));
    }
    private SeriesCollection _pieSeriesEnrollment;


    public bool IsAllTime
    {
        get => _isAllTime;
        set => SetProperty(ref _isAllTime, value, nameof(IsAllTime));
    }
    private bool _isAllTime;


    public bool IsSelectTime
    {
        get => _isSelectTime;
        set => SetProperty(ref _isSelectTime, value, nameof(IsSelectTime));
    }
    private bool _isSelectTime;


    /// <summary>
    /// Создание диаграммы
    /// </summary>
    public void CreatDiagram(int planId)
    {
        List<Operations> operations = new();
        selectedPlanId = planId;

        if (IsSelectTime)
            operations = _dataService.OperationsList.Where(x => x.Oper_Next_Date >= Start && x.Oper_Next_Date <= End).ToList();
        else
            operations = _dataService.OperationsList;


            PieSeriesExpenses = new SeriesCollection();

        foreach (var item in operations.Where(x => x.Oper_Plan_Id == selectedPlanId && x.Type_Name == "Оплата")
                                                        .GroupBy(x => x.Category_Name)
                                                        .Select(x => new { Category = x.Key, Total = x.Sum(x => x.Oper_Sum) })
                                                        .ToList())
        {
            PieSeriesExpenses.Add(new PieSeries
            {
                Title = item.Category,
                Values = new ChartValues<double> { item.Total },
                DataLabels = true
            });
        }

        PieSeriesEnrollment = new SeriesCollection();

        foreach (var item in operations.Where(x => x.Oper_Plan_Id == selectedPlanId && x.Type_Name == "Зачисление")
                                                        .GroupBy(x => x.Category_Name)
                                                        .Select(x => new { Category = x.Key, Total = x.Sum(x => x.Oper_Sum) })
                                                        .ToList())
        {
            PieSeriesEnrollment.Add(new PieSeries
            {
                Title = item.Category,
                Values = new ChartValues<double> { item.Total },
                DataLabels = true
            });
        }
    }


    public void AllTimeCommand()
    {
        if(IsAllTime)
        {
            IsAllTime = true;
            IsSelectTime = false;            
        }
        else
        {
            IsAllTime = false;
            IsSelectTime = true;
        }

        CreatDiagram(selectedPlanId);
    }

    public void SelectTimeCommand()
    {
        if (IsSelectTime)
        {
            IsAllTime = false;
            IsSelectTime = true;
        }
        else
        {
            IsAllTime = true;
            IsSelectTime = false;
        }

        CreatDiagram(selectedPlanId);
    }

}
