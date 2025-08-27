using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CoinPlanner.UI.Interface
{
    interface IViewModelDialogs
    {
        void OkCommand(Window window);
        void CancelCommand(Window window);
    }
}
