using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CoinPlanner.UI.ViewModel.Items;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CoinPlanner.UI.ViewModel.Controls;

public class CalendarViewModel : ObservableObject
{
    public CalendarViewModel()
    {
        SendInterval = new RelayCommand(SendIntervalCommand);
        UpdateButtons();
    }

    public EventHandler OnButtonPressed { get; set; }
    public ICommand SendInterval { get; set; }

    /// <summary>
    /// Начальная дата
    /// </summary>
    public DateTime Start
    {
        get => _start;
        set => SetProperty(ref _start, value, nameof(Start));
    }
    private DateTime _start = DateTime.Now;


    /// <summary>
    /// Конечная дата
    /// </summary>
    public DateTime End
    {
        get => _end;
        set => SetProperty(ref _end, value, nameof(End));
    }
    private DateTime _end = DateTime.Now;


    /// <summary>
    /// Тип календаря
    /// </summary>
    public string Type
    {
        get => _type;
        set => SetProperty(ref _type, value, nameof(Type));
    }
    private string _type = "День";


    /// <summary>
    /// Коллекция Button
    /// </summary>
    public ObservableCollection<ButtonItemsViewModel> Buttons
    {
        get => _buttons;
        set => SetProperty(ref _buttons, value, nameof(Buttons));
    }
    private ObservableCollection<ButtonItemsViewModel> _buttons = new();

    public DateTime? SelectedStart { get; set; }
    public DateTime? SelectedEnd { get; set; }

    /// <summary>
    /// Отправка инетервала после нажатия на кнопку
    /// </summary>
    private void SendIntervalCommand()
    {
        SelectedStart = Buttons.Where(x => x.IsChecked == true).Select(x => x.StartTime).FirstOrDefault();
        SelectedEnd = Buttons.Where(x => x.IsChecked == true).Select(x => x.EndTime).FirstOrDefault();

        // Сброc IsCheked
        foreach (var button in Buttons.Where(x => x.IsChecked == true))
            button.IsChecked = false;

        OnButtonPressed.Invoke(this, EventArgs.Empty);
    }


    /// <summary>
    /// Обновление Button при изменении интервала и типа
    /// </summary>
    public void UpdateButtons()
    {
        Buttons.Clear();

        switch (Type)
        {
            case "День":
                int totalDays = (End - Start).Days + 1;
                for (int i = 0; i < totalDays; i++)
                {
                    var date = Start.AddDays(i);
                    Buttons.Add(new ButtonItemsViewModel() { Content = date.ToString("dd MMMM yyyy 'г.'"), StartTime = date, EndTime = null });
                }
                break;

            case "Неделя":
                DateTime startOfWeek = Start;
                while (startOfWeek.DayOfWeek != DayOfWeek.Monday)
                {
                    startOfWeek = startOfWeek.AddDays(-1);
                }

                while (startOfWeek <= End)
                {
                    var weekEnd = startOfWeek.AddDays(6);
                    if (weekEnd > End) break;

                    Buttons.Add(new ButtonItemsViewModel { Content = $"{startOfWeek:dd}-{weekEnd:dd MMMM yyyy 'г.'}", StartTime = startOfWeek, EndTime = weekEnd.AddDays(1) });
                    startOfWeek = startOfWeek.AddDays(7);
                }
                break;

            case "Месяц":
                int totalMonths = ((End.Year - Start.Year) * 12) + End.Month - Start.Month + 1;
                for (int i = 0; i < totalMonths; i++)
                {
                    var monthStart = new DateTime(Start.Year, Start.Month, 1).AddMonths(i);
                    if (monthStart <= End)
                    {
                        var monthEnd = monthStart.AddMonths(1).AddDays(-1);

                        Buttons.Add(new ButtonItemsViewModel { Content = monthStart.ToString("MMMM yyyy 'г.'"), StartTime = monthStart, EndTime = monthEnd.AddDays(1) });
                    }
                }
                break;

            case "Год":
                int totalYears = End.Year - Start.Year + 1;
                for (int i = 0; i < totalYears; i++)
                {
                    var yearStart = new DateTime(Start.Year + i, 1, 1);
                    if (yearStart <= End)
                    {
                        var yearEnd = new DateTime(yearStart.Year, 12, 31);

                        Buttons.Add(new ButtonItemsViewModel { Content = yearStart.ToString("yyyy 'г.'"), StartTime = yearStart, EndTime = yearEnd.AddDays(1) });
                    }
                }
                break;

            case "Интервал":
                Buttons.Add(new ButtonItemsViewModel { Content = $"{Start:dd MMMM} - {End:dd MMMM yyyy 'г.'}", StartTime = Start, EndTime = End.AddDays(1) });
                break;
        }
    }
}
