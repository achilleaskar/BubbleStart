using System.Windows;
using System.Windows.Input;
using BubbleStart.ViewModels;

namespace BubbleStart.Views
{
    /// <summary>
    /// Interaction logic for FindCustomerWidnow.xaml
    /// </summary>
    public partial class FindCustomerWidnow : Window
    {
        public FindCustomerWidnow()
        {
            InitializeComponent();
        }

        private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is CustomersWindow_Viewmodel vm && vm.AddCustomerCommand.CanExecute(null))
                vm.AddCustomerCommand.Execute(0);
        }
    }
}
