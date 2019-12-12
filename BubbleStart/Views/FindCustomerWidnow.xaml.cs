using BubbleStart.ViewModels;
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
