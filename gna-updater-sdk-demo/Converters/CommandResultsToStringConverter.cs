using GNAUpdaterSDK_Demo.Models;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace GNAUpdaterSDK_Demo.Converters
{
    public class CommandResultsToStringConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is CommandResults val)
            {
                return getCommandResultsString(val);
            }
            else
            {
                return value.ToString();
            }
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

        public static string getCommandResultsString(CommandResults val)
        {
            if (val == CommandResults.NA)
            {
                return "N/A";
            }

            return val.ToString();
        }
    }
}
