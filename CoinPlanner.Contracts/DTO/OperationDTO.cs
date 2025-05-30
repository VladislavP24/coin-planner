using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoinPlanner.Contracts.DTO;

public class OperationDTO
{
    public Guid OperId { get; set; }
    public string OperName { get; set; }
    public string OperType { get; set; }
    public string OperCategory { get; set; }
    public double OperSum { get; set; }
    public bool OperCompleted { get; set; }
    public DateTime OperNextDate { get; set; }
    public Guid OperPlanId { get; set; }
}
