using System.Windows.Input;
using CoinPlanner.DataBase;
using CoinPlanner.DataBase.ModelsDB;
using CoinPlanner.UI.Model;
using CommunityToolkit.Mvvm.ComponentModel;
using LiveCharts;
using LiveCharts.Wpf;

namespace CoinPlanner.UI.ViewModel.Controls;

public class DiagramViewModel : ObservableObject
{
    public DiagramViewModel(DataService dataService)
    {
        _dataService = dataService;
    }

    private DataService _dataService { get; set; }
    public ICommand AllTime { get; set; }
    public ICommand SelectTime { get; set; }

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

    /// <summary>
    /// Создание диаграммы
    /// </summary>
    public void CreatDiagram(int planId)
    {

        PieSeriesExpenses = new SeriesCollection();

        foreach (var item in _dataService.OperationsList.Where(x => x.Oper_Plan_Id == planId && x.Type_Name == "Оплата")
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

        foreach (var item in _dataService.OperationsList.Where(x => x.Oper_Plan_Id == planId && x.Type_Name == "Зачисление")
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
}
