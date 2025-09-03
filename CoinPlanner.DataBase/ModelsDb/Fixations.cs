using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoinPlanner.DataBase.ModelsDb;

public class Fixations
{
    [Key]
    [Column("fix_id")]
    public Guid Fix_Id { get; set; }
    public Guid Fix_Plan_Id { get; set; }
    public string? Fix_Name { get; set; }
    public string? Type_Name { get; set; }
    public string? Category_Name { get; set; }
    public double Fix_Sum { get; set; }
    public bool Fix_Completed { get; set; }
    public DateTime Fix_Next_Date { get; set; }
}
