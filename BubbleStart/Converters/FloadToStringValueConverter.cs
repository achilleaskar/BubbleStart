using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace BubbleStart.Converters
{
    [ValueConversion(typeof(decimal), typeof(string))]
    public class FloadToStringValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            decimal decimalType = (decimal)value;
            return decimalType.ToString()+" €";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string strValue = (value as string).Replace(',', '.').Replace("€","").Replace(" ","");
            if (!string.IsNullOrEmpty(strValue) && strValue[strValue.Length - 1] != '.' && decimal.TryParse(strValue, NumberStyles.Any, new CultureInfo("en-US"), out var tmpdecimal))
            {
                return tmpdecimal;
            }
            return DependencyProperty.UnsetValue;
        }
    }
}