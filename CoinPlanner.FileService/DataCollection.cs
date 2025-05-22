using System.Xml.Serialization;
using CoinPlanner.FileService.DTO;

namespace CoinPlanner.FileService;

[XmlRoot("DataCollection")]
public class DataCollection
{
    [XmlArray("Operations")]
    [XmlArrayItem("Operation")]
    public IList<OperationDTO> Operations { get; set; } = new List<OperationDTO>();

    [XmlArray("Marks")]
    [XmlArrayItem("Mark")]
    public IList<MarkDTO> Marks { get; set; } = new List<MarkDTO>();

    [XmlArray("Fixations")]
    [XmlArrayItem("Fixation")]
    public IList<FixationDTO> Fixations { get; set; } = new List<FixationDTO>();



    [XmlArray("PlanConditions")]
    [XmlArrayItem("Pair")]
    public KeyValuePairDTO PlanConditionPairs { get; set; } = new KeyValuePairDTO();

    [XmlArray("OperConditions")]
    [XmlArrayItem("Pair")]
    public List<KeyValuePairDTO> OperConditionPairs { get; set; } = new List<KeyValuePairDTO>();

    [XmlArray("FixConditions")]
    [XmlArrayItem("Pair")]
    public IList<KeyValuePairDTO> FixConditionPairs { get; set; } = new List<KeyValuePairDTO>();

    [XmlArray("MarkConditions")]
    [XmlArrayItem("Pair")]
    public IList<KeyValuePairDTO> MarkConditionPairs { get; set; } = new List<KeyValuePairDTO>();
}
