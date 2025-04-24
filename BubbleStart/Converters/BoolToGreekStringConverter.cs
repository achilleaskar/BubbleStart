using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace BubbleStart.Converters
{
    public class BoolToGreekStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return boolValue ? "ΝΑΙ" : "ΟΧΙ";
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ComboBoxItem cbi && cbi.Content is string s)
            {
                switch (s)
                {
                    case "ΝΑΙ":
                        return true;
                    case "ΟΧΙ":
                        return false;
                    default:
                        return null;
                }
            }
            return null;
        }
    }
}
