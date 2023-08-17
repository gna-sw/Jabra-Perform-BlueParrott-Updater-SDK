using Newtonsoft.Json;
using System;
using GNAUpdaterSDK_Demo.Logger;

namespace GNAUpdaterSDK_Demo.Utils
{
    public class JsonHelper
    {
        public static string SerializeObject<T>(T toSerialize)
        {
            try
            {
                var val = JsonConvert.SerializeObject(toSerialize);
                return val;
            }
            catch (Exception ex)
            {
                DemoLogWriter.Error("Exception caught during object serialization.");
                DemoLogWriter.Error(DemoLogWriter.GetExceptionString(ex));
                return string.Empty;
            }
        }

        public static string SerializeObject<T>(T toSerialize, JsonSerializerSettings settings)
        {
            try
            {
                var val = JsonConvert.SerializeObject(toSerialize, settings);
                return val;
            }
            catch (Exception ex)
            {
                DemoLogWriter.Error("Exception caught during object serialization.");
                DemoLogWriter.Error(DemoLogWriter.GetExceptionString(ex));
                return string.Empty;
            }
        }

        public static string SerializeObject<T>(T toSerialize, params JsonConverter[] converters)
        {
            try
            {
                var val = JsonConvert.SerializeObject(toSerialize, converters);
                return val;
            }
            catch (Exception ex)
            {
                DemoLogWriter.Error("Exception caught during object serialization.");
                DemoLogWriter.Error(DemoLogWriter.GetExceptionString(ex));
                return string.Empty;
            }
        }

        public static T DeserializeObject<T>(string toDeserialize)
        {
            try
            {
                var val = JsonConvert.DeserializeObject<T>(toDeserialize);
                return val;
            }
            catch (Exception ex)
            {
                DemoLogWriter.Error("Exception caught during object deserialization.");
                DemoLogWriter.Error(DemoLogWriter.GetExceptionString(ex));
                return default(T);
            }
        }

        public static T DeserializeObject<T>(string toDeserialize, JsonSerializerSettings settings)
        {
            try
            {
                var val = JsonConvert.DeserializeObject<T>(toDeserialize, settings);
                return val;
            }
            catch (Exception ex)
            {
                DemoLogWriter.Error("Exception caught during object deserialization.");
                DemoLogWriter.Error(DemoLogWriter.GetExceptionString(ex));
                return default(T);
            }
        }

        public static T DeserializeObject<T>(string toDeserialize, params JsonConverter[] converters)
        {
            try
            {
                var val = JsonConvert.DeserializeObject<T>(toDeserialize, converters);
                return val;
            }
            catch (Exception ex)
            {
                DemoLogWriter.Error("Exception caught during object deserialization.");
                DemoLogWriter.Error(DemoLogWriter.GetExceptionString(ex));
                return default(T);
            }
        }
    }
}
