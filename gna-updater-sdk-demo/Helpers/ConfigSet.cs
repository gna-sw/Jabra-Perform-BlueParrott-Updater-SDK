using System;
using System.Collections.Generic;
using System.Linq;
using GNAUpdaterSDK_Demo.Logger;
using GNAUpdaterSDK_Demo.Utils;

namespace GNAUpdaterSDK_Demo.Helpers
{
    public class ConfigSet
    {
        public ICollection<KeyValuePair<string, string>> Settings { get; private set; }

        private ConfigSet(ICollection<KeyValuePair<string, string>> settings)
        {
            this.Settings = settings;
        }

        public bool Add(string name, string value)
        {
            try
            {
                Settings.Add(new KeyValuePair<string, string>(name, value));
                return true;
            }
            catch (Exception ex)
            {
                DemoLogWriter.Error($"Exception caught while adding an option {name}:{value} to ConfigSet");
                DemoLogWriter.Error(DemoLogWriter.GetExceptionString(ex));
                return false;
            }
        }

        public static ConfigSet Empty()
        {
            return new ConfigSet(new List<KeyValuePair<string, string>>());
        }

        public static ConfigSet FromJsonNameValue(string jsonString)
        {
            try
            {
                var settings = JsonHelper.DeserializeObject<List<KeyValuePair<string, string>>>(jsonString, new KvpListJsonConverter());
                return new ConfigSet(settings);
            }
            catch (Exception ex)
            {
                DemoLogWriter.Error($"Failed to parse JSON <{jsonString}> to ConfigSet");
                DemoLogWriter.Error(DemoLogWriter.GetExceptionString(ex));
            }

            return null;
        }

        public override string ToString()
        {
            try
            {
                return JsonHelper.SerializeObject(this.Settings.OrderBy(so => so.Key), new KvpListJsonConverter());
            }
            catch (Exception ex)
            {
                DemoLogWriter.Error($"Failed to serialize ConfigSet to JSON");
                DemoLogWriter.Error(DemoLogWriter.GetExceptionString(ex));
                return string.Empty;
            }
        }
    }
}