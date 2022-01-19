using BubbleStart.ViewModels;
using System.Windows;

namespace BubbleStart.Views
{
    /// <summary>
    /// Interaction logic for ShiftsManagement_Window.xaml
    /// </summary>
    public partial class ShiftsManagement_Window : Window
    {
        public ShiftsManagement_Window()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (DataContext is ShiftsManagement_Viewmodel u && u.Context.HasChanges())
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