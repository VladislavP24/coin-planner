namespace CoinPlanner.Contracts.Abstractions.ViewModel
{
    public interface IViewModelDialogs
    {
        void OkCommand(object currWindow);
        void CancelCommand(object currWindow);
    }
}
