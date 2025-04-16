using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoinPlanner.UI.Model;
using System.Windows.Data;
using System.Windows.Media;

namespace CoinPlanner.UI.View.Converters;

public class OperationTypeToBrushConverter : IValueConverter
{
    public Brush DepositBrush { get; set; } = (Brush)new BrushConverter().ConvertFrom("#2196F3"); // Цвет для "Зачисления"
    public Brush PaymentBrush { get; set; } = (Brush)new BrushConverter().ConvertFrom("#CDD0D2"); // Цвет для "Оплаты"
    public Brush DefaultBrush { get; set; } = Brushes.White; // Цвет по умолчанию

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is OperationModel operation)
        {
            switch (operation.OperType)
            {
                case "Зачисление":
                    return DepositBrush;
                case "Оплата":
                    return PaymentBrush;
                default:
                    return DefaultBrush;
            }
        }

        return DefaultBrush; // Или другой цвет по умолчанию, если значение не OperationModel
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
