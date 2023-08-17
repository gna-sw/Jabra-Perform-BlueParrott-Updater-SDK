using GNAUpdaterSDK_Demo.Logger;
using GNAUpdaterSDK_Demo.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GNAUpdaterSDK_Demo.Helpers
{
    public class GeneralHelper
    {
        private static bool _apiInitialized = false;
        public static bool ApiInitialized
        {
            get { return _apiInitialized; }
            set
            {
                _apiInitialized = value;
                RaiseStaticPropertyChanged("ApiInitialized");
            }
        }

        public static ImageSource SetImageSource()
        {
            ImageSource imageSource = null;

            try
            {
                imageSource = ToWpfBitmap(Resources.Default_380x380);
            }
            catch (Exception ex)
            {
                DemoLogWriter.Error(ex.ToString());
            }

            return imageSource;
        }

        public static BitmapImage GetBitmapFromByteArray(byte[] source)
        {
            using (var byteStream = new MemoryStream(source))
            {
                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = byteStream;
                bitmapImage.EndInit();
                bitmapImage.Freeze();
                return bitmapImage;
            }
        }

        public static BitmapSource ToWpfBitmap(Bitmap bitmap)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                bitmap.Save(stream, ImageFormat.Bmp);

                stream.Position = 0;
                BitmapImage result = new BitmapImage();

                result.BeginInit();
                // According to MSDN, "The default OnDemand cache option retains access to the stream until the image is needed."
                // Force the bitmap to load right now so we can dispose the stream.
                result.CacheOption = BitmapCacheOption.OnLoad;
                result.StreamSource = stream;
                result.EndInit();

                result.Freeze();
                return result;
            }
        }

        #region RaiseStaticPropertyChanged Members

        public static event EventHandler<PropertyChangedEventArgs> StaticPropertyChanged;
        public static void RaiseStaticPropertyChanged(string propName)
        {
            EventHandler<PropertyChangedEventArgs> handler = StaticPropertyChanged;
            if (handler != null)
                handler(null, new PropertyChangedEventArgs(propName));
        }

        #endregion
    }
}
