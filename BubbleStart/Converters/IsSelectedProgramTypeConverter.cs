using BubbleStart.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace BubbleStart.Converters
{
    public class IsSelectedProgramTypeConverter : IValueConverter
    {
        #region Methods

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Customer && parameter is string&& Int32.TryParse((string)parameter,out int r))
            {
                return ((Customer)value).DefaultProgramMode == r+1;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime datet;
            if (value is string date && parameter is string par)
            {
                if (!DateTime.TryParseExact(date, par, CultureInfo.CurrentUICulture, DateTimeStyles.None, out datet))
                {
                    return null;
                }
                return datet;
            }
            return null;
        }

        #endregion Methods
    }
}