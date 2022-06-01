using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BubbleStart.Model;
using BubbleStart.ViewModels;

namespace BubbleStart.Views
{
    /// <summary>
    /// Interaction logic for SearchCustomers.xaml
    /// </summary>
    public partial class SearchCustomers : UserControl
    {
        #region Constructors

        public SearchCustomers()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Methods

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

        private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ((SearchCustomer_ViewModel)DataContext).OpenActiveCustomerManagementCommand.Execute(null);
        }

        private void DataGridRow_MouseDoubleClickSide(object sender, MouseButtonEventArgs e)
        {
            ((SearchCustomer_ViewModel)DataContext).OpenActiveCustomerSideManagementCommand.Execute(null);
        }

        private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ((SearchCustomer_ViewModel)DataContext).OpenCustomerManagementCommand.Execute(null);
        }
        private void UserControl_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                if (DataContext is SearchCustomer_ViewModel sc && sc.PopupOpen)
                {
                    sc.PopupOpen = false;
                    PilFunToggle.IsChecked = false;
                }
            }
        }

        #endregion Methods

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (DataContext is SearchCustomer_ViewModel sc && (sc.PopupOpen || sc.PopupFinishOpen))
            {
                sc.PopupOpen = false;
                sc.PopupFinishOpen = false;

                PilFunToggle.IsChecked = false;
            }
        }

        private void ButtonFinishClicked(object sender, System.Windows.RoutedEventArgs e)
        {
            if (DataContext is SearchCustomer_ViewModel sc && !sc.PopupFinishOpen)
            {
                sc.PopupFinishOpen = true;
            }
        }

        private void MenuItem_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (sender is MenuItem r &&  r.DataContext is Customer c)
            {
                Clipboard.SetText(c.ToString());
            }

        }
    }
}