using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoinPlanner.DataBase.ModelsDB;

public class Plans
{
    [Key]
    [Column("plan_id")]
    public Guid Plan_Id { get; set; }
    public string? Plan_Name { get; set; }
    public DateTime Date_Create { get; set; }
    public DateTime Date_Update { get; set; }
}
