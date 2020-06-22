using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using BubbleStart.Model;

namespace BubbleStart.Converters
{
    public class RemainingAmountToColorConverter : IValueConverter
    {
        #region Methods

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Program p && p != null)
            {
                if (p.PaidCol)
                {
                    return new SolidColorBrush(Colors.Green);
                }
            }

            return new SolidColorBrush(Colors.Red);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();

        #endregion Methods
    }
}