using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace BusinessLayer
{
    public static class XMLHelper
    {
        public static T Deserialize<T>(this string toDeserialize)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(toDeserialize.GetType());
            using (StringReader textReader = new StringReader(toDeserialize))
            {
                return (T)xmlSerializer.Deserialize(textReader);
            }
        }

        public static string Serialize<T>(this T toSerialize)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(toSerialize.GetType());
            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, toSerialize);
                return textWriter.ToString();
            }
        }
        public static string ParseXML<T>(this T toParse)
        {
            XmlDocument xml = new XmlDocument();
            XmlElement root = xml.CreateElement("root");
            XmlElement child = xml.CreateElement("node");
            xml.AppendChild(root);

            Type T1 = toParse.GetType();
            PropertyInfo[] sourceProprties = T1.GetProperties(BindingFlags.Instance | BindingFlags.Public);

            foreach (var sourceProperty in sourceProprties)
            {
                if (sourceProperty.PropertyType.Name.ToString() != "SelectList" && sourceProperty.Name != "Filters")
                {
                    object sourceValue = sourceProperty.GetValue(toParse, null);
                    child.SetAttribute(sourceProperty.Name, sourceValue?.ToString());
                }
            }
            root.AppendChild(child);
            return xml.OuterXml;
        }
    }
}
