using GNAUpdaterSDK;
using GNAUpdaterSDK.GNA_HID_DFU_Library;
using GNAUpdaterSDK_Demo.Commands;
using GNAUpdaterSDK_Demo.Helpers;
using GNAUpdaterSDK_Demo.Models;
using GNAUpdaterSDK_Demo.Services;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using GNAUpdaterSDK_Demo.Logger;
using GNAUpdaterSDK_Demo.Utils;
using System;

namespace GNAUpdaterSDK_Demo.ViewModels
{
    public class ConnectedDevicesControlViewModel : GNBaseViewModel
    {
        #region Properties

        private bool _canExecute;

        private ConnectedDeviceModel _connectedDevicesControl;
        public ConnectedDeviceModel ConnectedDevicesControl
        {
            get { return _connectedDevicesControl; }
            set
            {
                _connectedDevicesControl = value;
                OnPropertyChanged("ConnectedDevicesControl");
            }
        }

        #endregion

        #region ICommands

        private ICommand _clickCommand;
        public ICommand ClickCommand
        {
            get
            {
                return _clickCommand ?? (_clickCommand = new CommandHandler(() => ClickAction(), _canExecute));
            }
        }

        private ICommand _clickInfoDeviceCommand;
        public ICommand ClickInfoDeviceCommand
        {
            get
            {
                return _clickInfoDeviceCommand ?? (_clickInfoDeviceCommand = new CommandHandler(() => ClickInfoDevice(), true));
            }
        }

        private ICommand _clickConfigureDeviceCommand;
        public ICommand ClickConfigureDeviceCommand
        {
            get
            {
                return _clickConfigureDeviceCommand ?? (_clickConfigureDeviceCommand = new CommandHandler(() => ClickConfigurationDevice(), true));
            }
        }

        private ICommand _clickUpdateDeviceCommand;
        public ICommand ClickUpdateDeviceCommand
        {
            get
            {
                return _clickUpdateDeviceCommand ?? (_clickUpdateDeviceCommand = new CommandHandler(() => ClickUpdateDevice(), true));
            }
        }

        private ICommand _clickCancelUpdateDeviceCommand;
        public ICommand ClickCancelUpdateDeviceCommand
        {
            get
            {
                return _clickCancelUpdateDeviceCommand ?? (_clickCancelUpdateDeviceCommand = new CommandHandler(() => ClickCancelUpdateDevice(), true));
            }
        }

        #endregion

        public ConnectedDevicesControlViewModel(GNBaseViewModel parent) : base(parent)
        {

        }

        internal ConnectedDevicesControlViewModel(GNBaseViewModel parent, Device theDevice) : base(parent)
        {
            _canExecute = true;

            _connectedDevicesControl = new ConnectedDeviceModel(theDevice);
        }

        public void ClickAction()
        {
            ChangeDevice();
        }

        public void ClickInfoDevice()
        {
            WindowService ws = new WindowService();
            ws.CreateInfoWindow(ConnectedDevicesControl);
        }

        public void ClickConfigurationDevice()
        {
            WindowService ws = new WindowService();
            ws.CreateConfigurationWindow(ConnectedDevicesControl);
        }

