using GNAUpdaterSDK_Demo.Helpers;
using GNAUpdaterSDK_Demo.Logger;
using System;
using System.IO;
using System.Windows;

namespace GNAUpdaterSDK_Demo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                if (ex.GetBaseException() is FileNotFoundException inner)
                {
                    DemoLogWriter.Error(ex.ToString());
                    MessageBoxHelper.ShowMessage($"Could not locate dependency file: {inner.FileName}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    Application.Current.MainWindow.Close();
                }
                else
                {
                    throw;
                }
            }
        }
    }
}
