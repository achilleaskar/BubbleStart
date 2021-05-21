using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using BubbleStart.Helpers;

namespace BubbleStart.Converters
{
    public class IndexToENumConverter : IValueConverter
    {
        #region Methods

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return System.Convert.ToInt32(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => (ExpenseCategory)System.Convert.ToInt32(value);

        #endregion Methods
    }
}