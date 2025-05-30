using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoinPlanner.DataBase.ModelsDB;

[Table("operations")]
public class Operations
{
    [Key]
    [Column("oper_id")]
    public Guid Oper_Id { get; set; }

    [Column("oper_plan_id")]
    public Guid Oper_Plan_Id { get; set; }

    [Column("oper_name")]
    public string? Oper_Name { get; set; }

    [Column("oper_type_id")]
    public int Oper_Type_Id { get; set; }

    [Column("oper_category_id")]
    public int Oper_Category_Id { get; set; }

    [Column("oper_sum")]
    public double Oper_Sum { get; set; }

    [Column("oper_completed")]
    public bool Oper_Completed { get; set; }

    [Column("oper_next_date")]
    public DateTime Oper_Next_Date { get; set; }
}