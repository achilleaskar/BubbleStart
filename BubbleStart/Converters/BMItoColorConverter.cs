using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace BubbleStart.Converters
{
    public class BMItoColorConverter : IValueConverter
    {
        #region Methods

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is float)
            {
                if ((float)value < 18.5 || ((float)value >= 25 && ((float)value < 30)))
                    return new SolidColorBrush(Colors.Orange);
                else if ((float)value >= 30)
                    return new SolidColorBrush(Colors.Red);
            }

            return new SolidColorBrush(Colors.Green);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();

        #endregion Methods
    }
}