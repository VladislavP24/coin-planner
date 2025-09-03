namespace CoinPlanner.Contracts.DTO.DataServieDTO
{
    public class PlansDTO
    {
        public Guid Plan_Id { get; set; }
        public string? Plan_Name { get; set; }
        public DateTime Date_Create { get; set; }
        public DateTime Date_Update { get; set; }
    }
}
