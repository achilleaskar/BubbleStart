using BubbleStart.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using static BubbleStart.Model.Program;

namespace BubbleStart.Helpers
{
    public static class StaticResources
    {

        public static string GetDescription(Enum en)
        {
            Type type = en.GetType();
            MemberInfo[] memInfo = type.GetMember(en.ToString());
            if (memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attrs != null && attrs.Length > 0)
                {
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }
            return en.ToString();
        }
        public static User User { get; set; }

        //public static string[] Districts { get; set; } = { "Ευζώνων", "Λαογραφικό Μουσείο", "Μπότσαρη", "Πανόραμα", "Σχολή Τυφλων", "Φάληρο", "Άλλο" };
        public static List<int> Months { get; set; } = new List<int> { 0, 1, 2, 3, 6, 12 };

        //public static ObservableCollection<District> Districts { get; set; } = new ObservableCollection<District>();

        public static string DecimalToString(decimal value) => value.ToString("C2").Replace(".00", "").Replace(",00", "").Replace(" ", "");

        //public static string ProgramEnumToString(ProgramTypes ProgramType)
        //{
        //    switch (ProgramType)
        //    {
        //        case ProgramTypes.ReformerPilates:
        //            return "Reformer Pilates";

        //        case ProgramTypes.Pilates:
        //            return "Pilates";

        //        case ProgramTypes.Functional:
        //            return "Functional";

        //        case ProgramTypes.PilatesFunctional:
        //            return "Pilates & Functional";

        //        case ProgramTypes.freeUse:
        //            return "Ελεύθερη Χρήση";

        //        case ProgramTypes.MedicalExersise:
        //            return "Medical Exercise";

        //        case ProgramTypes.dokimastiko:
        //            return "Personal";

        //        case ProgramTypes.yoga:
        //            return "Yoga";

        //        case ProgramTypes.aerial:
        //            return "Aerial Yoga";

        //        case ProgramTypes.masasRel30:
        //            return "Μασάζ Χαλαρωτικό 30'";

        //        case ProgramTypes.masazRel50:
        //            return "Μασάζ Χαλαρωτικό 50'";

        //        case ProgramTypes.masazTher30:
        //            return "Μασάζ Θεραπευτικό 30'";

        //        case ProgramTypes.masazTher50:
        //            return "Μασάζ Θεραπευτικό 50'";

        //        case ProgramTypes.blackfriday:
        //            return "Black Friday Deal";

        //        case ProgramTypes.massage41:
        //            return "4+1 massage";

        //        case ProgramTypes.online:
        //            return "Online";

        //        case ProgramTypes.summerDeal:
        //            return "Summer Deal";

        //        case ProgramTypes.OutDoor:
        //            return "OutDoor";

        //        case ProgramTypes.September:
        //            return "September Deal";

        //        case ProgramTypes.Month:
        //            return "Μηνιαίο πακέτο Γυμναστικής";
        //    }
        //    return "Σφάλμα";
        //}

        public static string ToGreek(string searchTerm)
        {
            string toReturn = "";
            foreach (char c in searchTerm)
            {
                if (c < 65 || c > 90)
                {
                    toReturn += c;
                }
                else
                {
                    switch ((int)c)
                    {
                        case 65:
                            toReturn += 'Α';
                            break;

                        case 66:
                            toReturn += 'Β';
                            break;

                        case 68:
                            toReturn += 'Δ';
                            break;

                        case 69:
                            toReturn += 'Ε';
                            break;

                        case 70:
                            toReturn += 'Φ';
                            break;

                        case 71:
                            toReturn += 'Γ';
                            break;

                        case 72:
                            toReturn += 'Η';
                            break;

                        case 73:
                            toReturn += 'Ι';
                            break;

                        case 75:
                            toReturn += 'Κ';
                            break;

                        case 76:
                            toReturn += 'Λ';
                            break;

                        case 77:
                            toReturn += 'Μ';
                            break;

                        case 78:
                            toReturn += 'Ν';
                            break;

                        case 79:
                            toReturn += 'Ο';
                            break;

                        case 80:
                            toReturn += 'Π';
                            break;

                        case 82:
                            toReturn += 'Ρ';
                            break;

                        case 83:
                            toReturn += 'Σ';
                            break;

                        case 84:
                            toReturn += 'Τ';
                            break;

                        case 86:
                            toReturn += 'Β';
                            break;

                        case 88:
                            toReturn += 'Χ';
                            break;

                        case 89:
                            toReturn += 'Υ';
                            break;

                        case 90:
                            toReturn += 'Ζ';
                            break;

                        default:
                            toReturn += c;
                            break;
                    }
                }
            }
            return toReturn;
        }
    }
}