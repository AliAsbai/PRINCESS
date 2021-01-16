using System.Globalization;
using System.IO;
using System.Xml.Serialization;

/** authors:
 *          @Ali Asbai
 *          
 **/

namespace PRINCESS.controller
{
    public class SerializeToXml
    {
        public static string SerializeObjectToXml<E>(E obj)
        {
            StringWriter writer = new StringWriter(CultureInfo.InvariantCulture);
            XmlSerializer serializer = new XmlSerializer(typeof(E));
            serializer.Serialize(writer, obj);
            return writer.ToString();
        }

        public static E DeSerializeXmlToObject<E>(string obj)
        {
            XmlSerializer ser = new XmlSerializer(typeof(E));

            using (StringReader sr = new StringReader(obj))
            {
                return (E)ser.Deserialize(sr);
            }
        }
    }
}
