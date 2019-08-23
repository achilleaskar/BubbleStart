using BubbleStart.Model;
using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Media;

namespace BubbleStart.Converters
{
    public class AppointmentShowedUptoColorConverter : IValueConverter
    {
        #region Methods

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Apointment a)
            {
                if (a.DateTime < DateTime.Now && !a.Customer.ShowUps.Any(s => s.Arrived.Date == a.DateTime.Date))
                    return new SolidColorBrush(Colors.Red);
                else if (a.Person == 1)
                    return new SolidColorBrush(Colors.Orange);
                else if (a.Person == 2)
                    return new SolidColorBrush(Colors.White);

            }
            return new SolidColorBrush(Colors.Transparent);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();

        #endregion Methods
    }
}