using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CoinPlanner.Contracts.Abstractions.DataBase;
using CoinPlanner.Contracts.Abstractions.ViewModel.Controls;
using CoinPlanner.UI.ViewModel.Dialogs;


namespace CoinPlanner.UI.View.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для AddDataDialogs.xaml
    /// </summary>
    public partial class AddDataDialogs : Window
    {
        public AddDataDialogs(IDataService dataService, IPanelControls panel, IContentControls content)
        {
            InitializeComponent();

            DataContext = new AddDataDialogsViewmodel(dataService, panel, content);
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
