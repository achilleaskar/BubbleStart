using BubbleStart.Model;
using BubbleStart.ViewModels;
using System.Windows.Controls;
using System.Windows.Input;

namespace BubbleStart.Views
{
    /// <summary>
    /// Interaction logic for EconomicData_UserControl.xaml
    /// </summary>
    public partial class EconomicData_UserControl : UserControl
    {
        public EconomicData_UserControl()
        {
            InitializeComponent();
        }

        private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is DataGridRow d && d.DataContext is Payment p)
            {
                ((EconomicData_ViewModel)DataContext).SelectedCustomer = p.Customer;
                ((EconomicData_ViewModel)DataContext).OpenCustomerManagementCommand.Execute(null);
            }
            else if(sender is DataGridRow dr && dr.DataContext is Program pr)
            {
                ((EconomicData_ViewModel)DataContext).SelectedCustomer = pr.Customer;
                ((EconomicData_ViewModel)DataContext).OpenCustomerManagementCommand.Execute(null);
            }
        }

        private void DatePicker_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && DataContext is EconomicData_ViewModel ed && ed.ShowExpensesDataCommand.CanExecute(null))
            {
                ed.ShowExpensesDataCommand.Execute(null);
            }
        }

        private void DatePickerInc_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && DataContext is EconomicData_ViewModel ed && ed.ShowIncomesDataCommand.CanExecute(null))
            {
                ed.ShowIncomesDataCommand.Execute(null);
            }
        }
    }
}