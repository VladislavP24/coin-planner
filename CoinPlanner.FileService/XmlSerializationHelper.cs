using System.Xml.Serialization;

namespace CoinPlanner.FileService;

public static class XmlSerializationHelper
{
    public static void SerializeToXml(DataCollection data, string filePath)
    {
        var serializer = new XmlSerializer(typeof(DataCollection));
        using (var writer = new StreamWriter(filePath))
        {
            serializer.Serialize(writer, data);
        }
    }

    public static DataCollection DeserializeFromXml(string filePath)
    {
        var serializer = new XmlSerializer(typeof(DataCollection));
        using (var reader = new StreamReader(filePath))
        {
            return (DataCollection)serializer.Deserialize(reader);
        }
    }
}
