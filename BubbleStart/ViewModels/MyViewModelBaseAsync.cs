using GalaSoft.MvvmLight;
using System.Threading.Tasks;

namespace BubbleStart.ViewModels
{
    public abstract class MyViewModelBaseAsync : ViewModelBase, IViewModel
    {

        public bool IsLoaded { get; set; }

        public abstract Task LoadAsync(int id = 0, MyViewModelBaseAsync previousViewModel = null);

        public abstract Task ReloadAsync();

    }
}