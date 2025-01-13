using System.ComponentModel;

namespace BubbleStart.Helpers
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum ProgramMode
    {
        [Description("Functional")]
        functional = 0,
        [Description("Massage")]
        massage = 1,
        [Description("Online")]
        online = 2,
        [Description("Outdoor")]
        outdoor = 3,
        [Description("Pilates")]
        pilates = 4,
        [Description("Βιβλίο")]
        yoga = 5,
        [Description("Pilates-Functional")]
        pilatesFunctional = 6,
        [Description("Σεμινάρια")]
        aerialYoga = 7,
        [Description("Personal")]
        personal = 8,
        [Description("Medical")]
        medical = 9
    }

    //[TypeConverter(typeof(EnumDescriptionTypeConverter))]
    //public enum ProgramTypes
    //{
    //    [Description("Reformer Pilates")]
    //    ReformerPilates = 0,

    //    [Description("Pilates")]
    //    Pilates = 1,

    //    [Description("Functional")]
    //    Functional = 2,

    //    [Description("Pilates &amp; Functional")]
    //    PilatesFunctional = 3,

    //    [Description("Ελεύθερη Χρήση")]
    //    freeUse = 4,

    //    [Description("Medical Exercise")]
    //    MedicalExersise = 5,

    //    [Description("Personal")]
    //    dokimastiko = 6,

    //    [Description("Yoga")]
    //    yoga = 7,

    //    [Description("Aerial Yoga")]
    //    aerial = 8,

    //    [Description("Μασάζ Χαλαρωτικό 30'")]
    //    masasRel30 = 9,

    //    [Description("Μασάζ Χαλαρωτικό 50'")]
    //    masazRel50 = 10,

    //    [Description("Μασάζ Θεραπευτικό 30'")]
    //    masazTher30 = 11,

    //    [Description("Μασάζ Θεραπευτικό 50'")]
    //    masazTher50 = 12,

    //    [Description("Black Friday Deal")]
    //    blackfriday = 13,

    //    [Description("4+1 massage")]
    //    massage41 = 14,

    //    [Description("Online")]
    //    online = 15,

    //    [Description("Summer Deal")]
    //    summerDeal = 16,

    //    [Description("OutDoor")]
    //    OutDoor = 17,

    //    [Description("September Deal")]
    //    September = 18,

    //    [Description("Μηνιαίο πακέτο Γυμναστικής")]
    //    Month = 19
    //}

    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum BodyPart
    {
        [EnumOrder(Order = 0)]
        [Description(" ")]
        Unknown = 0,
        [EnumOrder(Order = 0)]
        [Description("Total Body")]
        TotalBody = 1,
        [EnumOrder(Order = 0)]
        [Description("Lower Body")]
        LowerBody = 2,
        [EnumOrder(Order = 0)]
        [Description("Upper Body")]
        UpperBody = 3,
        [EnumOrder(Order = 0)]
        [Description("Core Strength")]
        Core = 4
        //[Description("Πόδια")]
        //Legs = 4,
        //[Description("Πλάτη")]
        //Back = 5,
        //[Description("Στήθος")]
        //Chest = 6,
        //[Description("Ώμοι")]
        //Shoulders = 7,
        //[Description("Δικέφαλος")]
        //Biceps = 8,
        //[Description("Τρικέφαλος")]
        //Triceps = 9
    }

    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum SecBodyPart
    {
        [EnumOrder(Order = 0)]
        [Description("Πόδια")]
        Legs = 4,
        [EnumOrder(Order = 1)]
        [Description("Πλάτη")]
        Back = 5,
        [EnumOrder(Order = 2)]
        [Description("Στήθος")]
        Chest = 6,
        [EnumOrder(Order = 3)]
        [Description("Ώμοι")]
        Shoulders = 7,
        [EnumOrder(Order = 4)]
        [Description("Κορμός")]
        Main = 10,
        [EnumOrder(Order = 5)]
        [Description("Αερόβιο")]
        Aerial = 11,
        [EnumOrder(Order = 6)]
        [Description("Δικέφαλος")]
        Biceps = 8,
        [EnumOrder(Order = 7)]
        [Description("Τρικέφαλος")]
        Triceps = 9,
        [EnumOrder(Order = 8)]
        [Description("Τετρακέφαλος")]
        Tetraceps = 12,
        [EnumOrder(Order = 9)]
        [Description("Προσαγωγός")]
        Prosagogos = 13,
        [EnumOrder(Order = 10)]
        [Description("Γλουτός")]
        Glute = 14,
        [EnumOrder(Order = 11)]
        [Description("Γαστροκνήμιος")]
        Gastroknimio = 15
    }

    public enum SelectedPersonEnum
    {
        Gogo = 0,
        Functional = 1,
        Yoga = 2,
        Massage = 3,
        Online = 4,
        Personal = 5,
        PilatesMat = 6

    }

    public enum ForceDisable
    {
        normal,
        forceDisable,
        forceEnable,
        marketing
    }

    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum PaymentType
    {
        [Description("Μετρητά")]
        Cash = 0,

        [Description("VISA")]
        Visa = 5,

        [Description("Τραπεζικά")]
        Bank = 6,
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

    //[TypeConverter(typeof(EnumDescriptionTypeConverter))]
    //public enum ExpenseCategory
    //{
    //    [Description("Πάγια")]
    //    pagia,
    //    [Description("Μισθοί")]
    //    misthoi,
    //    [Description("Έκτακτα")]
    //    ektakta,
    //    [Description("Σπιτιού")]
    //    spitiou,
    //    [Description("Γωγώ")]
    //    gwgw,
    //    [Description("Τιμολόγια")]
    //    timologia,
    //    [Description("Πιστωτικά")]
    //    pistotika,
    //    [Description("Φαινομενικά")]
    //    fainomenika
    //}

    public enum RoomEnum
    {
        Functional = 0,
        FunctionalB = 6,
        Pilates = 1,
        Personal = 5,
        Massage = 2,
        MassageHalf = 4,
        Outdoor = 3,

        Fitness = 10,
        Strength = 16,
        Personal2 = 11,
        FreeSpace = 15,
        Massage2 = 12
    }
}