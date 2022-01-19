using System.Windows.Controls;
using System.Windows.Input;
using BubbleStart.Model;
using BubbleStart.ViewModels;
using GalaSoft.MvvmLight.Messaging;

namespace BubbleStart.Views
{
    /// <summary>
    /// Interaction logic for Apointments_UserControl.xaml
    /// </summary>
    public partial class Apointments_UserControl : UserControl
    {
        public Apointments_UserControl()
        {
            InitializeComponent();
        }

        private void OnDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is ContentControl cc && cc.DataContext is Apointment ap && ap.Customer != null)
            {
                ap.Customer.FromProgram = true;
                ((Apointments_ViewModel)DataContext).OpenCustomerManagement(ap.Customer);
            }
        }

        private void CloseButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            TimePopup.IsOpen = false;
            ((Apointments_ViewModel)DataContext).CustomTime = "";

        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            TimePopup.IsOpen = true;
        }
    }
}