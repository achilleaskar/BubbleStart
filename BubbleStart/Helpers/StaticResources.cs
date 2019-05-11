using BubbleStart.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BubbleStart.Helpers
{
    public static class StaticResources
    {
        public static User User { get; set; }

        //public static string[] Districts { get; set; } = { "Ευζώνων", "Λαογραφικό Μουσείο", "Μπότσαρη", "Πανόραμα", "Σχολή Τυφλων", "Φάληρο", "Άλλο" };
        public static List<int> Months { get; set; } = new List<int> {0, 1, 3, 6, 12 };
        public static ObservableCollection<District> Districts { get; set; } = new ObservableCollection<District>();
    }
}