using GalaSoft.MvvmLight;

namespace BubbleStart.ViewModels
{
    public abstract class MyViewModelBase : ViewModelBase
    {
        public bool IsLoaded { get; set; }

        public abstract void Load(int id = 0, MyViewModelBaseAsync previousViewModel = null);

        public abstract void Reload();
    }
}
