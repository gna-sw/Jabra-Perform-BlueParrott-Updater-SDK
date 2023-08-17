using GNAUpdaterSDK_Demo.Models;
using GNAUpdaterSDK_Demo.ViewModels;
using GNAUpdaterSDK_Demo.Views;

namespace GNAUpdaterSDK_Demo.Services
{
    internal interface IWindowService
    {
        void CreateInfoWindow(ConnectedDeviceModel dataContext);
        void CreateOtherFunctionsWindow(GNBaseViewModel parent, ConnectedDeviceModel dataContext);
    }

    public class WindowService : IWindowService
    {
        public void CreateInfoWindow(ConnectedDeviceModel dataContext)
        {
            InfoWindowView configWindow = new InfoWindowView
            {
                DataContext = new InfoWindowViewModel(null, dataContext)
            };

            configWindow.ShowDialog();
        }

        public void CreateConfigurationWindow(ConnectedDeviceModel dataContext)
        {
            ConfigurationWindowView configWindow = new ConfigurationWindowView
            {
                DataContext = new ConfigurationWindowViewModel(null, dataContext)
            };
            configWindow.Owner = App.Current.MainWindow;
            configWindow.Show();
        }


        public void CreateOtherFunctionsWindow(GNBaseViewModel parent, ConnectedDeviceModel dataContext)
        {
            OtherFunctionsWindowView configWindow = new OtherFunctionsWindowView
            {
                DataContext = new OtherFunctionsWindowViewModel(parent, dataContext)
            };

            configWindow.ShowDialog();
        }
    }
}
