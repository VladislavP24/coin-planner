using CoinPlanner.Contracts.DTO.DataServieDTO;

namespace CoinPlanner.Contracts.Abstractions.ViewModel.Controls
{
    public interface IPanelControls
    {
        bool IsCheckedEnroll { get; set; }
        bool IsCheckedExpenses { get; set; }
        bool IsCheckedTable { get; set; }
        bool IsCheckedDiagram { get; set; }
        Dictionary<int, string> Categories { get; set; }
        PlansDTO SelectedItemPlan { get; set; }

        void PlanUpdate();
        void UpdateDatePlan();
    }
}
