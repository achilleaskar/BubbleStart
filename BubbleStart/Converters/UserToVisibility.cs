﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace BubbleStart.Converters
{
    public class UserToVisibility : IValueConverter
    {
        #region Methods

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null && value is int)
            {
                if (Helpers.StaticResources.User.Level <= (int)value)
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }
            if (Helpers.StaticResources.User != null && parameter != null)
            {
                //if (value is Reservation r && StaticResources.User.Level == StaticResources.UserLevel.OfficeManager)
                //    if (r.User.Grafeio == StaticResources.User.Grafeio)
                //        return Visibility.Visible;
                //    else
                //        return Visibility.Collapsed;
                if (Helpers.StaticResources.User.Level <= int.Parse(parameter.ToString()))
                    return Visibility.Visible;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();

        #endregion Methods
    }
}