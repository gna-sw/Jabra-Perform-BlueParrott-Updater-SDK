using GNAUpdaterSDK;
using System.ComponentModel;
using System.Windows.Media;

namespace GNAUpdaterSDK_Demo.Models
{
    public class InfoWindowModel : INotifyPropertyChanged
    {
        public InfoWindowModel(Device theDevice, ImageSource theImageSource)
        {
            DeviceID = theDevice.DeviceID;
            Name = theDevice.Name;
            FWVersion = theDevice.FWVersion;

            ProductID = theDevice.DeviceProperties.ProductId;
            VendorID = theDevice.DeviceProperties.VendorId;
            DeviceLocation = theDevice.DeviceProperties.DeviceLocation;
            DevicePath = theDevice.DeviceProperties.DevicePath;
            DeviceClass = theDevice.DeviceProperties.DeviceClass;
            ContainerID = theDevice.DeviceProperties.ContainerID;

            ImageSource = theImageSource;
        }

        public InfoWindowModel() { }

        private int _deviceID;
        public int DeviceID
        {
            get { return _deviceID; }
            set
            {
                _deviceID = value;
                OnPropertyChanged("DeviceId");
            }
        }

        private string _name = string.Empty;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        private string _fwVersion = string.Empty;
        public string FWVersion
        {
            get { return _fwVersion; }
            set
            {
                _fwVersion = value;
                OnPropertyChanged("FWVersion");
            }
        }

        private int _productID = -1;
        public int ProductID
        {
            get { return _productID; }
            set
            {
                _productID = value;
                OnPropertyChanged("ProductID");
            }
        }


        private int _vendorID = -1;
        public int VendorID
        {
            get { return _vendorID; }
            set
            {
                _vendorID = value;
                OnPropertyChanged("VendorID");
            }
        }

        private string _deviceLocation = string.Empty;
        public string DeviceLocation
        {
            get { return _deviceLocation; }
            set
            {
                _deviceLocation = value;
                OnPropertyChanged("DeviceLocation");
            }
        }

        private string _devicePath = string.Empty;
        public string DevicePath
        {
            get { return _devicePath; }
            set
            {
                _devicePath = value;
                OnPropertyChanged("DevicePath");
            }
        }

        private string _deviceClass = string.Empty;
        public string DeviceClass
        {
            get { return _deviceClass; }
            set
            {
                _deviceClass = value;
                OnPropertyChanged("DeviceClass");
            }
        }

        private string _containerID = string.Empty;
        public string ContainerID
        {
            get { return _containerID; }
            set
            {
                _containerID = value;
                OnPropertyChanged("ContainerID");
            }
        }

        private ImageSource _imageSource;
        public ImageSource ImageSource
        {
            get { return _imageSource; }
            set
            {
                _imageSource = value;
                OnPropertyChanged("ImageSource");
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
