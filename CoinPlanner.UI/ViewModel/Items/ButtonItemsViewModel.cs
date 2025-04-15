using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CoinPlanner.UI.ViewModel.Items;

public class ButtonItemsViewModel : ObservableObject
{
    public ButtonItemsViewModel() { }

    public ButtonItemsViewModel(string content, DateTime startTime, DateTime endTime, bool isChecked = false)
    {
        _content = content;
        _startTime = startTime;
        _endTime = endTime;
        _isChecked = isChecked;
    }

    private bool _isChecked;
    /// <summary>
    /// Факт выбора
    /// </summary>
    public bool IsChecked
    {
        get => _isChecked;
        set => SetProperty(ref _isChecked, value, nameof(IsChecked));
    }


    private string _content;
    /// <summary>
    /// Элемент, который подлежит выбору
    /// </summary>
    public string Content
    {
        get => _content;
        set => SetProperty(ref _content, value, nameof(Content));
    }

    private DateTime _startTime;
    /// <summary>
    /// Начальная дата в формате DateTime
    /// </summary>
    public DateTime StartTime
    {
        get => _startTime;
        set => SetProperty(ref _startTime, value, nameof(StartTime));
    }

    private DateTime? _endTime;
    /// <summary>
    /// Начальная дата в формате DateTime
    /// </summary>
    public DateTime? EndTime
    {
        get => _endTime;
        set => SetProperty(ref _endTime, value, nameof(EndTime));
    }
}
