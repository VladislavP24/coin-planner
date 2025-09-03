using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CoinPlanner.Contracts.Abstractions.DataBase;
using CoinPlanner.Contracts.Abstractions.ViewModel.Controls;
using CoinPlanner.UI.ViewModel.Dialogs;

namespace CoinPlanner.UI.View.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для EditDataDialogs.xaml
    /// </summary>
    public partial class EditDataDialogs : Window
    {
        public EditDataDialogs(IPanelControls panel, IDataService dataService, IContentControls content)
        {
            InitializeComponent();

            DataContext = new EditDataDialogsViewModel(dataService, content, panel);
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, 0) && e.Text != ".")
                e.Handled = true;

            if (e.Text == "." && ((TextBox)sender).Text.Contains("."))
                e.Handled = true;
        }

        private void TextBox_PreviewTextInputInt(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, 0))
                e.Handled = true;
        }
    }
}
