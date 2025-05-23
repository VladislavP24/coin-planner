using System.Xml.Serialization;
using CoinPlanner.FileService.DTO;

namespace CoinPlanner.FileService;

[XmlRoot("DataCollection")]
public class DataCollection
{
    [XmlElement("Plan")]
    public PlanDTO Plan { get; set; }


    [XmlArray("Operations")]
    [XmlArrayItem("Operation")]
    public List<OperationDTO> Operations { get; set; } = new();

    [XmlArray("Marks")]
    [XmlArrayItem("Mark")]
    public List<MarkDTO> Marks { get; set; } = new();

    [XmlArray("Fixations")]
    [XmlArrayItem("Fixation")]
    public List<FixationDTO> Fixations { get; set; } = new();



    [XmlElement("PlanConditionPairs")]
    public KeyValuePairDTO PlanConditionPairs { get; set; } = new KeyValuePairDTO();

    [XmlArray("OperConditions")]
    [XmlArrayItem("Pair")]
    public List<KeyValuePairDTO> OperConditionPairs { get; set; } = new();

    [XmlArray("FixConditions")]
    [XmlArrayItem("Pair")]
    public List<KeyValuePairDTO> FixConditionPairs { get; set; } = new();

    [XmlArray("MarkConditions")]
    [XmlArrayItem("Pair")]
    public List<KeyValuePairDTO> MarkConditionPairs { get; set; } = new();
}
