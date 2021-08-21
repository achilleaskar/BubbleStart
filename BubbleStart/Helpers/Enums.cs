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

    public enum SelectedPersonEnum
    {
        Gogo = 0,
        Dimitris = 1,
        Yoga = 2,
        Massage = 3,
        Online = 4,
        Personal = 5
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
        Cash = 0,

        [Description("VISA")]
        Visa = 5,
    }

    public enum SizeEnum
    {
        XS,
        S,
        M,
        L,
        XL,
        XXL,
        XXXL
    }

    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum ClothColors
    {
        [Description("Μαύρο")]
        mavro,
        [Description("Άσπρο")]
        aspro,
        [Description("Πράσινο")]
        prasino,
        [Description("Γκρί")]
        gri
    }

    public enum ExpenseCategory
    {
        pagia,
        misthoi,
        ektakta,
        spitiou,
        gwgw,
        timologia,
        pistotika,
        fainomenika
    }
}