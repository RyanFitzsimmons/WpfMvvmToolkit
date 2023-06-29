using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows;

namespace WpfMvvmToolkit.Converters;
public class DateOnlyConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var dateTime = (DateOnly?)value;
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
        DateOnly date;
        if (DateOnly.TryParseExact((string)value, format, culture, DateTimeStyles.None, out date))
        {
            return date;
        }
        return DependencyProperty.UnsetValue;
    }
}
