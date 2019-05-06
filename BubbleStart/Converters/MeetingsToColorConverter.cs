using BubbleStart.Model;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace BubbleStart.Converters
{
    public class MeetingsToColorConverter : IMultiValueConverter
    {
        #region Methods

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is int a && values[1] is decimal b)
            {
                if (a <= 0)
                    return new SolidColorBrush(Colors.Red);
                //else if(b > 0)
                //    return new SolidColorBrush(Colors.Orange);
                else 
                    return new SolidColorBrush(Colors.LightGreen);
            }

            return new SolidColorBrush(Colors.Transparent);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion Methods
    }
}