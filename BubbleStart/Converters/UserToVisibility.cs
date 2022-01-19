using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using BubbleStart.Helpers;

namespace BubbleStart.Converters
{
    public class UserToVisibility : IValueConverter
    {
        #region Methods

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null && value is int)
            {
                if (StaticResources.User.Level <= (int)value)
                    return Visibility.Visible;
                return Visibility.Collapsed;
            }
            if (parameter == null && value is string)
            {
                if (string.IsNullOrWhiteSpace((string)value))
                    return Visibility.Collapsed;
                return Visibility.Visible;
            }
            if (StaticResources.User != null && parameter != null)
            {
                //if (value is Reservation r && StaticResources.User.Level == StaticResources.UserLevel.OfficeManager)
                //    if (r.User.Grafeio == StaticResources.User.Grafeio)
                //        return Visibility.Visible;
                //    else
                //        return Visibility.Collapsed;
                if (StaticResources.User.Level <= int.Parse(parameter.ToString()))
                    return Visibility.Visible;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();

        #endregion Methods
    }
}