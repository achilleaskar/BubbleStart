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
            if (parameter == null && value is int i)
            {
                if (StaticResources.User.Level <= i)
                    return Visibility.Visible;
                return Visibility.Collapsed;
            }
            if (parameter == null && value is string s)
            {
                if (string.IsNullOrWhiteSpace(s))
                    return Visibility.Collapsed;
                return Visibility.Visible;
            }
            if (parameter is string sa)
            {
                if (sa.Contains("a"))//afro
                {
                    if (StaticResources.User?.Id == 36)
                    {
                        return Visibility.Visible;
                    }
                    else
                    {
                        sa = sa.Replace("a", "");
                    }
                }

                if (StaticResources.User != null && parameter != null && StaticResources.User.Level <= int.Parse(sa))
                    return Visibility.Visible;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();

        #endregion Methods
    }
}