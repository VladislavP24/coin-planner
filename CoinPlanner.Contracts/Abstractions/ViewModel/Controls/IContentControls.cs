using System.Collections.ObjectModel;
using CoinPlanner.Contracts.DTO.DataServieDTO;

namespace CoinPlanner.Contracts.Abstractions.ViewModel.Controls
{
    public interface IContentControls
    {
        PlansDTO? Plan { get; set; }
        DateTime? StartDate { get; set; }
        DateTime? EndDate { get; set; }
        ObservableCollection<OperationsDTO> DynamicOperationCollection { get; set; }

        void UpdateOperation();
        void CalculationOfParameters();
    }
}
