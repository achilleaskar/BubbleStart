using BubbleStart.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BubbleStart.Helpers
{
    public static class StaticResources
    {
        public static User User { get; set; }

        //public static string[] Districts { get; set; } = { "Ευζώνων", "Λαογραφικό Μουσείο", "Μπότσαρη", "Πανόραμα", "Σχολή Τυφλων", "Φάληρο", "Άλλο" };
        public static ObservableCollection<District> Districts { get; set; } = new ObservableCollection<District>();


    }
}
