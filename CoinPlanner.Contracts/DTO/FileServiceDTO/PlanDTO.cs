using System.Xml.Serialization;

namespace CoinPlanner.Contracts.DTO.FileServiceDTO;

[XmlRoot("Plan")]
public class PlanDTO
{
    [XmlElement("PlanId")]
    public Guid PlanId { get; set; }

    [XmlElement("PlanName")]
    public string? PlanName { get; set; }

    [XmlElement("DataCreate")]
    public DateTime DataCreate { get; set; }

    [XmlElement("DataUpdate")]
    public DateTime DataUpdate { get; set; }
}
