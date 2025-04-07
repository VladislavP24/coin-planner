using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoinPlanner.DataBase.DbModels;

public class Operations
{
    public int Oper_Id { get; set; }
    public int Oper_Plan_Id { get; set; }
    public string? Oper_Name { get; set; }
    public int Oper_Type_Id { get; set; }
    public double Oper_Sum { get; set; }
    public bool Oper_Completed { get; set; }
    public DateTime Oper_Next_Date { get; set; }
}
