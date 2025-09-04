using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace CoinPlanner.UI.View.Converters
{
    public class BooleanToYesNoConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool booleanValue)
            {
                return booleanValue ? "Да" : "Нет";
            }
            return "Нет";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string stringValue)
            {
                if (stringValue.Equals("Да", StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
                else if (stringValue.Equals("Нет", StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
            }
            return false;
        }
    }
}
