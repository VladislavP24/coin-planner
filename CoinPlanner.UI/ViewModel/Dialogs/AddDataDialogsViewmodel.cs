using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CoinPlanner.UI.View.Dialogs;
using CoinPlanner.UI.ViewModel.Controls;
using CommunityToolkit.Mvvm.Input;

namespace CoinPlanner.UI.ViewModel.Dialogs;

public class AddDataDialogsViewmodel
{
    public AddDataDialogsViewmodel(AddDataDialogs addDataDialogs, ContentViewModel contentViewModel, Dictionary<int, string> categories) 
    { 
        _categories = categories;
        _contentViewModel = contentViewModel;
        _addDataDialogs = addDataDialogs;

        Ok = new RelayCommand(OkCommand);
        Cancel = new RelayCommand(CancelCommand);
    }

    private AddDataDialogs _addDataDialogs;
    private ContentViewModel _contentViewModel;
    private Dictionary<int, string> _categories;

    public ICommand Ok { get; set; }
    public ICommand Cancel { get; set; }

    private void OkCommand()
    {
        _addDataDialogs.Close();
    }

    private void CancelCommand()
        => _addDataDialogs.Close();
}
