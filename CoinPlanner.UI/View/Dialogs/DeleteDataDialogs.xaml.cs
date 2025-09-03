using System.Windows;
using System.Windows.Input;
using CoinPlanner.Contracts.Abstractions.DataBase;
using CoinPlanner.Contracts.Abstractions.ViewModel.Controls;
using CoinPlanner.UI.ViewModel.Dialogs;

namespace CoinPlanner.UI.View.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для DeleteDataDialogs.xaml
    /// </summary>
    public partial class DeleteDataDialogs : Window
    {
        public DeleteDataDialogs(IPanelControls panel, IDataService dataService, IContentControls content)
        {
            InitializeComponent();

            DataContext = new DeleteDataDialogsViewModel(panel, dataService, content);
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, 0))
                e.Handled = true;
        }
    }
}
