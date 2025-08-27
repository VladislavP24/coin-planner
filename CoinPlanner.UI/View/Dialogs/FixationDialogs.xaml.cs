using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using CoinPlanner.DataBase;
using CoinPlanner.UI.ViewModel.Controls;
using CoinPlanner.UI.ViewModel.Dialogs;

namespace CoinPlanner.UI.View.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для FixationDialogs.xaml
    /// </summary>
    public partial class FixationDialogs : Window
    {
        public FixationDialogs(PanelViewModel panelViewModel, DataService dataService, ContentViewModel contentViewModel)
        {
            InitializeComponent();
            
            DataContext = new FixationDialogsViewModel(panelViewModel, dataService, contentViewModel);
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
