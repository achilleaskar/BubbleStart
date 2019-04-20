using BubbleStart.ViewModels;
using System.Windows;

namespace BubbleStart.Messages
{
    internal class OpenChildWindowCommand
    {
        #region constructors and destructors

        public OpenChildWindowCommand(Window window)
        {
            Window = window;
        }

        public OpenChildWindowCommand(Window window, MyViewModelBase viewModel)
        {
            Window = window;
            ViewModel = viewModel;
        }

        #endregion constructors and destructors

        public Window Window { get; set; }
        public MyViewModelBase ViewModel { get; set; }
    }
}