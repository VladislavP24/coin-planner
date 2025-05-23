using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CoinPlanner.FileService.DTO;

[XmlRoot("Plan")]
public class PlanDTO
{
    [XmlElement("PlanId")]
    public int PlanId { get; set; }

    [XmlElement("PlanName")]
    public string? PlanName { get; set; }

    [XmlElement("DataCreate")]
    public DateTime DataCreate { get; set; }

    [XmlElement("DataUpdate")]
    public DateTime DataUpdate { get; set; }

    [XmlElement("IsSynchro")]
    public bool IsSynchro { get; set; }
}
