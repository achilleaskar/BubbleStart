using System.Threading.Tasks;

namespace BubbleStart.ViewModels
{
    public interface IViewModel
    {
        Task LoadAsync(int id = 0, MyViewModelBaseAsync previousViewModel = null);

        Task ReloadAsync();

        bool IsLoaded { get; set; }
    }
}