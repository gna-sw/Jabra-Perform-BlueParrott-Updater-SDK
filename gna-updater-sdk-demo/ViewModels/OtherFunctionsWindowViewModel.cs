using GNAUpdaterSDK;
using GNAUpdaterSDK_Demo.Commands;
using GNAUpdaterSDK_Demo.Converters;
using GNAUpdaterSDK_Demo.Helpers;
using GNAUpdaterSDK_Demo.Models;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using GNAUpdaterSDK_Demo.Logger;

namespace GNAUpdaterSDK_Demo.ViewModels
{
    public class OtherFunctionsWindowViewModel : GNBaseViewModel
    {
        #region Properties

        private OtherFunctionsWindowModel _otherFunctionsWindowModel;
        public OtherFunctionsWindowModel OtherFunctionsWindowModel
        {
            get { return _otherFunctionsWindowModel; }
            set
            {
                _otherFunctionsWindowModel = value;
                OnPropertyChanged("OtherFunctionsWindowModel");
            }
        }

        #endregion

        #region Server ICommands

        private ICommand _clickTestServerConnectionCommand;
        public ICommand ClickTestServerConnectionCommand
        {
            get
            {
                return _clickTestServerConnectionCommand ?? (_clickTestServerConnectionCommand = new CommandHandler(() => ClickTestServerConnection(), true));
            }
        }

        private ICommand _clickRefreshConfigCommand;
        public ICommand ClickRefreshConfigCommand
        {
            get
            {
                return _clickRefreshConfigCommand ?? (_clickRefreshConfigCommand = new CommandHandler(() => ClickRefreshConfig(), true));
            }
        }

        #endregion Server ICommands

        #region ICommands

        private ICommand _windowClosing;

        public ICommand WindowClosing
        {
            get
            {
                return _windowClosing ?? (_windowClosing = new CommandHandler(() => OnWindowClosing(), true));
            }
        }

        private ICommand _clickGetCurrentDeviceNameCommand;
        public ICommand ClickGetCurrentDeviceNameCommand
        {
            get
            {
                return _clickGetCurrentDeviceNameCommand ?? (_clickGetCurrentDeviceNameCommand = new CommandHandler(() => ClickGetCurrentDeviceName(), true));
            }
        }

        private ICommand _clickGetCurrentDeviceFWVersionCommand;
        public ICommand ClickGetCurrentDeviceFWVersionCommand
        {
            get
            {
                return _clickGetCurrentDeviceFWVersionCommand ?? (_clickGetCurrentDeviceFWVersionCommand = new CommandHandler(() => ClickGetCurrentDeviceFWVersion(), true));
            }
        }

        private ICommand _clickGetCurrentDeviceAvailableFWVersionsCommand;
        public ICommand ClickGetCurrentDeviceAvailableFWVersionsCommand
        {
            get
            {
                return _clickGetCurrentDeviceAvailableFWVersionsCommand ?? (_clickGetCurrentDeviceAvailableFWVersionsCommand = new CommandHandler(() => ClickGetCurrentDeviceAvailableFWVersions(), true));
            }
        }

        private ICommand _clickGetCurrentDeviceLastFWVersionCommand;
        public ICommand ClickGetCurrentDeviceLastFWVersionCommand
        {
            get
            {
                return _clickGetCurrentDeviceLastFWVersionCommand ?? (_clickGetCurrentDeviceLastFWVersionCommand = new CommandHandler(() => ClickGetCurrentDeviceLastFWVersion(), true));
            }
        }

        private ICommand _clickSubscribeEventsCommand;
        public ICommand ClickSubscribeEventsCommand
        {
            get
            {
                return _clickSubscribeEventsCommand ?? (_clickSubscribeEventsCommand = new CommandHandler(() => ClickSubscribeEvents(), true));
            }
        }

        private ICommand _clickClearSubscriptionsCommand;
        public ICommand ClickClearSubscriptionsCommand
        {
            get
            {
                return _clickClearSubscriptionsCommand ?? (_clickClearSubscriptionsCommand = new CommandHandler(() => ClickClearSubscriptions(), true));
            }
        }

        private ICommand _clickOfflineModeCommand;
        public ICommand ClickOfflineModeCommand
        {
            get
            {
                return _clickOfflineModeCommand ?? (_clickOfflineModeCommand = new CommandHandler(() => ClickOfflineMode(), true));
            }
        }

        private ICommand _clickSendNoOpCommand;
        public ICommand ClickSendNoOpCommand
        {
            get
            {
                return _clickSendNoOpCommand ?? (_clickSendNoOpCommand = new CommandHandler(() => ClickSendNoOp(), true));
            }
        }

        private ICommand _clickGetSDKVersionAsyncCommand;
        public ICommand ClickGetSDKVersionAsyncCommand
        {
            get
            {
                return _clickGetSDKVersionAsyncCommand ?? (_clickGetSDKVersionAsyncCommand = new CommandHandler(() => ClickGetSDKVersionAsync(), true));
            }
        }

