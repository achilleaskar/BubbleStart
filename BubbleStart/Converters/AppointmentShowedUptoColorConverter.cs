using System;
using System.Globalization;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using BubbleStart.Helpers;
using BubbleStart.Model;

namespace BubbleStart.Converters
{
    public class AppointmentShowedUptoColorConverter : IValueConverter
    {
        #region Methods

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Apointment a)
            {
                if (a.Customer != null)
                {
                    if (a.DateTime < DateTime.Now && !a.Customer.ShowUps.Any(s => s.Arrived.Date == a.DateTime.Date))
                        return new SolidColorBrush(Colors.Red);
                }
                else
                {

                }
                if (parameter == null || parameter is RadioButton b && b.IsChecked == true)
                {

                    if (a.Person == SelectedPersonEnum.Gogo)
                        return new SolidColorBrush(Colors.LimeGreen);
                    if (a.Person == SelectedPersonEnum.Functional)
                        return new SolidColorBrush(Colors.Orange);
                    if (a.Person == SelectedPersonEnum.Yoga)
                        return new SolidColorBrush(Colors.LightBlue);
                    if (a.Person == SelectedPersonEnum.Massage)
                        return new SolidColorBrush(Colors.HotPink);
                    if (a.Person == SelectedPersonEnum.Online)
                        return new SolidColorBrush(Colors.Yellow);
                    if (a.Person == SelectedPersonEnum.Personal)
                        return new SolidColorBrush(Colors.Cyan);
                    if (a.Person == SelectedPersonEnum.PilatesMat)
                        return new SolidColorBrush(Colors.Pink);
                }
                else
                {
                    if (a.Gymnast == null)
                        return new SolidColorBrush(Colors.Transparent);
                    if (a.Gymnast == null)
                        return new SolidColorBrush(Colors.White);
                }
            }
            return new SolidColorBrush(Colors.Transparent);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();

        #endregion Methods
    }
}