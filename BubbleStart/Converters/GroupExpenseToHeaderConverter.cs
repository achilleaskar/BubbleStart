using BubbleStart.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace BubbleStart.Converters
{
    public class GroupExpenseToHeaderConverter : IValueConverter
    {
        #region Methods

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if (value is IGrouping<ExpenseCategoryClass, Expense> b)
            {
                if (b.Key != null)
                    return $"{b.Key.Name} - {b.Sum(e => e.Amount)} €";
                else
                    return $"Χωρίς κατηγορία - {b.Sum(e => e.Amount)} €";

            }
            return "Error";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();

        #endregion Methods
    }
}