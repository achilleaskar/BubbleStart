using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BubbleStart.ViewModels
{
    public interface IViewModel
    {
        Task LoadAsync(int id = 0, MyViewModelBase previousViewModel = null);

        Task ReloadAsync();

        bool IsLoaded { get; set; }
    }
}