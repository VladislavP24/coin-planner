using System;
using System.Xml.Serialization;

namespace CoinPlanner.FileService.DTO;

[XmlRoot("Operation")]
public class OperationDTO
{
    [XmlElement("OperId")]
    public int OperId { get; set; }

    [XmlElement("OperName")]
    public string OperName { get; set; }

    [XmlElement("OperType")]
    public string OperType { get; set; }

    [XmlElement("OperCategory")]
    public string OperCategory { get; set; }

    [XmlElement("OperSum")]
    public double OperSum { get; set; }

    [XmlElement("OperCompleted")]
    public bool OperCompleted { get; set; }

    [XmlElement("OperNextDate")]
    public DateTime OperNextDate { get; set; }

    [XmlElement("OperPlanId")]
    public int OperPlanId { get; set; }
}


