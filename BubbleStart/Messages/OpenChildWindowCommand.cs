﻿using System.Windows;
using BubbleStart.ViewModels;

namespace BubbleStart.Messages
{
    internal class OpenChildWindowCommand
    {
        #region constructors and destructors

        public OpenChildWindowCommand(Window window)
        {
            Window = window;
        }

        public OpenChildWindowCommand(Window window, MyViewModelBaseAsync viewModel)
        {
            Window = window;
            ViewModel = viewModel;
        }

        #endregion constructors and destructors

        public Window Window { get; set; }
        public MyViewModelBaseAsync ViewModel { get; set; }
    }
}