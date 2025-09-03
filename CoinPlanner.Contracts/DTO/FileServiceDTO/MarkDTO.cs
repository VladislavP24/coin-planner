using System.Xml.Serialization;

namespace CoinPlanner.Contracts.DTO.FileServiceDTO;

[XmlRoot("Mark")]
public class MarkDTO
{
    [XmlElement("MarkId")]
    public Guid MarkId { get; set; }

    [XmlElement("MarkName")]
    public string? MarkName { get; set; }

    [XmlElement("MarkDate")]
    public DateTime MarkDate { get; set; }

    [XmlElement("MarkPlanId")]
    public Guid MarkPlanId { get; set; }
}


