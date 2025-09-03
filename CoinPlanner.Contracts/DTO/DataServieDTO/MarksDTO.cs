namespace CoinPlanner.Contracts.DTO.DataServieDTO
{
    public class MarksDTO
    {
        public Guid Mark_Id { get; set; }
        public string? Mark_Name { get; set; }
        public DateTime Mark_Date { get; set; }
        public Guid Mark_Plan_Id { get; set; }
    }
}
