using GalaSoft.MvvmLight;
using GNAUpdaterSDK;
using GNAUpdaterSDK_Demo.Commands;
using GNAUpdaterSDK_Demo.Helpers;
using GNAUpdaterSDK_Demo.Logger;
using GNAUpdaterSDK_Demo.Models;
using GNAUpdaterSDK_Demo.Properties;
using GNAUpdaterSDK_Demo.Services;
using GNAUpdaterSDK_Demo.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;
using Application = System.Windows.Application;

namespace GNAUpdaterSDK_Demo.ViewModels
{
    public class MainWindowViewModel : GNBaseViewModel
    {
        public Action<Device> ActionDeviceAttached;
        public Action<Device> ActionDeviceDetached;
        public Action<Device> ActionUpgradeStatus;

        #region Properties

        private Models.MainWindowModel _mainWindow;
        public Models.MainWindowModel MainWindow
        {
            get { return _mainWindow; }
            set
            {
                _mainWindow = value;
                RaisePropertyChanged("MainWindow");
            }
        }

        private ViewModelBase _defaultDeviceInformationViewModel;
        public ViewModelBase DefaultDeviceInformationViewModel
        {
            get
            {
                return _defaultDeviceInformationViewModel;
            }
            set
            {
                if (_defaultDeviceInformationViewModel == value)
                    return;
                _defaultDeviceInformationViewModel = value;
                RaisePropertyChanged("DefaultDeviceInformationViewModel");
            }
        }

        private ObservableCollection<ConnectedDevicesControlViewModel> _connectedDeviceList = new ObservableCollection<ConnectedDevicesControlViewModel>();
        public ObservableCollection<ConnectedDevicesControlViewModel> ConnectedDeviceList
        {
            get { return _connectedDeviceList; }
            set
            {
                _connectedDeviceList = value;
                RaisePropertyChanged("ConnectedDeviceList");
            }
        }

        private bool _isFirstScanDone;
        public bool IsFirstScanDone
        {
            get { return _isFirstScanDone; }
            private set
            {
                _isFirstScanDone = value;
                RaisePropertyChanged("IsFirstScanDone");
            }
        }

        public static ConnectedDeviceModel SelectedConnectedDevicesControl;
        public static HashSet<Device> AvailableDevices = new HashSet<Device>();
        private ICollectionView DefaultView;
        private object lockObject = new object();

        #endregion

        #region ICommands

        private ICommand _windowLoaded;

        public ICommand WindowLoaded
        {
            get
            {
                return _windowLoaded ?? (_windowLoaded = new CommandHandler(() => OnWindowLoaded(), true));
            }
        }

        private ICommand _windowClosing;

        public ICommand WindowClosing
        {
            get
            {
                return _windowClosing ?? (_windowClosing = new CommandHandler(() => OnWindowClosing(), true));
            }
        }

        private ICommand _clickExitCommand;
        public ICommand ClickExitCommand
        {
            get
            {
                return _clickExitCommand ?? (_clickExitCommand = new CommandHandler(() => ClickExit(), true));
            }
        }

        private void ClickExit()
        {
            System.Windows.Application.Current.Shutdown();
        }

        private ICommand _clickAboutCommand;
        public ICommand ClickAboutCommand
        {
            get
            {
                return _clickAboutCommand ?? (_clickAboutCommand = new CommandHandler(() => ClickAbout(), true));
            }
        }

