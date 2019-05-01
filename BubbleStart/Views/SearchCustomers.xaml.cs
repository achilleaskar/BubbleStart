using BubbleStart.ViewModels;
using System.Windows.Controls;
using System.Windows.Input;

namespace BubbleStart.Views
{
    /// <summary>
    /// Interaction logic for SearchCustomers.xaml
    /// </summary>
    public partial class SearchCustomers : UserControl
    {
        public SearchCustomers()
        {
            InitializeComponent();
        }

        private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ((SearchCustomer_ViewModel)DataContext).OpenCustomerManagementCommand.Execute(null);
        }

       

        private void Button_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                if (sender is Button b && b.IsEnabled)
                {
                    ((SearchCustomer_ViewModel)DataContext).ShowedUpCommand.Execute(null);
                    e.Handled = true;
                }
            }
        }
    }
}