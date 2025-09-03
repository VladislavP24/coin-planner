using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using CoinPlanner.Contracts.Abstractions.ViewModel;
using CoinPlanner.Contracts.Abstractions.ViewModel.Controls;
using CoinPlanner.LogService;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CoinPlanner.UI.ViewModel.Dialogs;

public class TypeDialogsViewModel : ObservableObject, IViewModelDialogs
{
    public TypeDialogsViewModel(ICalendarControls calendar)
    {
        Ok = new RelayCommand<Window>(OkCommand);
        Cancel = new RelayCommand<Window>(CancelCommand);
        _calendar = calendar;
        _selectedItem = _calendar.Type;

        Items = new ObservableCollection<string> { "Год", "Месяц", "Неделя", "День", "Интервал" };

        Log.Send(EventLevel.Info, logSender, "Открытие окна");
    }

    private readonly ICalendarControls _calendar;
    public ICommand Ok { get; set; }
    public ICommand Cancel { get; set; }

    public ObservableCollection<string> Items { get; set; }
    private const string logSender = "Type Calendar";

    public string SelectedItem
    {
        get => _selectedItem;
        set => SetProperty(ref _selectedItem, value, nameof(SelectedItem));
    }
    private string _selectedItem;

    public void OkCommand(object currWindow)
    {
        if (SelectedItem == null)
            return;

        Log.Send(EventLevel.Info, logSender, "Тип календаря установлен.");

        _calendar.Type = SelectedItem;
        _calendar.UpdateButtons();
        Window window = currWindow as Window;
        window.Close();
    }

    public void CancelCommand(object currWindow)
    {
        Log.Send(EventLevel.Info, logSender, "Окно закрыто");

        Window window = currWindow as Window;
        window.Close();
        window.Close();
    }
}
