using System.Xml.Serialization;

namespace testTaskUzkikh.Helpers
{
    public static class XmlSerializerHelper
    {
        public static string SerializeInXmlString<T>(T obj)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(obj.GetType());

            using (StringWriter sw = new StringWriter())
            {
                xmlSerializer.Serialize(sw, obj);

                return sw.ToString();
            }
        }
    }
}
