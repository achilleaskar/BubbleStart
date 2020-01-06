using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BubbleStart.ViewModels
{
    public abstract class MyViewModelBase : ViewModelBase
    {
        public bool IsLoaded { get; set; }

        public abstract void Load(int id = 0, MyViewModelBaseAsync previousViewModel = null);

        public abstract void Reload();
    }
}
