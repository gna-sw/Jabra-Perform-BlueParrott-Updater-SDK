using GNAUpdaterSDK_Demo.Logger;
using GNAUpdaterSDK_Demo.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace GNAUpdaterSDK_Demo.Helpers
{
    public class ConfigOptionValueDescription : IComparable
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int CompareTo(object obj)
        {
            if (obj is ConfigOptionValueDescription coValueDescription)
            {
                return string.Compare(this.Name, coValueDescription.Name, StringComparison.Ordinal);
            }
            else
            {
                throw new ArgumentException("Object is not a ConfigOptionValueDescription");
            }
        }
    }

    public class ConfigOptionDescription : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ObservableCollection<ConfigOptionValueDescription> Options { get; set; }
        
        private ConfigOptionValueDescription _selectedOption;
        public ConfigOptionValueDescription SelectedOption 
        {
            get { return _selectedOption; }
            set
            {
                _selectedOption = value;
                NotifyPropertyChanged(nameof(SelectedOption));
            }
        }
        
        public ConfigOptionDescription(string name, string description,
            ObservableCollection<ConfigOptionValueDescription> options = null)
        {
            this.Name = name;
            this.Description = description;
            this.Options = options ?? new ObservableCollection<ConfigOptionValueDescription>();
        }

        public bool Add(ConfigOptionValueDescription option)
        {
            try
            {
                if (option != null)
                {
                    Options.Add(option);
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                DemoLogWriter.Error($"Exception caught while adding an option {option?.Name}:{option?.Description} to ConfigOptionDescription");
                DemoLogWriter.Error(DemoLogWriter.GetExceptionString(ex));
                return false;
            }
        }

        public override string ToString()
        {
            return SelectedOption?.Name ?? string.Empty;
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion
    }

    public class ConfigDescription
    {
        public ObservableCollection<ConfigOptionDescription> Settings { get; private set; }

        private ConfigDescription(ObservableCollection<ConfigOptionDescription> settings)
        {
            this.Settings = settings;
        }

        public bool Add(ConfigOptionDescription option)
        {
            try
            {
                if (option != null)
                {
                    Settings.Add(option);
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                DemoLogWriter.Error($"Exception caught while adding an option {option?.Name}:{option?.Description} to ConfigDescription");
                DemoLogWriter.Error(DemoLogWriter.GetExceptionString(ex));
                return false;
            }
        }

        public bool AddRange(IEnumerable<ConfigOptionDescription> options)
        {
            try
            {
                if (options != null)
                {
                    foreach (var option in options)
                    {
                        Settings.Add(option);
                    }
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                DemoLogWriter.Error($"Exception caught while adding range of options to ConfigDescription");
                DemoLogWriter.Error(DemoLogWriter.GetExceptionString(ex));
                return false;
            }
        }

        public static ConfigDescription FromJson(string jsonString)
        {
            try
            {
                var settings = JsonHelper.DeserializeObject<ObservableCollection<ConfigOptionDescription>>(jsonString);
                return new ConfigDescription(settings);
            }
            catch (Exception ex)
            {
                DemoLogWriter.Error($"Failed to parse JSON <{jsonString}> to ConfigDescription");
                DemoLogWriter.Error(DemoLogWriter.GetExceptionString(ex));
            }

            return null;
        }

        public static ConfigDescription Empty()
        {
            return new ConfigDescription(new ObservableCollection<ConfigOptionDescription>());
        }

        public override string ToString()
        {
            try
            {
                return JsonHelper.SerializeObject(this.Settings.OrderBy(co => co.Name));
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
