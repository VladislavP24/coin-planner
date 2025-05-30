using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoinPlanner.DataBase.ModelsDb;

[Table("marks")]
public class Marks
{
    [Key]
    [Column("mark_id")]
    public Guid Mark_Id { get; set; }

    [Column("mark_name")]
    public string? Mark_Name { get; set; }

    [Column("mark_date")]
    public DateTime Mark_Date { get; set; }

    [Column("mark_plan_id")]
    public Guid Mark_Plan_Id { get; set; }
}
