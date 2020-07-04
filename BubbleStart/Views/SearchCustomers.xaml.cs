﻿using System.Windows.Controls;
using System.Windows.Input;
using BubbleStart.ViewModels;

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
                    ((SearchCustomer_ViewModel)DataContext).OpenPopupCommand.Execute(null);
                    e.Handled = true;
                }
            }
        }
        

        private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ((SearchCustomer_ViewModel)DataContext).OpenActiveCustomerManagementCommand.Execute(null);
        }

        private void DataGridRow_MouseDoubleClickSide(object sender, MouseButtonEventArgs e)
        {
            ((SearchCustomer_ViewModel)DataContext).OpenActiveCustomerSideManagementCommand.Execute(null);
        }

        private void Button_PreviewMouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                if (sender is Button b && b.IsEnabled)
                {
                    ((SearchCustomer_ViewModel)DataContext).ShowedUpCommand.Execute(1);
                    e.Handled = true;
                }
            }
        }
    }
}