using GNAUpdaterSDK_Demo.Converters;
using GNAUpdaterSDK_Demo.Helpers;
using System.ComponentModel;

namespace GNAUpdaterSDK_Demo.Models
{
    public class OtherFunctionsWindowModel : INotifyPropertyChanged
    {
        public OtherFunctionsWindowModel(int theDeviceID)
        {
            DeviceID = theDeviceID;
        }

        private int _deviceID = -1;
        public int DeviceID
        {
            get { return _deviceID; }
            set
            {
                _deviceID = value;
                OnPropertyChanged("DeviceId");
            }
        }

        public bool HasDevice
        {
            get { return DeviceID != -1; }
        }

        public bool HasDeviceAndInitialized
        {
            get { return HasDevice && GeneralHelper.ApiInitialized; }
        }

        private string _name = CommandResultsToStringConverter.getCommandResultsString(CommandResults.NA);
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        private string _fwVersion = CommandResultsToStringConverter.getCommandResultsString(CommandResults.NA);
        public string FWVersion
        {
            get { return _fwVersion; }
            set
            {
                _fwVersion = value;
                OnPropertyChanged("FWVersion");
            }
        }

        private string _fwVersions = CommandResultsToStringConverter.getCommandResultsString(CommandResults.NA);
        public string FWVersions
        {
            get { return _fwVersions; }
            set
            {
                _fwVersions = value;
                OnPropertyChanged("FWVersions");
            }
        }

        private string _lastFWVersion = CommandResultsToStringConverter.getCommandResultsString(CommandResults.NA);
        public string LastFWVersion
        {
            get { return _lastFWVersion; }
            set
            {
                _lastFWVersion = value;
                OnPropertyChanged("LastFWVersion");
            }
        }

        private CommandResults _resultTestServerConnection = CommandResults.NA;
        public CommandResults ResultTestServerConnection
        {
            get { return _resultTestServerConnection; }
            set
            {
                _resultTestServerConnection = value;
                OnPropertyChanged("ResultTestServerConnection");
            }
        }

        private CommandResults _resultRefreshConfig = CommandResults.NA;
        public CommandResults ResultRefreshConfig
        {
            get { return _resultRefreshConfig; }
            set
            {
                _resultRefreshConfig = value;
                OnPropertyChanged("ResultRefreshConfig");
            }
        }

        private CommandResults _resultSubscribeEvents = CommandResults.NA;
        public CommandResults ResultSubscribeEvents
        {
            get { return _resultSubscribeEvents; }
            set
            {
                _resultSubscribeEvents = value;
                OnPropertyChanged("ResultSubscribeEvents");
            }
        }

        private CommandResults _resultClearSubscriptions = CommandResults.NA;
        public CommandResults ResultClearSubscriptions
        {
            get { return _resultClearSubscriptions; }
            set
            {
                _resultClearSubscriptions = value;
                OnPropertyChanged("ResultClearSubscriptions");
            }
        }

        private string _resultOfflineMode = CommandResultsToStringConverter.getCommandResultsString(CommandResults.NA);
        public string ResultOfflineMode
        {
            get { return _resultOfflineMode; }
            set
            {
                _resultOfflineMode = value;
                OnPropertyChanged("ResultOfflineMode");
            }
        }

        private string _resultSendNoOp = CommandResultsToStringConverter.getCommandResultsString(CommandResults.NA);
        public string ResultSendNoOp
        {
            get { return _resultSendNoOp; }
            set
            {
                _resultSendNoOp = value;
                OnPropertyChanged("ResultSendNoOp");
            }
        }

        private string _resultGetSDKVersionAsync = CommandResultsToStringConverter.getCommandResultsString(CommandResults.NA);
        public string ResultGetSDKVersionAsync
        {
            get { return _resultGetSDKVersionAsync; }
            set
            {
                _resultGetSDKVersionAsync = value;
                OnPropertyChanged("ResultGetSDKVersionAsync");
            }
        }

        private CommandResults _resultCancelAsyncCallback = CommandResults.NA;
        public CommandResults ResultCancelAsyncCallback
        {
            get { return _resultCancelAsyncCallback; }
            set
            {
                _resultCancelAsyncCallback = value;
                OnPropertyChanged("ResultCancelAsyncCallback");
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

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

    public enum CommandResults
    {
        Error,
        OK,
        NA,
        Checking
    }
}
