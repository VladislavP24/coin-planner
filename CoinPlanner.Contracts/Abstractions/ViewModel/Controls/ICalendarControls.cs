namespace CoinPlanner.Contracts.Abstractions.ViewModel.Controls
{
    public interface ICalendarControls
    {
        DateTime Start { get; set; }
        DateTime End { get; set; }
        string Type { get; set; }

        void UpdateButtons();
        void AddMarkToButton();
    }
}
