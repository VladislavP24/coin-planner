using CoinPlanner.Contracts.Abstractions.DataBase;
using CoinPlanner.Contracts.Abstractions.ViewModel.Controls;
using CoinPlanner.Contracts.Abstractions.ViewModel.Factory;
using CoinPlanner.UI.View.Dialogs;

namespace CoinPlanner.UI.ViewModel.Factory
{
    public class DialogFactory : IDialogFactory
    {
        public void ShowAddDataDialogs(IPanelControls controls, IDataService dataService, IContentControls content) => new AddDataDialogs(dataService, controls, content).ShowDialog();

        public void ShowCreatePlanDialogs(IPanelControls controls, IDataService dataService) => new CreatePlanDialogs(controls, dataService).ShowDialog();

        public void ShowDeleteDataDialogs(IPanelControls controls, IDataService dataService, IContentControls content) => new DeleteDataDialogs(controls, dataService, content).ShowDialog();

        public void ShowDeletePlanDialogs(IPanelControls controllers, IDataService dataService) => new DeletePlanDialogs(controllers, dataService).ShowDialog();

        public void ShowEditDataDialogs(IPanelControls panel, IDataService dataService, IContentControls content) => new EditDataDialogs(panel, dataService, content).ShowDialog();

        public void ShowFixationDataDialogs(IPanelControls panel, IDataService dataService, IContentControls content) => new FixationDialogs(panel, dataService, content).ShowDialog();

        public void ShowIntervalDialogs(ICalendarControls calendar) => new IntervalDialogs(calendar).ShowDialog();

        public void ShowMarkDialogs(IPanelControls panel, IDataService dataService, ICalendarControls calendar) => new MarkDialogs(panel, dataService, calendar).ShowDialog();

        public void ShowRenamePlanDialogs(IPanelControls panel, IDataService dataService) => new RenamePlanDialogs(panel, dataService).ShowDialog();

        public void ShowTypeDialogs(ICalendarControls calendar) => new TypeDialogs(calendar).ShowDialog();
    }
}
