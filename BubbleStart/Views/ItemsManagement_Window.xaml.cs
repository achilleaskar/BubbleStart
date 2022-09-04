using BubbleStart.Messages;
using BubbleStart.ViewModels;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BubbleStart.Views
{
    /// <summary>
    /// Interaction logic for ItemsManagement_Window.xaml
    /// </summary>
    public partial class ItemsManagement_Window : Window
    {
        public ItemsManagement_Window()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (DataContext is ItemsManagement_ViewModel u && u.Context.HasChanges())
            {
                var failed = false;
                MessageBoxResult result = MessageBox.Show("Υπάρχουν μη απόθηκευμένες αλλαγές, θέλετε σίγουρα να κλείσετε?", "Προσοχή", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.No)
                {
                    e.Cancel = true;
                }
                else
                {
                    failed = u.Context.Context.RollBack();
                }
                if (failed)
                    u.Context.RefreshCommand.Execute(null);
            }
        }
    }
}
