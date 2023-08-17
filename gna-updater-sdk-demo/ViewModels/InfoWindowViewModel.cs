using GNAUpdaterSDK;
using GNAUpdaterSDK_Demo.Logger;
using GNAUpdaterSDK_Demo.Models;
using GNAUpdaterSDK_Demo.Utils;
using System.ComponentModel;

namespace GNAUpdaterSDK_Demo.ViewModels
{
    public class InfoWindowViewModel : GNBaseViewModel
    {
        #region Properties

        private InfoWindowModel _infoWindowModel;
        public InfoWindowModel InfoWindowModel
        {
            get { return _infoWindowModel; }
            set
            {
                _infoWindowModel = value;
                OnPropertyChanged("InfoWindowModel");
            }
        }

        #endregion

        public InfoWindowViewModel(GNBaseViewModel parent) : base(parent)
        {

        }

        internal InfoWindowViewModel(GNBaseViewModel parent, ConnectedDeviceModel theDevice) : base(parent)
        {
            string resDevice;
            GNAUpdaterApi.Instance.GetHeadsetDetails(theDevice.DeviceID, out resDevice);

            if (resDevice == null)
            {
                DemoLogWriter.Error($"Unable to find device with deviceID={theDevice.DeviceID}");
                InfoWindowModel = new InfoWindowModel();
            }
            else
            {
                Device devDetails = SerializationHelper.DeserializeXmlString<Device>(resDevice);
                InfoWindowModel = new InfoWindowModel(devDetails, theDevice.ImageSource);
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
