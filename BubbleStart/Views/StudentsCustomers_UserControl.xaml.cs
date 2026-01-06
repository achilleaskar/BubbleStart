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
using System.Windows.Navigation;
using System.Windows.Shapes;
using BubbleStart.Model;
using BubbleStart.ViewModels;

namespace BubbleStart.Views
{
    /// <summary>
    /// Interaction logic for InActiveCustomers_UserControl.xaml
    /// </summary>
    public partial class StudentsCustomers_UserControl : UserControl
    {
        public StudentsCustomers_UserControl()
        {
            InitializeComponent();
        }

        private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is DataGridRow dc && dc.DataContext is Customer c)
            {
                ((StudentsCustomers_ViewModel)DataContext).SelectedCustomer = c;
                ((StudentsCustomers_ViewModel)DataContext).OpenCustomerManagementCommand.Execute(null);
            }
        }
    }
}
