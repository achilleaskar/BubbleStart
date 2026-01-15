using BubbleStart.Helpers;
using BubbleStart.Model;
using BubbleStart.Wrappers;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace BubbleStart.ViewModels
{
    public class DealsManagement_ViewModel : AddEditBase<DealWrapper, Deal>
    {
        public DealsManagement_ViewModel(BasicDataManager context) : base(context)
        {
            ControlName = "?????????? ?????????";
        }

        public override async Task LoadAsync(int id = 0, MyViewModelBaseAsync previousViewModel = null)
        {
            MainCollection = new ObservableCollection<DealWrapper>(Context.Deals.Select(d => new DealWrapper(d)));
        }

        public override async Task ReloadAsync()
        {
            MainCollection = new ObservableCollection<DealWrapper>(Context.Deals.Select(d => new DealWrapper(d)));
        }
    }
}
