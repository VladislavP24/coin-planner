using System.Windows;
using CoinPlanner.UI.ViewModel.Controls;
using CoinPlanner.UI.ViewModel.Dialogs;


namespace CoinPlanner.UI.View.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для AddDataDialogs.xaml
    /// </summary>
    public partial class AddDataDialogs : Window
    {
        public AddDataDialogs(ContentViewModel contentViewModel, Dictionary<int, string> categories)
        {
            InitializeComponent();

            DataContext = new AddDataDialogsViewmodel(this, contentViewModel, categories);
        }
    }
}
