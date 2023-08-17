using GalaSoft.MvvmLight.Command;
using GNAUpdaterSDK;
using GNAUpdaterSDK_Demo.Commands;
using GNAUpdaterSDK_Demo.Helpers;
using GNAUpdaterSDK_Demo.Logger;
using GNAUpdaterSDK_Demo.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace GNAUpdaterSDK_Demo.ViewModels
{
    public class ConfigurationWindowViewModel : GNBaseViewModel, INotifyPropertyChanged
    {
        ConnectedDeviceModel connectedDevice;

        private ObservableCollection<ConfigOptionDescription> _settings;
        public ObservableCollection<ConfigOptionDescription> Settings
        {
            get => _settings;
            set
            {
                _settings = value;
                NotifyPropertyChanged(nameof(Settings));
            }
        }

        private bool _isReadInProgress = false;
        public bool IsReadInProgress
        {
            get => _isReadInProgress;
            set
            {
                _isReadInProgress = value;
                NotifyPropertyChanged(nameof(IsReadInProgress));
            }
        }

        private bool _isWriteInProgress = false;
        public bool IsWriteInProgress
        {
            get => _isWriteInProgress;
            set
            {
                _isWriteInProgress = value;
                NotifyPropertyChanged(nameof(IsWriteInProgress));
            }
        }

        private string _readWriteStatus;
        public string ReadWriteStatus
        {
            get => _readWriteStatus;
            set
            {
                _readWriteStatus = value;
                NotifyPropertyChanged(nameof(ReadWriteStatus));
            }
        }

        private ICommand _clickApplyConfigurationCommand;
        public ICommand ClickApplyConfigurationCommand
        {
            get
            {
                return _clickApplyConfigurationCommand ?? (_clickApplyConfigurationCommand = new CommandHandler(() => ClickApplyConfiguration(), true));
            }
        }

        public RelayCommand<Window> CloseWindowCommand { get; private set; }

        private void ClickApplyConfiguration()
        {
            try
            {
                var configSet = ConfigSet.Empty();
                foreach (var option in Settings)
                {
                    // Fix for US-153 SRD X.X - Add support for reading a Parrott Button value that is not in the database and not force the user to set it to a value that is set in the database
                    if (option.SelectedOption != null && !string.IsNullOrEmpty(option.SelectedOption.Name))
                    {
                        configSet.Add(option.Name, option.SelectedOption?.Name ?? string.Empty);
                    }
                }

                var configurationJSON = configSet.ToString();

                IsWriteInProgress = true;
                ReadWriteStatus = "Write in progress...";
                var res = GNAUpdaterApi.Instance.SetHeadsetConfiguration(connectedDevice.DeviceID, configurationJSON, SetHeadsetConfigurationByDeviceHandler, SetHeadsetConfigurationHandler, 1);
                if (res != 0)
                {
                    IsWriteInProgress = false;
                    ReadWriteStatus = "Write failed...";
                    MessageBoxHelper.ShowMessage($"Failed to set headset configuration, error code is {res}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                DemoLogWriter.Error("Exception caught while trying to apply configuration");
                DemoLogWriter.Error(DemoLogWriter.GetExceptionString(ex));
            }
        }

        public void SetHeadsetConfigurationByDeviceHandler(int deviceId, int resultCode, ulong context)
        {
            try
            {
                if (resultCode != 0)
                {
                    ReadWriteStatus = $"Write failed for deviceId {deviceId}...";
                }
                else
                {
                    ReadWriteStatus = $"Write succeeded for deviceId {deviceId}...";
                }
            }
            catch (Exception ex)
            {
                DemoLogWriter.Error("Exception caught while trying to handle SHC device callback");
                DemoLogWriter.Error(DemoLogWriter.GetExceptionString(ex));
            }
        }

        public void SetHeadsetConfigurationHandler(int resultCode, int succededDevicesCount, int attemptedDevicesCount, ulong context)
        {
            try
            {
                IsWriteInProgress = false;
                if (resultCode == 0)
                {
                    ReadWriteStatus = "Write succeeded...";
                }
                else if (resultCode == -140)
                {
                    ReadWriteStatus = $"Write partially succeeded... {succededDevicesCount} out of {attemptedDevicesCount} devices succeded.";
                }
                else
                {
                    ReadWriteStatus = "Write failed...";
                    MessageBoxHelper.ShowMessage($"Failed to set headset configuration, error code is {resultCode}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                DemoLogWriter.Error("Exception caught while trying to handle SHC callback");
                DemoLogWriter.Error(DemoLogWriter.GetExceptionString(ex));
            }
        }

        internal ConfigurationWindowViewModel(GNBaseViewModel parent, ConnectedDeviceModel theDevice) : base(parent)
        {
            this.CloseWindowCommand = new RelayCommand<Window>(this.CloseWindow);
            connectedDevice = theDevice;
            PopulateData(connectedDevice.DeviceID);
        }

        private void CloseWindow(Window window)
        {
            if (window != null)
            {
                window.Close();
            }
        }

        private void PopulateData (int deviceId)
        {
            try
            {
                var qRes = GNAUpdaterApi.Instance.QueryConfigurationSettings(deviceId, out var configurationSettings);
                if (qRes != 0)
                {
                    IsReadInProgress = false;
                    ReadWriteStatus = "Read settings failed...";
                    MessageBoxHelper.ShowMessage($"Failed to read headset configuration settings, error code is {qRes}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                var configDescription = ConfigDescription.FromJson(configurationSettings);
                Settings = configDescription.Settings;

                IsReadInProgress = true;
                ReadWriteStatus = "Read in progress...";
                var res = GNAUpdaterApi.Instance.QueryHeadsetConfiguration(deviceId, GetHeadsetConfigurationHandler, 1);
                if (res != 0)
                {
                    IsReadInProgress = false;
                    ReadWriteStatus = "Read failed...";
                    MessageBoxHelper.ShowMessage($"Failed to read headset configuration, error code is {res}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                DemoLogWriter.Error("Exception caught while trying to populate device data");
                DemoLogWriter.Error(DemoLogWriter.GetExceptionString(ex));
            }
        }

        public void GetHeadsetConfigurationHandler(int resultCode, string configuration, ulong context)
        {
            try
            {
                IsReadInProgress = false;
                if (resultCode == 0)
                {
                    ReadWriteStatus = "Read succeeded...";
                }
                else if (resultCode == -140)
                {
                    ReadWriteStatus = "Read partially succeeded...";
                }
                else
                {
                    ReadWriteStatus = "Read failed...";
                    MessageBoxHelper.ShowMessage($"Failed to read headset configuration, error code is {resultCode}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                if (!string.IsNullOrEmpty(configuration))
                {
                    var configSet = ConfigSet.FromJsonNameValue(configuration);

                    foreach (var setting in Settings)
                    {
                        var matchingOption = configSet.Settings.FirstOrDefault(s => s.Key == setting.Name);
                        if (matchingOption.Key != null)
                        {
                            setting.SelectedOption = setting.Options.FirstOrDefault(o => o.Name.Equals(matchingOption.Value));
                        }
                        else
                        {
                            DemoLogWriter.Error($"Matching option not found for {setting.Name} setting");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DemoLogWriter.Error("Exception caught while trying to handle GHC callback");
                DemoLogWriter.Error(DemoLogWriter.GetExceptionString(ex));
            }
        }

        #region INotifyPropertyChanged Members

        public new event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion
    }
}
