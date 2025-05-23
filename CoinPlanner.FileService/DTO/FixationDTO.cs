using System;
using System.Xml.Serialization;

namespace CoinPlanner.FileService.DTO;

[XmlRoot("Fixation")]
public class FixationDTO
{
    [XmlElement("FixId")]
    public int FixId { get; set; }

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
    public int FixPlanId { get; set; }
}

