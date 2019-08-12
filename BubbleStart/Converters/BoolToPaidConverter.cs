using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace BubbleStart.Converters
{
    public class BoolToPaidConverter       : IValueConverter
    {
        #region Methods

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool b)
            {
                if (b)
                {
                    return "ΑΓΟΡΑ";
                }
                return "ΠΙΣΤΩΣΗ";
            }
            return "Error";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();

        #endregion Methods
    }
}