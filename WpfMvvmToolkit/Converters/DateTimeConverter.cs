using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WpfMvvmToolkit.Converters
{
    public class DateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var dateTime = (DateTime?)value;
            if (!dateTime.HasValue)
            {
                return string.Empty;
            }
            var format = (string)parameter;
            return dateTime.Value.ToString(format, culture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var format = (string)parameter;
            DateTime dateTime;
            if (DateTime.TryParseExact((string)value, format, culture, DateTimeStyles.None, out dateTime))
            {
                return dateTime;
            }
            return DependencyProperty.UnsetValue;
        }
    }
}
