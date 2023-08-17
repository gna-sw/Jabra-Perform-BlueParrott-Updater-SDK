using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace GNAUpdaterSDK_Demo.Converters
{
    public class EmptyStringConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType,
                              object parameter, CultureInfo culture)
        {
            string result;

            if (value is int)
            {
                result = value.ToString();
            }
            else
            {
                result = value as string;
            }

            return string.IsNullOrEmpty(result) ? parameter : result;
        }

        public object ConvertBack(object value, Type targetType,
                                  object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
