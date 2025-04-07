using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoinPlanner.DataBase.ModelsDb;

public class Plans
{
    public int Plan_Id { get; set; }
    public string? Plan_Name { get; set; } 
    public DateTime Data_Create { get; set; }
    public DateTime Data_Update { get; set; }
}
