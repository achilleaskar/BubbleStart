using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace BubbleStart.ViewModels
{
    public abstract class MyViewModelBaseAsync : ViewModelBase, IViewModel
    {

        public bool IsLoaded { get; set; }

        public abstract Task LoadAsync(int id = 0, MyViewModelBaseAsync previousViewModel = null);

        public abstract Task ReloadAsync();

    }
}