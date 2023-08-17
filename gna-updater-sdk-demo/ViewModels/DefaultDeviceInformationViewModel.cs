using GNAUpdaterSDK_Demo.Models;
using System.ComponentModel;

namespace GNAUpdaterSDK_Demo.ViewModels
{
    public class DefaultDeviceInformationViewModel : GNBaseViewModel
    {
        private DefaultDeviceInformationModel _defaultDeviceInformation;

        public DefaultDeviceInformationModel DefaultDeviceInformation
        {
            get { return _defaultDeviceInformation; }
            set
            {
                _defaultDeviceInformation = value;
                OnPropertyChanged("DefaultDeviceInformation");
            }
        }
        public DefaultDeviceInformationViewModel(GNBaseViewModel parent) : base(parent)
        {
            _defaultDeviceInformation = new DefaultDeviceInformationModel();
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
