using System;
using System.Globalization;
using System.Windows.Data;

namespace BubbleStart.Converters
{
    public class BoolToShowedUp_Converter : IValueConverter
    {
        #region Methods

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool b)
            {
                if (b)
                {
                    return "ΝΑΙ";
                }
                return "ΌΧΙ";
            }
            return "Error";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();

        #endregion Methods
    }
}