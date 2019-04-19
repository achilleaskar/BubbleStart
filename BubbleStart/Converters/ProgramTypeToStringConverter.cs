using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using static BubbleStart.Model.Program;

namespace BubbleStart.Converters
{
    public class ProgramTypeToStringConverter : IValueConverter
    {
        #region Methods

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int a && Enum.IsDefined(typeof(ProgramTypes), a))
            {
                value = (ProgramTypes)value;
            }

            if (value is ProgramTypes p)
            {
                switch (p)
                {
                    case ProgramTypes.daily30:
                        return "Ημερήσιο 30'";

                    case ProgramTypes.daily60:
                        return "Ημερήσιο 60'";

                    case ProgramTypes.pilates2:
                        return "Reformer Pilates (1-2)";

                    case ProgramTypes.functional2:
                        return "Functional Training(1-2)";

                    case ProgramTypes.pilates5:
                        return "Reformer Pilates (3-5)";

                    case ProgramTypes.functional5:
                        return "Functional Training (3-5)";

                    case ProgramTypes.freeUse:
                        return "Ελέυθερη Χρήση";
                }
            }
            return "Ανενεργό";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();

        #endregion Methods
    }
}