using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoinPlanner.DataBase.ModelsDB;

public class Plans
{
    [Key]
    [Column("plan_id")]
    public Guid Plan_Id { get; set; }
    public string? Plan_Name { get; set; }
    public DateTime Date_Create { get; set; }
    public DateTime Date_Update { get; set; }
    public bool Is_Synchro { get; set; }
}
