using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoinPlanner.Contracts.DTO;

public class MarkDTO
{
    public Guid MarkId { get; set; }
    public string? MarkName { get; set; }
    public DateTime MarkDate { get; set; }
    public Guid MarkPlanId { get; set; }
}
