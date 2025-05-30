using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoinPlanner.DataBase.ModelsDb;

[Table("fixations")]
public class Fixations
{
    [Key]
    [Column("fix_id")]
    public Guid Fix_Id { get; set; }

    [Column("fix_plan_id")]
    public Guid Fix_Plan_Id { get; set; }

    [Column("fix_name")]
    public string? Fix_Name { get; set; }

    [Column("fix_type_id")]
    public int Fix_Type_Id { get; set; }

    [Column("fix_category_id")]
    public int Fix_Category_Id { get; set; }

    [Column("fix_sum")]
    public double Fix_Sum { get; set; }

    [Column("fix_completed")]
    public bool Fix_Completed { get; set; }

    [Column("fix_next_date")]
    public DateTime Fix_Next_Date { get; set; }
}