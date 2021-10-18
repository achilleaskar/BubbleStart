using System;
using System.Globalization;
using System.Windows.Data;

namespace BubbleStart.Converters
{
    public class DoubleParameterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new object[] { value, parameter };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}