        private ICommand _clickCancelAsyncCallbackCommand;
        public ICommand ClickCancelAsyncCallbackCommand
        {
            get
            {
                return _clickCancelAsyncCallbackCommand ?? (_clickCancelAsyncCallbackCommand = new CommandHandler(() => ClickCancelAsyncCallback(), true));
            }
        }

        #endregion ICommands

        public OtherFunctionsWindowViewModel(GNBaseViewModel parent) : base(parent)
        {

        }

        internal OtherFunctionsWindowViewModel(GNBaseViewModel parent, ConnectedDeviceModel theDevice) : base(parent)
        {
            OtherFunctionsWindowModel = new OtherFunctionsWindowModel(theDevice != null ? theDevice.DeviceID : -1);
        }

        private void OnWindowClosing()
        {
            DialogService.CloseDialogs();
                        
        }

        #region Server Command Handlers

        public void ClickTestServerConnection()
        {
            try
            {
                OtherFunctionsWindowModel.ResultTestServerConnection = CommandResults.Checking;
                var res = GNAUpdaterApi.Instance.TestServerAccess(TestServerAccessHandler, 1);
                if (res != 0)
                {
                    OtherFunctionsWindowModel.ResultTestServerConnection = CommandResults.Error;
                }
            }
            catch (Exception ex)
            {
                DemoLogWriter.Error("Exception caught while trying to test server connection");
                DemoLogWriter.Error(DemoLogWriter.GetExceptionString(ex));
            }
        }

