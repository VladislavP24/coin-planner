using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoinPlanner.UI.Model;

public class PlanModel
{
    public Guid PlanId { get; set; }
    public string? PlanName { get; set; }
    public DateTime DateCreate { get; set; }
    public DateTime DataUpdate { get; set; }
}
