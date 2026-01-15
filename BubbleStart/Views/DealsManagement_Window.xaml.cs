using BubbleStart.ViewModels;
using System.Windows;

namespace BubbleStart.Views
{
    /// <summary>
    /// Interaction logic for DealsManagement_Window.xaml
    /// </summary>
    public partial class DealsManagement_Window : Window
    {
        public DealsManagement_Window()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (DataContext is DealsManagement_ViewModel u && u.Context.HasChanges())
            {
                var failed = false;
                MessageBoxResult result = MessageBox.Show("???????? ?? ????????????? ???????, ?????? ??????? ?? ?????????", "???????", MessageBoxButton.YesNo, MessageBoxImage.Warning);
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
