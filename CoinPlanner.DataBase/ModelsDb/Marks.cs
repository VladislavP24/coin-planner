using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoinPlanner.DataBase.ModelsDb;

public class Marks
{
    [Key]
    [Column("mark_id")]
    public Guid Mark_Id { get; set; }
    public string? Mark_Name { get; set; }
    public DateTime Mark_Date { get; set; }
    public Guid Mark_Plan_Id { get; set; }
}