        public void TestServerAccessHandler(int resultCode, ulong context)
        {
            if (context == 1)
            {
                OtherFunctionsWindowModel.ResultTestServerConnection = resultCode == 0 ? CommandResults.OK : CommandResults.Error;
            }
            else
            {
                DemoLogWriter.Error("Invalid context returned by SDK.");
                MessageBoxHelper.ShowMessage($"Invalid context returned by SDK. Expected=1, Actual={context}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void ClickRefreshConfig()
        {
            try
            {
                OtherFunctionsWindowModel.ResultRefreshConfig = CommandResults.Checking;
                var res = GNAUpdaterApi.Instance.RefreshConfig(RefreshConfigHandler, 1);
                if (res != 0)
                {
                    OtherFunctionsWindowModel.ResultRefreshConfig = CommandResults.Error;
                }
            }
            catch (Exception ex)
            {
                DemoLogWriter.Error("Exception caught while trying to refresh config");
                DemoLogWriter.Error(DemoLogWriter.GetExceptionString(ex));
            }
        }

        public void RefreshConfigHandler(int resultCode, ulong context)
        {
            try
            {
                if (context == 1)
                {
                    OtherFunctionsWindowModel.ResultRefreshConfig = resultCode == 0 ? CommandResults.OK : CommandResults.Error;
                }
                else
                {
                    DemoLogWriter.Error("Invalid context returned by SDK.");
                    MessageBoxHelper.ShowMessage($"Invalid context returned by SDK. Expected=1, Actual={context}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                DemoLogWriter.Error("Exception caught while trying to process SDK refresh config callback");
                DemoLogWriter.Error(DemoLogWriter.GetExceptionString(ex));
            }
        }

        #endregion Server Command Handlers

        #region Command Handlers

        public void ClickGetCurrentDeviceName()
        {
            try
            {
                string deviceName;
                int res = GNAUpdaterApi.Instance.GetDeviceName(OtherFunctionsWindowModel.DeviceID, out deviceName);
                OtherFunctionsWindowModel.Name = deviceName;
            }
            catch (Exception ex)
            {
                DemoLogWriter.Error("Exception caught while trying to get current device name");
                DemoLogWriter.Error(DemoLogWriter.GetExceptionString(ex));
            }
        }

        public void ClickGetCurrentDeviceFWVersion()
        {
            try
            {
                string fwVersion;
                int res = GNAUpdaterApi.Instance.GetCurrentFirmwareVersion(OtherFunctionsWindowModel.DeviceID, out fwVersion);
                OtherFunctionsWindowModel.FWVersion = fwVersion;
            }
            catch (Exception ex)
            {
                DemoLogWriter.Error("Exception caught while trying to get current device FW version");
                DemoLogWriter.Error(DemoLogWriter.GetExceptionString(ex));
            }
        }

        public void ClickGetCurrentDeviceAvailableFWVersions()
        {
            try
            {
                string[] fwVersions;
                int res = GNAUpdaterApi.Instance.GetAvailableFirmwareVersionsForDevice(OtherFunctionsWindowModel.DeviceID, out fwVersions);
                OtherFunctionsWindowModel.FWVersions = String.Join(", ", fwVersions);
            }
            catch (Exception ex)
            {
                DemoLogWriter.Error("Exception caught while trying to get current device available FW versions");
                DemoLogWriter.Error(DemoLogWriter.GetExceptionString(ex));
            }
        }

        public void ClickGetCurrentDeviceLastFWVersion()
        {
            try
            {
                string lastFWVersion;
                int res = GNAUpdaterApi.Instance.GetLatestFirmwareVersion(OtherFunctionsWindowModel.DeviceID, out lastFWVersion);
                OtherFunctionsWindowModel.LastFWVersion = lastFWVersion;
            }
            catch (Exception ex)
            {
                DemoLogWriter.Error("Exception caught while trying to get current device latest FW version");
                DemoLogWriter.Error(DemoLogWriter.GetExceptionString(ex));
            }
        }

        public void ClickSubscribeEvents()
        {
            try
            {
                OtherFunctionsWindowModel.ResultSubscribeEvents = CommandResults.Checking;

                MainWindowViewModel mainViewModel = ParentViewModel as MainWindowViewModel;
                OtherFunctionsWindowModel.ResultSubscribeEvents = GNAUpdaterApi.Instance.SubscribeEvents(
                    mainViewModel.DeviceAttached,
                    mainViewModel.DeviceDetached,
                    mainViewModel.DevicePropertiesUpdatedCallback,
                    mainViewModel.DBUpdatedCallback,
                    mainViewModel.DefferedErrorCallback,
                    NoopCallback) == 0 ? CommandResults.OK : CommandResults.Error;

                mainViewModel.FillDevices();
            }
            catch (Exception ex)
            {
                DemoLogWriter.Error("Exception caught while trying to subscribe SDK event handlers");
                DemoLogWriter.Error(DemoLogWriter.GetExceptionString(ex));
            }
        }

        public void ClickClearSubscriptions()
        {
            try
            {
                OtherFunctionsWindowModel.ResultClearSubscriptions = CommandResults.Checking;
                OtherFunctionsWindowModel.ResultClearSubscriptions = GNAUpdaterApi.Instance.ClearSubscriptions() == 0 ? CommandResults.OK : CommandResults.Error;
                OtherFunctionsWindowModel.ResultSendNoOp = CommandResultsToStringConverter.getCommandResultsString(CommandResults.NA);
            }
            catch (Exception ex)
            {
                DemoLogWriter.Error("Exception caught while trying to clear SDK event handlers");
                DemoLogWriter.Error(DemoLogWriter.GetExceptionString(ex));
            }
        }

        public void ClickOfflineMode()
        {
            try
            {
                OtherFunctionsWindowModel.ResultOfflineMode = GNAUpdaterApi.Instance.OfflineModeEnabled ? "Enabled" : "Disabled";
            }
            catch (Exception ex)
            {
                DemoLogWriter.Error("Exception caught while trying to get SDK Offline Mode value");
                DemoLogWriter.Error(DemoLogWriter.GetExceptionString(ex));
            }
        }

        public void ClickSendNoOp()
        {
            try
            {
                int res = GNAUpdaterApi.Instance.SendNoOp("Hello World!");
            }
            catch (Exception ex)
            {
                DemoLogWriter.Error("Exception caught while trying to send No-Op request to SDK");
                DemoLogWriter.Error(DemoLogWriter.GetExceptionString(ex));
            }
        }

        public void NoopCallback(string text)
        {
            try
            {
                OtherFunctionsWindowModel.ResultSendNoOp = text;
            }
            catch (Exception ex)
            {
                DemoLogWriter.Error("Exception caught while trying to handle No-Op SDK callback");
                DemoLogWriter.Error(DemoLogWriter.GetExceptionString(ex));
            }
        }

        public void ClickGetSDKVersionAsync()
        {
            try
            {
                OtherFunctionsWindowModel.ResultGetSDKVersionAsync = CommandResultsToStringConverter.getCommandResultsString(CommandResults.Checking);
                int res = GNAUpdaterApi.GetSDKVersionAsync(GetSDKVersionAsyncCallback, 1);
            }
            catch (Exception ex)
            {
                DemoLogWriter.Error("Exception caught while trying to get SDK version");
                DemoLogWriter.Error(DemoLogWriter.GetExceptionString(ex));
            }
        }

        public void GetSDKVersionAsyncCallback(int resultCode, string version, ulong context)
        {
            try
            {
                if (context == 1)
                {
                    OtherFunctionsWindowModel.ResultGetSDKVersionAsync = version;
                }
                else
                {
                    DemoLogWriter.Error("Invalid context returned by SDK.");
                    MessageBoxHelper.ShowMessage($"Invalid context returned by SDK. Expected=1, Actual={context}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                DemoLogWriter.Error("Exception caught while trying to handle get SDK version callback");
                DemoLogWriter.Error(DemoLogWriter.GetExceptionString(ex));
            }
        }

        public void ClickCancelAsyncCallback()
        {
            try
            {
                OtherFunctionsWindowModel.ResultCancelAsyncCallback = GNAUpdaterApi.Instance.CancelAsyncCallback(1) == 0 ? CommandResults.OK : CommandResults.Error;
            }
            catch (Exception ex)
            {
                DemoLogWriter.Error("Exception caught while trying to cancel SDK async callbacks");
                DemoLogWriter.Error(DemoLogWriter.GetExceptionString(ex));
            }
        }

        #endregion Command Handlers

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
