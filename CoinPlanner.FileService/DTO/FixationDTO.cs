using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoinPlanner.FileService.DTO;

public class FixationDTO
{
    public int FixId { get; set; }
    public string FixName { get; set; }
    public string FixType { get; set; }
    public string FixCategory { get; set; }
    public double FixSum { get; set; }
    public bool FixCompleted { get; set; }
    public DateTime FixNextDate { get; set; }
    public int FixPlanId { get; set; }
}
