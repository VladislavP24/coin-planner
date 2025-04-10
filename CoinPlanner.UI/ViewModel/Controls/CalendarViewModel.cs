using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoinPlanner.UI.ViewModel.Controls;

public class CalendarViewModel
{
    public CalendarViewModel() 
    {
        
    }

    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public string Type { get; set; }

}
