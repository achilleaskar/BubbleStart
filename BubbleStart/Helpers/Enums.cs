using System.ComponentModel;

namespace BubbleStart.Helpers
{
        public enum ProgramMode
        {
            normal,
            massage,
            online,
            outdoor
        }

        public enum ForceDisable
        {
            normal,
            forceDisable,
            forceEnable
        }

        [TypeConverter(typeof(EnumDescriptionTypeConverter))]
        public enum PaymentType
        {
            [Description("Μετρητά")]
            Cash=0,
            [Description("VISA")]
            Visa=5,
        }

        public enum ExpenseCategory
        {
            pagia,
            misthoi,
            ektakta,
            spitiou,
            gwgw,
            timologia
        }
    }
