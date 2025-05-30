using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoinPlanner.Contracts.DTO;

public class PlanDTO
{
    public Guid PlanId { get; set; }
    public string? PlanName { get; set; }
    public DateTime DataCreate { get; set; }
    public DateTime DataUpdate { get; set; }
}
