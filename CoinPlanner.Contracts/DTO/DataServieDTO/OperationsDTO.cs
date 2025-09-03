namespace CoinPlanner.Contracts.DTO.DataServieDTO
{
    public class OperationsDTO
    {
        public Guid Oper_Id { get; set; }
        public Guid Oper_Plan_Id { get; set; }
        public string? Oper_Name { get; set; }
        public string? Type_Name { get; set; }
        public string? Category_Name { get; set; }
        public double Oper_Sum { get; set; }
        public bool Oper_Completed { get; set; }
        public DateTime Oper_Next_Date { get; set; }
        public int Oper_Id_Table { get; set; }
    }
}
