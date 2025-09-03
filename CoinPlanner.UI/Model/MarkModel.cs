using CommunityToolkit.Mvvm.ComponentModel;

namespace CoinPlanner.UI.Model;

public class MarkModel : ObservableObject
{
    /// <summary>
    /// Id отметок
    /// </summary>
    public Guid MarkId
    {
        get => _markId;
        set => SetProperty(ref _markId, value, nameof(MarkId));
    }
    private Guid _markId;

    /// <summary>
    /// Текст отметки
    /// </summary>
    public string? MarkName
    {
        get => _markName;
        set => SetProperty(ref _markName, value, nameof(MarkName));
    }
    private string? _markName;

    /// <summary>
    /// Дата отметки
    /// </summary>
    public DateTime MarkDate
    {
        get => _markDate;
        set => SetProperty(ref _markDate, value, nameof(MarkDate));
    }
    private DateTime _markDate;

    /// <summary>
    /// План Id отметки
    /// </summary>
    public Guid MarkPlanId
    {
        get => _markPlanId;
        set => SetProperty(ref _markPlanId, value, nameof(MarkPlanId));
    }
    private Guid _markPlanId;
}
