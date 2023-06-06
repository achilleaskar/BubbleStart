using System;
using System.Globalization;
using System.Windows.Data;

namespace BubbleStart.Converters
{
    public class RatingToStatConverter : IValueConverter
    {
        #region Methods

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double d && parameter is string p && int.TryParse(p, out int i))
            {
                if (d >= i)
                {
                    return "StarSolid";

                }
                if (d + 0.5 == i)
                {
                    return "StarHalfSolid";
                }
            }
            return "StarRegular";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();

        #endregion Methods
    }
}