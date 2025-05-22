using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoinPlanner.FileService.DTO;

public class MarkDTO
{
    public int MarkId { get; set; }
    public string? MarkName { get; set; }
    public DateTime MarkDate { get; set; }
    public int MarkPlanId { get; set; }
}
