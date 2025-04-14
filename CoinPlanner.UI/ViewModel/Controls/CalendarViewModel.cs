using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CoinPlanner.UI.ViewModel.Controls;

public class CalendarViewModel : ObservableObject
{
    public CalendarViewModel() 
    {
        Buttons = new ObservableCollection<string>();
        UpdateButtons();
    }

    public DateTime Start
    {
        get => _start;
        set => SetProperty(ref _start, value, nameof(Start));

    }
    private DateTime _start = DateTime.Now;

    public DateTime End
    {
        get => _end;
        set => SetProperty(ref _end, value, nameof(End));
    }
    private DateTime _end = DateTime.Now;

    public string Type
    {
        get => _type;
        set => SetProperty(ref _type, value, nameof(Type));
    }
    private string _type = "День";


    public ObservableCollection<string> Buttons
    {
        get => _buttons;
        set => SetProperty(ref _buttons, value, nameof(Buttons));
    }
    private ObservableCollection<string> _buttons;



    public void UpdateButtons()
    {
        Buttons.Clear();

        switch (Type)
        {
            case "День":
                int totalDays = (End - Start).Days + 1; // +1 для включения конечной даты
                for (int i = 0; i < totalDays; i++)
                {
                    var date = Start.AddDays(i);
                    Buttons.Add(date.ToString("dd MMMM yyyy 'г.'"));
                }
                break;

            case "Неделя":
                // Находим ближайший понедельник к начальной дате
                DateTime startOfWeek = Start;
                while (startOfWeek.DayOfWeek != DayOfWeek.Monday)
                {
                    startOfWeek = startOfWeek.AddDays(-1);
                }

                // Генерируем недели от ближайшего понедельника
                while (startOfWeek <= End)
                {
                    var weekEnd = startOfWeek.AddDays(6);
                    if (weekEnd > End) break; // Если конец недели выходит за пределы конечной даты, выходим из цикла

                    Buttons.Add($"{startOfWeek:dd}-{weekEnd:dd MMMM yyyy 'г.'}");
                    startOfWeek = startOfWeek.AddDays(7); // Переходим к следующей неделе
                }
                break;

            case "Месяц":
                int totalMonths = ((End.Year - Start.Year) * 12) + End.Month - Start.Month + 1; // +1 для включения конечного месяца
                for (int i = 0; i < totalMonths; i++)
                {
                    var monthStart = new DateTime(Start.Year, Start.Month, 1).AddMonths(i);
                    if (monthStart <= End)
                    {
                        Buttons.Add(monthStart.ToString("MMMM yyyy 'г.'"));
                    }
                }
                break;

            case "Год":
                int totalYears = End.Year - Start.Year + 1; // +1 для включения конечного года
                for (int i = 0; i < totalYears; i++)
                {
                    var yearStart = new DateTime(Start.Year + i, 1, 1);
                    if (yearStart <= End)
                    {
                        Buttons.Add(yearStart.ToString("yyyy 'г.'"));
                    }
                }
                break;
        }
    }
}
