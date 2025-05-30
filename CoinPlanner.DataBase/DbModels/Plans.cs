using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoinPlanner.DataBase.ModelsDB;

[Table("plans")]
public class Plans
{
    [Key]
    [Column("plan_id")]
    public Guid Plan_Id { get; set; }

    [Column("plan_name")]
    public string? Plan_Name { get; set; }

    [Column("date_create")]
    public DateTime Date_Create { get; set; }

    [Column("date_update")]
    public DateTime Date_Update { get; set; }
}