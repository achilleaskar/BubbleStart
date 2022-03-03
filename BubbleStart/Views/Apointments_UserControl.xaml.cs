using BubbleStart.Model;
using BubbleStart.ViewModels;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;

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

        private void Border_PreviewMouseLeftButtonUpF(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border b && b.DataContext is Hour h)
                if ((Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)))
                    h.SelectedF = !h.SelectedF;
                else if (DataContext is Apointments_ViewModel vm)
                    foreach (var hour in vm.Days.FirstOrDefault(d => d.Date.DayOfYear == h.Time.DayOfYear).Hours)
                        hour.DeselectAll();
        }

        private void Border_PreviewMouseLeftButtonUpR(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border b && b.DataContext is Hour h)
                if ((Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)))
                    h.SelectedR = !h.SelectedR;
                else if (DataContext is Apointments_ViewModel vm)
                    foreach (var hour in vm.Days.FirstOrDefault(d => d.Date.DayOfYear == h.Time.DayOfYear).Hours)
                        hour.DeselectAll();
        }

        private void Border_PreviewMouseLeftButtonUpM(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border b && b.DataContext is Hour h)
                if ((Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)))
                    h.SelectedM = !h.SelectedM;
                else if (DataContext is Apointments_ViewModel vm)
                    foreach (var hour in vm.Days.FirstOrDefault(d => d.Date.DayOfYear == h.Time.DayOfYear).Hours)
                        hour.DeselectAll();
        }

        private void Border_PreviewMouseLeftButtonUpO(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border b && b.DataContext is Hour h)
                if ((Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)))
                    h.SelectedO = !h.SelectedO;
                else if (DataContext is Apointments_ViewModel vm)
                    foreach (var hour in vm.Days.FirstOrDefault(d => d.Date.DayOfYear == h.Time.DayOfYear).Hours)
                        hour.DeselectAll();
        }
    }
}