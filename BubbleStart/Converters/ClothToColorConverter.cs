using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using BubbleStart.Helpers;
using BubbleStart.Model;

namespace BubbleStart.Converters
{
    internal class ClothToColorConverter : IValueConverter
    {
        #region Methods

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ItemPurchase item)
            {
                if (item.Item.Id == 3)
                {
                    return new SolidColorBrush(Colors.White);
                    
                }
                switch (item.Color)
                {
                    case ClothColors.mavro:
                        return new SolidColorBrush(Colors.Black);

                    case ClothColors.aspro:
                        return new SolidColorBrush(Colors.White);

                    case ClothColors.prasino:
                        return new SolidColorBrush(Colors.Green);

                    case ClothColors.gri:
                        return new SolidColorBrush(Colors.Gray);

                    default:
                        return new SolidColorBrush(Colors.Pink);
                }
            }
            return new SolidColorBrush(Colors.Pink);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();

        #endregion Methods
    }
}