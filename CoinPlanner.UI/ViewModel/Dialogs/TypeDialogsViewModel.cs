using CoinPlanner.LogService;
using CoinPlanner.UI.Interface;
using CoinPlanner.UI.View.Dialogs;
using CoinPlanner.UI.ViewModel.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace CoinPlanner.UI.ViewModel.Dialogs;

public class TypeDialogsViewModel : ObservableObject, IViewModelDialogs
{
    public TypeDialogsViewModel(CalendarViewModel calendarViewModel)
    {
        Ok = new RelayCommand<Window>(OkCommand);
        Cancel = new RelayCommand<Window>(CancelCommand);
        _calendarViewModel = calendarViewModel;
        _selectedItem = calendarViewModel.Type;

        Items = new ObservableCollection<string> { "Год", "Месяц", "Неделя", "День", "Интервал" };

        Log.Send(EventLevel.Info, logSender, "Открытие окна");
    }

    private CalendarViewModel _calendarViewModel { get; }
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

    public void OkCommand(Window window)
    {
        if (SelectedItem == null)
            return;

        Log.Send(EventLevel.Info, logSender, "Тип календаря установлен.");

        _calendarViewModel.Type = SelectedItem;
        _calendarViewModel.UpdateButtons();
        window.Close();
    }

    public void CancelCommand(Window window)
    {
        Log.Send(EventLevel.Info, logSender, "Окно закрыто");
        window.Close();
    }
}
