using System;
using System.IO;
using System.Xml.Serialization;
using GNAUpdaterSDK_Demo.Logger;

namespace GNAUpdaterSDK_Demo.Utils
{
    public class SerializationHelper
    {
        public static void SerializeXmlFile<T>(string filename, T obj)
        {
            try
            {
                XmlSerializer xmlFormat = new XmlSerializer(typeof(T));
                using (var fStream = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    xmlFormat.Serialize(fStream, obj);
                }
            }
            catch (Exception ex)
            {
                DemoLogWriter.Error(ex.Message);
            }
        }

        public static T DeserializeXmlFile<T>(string filename)
        {
            try
            {
                XmlSerializer xmlFormat = new XmlSerializer(typeof(T));
                using (var fStream = File.OpenRead(filename))
                {
                    return (T) xmlFormat.Deserialize(fStream);
                }
            }
            catch (Exception ex)
            {
                DemoLogWriter.Error(ex.Message);
                return default(T);
            }
        }

        public static T DeserializeXmlString<T>(string xmlText)
        {
            try
            {
                XmlSerializer xmlFormat = new XmlSerializer(typeof(T));
                using (var reader = new StringReader(xmlText))
                {
                    return (T) xmlFormat.Deserialize(reader);
                }
            }
            catch (Exception ex)
            {
                DemoLogWriter.Error(ex.Message);
                return default(T);
            }
        }

        public static string SerializeObject<T>(T objectToSerialize)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(objectToSerialize.GetType());

            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, objectToSerialize);
                return textWriter.ToString();
            }
        }
    }
}
