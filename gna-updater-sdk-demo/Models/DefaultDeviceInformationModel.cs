using System.ComponentModel;

namespace GNAUpdaterSDK_Demo.Models
{
    public class DefaultDeviceInformationModel : INotifyPropertyChanged
    {
        private string _defaultMsg = "There are no connected devices";
        public string DefaultMsg
        {
            get { return _defaultMsg; }
        }

        private int _deviceId = -1;
        public int DeviceId
        {
            get { return _deviceId; }
            set
            {
                _deviceId = value;
                OnPropertyChanged("DeviceId");
            }
        }

        private string _deviceName = "No Device";
        public string DeviceName
        {
            get { return _deviceName; }
            set
            {
                _deviceName = value;
                OnPropertyChanged("DeviceName");
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
}
