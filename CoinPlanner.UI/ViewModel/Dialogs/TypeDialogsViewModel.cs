using CoinPlanner.LogService;
using CoinPlanner.UI.View.Dialogs;
using CoinPlanner.UI.ViewModel.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CoinPlanner.UI.ViewModel.Dialogs;

public class TypeDialogsViewModel : ObservableObject
{
    public TypeDialogsViewModel(TypeDialogs typeDialogs, CalendarViewModel calendarViewModel)
    {
        Ok = new RelayCommand(OkCommand);
        Cancel = new RelayCommand(CancelCommand);
        _typeDialogs = typeDialogs;
        _calendarViewModel = calendarViewModel;
        _selectedItem = calendarViewModel.Type;

        Items = new ObservableCollection<string> { "Год", "Месяц", "Неделя", "День", "Интервал" };

        Log.Send(EventLevel.Info, logSender, "Открытие окна");
    }

    private CalendarViewModel _calendarViewModel { get; }
    private TypeDialogs _typeDialogs { get; }
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

    private void OkCommand()
    {
        if (SelectedItem == null)
            return;

        Log.Send(EventLevel.Info, logSender, "Тип календаря установлен.");

        _calendarViewModel.Type = SelectedItem;
        _calendarViewModel.UpdateButtons();
        _typeDialogs.Close();
    }

    private void CancelCommand()
    {
        Log.Send(EventLevel.Info, logSender, "Окно закрыто");
        _typeDialogs.Close();
    }
}
