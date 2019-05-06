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
            if (value is decimal)
            {
                if ((decimal)value < 18.5m || ((decimal)value >= 25 && ((decimal)value < 30)))
                    return new SolidColorBrush(Colors.Orange);
                else if ((decimal)value >= 30)
                    return new SolidColorBrush(Colors.Red);
            }

            return new SolidColorBrush(Colors.Green);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();

        #endregion Methods
    }
}