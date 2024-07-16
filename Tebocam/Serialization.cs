using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace TeboCam
{
    public static class Serialization
    {
        public static IException tebowebException;

        public static bool SerializeToXmlFile<T>(string file, T serializeThis)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(file))
                {
                    XmlSerializer xmlSerializer = XmlSerializer.FromTypes(new[] { typeof(T) }).First();
                    StringWriter textWriter = new StringWriter();
                    xmlSerializer.Serialize(textWriter, serializeThis);
                    string serializedXml = textWriter.ToString();
                    writer.Write(serializedXml);
                    writer.Flush();
                }
            }
            catch (Exception e)
            {
                TebocamState.tebowebException.LogException(e);
                throw e;
            }
            return true;
        }

        public static T SerializeFromXmlFile<T>(string file)
        {
            try
            {
                string xmlIn;
                using (StreamReader streamReader = new StreamReader(file, Encoding.UTF8))
                {
                    xmlIn = streamReader.ReadToEnd();
                }
                XmlSerializer xmlDeSerializer = XmlSerializer.FromTypes(new[] { typeof(T) }).First();
                StringReader textReader = new StringReader(xmlIn);
                return (T)xmlDeSerializer.Deserialize(textReader);
            }
            catch (Exception e)
            {
                TebocamState.tebowebException.LogException(e);
                throw e;
            }
        }
    }
}
