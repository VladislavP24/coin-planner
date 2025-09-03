namespace CoinPlanner.Contracts.Abstractions.ViewModel.Controls
{
    public interface IDiagramControls
    {
        bool IsVisibleDiagram { get; set; }
        bool IsAllTime { get; set; }
        bool IsSelectTime { get; set; }
        void CreatDiagram(Guid planId);
        void AllTimeCommand();
        void SelectTimeCommand();
        void CategoryFilling();
    }
}
