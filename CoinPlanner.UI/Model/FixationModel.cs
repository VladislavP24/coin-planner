using CommunityToolkit.Mvvm.ComponentModel;

namespace CoinPlanner.UI.Model;

public class FixationModel : ObservableObject
{
    /// <summary>
    /// Номер операции
    /// </summary>
    public Guid FixId
    {
        get => _fixId;
        set => SetProperty(ref _fixId, value, nameof(FixId));
    }
    private Guid _fixId;

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
    public bool FixCompleted
    {
        get => _fixCompleted;
        set => SetProperty(ref _fixCompleted, value, nameof(FixCompleted));
    }
    private bool _fixCompleted;

    /// <summary>
    /// Время
    /// </summary>
    public DateTime FixNextDate
    {
        get => _fixNextDate;
        set => SetProperty(ref _fixNextDate, value, nameof(FixNextDate));
    }
    private DateTime _fixNextDate;

    /// <summary>
    /// Номер плана
    /// </summary>
    public Guid FixPlanId { get; set; }

    /// <summary>
    /// Используется ли для плана
    /// </summary>
    public bool IsCheckFix
    {
        get => _isCheckFix;
        set => SetProperty(ref _isCheckFix, value, nameof(IsCheckFix));
    }
    private bool _isCheckFix;
}
