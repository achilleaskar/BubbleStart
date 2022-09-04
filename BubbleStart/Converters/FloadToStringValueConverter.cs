﻿using System;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace BubbleStart.Converters
{
    [ValueConversion(typeof(decimal), typeof(string))]
    public class FloadToStringValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            decimal decimalValue = (decimal)value;

            var decimalString = decimalValue.ToString();
            string s = decimalString.IndexOfAny(new[] { ',', '.' }) >= 0 ? decimalString.TrimEnd('0').TrimEnd('0').TrimEnd('.').TrimEnd(',') : decimalString;
            int indexofcomma = s.IndexOf(',');
            if (indexofcomma <= 0)
            {
                indexofcomma = s.Length;
            }
            if (indexofcomma > 3)
            {
                int firstDot = indexofcomma - 3;
                while (firstDot > 0)
                {
                    s = s.Insert(firstDot, ".");
                    firstDot -= 3;
                }

            }

            return s + " €";
            //return decimalValue > 0 ? decimalValue.ToString() + " €" : "0 €";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string strValue = (value as string)
                .Replace("€", "").Replace(" ", "");
            if (strValue.EndsWith(".") || strValue.EndsWith(","))
                return DependencyProperty.UnsetValue;
            int lastsindexofdot = strValue.LastIndexOf('.');
            if (lastsindexofdot > 0 && strValue.Length - lastsindexofdot <= 3)
            {
                StringBuilder sb = new StringBuilder(strValue);
                sb[lastsindexofdot] = '~';
                sb = sb.Replace(".", "").Replace(',', '.').Replace('~', '.');
                strValue = sb.ToString();
            }
            else
            {
                strValue = strValue.Replace(".", "").Replace(',', '.').Replace("€", "").Replace(" ", "");
            }
            if (!string.IsNullOrEmpty(strValue) && !strValue.EndsWith(".0") && decimal.TryParse(strValue, NumberStyles.Any, new CultureInfo("en-US"), out var tmpdecimal))
            {
                return decimal.Round(tmpdecimal, 2);
            }
            return DependencyProperty.UnsetValue;
        }
    }
}