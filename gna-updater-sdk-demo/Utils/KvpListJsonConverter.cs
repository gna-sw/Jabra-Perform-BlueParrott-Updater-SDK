using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace GNAUpdaterSDK_Demo.Utils
{
    public class KvpListJsonConverter : JsonConverter<IEnumerable<KeyValuePair<string, string>>>
    {
        public override void WriteJson(JsonWriter writer, IEnumerable<KeyValuePair<string, string>> value, JsonSerializer serializer)
        {
            writer.WriteStartObject();
            foreach (var kvp in value)
            {
                writer.WritePropertyName(@kvp.Key);
                writer.WriteValue(@kvp.Value);
            }
            writer.WriteEndObject();
        }

        public override IEnumerable<KeyValuePair<string, string>> ReadJson(JsonReader reader, Type objectType, IEnumerable<KeyValuePair<string, string>> existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var list = new List<KeyValuePair<string, string>>();

            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.PropertyName)
                {
                    var key = reader.Value as string;
                    var value = reader.ReadAsString();
                    list.Add(new KeyValuePair<string, string>(key, value));
                }
            }
            
            return list;
        }
    }
}