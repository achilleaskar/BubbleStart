using BubbleStart.Helpers;
using BubbleStart.Model;
using BubbleStart.Wrappers;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace BubbleStart.ViewModels
{
    public class ProgramTypesManagement_ViewModel : AddEditBase<ProgramTypeWrapper, ProgramType>
    {
        public ProgramTypesManagement_ViewModel(BasicDataManager context) : base(context)
        {
            ControlName = "Διαχείριση Τύπων Πακέτων";
            Context = context;
        }

        public override async Task LoadAsync(int id = 0, MyViewModelBaseAsync previousViewModel = null)
        {
            MainCollection = new ObservableCollection<ProgramTypeWrapper>((Context.ProgramTypes).Select(p => new ProgramTypeWrapper(p)));
        }

        public override async Task ReloadAsync()
        {
            MainCollection = new ObservableCollection<ProgramTypeWrapper>((Context.ProgramTypes).Select(p => new ProgramTypeWrapper(p)));
        }
    }
}