using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CoinPlanner.DataBase;
using CoinPlanner.UI.ViewModel.Controls;
using CoinPlanner.UI.ViewModel.Dialogs;


namespace CoinPlanner.UI.View.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для AddDataDialogs.xaml
    /// </summary>
    public partial class AddDataDialogs : Window
    {
        public AddDataDialogs(PanelViewModel panelViewModel, DataService dataService, ContentViewModel contentViewModel)
        {
            InitializeComponent();

            DataContext = new AddDataDialogsViewmodel(this, dataService, panelViewModel, contentViewModel);
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
