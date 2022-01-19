using BubbleStart.Helpers;
using BubbleStart.Model;
using BubbleStart.Wrappers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BubbleStart.ViewModels
{
    public class ShiftsManagement_Viewmodel : AddEditBase<ShiftWrapper, Shift>
    {
        public ShiftsManagement_Viewmodel(BasicDataManager context) : base(context)
        {
            ControlName = "Διαχείριση Συνεργατών";
            Hours = StaticResources.GetHours(8, 0, 23, 0, 30);
            Context = context;
        }

        public List<DateTime> Hours { get; set; }
        public BasicDataManager Context { get; }

        public override async Task LoadAsync(int id = 0, MyViewModelBaseAsync previousViewModel = null)
        {
            MainCollection = new ObservableCollection<ShiftWrapper>((Context.Shifts).Select(p => new ShiftWrapper(p)));
        }

        public override async Task ReloadAsync()
        {
            MainCollection = new ObservableCollection<ShiftWrapper>((Context.Shifts).Select(p => new ShiftWrapper(p)));

        }
    }
}