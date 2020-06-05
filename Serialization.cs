using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;


namespace TeboCam
{

    public static class Serialization
    {

        public static IException tebowebException;

        public static string SerializeToXml(object serializeFrom)
        {

            return string.Empty;

        }

        public static bool SerializeToXmlFile(string file, object serializeFrom)
        {

            try
            {

                using (StreamWriter writer = new StreamWriter(file))
                {

                    XmlSerializer xmlSerializer = XmlSerializer.FromTypes(new[] { serializeFrom.GetType() })[0];
                    StringWriter textWriter = new StringWriter();
                    xmlSerializer.Serialize(textWriter, serializeFrom);
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


        public static object SerializeFromXml(string file, object serializeTo)
        {

            return new object();

        }

        public static object SerializeFromXmlFile(string file, object serializeTo)
        {

            try
            {

                string xmlIn;
                using (StreamReader streamReader = new StreamReader(file, Encoding.UTF8))
                {
                    xmlIn = streamReader.ReadToEnd();
                }

                XmlSerializer xmlDeSerializer = XmlSerializer.FromTypes(new[] { serializeTo.GetType() })[0];
                StringReader textReader = new StringReader(xmlIn);

                return xmlDeSerializer.Deserialize(textReader);

            }
            catch (Exception e)
            {
                TebocamState.tebowebException.LogException(e);
                throw e;
            }

        }



    }
}
