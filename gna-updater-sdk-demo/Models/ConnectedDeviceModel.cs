using GNAUpdaterSDK;
using GNAUpdaterSDK_Demo.Helpers;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace GNAUpdaterSDK_Demo.Models
{
    public class ConnectedDeviceModel : INotifyPropertyChanged
    {
        public ConnectedDeviceModel(Device theDevice)
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

            BackgroundColor = ColorHelper.NonActiveConnectedDevicesControlColor;
            BorderColor = ColorHelper.NonActiveConnectedDevicesControlColor;

            byte[] imageBytes;
            if (GNAUpdaterApi.Instance.GetHeadsetImage(theDevice.DeviceID, out imageBytes) == 0)
            {
                ImageSource = GeneralHelper.GetBitmapFromByteArray(imageBytes);
            }
            else
            {
                ImageSource = GeneralHelper.SetImageSource();
                //Force retry getting image after a second
                Application.Current.Dispatcher.BeginInvoke(new Action(async () =>
                {
                    var imageFound = false;
                    while (!imageFound)
                    {
                        await Task.Delay(1000).ContinueWith((res) =>
                        {
                            if (GNAUpdaterApi.Instance.GetHeadsetImage(theDevice.DeviceID, out imageBytes) == 0)
                            {
                                ImageSource = GeneralHelper.GetBitmapFromByteArray(imageBytes);
                                imageFound = true;
                            }
                        });
                    }
                }));
            }
            
        }

        public ConnectedDeviceModel() { }

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

        private Brush _backgroundColor;
        public Brush BackgroundColor
        {
            get { return _backgroundColor; }
            set
            {
                _backgroundColor = value;
                OnPropertyChanged("BackgroundColor");
            }
        }

        private Brush _borderColor;
        public Brush BorderColor
        {
            get { return _borderColor; }
            set
            {
                _borderColor = value;
                OnPropertyChanged("BorderColor");
            }
        }

        #region Progress Bar

        private int _progressBarValue = 0;
        public int ProgressBarValue
        {
            get { return _progressBarValue; }
            set
            {
                if (value > 100)
                {
                    value = 100;
                }
                else if (value < 0)
                {
                    value = 0;
                }

                _progressBarValue = value;
                OnPropertyChanged("ProgressBarValue");
            }
        }

        private string _progressBarStatus = "Update status";
        public string ProgressBarStatus
        {
            get { return _progressBarStatus; }
            set
            {
                _progressBarStatus = value;
                OnPropertyChanged("ProgressBarStatus");
            }
        }

        private bool _progressBarIsEnabled = false;
        public bool ProgressBarIsEnabled
        {
            get { return _progressBarIsEnabled; }
            set
            {
                _progressBarIsEnabled = value;
                OnPropertyChanged("ProgressBarIsEnabled");
            }
        }

        public void StartProgressBar()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                ProgressBarIsEnabled = true;
                ProgressBarValue = 0;
                ProgressBarStatus = "Starting Update...";
            });
        }

        public void SetProgressBarStatus(int progress, string status)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                if (ProgressBarIsEnabled)
                {
                    ProgressBarValue = progress;
                    ProgressBarStatus = status;
                }
            });
        }

        public void StopProgressBar(string status)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                ProgressBarIsEnabled = false;
                ProgressBarValue = 100;
                ProgressBarStatus = status;
            });
        }

        #endregion

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
