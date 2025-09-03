using System.Xml.Serialization;

namespace CoinPlanner.Contracts.DTO.FileServiceDTO;

[XmlRoot("Fixation")]
public class FixationDTO
{
    [XmlElement("FixId")]
    public Guid FixId { get; set; }

    [XmlElement("FixName")]
    public string FixName { get; set; }

    [XmlElement("FixType")]
    public string FixType { get; set; }

    [XmlElement("FixCategory")]
    public string FixCategory { get; set; }

    [XmlElement("FixSum")]
    public double FixSum { get; set; }

    [XmlElement("FixCompleted")]
    public bool FixCompleted { get; set; }

    [XmlElement("FixNextDate")]
    public DateTime FixNextDate { get; set; }

    [XmlElement("FixPlanId")]
    public Guid FixPlanId { get; set; }
}

