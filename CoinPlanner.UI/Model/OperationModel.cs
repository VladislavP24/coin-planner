using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoinPlanner.UI.Model;

public class OperationModel
{
    public int OperId { get; set; }
    public int OperPlanId { get; set; }
    public string? OperName { get; set; }
    public int OperType_Id { get; set; }
    public double OperSum { get; set; }
    public bool OperCompleted { get; set; }
    public DateTime OperNextDate { get; set; }
}
