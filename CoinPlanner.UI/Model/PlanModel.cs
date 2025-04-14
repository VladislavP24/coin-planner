using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoinPlanner.UI.Model;

public class PlanModel
{
    public int PlanId { get; set; }
    public string? PlanName { get; set; }
    public DateTime DataCreate { get; set; }
    public DateTime DataUpdate { get; set; }
    public List<OperationModel>? OperationModels { get; set; }
}
