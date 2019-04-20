using System;
using System.Globalization;
using System.Windows.Data;
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
                    case ProgramTypes.ReformerPilates:
                        return "Reformer & Pilates";

                    case ProgramTypes.Pilates:
                        return "Pilates";

                    case ProgramTypes.Functional:
                        return "Functional";

                    case ProgramTypes.PilatesFunctional:
                        return "Pilates & Functional";

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