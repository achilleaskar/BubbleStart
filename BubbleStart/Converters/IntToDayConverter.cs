using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace BubbleStart.Converters
{
    public class IntToDayConverter : IValueConverter
    {
        #region Methods

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int i && i >= 0 && i < 7)
            {
                if (parameter is string s)
                {
                    return s.Replace("_{0}", Enum.GetName(typeof(DayOfWeek), i));

                }
                return Enum.GetName(typeof(DayOfWeek), i);
            }
            return "ERROR";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();

        #endregion Methods
    }
}