        public void ClickUpdateDevice()
        {
            try
            {
                MainWindowViewModel mainViewModel = ParentViewModel as MainWindowViewModel;
                var res = GNAUpdaterApi.Instance.StartUpgrade(ConnectedDevicesControl.DeviceID, mainViewModel.UpdateDevicesHandler, mainViewModel.EndUpdateDevicesHandler, 1, mainViewModel.MainWindow.FirmwareFile);
                if (res != 0)
                {
                    MessageBoxHelper.ShowMessage($"Unable to start an upgrade, error code is {res}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                DemoLogWriter.Error("Exception caught while trying to start device upgrade");
                DemoLogWriter.Error(DemoLogWriter.GetExceptionString(ex));
            }
        }

        public void ProgressUpdate(int state, int eventType, int percent, ulong context)
        {
            if (context == 1)
            {
                State aState = (State)state;
                if (aState == State.Starting)
                {
                    ConnectedDevicesControl.StartProgressBar();
                }
                else if (aState == State.Aborting)
                {
                    ConnectedDevicesControl.StopProgressBar("Update Ended");
                }
                else
                {
                    ConnectedDevicesControl.SetProgressBarStatus(percent, aState.ToString());
                }
            }
            else
            {
                DemoLogWriter.Error("Invalid context returned by SDK.");
                MessageBoxHelper.ShowMessage($"Invalid context returned by SDK. Expected=1, Actual={context}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void EndProgressUpdate(Device device, ulong context)
        {
            if (context == 1)
            {
                //MessageBoxHelper.ShowMessage("The device was updated.", "The device was updated", MessageBoxButton.OK, MessageBoxImage.Information);
                ConnectedDevicesControl.StopProgressBar("Update Ended");
                RefreshProperties(device.DeviceID);
            }
            else
            {
                DemoLogWriter.Error("Invalid context returned by SDK.");
                MessageBoxHelper.ShowMessage($"Invalid context returned by SDK. Expected=1, Actual={context}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void ClickCancelUpdateDevice()
        {
            try
            {
                int res = GNAUpdaterApi.Instance.CancelFWUpgrade();
            }
            catch (Exception ex)
            {
                DemoLogWriter.Error("Exception caught while trying to cancel an upgrade");
                DemoLogWriter.Error(DemoLogWriter.GetExceptionString(ex));
            }
        }

        private void ChangeDevice()
        {
            MainWindowViewModel.SelectedConnectedDevicesControl.BackgroundColor = ColorHelper.NonActiveConnectedDevicesControlColor;
            MainWindowViewModel.SelectedConnectedDevicesControl.BorderColor = ColorHelper.NonActiveConnectedDevicesControlColor;

            ConnectedDevicesControl.BackgroundColor = ColorHelper.ActiveConnectedDevicesControlBackgroundColor;
            ConnectedDevicesControl.BorderColor = ColorHelper.ActiveConnectedDevicesControlBorderColor;

            MainWindowViewModel.SelectedConnectedDevicesControl = ConnectedDevicesControl;
        }

        public void RefreshProperties(int deviceID)
        {
            try
            {
                string resDevice;
                GNAUpdaterApi.Instance.GetHeadsetDetails(deviceID, out resDevice);

                if (resDevice == null)
                {
                    DemoLogWriter.Error($"Unable to find device with deviceId={deviceID}");
                }

                Device devDetails = SerializationHelper.DeserializeXmlString<Device>(resDevice);
                ConnectedDevicesControl.Name = devDetails.Name;
                ConnectedDevicesControl.FWVersion = devDetails.FWVersion;

                ConnectedDevicesControl.ProductID = devDetails.DeviceProperties.ProductId;
                ConnectedDevicesControl.VendorID = devDetails.DeviceProperties.VendorId;
                ConnectedDevicesControl.DeviceLocation = devDetails.DeviceProperties.DeviceLocation;
                ConnectedDevicesControl.DevicePath = devDetails.DeviceProperties.DevicePath;
                ConnectedDevicesControl.DeviceClass = devDetails.DeviceProperties.DeviceClass;
                ConnectedDevicesControl.ContainerID = devDetails.DeviceProperties.ContainerID;

                byte[] imageBytes;
                int res = GNAUpdaterApi.Instance.GetHeadsetImage(devDetails.DeviceID, out imageBytes);
                if (res == 0)
                {
                    ConnectedDevicesControl.ImageSource = GeneralHelper.GetBitmapFromByteArray(imageBytes);
                }
                else
                {
                    ConnectedDevicesControl.ImageSource = GeneralHelper.SetImageSource();
                }
            }
            catch (Exception ex)
            {
                DemoLogWriter.Error("Exception caught while trying to update device properties");
                DemoLogWriter.Error(DemoLogWriter.GetExceptionString(ex));
            }
        }

        #region INotifyPropertyChanged Members

        public new event PropertyChangedEventHandler PropertyChanged;

        // Create the OnPropertyChanged method to raise the event
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        #endregion
    }
}
