using System.ComponentModel;

namespace GNAUpdaterSDK_Demo.Models
{
    public class MainWindowModel : INotifyPropertyChanged
    {
        public MainWindowModel()
        {

        }

        private string _sdkVersion = "SDK Version: 0";
        public string SdkVersion
        {
            get { return _sdkVersion; }
            set
            {
                _sdkVersion = "SDK Version: " + value;
                OnPropertyChanged("SdkVersion");
            }
        }

        private string _demoVersion = "Demo Version: 0";
        public string DemoVersion
        {
            get { return _demoVersion; }
            set
            {
                _demoVersion = "Demo Version: " + value;
                OnPropertyChanged("DemoVersion");
            }
        }

        private string _firmwareFile = string.Empty;
        public string FirmwareFile
        {
            get { return _firmwareFile; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    FirmwareFileIsEmpty = true;
                }
                else
                {
                    FirmwareFileIsEmpty = false;
                }

                _firmwareFile = value;
                OnPropertyChanged("FirmwareFile");
            }
        }

        private bool _firmwareFileIsEmpty = true;
        public bool FirmwareFileIsEmpty
        {
            get { return _firmwareFileIsEmpty; }
            set
            {
                _firmwareFileIsEmpty = value;
                OnPropertyChanged("FirmwareFileIsEmpty");
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