        private void ClickAbout()
        {
            var aboutString = $"SDK Demo Version: {App.DemoVersion.ToString(4)}";
            MessageBoxHelper.ShowMessage(aboutString, "About", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private ICommand _clickOtherFunctionsCommand;
        public ICommand ClickOtherFunctionsCommand
        {
            get
            {
                return _clickOtherFunctionsCommand ?? (_clickOtherFunctionsCommand = new CommandHandler(() => ClickOtherFunctions(), true));
            }
        }

        private void ClickOtherFunctions()
        {
            WindowService ws = new WindowService();
            ws.CreateOtherFunctionsWindow(this, SelectedConnectedDevicesControl);
        }

        private ICommand _clickDriversUpgradeCommand;
        public ICommand ClickDriversUpgradeCommand
        {
            get
            {
                return _clickDriversUpgradeCommand ?? (_clickDriversUpgradeCommand = new CommandHandler(() => ClickDriversUpgrade(), true));
            }
        }

        private ICommand _clickInitializeCommand;
        public ICommand ClickInitializeCommand
        {
            get
            {
                return _clickInitializeCommand ?? (_clickInitializeCommand = new CommandHandler(() => ClickInitialize(), true));
            }
        }

        private ICommand _clickCeaseActivityCommand;
        public ICommand ClickCeaseActivityCommand
        {
            get
            {
                return _clickCeaseActivityCommand ?? (_clickCeaseActivityCommand = new CommandHandler(() => ClickCeaseActivity(), true));
            }
        }

        private ICommand _clickBrowseFirmwareCommand;
        public ICommand ClickBrowseFirmwareCommand
        {
            get
            {
                return _clickBrowseFirmwareCommand ?? (_clickBrowseFirmwareCommand = new CommandHandler(() => ClickBrowseFirmware(), true));
            }
        }

        private ICommand _clickClearBrowseFirmwareCommand;
        public ICommand ClickClearBrowseFirmwareCommand
        {
            get
            {
                return _clickClearBrowseFirmwareCommand ?? (_clickClearBrowseFirmwareCommand = new CommandHandler(() => ClickClearBrowseFirmware(), true));
            }
        }

        #endregion

        public MainWindowViewModel() : base(null)
        {
            //Setup unhandled exception logging
            // Add the event handler for handling UI thread exceptions to the event.
            System.Windows.Forms.Application.ThreadException += Application_ThreadException;

            // Set the unhandled exception mode to force all Windows Forms errors to go through
            // our handler.
            System.Windows.Forms.Application.SetUnhandledExceptionMode(System.Windows.Forms.UnhandledExceptionMode.CatchException);

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            DemoLogWriter.Info("Initializing Main Window.");
            DemoLogWriter.Info($"GNA Updater SDK Demo version {App.DemoVersion}");
            var logLevel = Settings.Default.DemoLoggingLevel;
            if (!Enum.IsDefined(typeof(LogLevels), logLevel))
            {
                DemoLogWriter.Warn($"Unknown LogLevel value {logLevel} detected, defaulting to Info");
                logLevel = (int)LogLevels.Info;
            }
            DemoLogWriter.Info($"Setting LogLevel to {(LogLevels)logLevel}");
            DemoLogWriter.SetLoggerConfiguration(DemoLogWriter.LoggerConfiguration((LogLevels)logLevel));

            _mainWindow = new Models.MainWindowModel();
            DefaultDeviceInformationViewModel = new DefaultDeviceInformationViewModel(this);
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            DemoLogWriter.Fatal(string.Format("GNAUpdaterSDK Demo Domain Exception: {0}", (Exception)e.ExceptionObject));
        }

        private void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            DemoLogWriter.Fatal(String.Format("GNAUpdaterSDK Demo Thread Exception: {0}", e.Exception));
        }

        public void InitializeHandler(int resultCode, GNAUpdaterApi api, ulong context)
        {
            try
            {
                if (context == 1)
                {
                    GeneralHelper.ApiInitialized = true;

                    GNAUpdaterApi.GetSDKVersion(out var version, 4);
                    MainWindow.SdkVersion = version;

                    api.SubscribeEvents(DeviceAttached, DeviceDetached, DevicePropertiesUpdatedCallback);

                    Task.Delay(500).ContinueWith((res) => FillDevices());
                }
                else
                {
                    DemoLogWriter.Error("Invalid context returned by SDK.");
                    MessageBoxHelper.ShowMessage($"Invalid context returned by SDK. Expected=1, Actual={context}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                DemoLogWriter.Error("Exception caught while trying to process SDK initialization callback");
                DemoLogWriter.Error(DemoLogWriter.GetExceptionString(ex));
            }
        }

        private void OnWindowLoaded()
        {
            try
            {
                MainWindow.DemoVersion = App.DemoVersion.ToString(4);

                DefaultView = CollectionViewSource.GetDefaultView(ConnectedDeviceList);

                int res = GNAUpdaterApi.Initialize(InitializeHandler, 1, logLevel: Settings.Default.SDKLoggingLevel);
            }
            catch (Exception ex)
            {
                DemoLogWriter.Error(ex.ToString());
                MessageBoxHelper.ShowMessage($"MainWindow execption: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.MainWindow.Close();
            }
        }

        private void OnWindowClosing()
        {
            if (GNAUpdaterApi.Instance != null)
            {
                int res = GNAUpdaterApi.Instance.CeaseActivity(true);
            }

            DialogService.CloseDialogs();
        }

        public void DeviceAttached(string theDevice)
        {
            try
            {
                Device device = SerializationHelper.DeserializeXmlString<Device>(theDevice);
                DeviceAttached(device);
            }
            catch (Exception ex)
            {
                DemoLogWriter.Error("Exception caught while trying to handle device attached callback");
                DemoLogWriter.Error(DemoLogWriter.GetExceptionString(ex));
            }
        }

        public void DeviceAttached(Device device)
        {
            lock (lockObject)
            {
                try
                {
                    if (!AvailableDevices.Contains(device))
                    {
                        AvailableDevices.Add(device);
                        Application.Current.Dispatcher.Invoke((Action)(() =>
                        {
                            AddDevice(device);
                        }), DispatcherPriority.Background, null);
                    }
                }
                catch (Exception ex)
                {
                    DemoLogWriter.Error(ex.ToString());
                }
            }
        }

        private void AddDevice(Device device)
        {
            lock (device)
            {
                try
                {
                    ConnectedDeviceList.Add(new ConnectedDevicesControlViewModel(this, device));

                    if (ConnectedDeviceList.Count == 1)
                    {
                        SelectedConnectedDevicesControl = ConnectedDeviceList[0].ConnectedDevicesControl;
                        SelectedConnectedDevicesControl.BackgroundColor = ColorHelper.ActiveConnectedDevicesControlBackgroundColor;
                        SelectedConnectedDevicesControl.BorderColor = ColorHelper.ActiveConnectedDevicesControlBorderColor;
                    }
                }
                catch (Exception ex)
                {
                    DemoLogWriter.Error(ex.ToString());
                }
            }
        }

        public void DeviceDetached(string theDevice)
        {
            try
            {
                Device device = SerializationHelper.DeserializeXmlString<Device>(theDevice);
                DeviceDetached(device);
            }
            catch (Exception ex)
            {
                DemoLogWriter.Error("Exception caught while trying to handle device detached callback");
                DemoLogWriter.Error(DemoLogWriter.GetExceptionString(ex));
            }
        }

        public void DeviceDetached(Device device)
        {
            try
            {
                AvailableDevices.RemoveWhere(dev => dev.DeviceID == device.DeviceID);
            }
            catch (Exception ex)
            {
                DemoLogWriter.Error(ex.ToString());
            }

            Application.Current.Dispatcher.BeginInvoke((Action)(() =>
            {
                try
                {
                    var modelDisConnectedDevice = (from e in ConnectedDeviceList
                                                   where e.ConnectedDevicesControl.DeviceID == device.DeviceID
                                                   select e).FirstOrDefault();
                    if (modelDisConnectedDevice != null)
                    {
                        ConnectedDeviceList.Remove(modelDisConnectedDevice);
                    }
                }
                catch (Exception ex)
                {
                    DemoLogWriter.Error(ex.ToString());
                }
            }), DispatcherPriority.Send, null);

            Application.Current.Dispatcher.Invoke(() =>
            {
                bool isReadyForChange = true;
                try
                {
                    var selectedDeviceDetails = (from det in ConnectedDeviceList
                                                 where det.ConnectedDevicesControl.DeviceID == device.DeviceID
                                                 select det.ConnectedDevicesControl).FirstOrDefault();

                    if (selectedDeviceDetails == null)
                    {
                        if (SelectedConnectedDevicesControl != null)
                        {
                            var selectedDevice = (from e in ConnectedDeviceList
                                                  where e.ConnectedDevicesControl.DeviceID == SelectedConnectedDevicesControl.DeviceID
                                                  select e).FirstOrDefault();

                            if (selectedDevice != null)
                            {
                                isReadyForChange = false;
                            }
                        }

                        if (isReadyForChange)
                        {
                            var newDeviceDetails = (from det in ConnectedDeviceList
                                                    select det).FirstOrDefault();

                            if (newDeviceDetails != null)
                            {
                                SelectedConnectedDevicesControl = newDeviceDetails.ConnectedDevicesControl;
                                SelectedConnectedDevicesControl.BackgroundColor = ColorHelper.NonActiveConnectedDevicesControlColor;
                                SelectedConnectedDevicesControl.BorderColor = ColorHelper.NonActiveConnectedDevicesControlColor;
                            }
                            else
                            {
                                SelectedConnectedDevicesControl = null;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    DemoLogWriter.Error(ex.ToString());
                }

            }, DispatcherPriority.Send);
        }

        public void FillDevices()
        {
            try
            {
                int res = GNAUpdaterApi.Instance.GetSerializedAvailableDevices(out var serializedDevices);
                Device[] devices = SerializationHelper.DeserializeXmlString<Device[]>(serializedDevices);

                AvailableDevices.Clear();
                Application.Current.Dispatcher.BeginInvoke((Action)(() =>
                {
                    try
                    {
                        ConnectedDeviceList.Clear();
                    }
                    catch (Exception ex)
                    {
                        DemoLogWriter.Error(ex.ToString());
                    }
                }), DispatcherPriority.Send, null);

                foreach (var item in devices)
                {
                    DeviceAttached(item);
                }
            }
            catch (Exception ex)
            {
                DemoLogWriter.Error("Exception caught while trying to fill devices");
                DemoLogWriter.Error(DemoLogWriter.GetExceptionString(ex));
            }
        }

        public void DBUpdatedCallback()
        {
            MessageBoxHelper.ShowMessage("Database has just updated.", "Database update",
                    MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void DevicePropertiesUpdatedCallback(int deviceID)
        {
            ConnectedDeviceList.FirstOrDefault(item => item.ConnectedDevicesControl.DeviceID == deviceID).RefreshProperties(deviceID);
        }

        public void DefferedErrorCallback(int errorCode)
        {
            MessageBoxHelper.ShowMessage($"Deferred error occured. Error code is {errorCode}. See logs for details.", "Deferred error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void OnFirstScanDone(object sender, EventArgs e)
        {
            IsFirstScanDone = true;
        }

        private void ClickDriversUpgrade()
        {
            try
            {
                int res = GNAUpdaterApi.Instance.InstallDrivers(InstallDriverHandler, 1);
            }
            catch (Exception ex)
            {
                DemoLogWriter.Error("Exception caught while trying to invoke install drivers");
                DemoLogWriter.Error(DemoLogWriter.GetExceptionString(ex));
            }
        }

        public void InstallDriverHandler(int resultCode, ulong context)
        {
            try
            {
                if (context == 1)
                {
                    if (resultCode == 0)
                    {
                        MessageBoxHelper.ShowMessage("The drivers were successfully installed.", "Drivers installation",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBoxHelper.ShowMessage($"The drivers were not installed, error code is {resultCode}. See logs for details.", "Drivers installation",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    DemoLogWriter.Error("Invalid context returned by SDK.");
                    MessageBoxHelper.ShowMessage($"Invalid context returned by SDK. Expected=1, Actual={context}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                DemoLogWriter.Error("Exception caught while trying to handle install drivers callback");
                DemoLogWriter.Error(DemoLogWriter.GetExceptionString(ex));
            }
        }

        private void ClickInitialize()
        {
            try
            {
                int res = GNAUpdaterApi.Initialize(InitializeHandler, 1, logLevel: Settings.Default.SDKLoggingLevel);
            }
            catch (Exception ex)
            {
                DemoLogWriter.Error("Exception caught while trying to start SDK initialization");
                DemoLogWriter.Error(DemoLogWriter.GetExceptionString(ex));
            }
        }

        private void ClickCeaseActivity()
        {
            try
            {
                int res = GNAUpdaterApi.Instance.CeaseActivity();
                if (res == 0)
                {
                    GeneralHelper.ApiInitialized = false;

                    HashSet<Device> aDevices = new HashSet<Device>();
                    foreach (var item in AvailableDevices)
                    {
                        aDevices.Add(item);
                    }

                    foreach (var item in aDevices)
                    {
                        DeviceDetached(item);
                    }
                }
            }
            catch (Exception ex)
            {
                DemoLogWriter.Error("Exception caught while trying to cease SDK activity");
                DemoLogWriter.Error(DemoLogWriter.GetExceptionString(ex));
            }
        }

        public void UpdateDevicesHandler(int deviceID, int state, int eventType, int percent, ulong context)
        {
            try
            {
                ConnectedDeviceList?.FirstOrDefault(item => item.ConnectedDevicesControl.DeviceID == deviceID)?.ProgressUpdate(state, eventType, percent, context);
            }
            catch (Exception ex)
            {
                DemoLogWriter.Error("Exception caught while trying to handle device update callback");
                DemoLogWriter.Error(DemoLogWriter.GetExceptionString(ex));
            }
        }

        public void EndUpdateDevicesHandler(int resultCode, string device, ulong context)
        {
            try
            {
                if (resultCode == 0)
                {
                    var devDetails = SerializationHelper.DeserializeXmlString<Device>(device);
                    ConnectedDeviceList?.FirstOrDefault(item => item.ConnectedDevicesControl.DeviceID == devDetails.DeviceID)?.EndProgressUpdate(devDetails, context);
                }
                else if (resultCode == -123)
                {
                    MessageBoxHelper.ShowMessage($"Another upgrade is already in progress, error code is {resultCode}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    MessageBoxHelper.ShowMessage($"The device was not updated. Error code is {resultCode}", "The device was not updated", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                DemoLogWriter.Error("Exception caught while trying to handle overall update callback");
                DemoLogWriter.Error(DemoLogWriter.GetExceptionString(ex));
            }
        }

        public void ClickBrowseFirmware()
        {
            try
            {
                System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
                openFileDialog.Filter = "BIN files (*.bin)|*.bin|DFU files (*.dfu)|*.dfu|All files (*.*)|*.*";

                if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    MainWindow.FirmwareFile = openFileDialog.FileName;
                }
            }
            catch (Exception ex)
            {
                DemoLogWriter.Error("Exception caught while trying to browse for firmware");
                DemoLogWriter.Error(DemoLogWriter.GetExceptionString(ex));
            }
        }

        public void ClickClearBrowseFirmware()
        {
             MainWindow.FirmwareFile = "";
        }
    }
}