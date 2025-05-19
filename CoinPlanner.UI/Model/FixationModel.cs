using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CoinPlanner.UI.Model;

public class FixationModel : ObservableObject
{
    /// <summary>
    /// Номер операции
    /// </summary>
    public int FixId
    {
        get => _fixId;
        set => SetProperty(ref _fixId, value, nameof(FixId));
    }
    private int _fixId;

    /// <summary>
    /// Наименование
    /// </summary>
    public string FixName
    {
        get => _fixName;
        set => SetProperty(ref _fixName, value, nameof(FixName));
    }
    private string _fixName;

    /// <summary>
    /// Вид операции
    /// </summary>
    public string FixType
    {
        get => _fixType;
        set => SetProperty(ref _fixType, value, nameof(FixType));
    }
    private string _fixType;

    /// <summary>
    /// Вид операции
    /// </summary>
    public string FixCategory
    {
        get => _fixCategory;
        set => SetProperty(ref _fixCategory, value, nameof(FixCategory));
    }
    private string _fixCategory;

    /// <summary>
    /// Сумма
    /// </summary>
    public double FixSum
    {
        get => _fixSum;
        set => SetProperty(ref _fixSum, value, nameof(FixSum));
    }
    private double _fixSum;

    /// <summary>
    /// Выполнен
    /// </summary>
    public string? FixCompleted
    {
        get => _fixCompleted;
        set => SetProperty(ref _fixCompleted, value, nameof(FixCompleted));
    }
    private string? _fixCompleted;

    /// <summary>
    /// Время
    /// </summary>
    public string? FixNextDate
    {
        get => _fixNextDate;
        set => SetProperty(ref _fixNextDate, value, nameof(FixNextDate));
    }
    private string? _fixNextDate;

    /// <summary>
    /// Номер плана
    /// </summary>
    public int FixPlanId { get; set; }
}
