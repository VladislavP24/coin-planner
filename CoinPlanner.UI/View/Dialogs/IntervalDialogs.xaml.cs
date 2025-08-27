using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CoinPlanner.UI.ViewModel.Controls;
using CoinPlanner.UI.ViewModel.Dialogs;


namespace CoinPlanner.UI.View.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для IntervalDIalogs.xaml
    /// </summary>
    public partial class IntervalDialogs : Window
    {
        public IntervalDialogs(CalendarViewModel calendarViewModel)
        {
            InitializeComponent();

            DataContext = new IntervalDialogsViewModel(calendarViewModel);
        }

        /// <summary>
        /// Проверка на ввод даты
        /// </summary>
        private void PART_TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (!Regex.IsMatch(e.Text, @"^[0-9\.]+$"))
                {
                    e.Handled = true;
                    return;
                }

                string currentText = textBox.Text;

                if (currentText.Length >= 10)
                {
                    e.Handled = true;
                    return;
                }


                int dotCount = currentText.Count(c => c == '.');

                if (e.Text == ".")
                {
                    if (dotCount >= 2 || currentText.Length == 0 || currentText[currentText.Length - 1] == '.')
                    {
                        e.Handled = true;
                        return;
                    }
                }

                string newText = currentText.Insert(textBox.SelectionStart, e.Text);

                if (!IsValidDateFormat(newText))
                    e.Handled = true;
            }
        }

        private bool IsValidDateFormat(string date)
            => Regex.IsMatch(date, @"^(\d{2}\.\d{2}\.\d{4})?$");

    }
}
