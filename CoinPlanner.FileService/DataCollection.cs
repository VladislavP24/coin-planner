using System.Xml.Serialization;
using CoinPlanner.FileService.DTO;

namespace CoinPlanner.FileService;

[XmlRoot("DataCollection")]
public class DataCollection
{
    [XmlElement("Plan")]
    public PlanXML Plan { get; set; }


    [XmlArray("Operations")]
    [XmlArrayItem("Operation")]
    public List<OperationXML> Operations { get; set; } = new();

    [XmlArray("Marks")]
    [XmlArrayItem("Mark")]
    public List<MarkXML> Marks { get; set; } = new();

    [XmlArray("Fixations")]
    [XmlArrayItem("Fixation")]
    public List<FixationXML> Fixations { get; set; } = new();



    [XmlElement("PlanConditionPairs")]
    public KeyValuePairXML PlanConditionPairs { get; set; } = new KeyValuePairXML();

    [XmlArray("OperConditions")]
    [XmlArrayItem("Pair")]
    public List<KeyValuePairXML> OperConditionPairs { get; set; } = new();

    [XmlArray("FixConditions")]
    [XmlArrayItem("Pair")]
    public List<KeyValuePairXML> FixConditionPairs { get; set; } = new();

    [XmlArray("MarkConditions")]
    [XmlArrayItem("Pair")]
    public List<KeyValuePairXML> MarkConditionPairs { get; set; } = new();
}
