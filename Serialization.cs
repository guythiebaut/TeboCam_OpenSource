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


        public static string SerializeToXML(object SerializeFrom)
        {

            return string.Empty;

        }

        public static bool SerializeToXMLFile(string file, object SerializeFrom)
        {

            try
            {

                using (StreamWriter writer = new StreamWriter(file))
                {

                    XmlSerializer xmlSerializer = XmlSerializer.FromTypes(new[] { SerializeFrom.GetType() })[0];
                    StringWriter textWriter = new StringWriter();
                    xmlSerializer.Serialize(textWriter, SerializeFrom);
                    string SerializedXML = textWriter.ToString();
                    writer.Write(SerializedXML);
                    writer.Flush();

                }


            }
            catch (Exception e)
            {

                throw e;

            }

            return true;

        }


        public static object SerializeFromXML(string file, object SerializeTo)
        {

            return new object();

        }

        public static object SerializeFromXMLFile(string file, object SerializeTo)
        {

            try
            {

                string xmlIn;
                using (StreamReader streamReader = new StreamReader(file, Encoding.UTF8))
                {
                    xmlIn = streamReader.ReadToEnd();
                }

                XmlSerializer xmlDeSerializer = XmlSerializer.FromTypes(new[] { SerializeTo.GetType() })[0];
                StringReader textReader = new StringReader(xmlIn);

                return xmlDeSerializer.Deserialize(textReader);

            }
            catch (Exception e)
            {

                throw e;


            }

        }



    }
}
