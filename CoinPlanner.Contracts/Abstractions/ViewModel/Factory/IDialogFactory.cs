using CoinPlanner.Contracts.Abstractions.DataBase;
using CoinPlanner.Contracts.Abstractions.ViewModel.Controls;

namespace CoinPlanner.Contracts.Abstractions.ViewModel.Factory
{
    public interface IDialogFactory
    {
        void ShowAddDataDialogs(IPanelControls controls, IDataService dataService, IContentControls content);
        void ShowCreatePlanDialogs(IPanelControls controls, IDataService dataService);
        void ShowDeleteDataDialogs(IPanelControls controls, IDataService dataService, IContentControls content);
        void ShowDeletePlanDialogs(IPanelControls controllers, IDataService dataService);
        void ShowEditDataDialogs(IPanelControls panel, IDataService dataService, IContentControls content);
        void ShowFixationDataDialogs(IPanelControls panel, IDataService dataService, IContentControls content);
        void ShowIntervalDialogs(ICalendarControls calendar);
        void ShowMarkDialogs(IPanelControls panel, IDataService dataService, ICalendarControls calendar);
        void ShowRenamePlanDialogs(IPanelControls panel, IDataService dataService);
        void ShowTypeDialogs(ICalendarControls calendar);
    }
}
