using BubbleStart.ViewModels;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace BubbleStart.Converters
{
    public class HourClosedToColorConverter : IValueConverter
    {
        #region Methods

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Hour h && parameter is string p)
            {
                if ((h.ClosedHour0 != null && p == "0") || (h.ClosedHour1 != null && p == "1"))
                {
                    return new SolidColorBrush(Colors.Red);
                }
                else
                {
                    return p == "0" ? new SolidColorBrush(Colors.LightYellow) : new SolidColorBrush(Colors.BlanchedAlmond);
                }
            }
            return new SolidColorBrush(Colors.BlanchedAlmond);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();

        #endregion Methods
    }
}