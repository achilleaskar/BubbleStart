using BubbleStart.Model;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BubbleStart.Views
{
    /// <summary>
    /// Interaction logic for Shop_UserControl.xaml
    /// </summary>
    public partial class Shop_UserControl : UserControl
    {
        public Shop_UserControl()
        {
            InitializeComponent();
        }

        private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is DataGridRow d && d.DataContext is ItemPurchase p)
            {
                if (p.Customer==null)
                {
                    return;
                }
                ((Shop_ViewModel)DataContext).SelectedCustomer = p.Customer;
                if (!p.Customer.Enabled)
                {
                    ((Shop_ViewModel)DataContext).FullyLoadCustomerCommand.Execute(p.Customer);
                    return;
                }
                ((Shop_ViewModel)DataContext).OpenCustomerManagementCommand.Execute(null);
            }
        }
    }
}
