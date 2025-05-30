﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoinPlanner.DataBase.ModelsDB;

public class Operations
{
    [Key]
    [Column("oper_id")]
    public Guid Oper_Id { get; set; }
    public Guid Oper_Plan_Id { get; set; }
    public string? Oper_Name { get; set; }
    public string? Type_Name { get; set; }
    public string? Category_Name { get; set; }
    public double Oper_Sum { get; set; }
    public bool Oper_Completed { get; set; }
    public DateTime Oper_Next_Date { get; set; }
}
