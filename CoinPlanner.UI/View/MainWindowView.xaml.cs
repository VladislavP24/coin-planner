using System.Windows;
using CoinPlanner.DataBase;
using CoinPlanner.UI.ViewModel;

namespace CoinPlanner.UI.View
{
    /// <summary>
    /// Логика взаимодействия для MainWindowView.xaml
    /// </summary>
    public partial class MainWindowView : Window
    {
        public MainWindowView(DBProcessing dbProcessing)
        {
            InitializeComponent();

            DataContext = new MainWindowViewModel(dbProcessing);
        }
    }
}
