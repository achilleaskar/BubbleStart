using GalaSoft.MvvmLight;

namespace BubbleStart.ViewModels
{
    public abstract class MyViewModelBase : ViewModelBase
    {



        private bool _IsLoaded;


        public bool IsLoaded
        {
            get
            {
                return _IsLoaded;
            }

            set
            {
                if (_IsLoaded == value)
                {
                    return;
                }

                _IsLoaded = value;
                RaisePropertyChanged();
            }
        }

        public abstract void Load(int id = 0, MyViewModelBaseAsync previousViewModel = null);

        public abstract void Reload();
    }
}
