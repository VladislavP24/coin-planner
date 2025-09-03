using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CoinPlanner.Contracts.Abstractions.DataBase;
using CoinPlanner.Contracts.Abstractions.ViewModel.Controls;
using CoinPlanner.UI.ViewModel.Dialogs;

namespace CoinPlanner.UI.View.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для FixationDialogs.xaml
    /// </summary>
    public partial class FixationDialogs : Window
    {
        public FixationDialogs(IPanelControls panel, IDataService dataService, IContentControls content)
        {
            InitializeComponent();

            DataContext = new FixationDialogsViewModel(panel, dataService, content);
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, 0) && e.Text != ".")
                e.Handled = true;

            if (e.Text == "." && ((TextBox)sender).Text.Contains("."))
                e.Handled = true;
        }
    }
}
