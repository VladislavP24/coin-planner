using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CoinPlanner.UI.Model;

public class OperationModel : ObservableObject
{
    /// <summary>
    /// Номер операции
    /// </summary>
    public Guid OperId
    {
        get => _operId;
        set => SetProperty(ref _operId, value, nameof(OperId));
    }
    private Guid _operId;

    /// <summary>
    /// Наименование
    /// </summary>
    public string OperName
    {
        get => _operName;
        set => SetProperty(ref _operName, value, nameof(OperName));
    }
    private string _operName;

    /// <summary>
    /// Вид операции
    /// </summary>
    public string OperType
    { 
        get => _operType;
        set => SetProperty(ref _operType, value, nameof(OperType));
    }
    private string _operType;

    /// <summary>
    /// Вид операции
    /// </summary>
    public string OperCategory
    {
        get => _operCategory;
        set => SetProperty(ref _operCategory, value, nameof(OperCategory));
    }
    private string _operCategory;

    /// <summary>
    /// Сумма
    /// </summary>
    public double OperSum
    {
        get => _operSum;
        set => SetProperty(ref _operSum, value, nameof(OperSum));
    }
    private double _operSum;

    /// <summary>
    /// Выполнен
    /// </summary>
    public string? OperCompleted
    {
        get => _operCompleted;
        set => SetProperty(ref _operCompleted, value, nameof(OperCompleted));
    }
    private string? _operCompleted;

    /// <summary>
    /// Время
    /// </summary>
    public string? OperNextDate
    {
        get => _operNextDate;
        set => SetProperty(ref _operNextDate, value, nameof(OperNextDate));
    }
    private string? _operNextDate;

    /// <summary>
    /// Номер плана
    /// </summary>
    public Guid OperPlanId { get; set; }

    /// <summary>
    /// Id в таблице 
    /// </summary>
    public int OperIdTable { get; set; }
}
