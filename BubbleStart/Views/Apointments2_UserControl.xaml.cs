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
    public partial class Apointments2_UserControl : UserControl
    {
        public Apointments2_UserControl()
        {
            InitializeComponent();
        }

        private async void OnDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is ContentControl cc && cc.DataContext is Apointment ap && ap.Customer != null)
            {
                if (!ap.Customer.Enabled)
                {
                    Mouse.OverrideCursor = Cursors.Wait;
                    await ((Apointments_ViewModel)DataContext).BasicDataManager.Context.GetFullCustomerByIdAsync(ap.Customer.Id);
                    ap.Customer.Loaded = true;
                    ap.Customer.InitialLoad();
                    ap.Customer.UpdateCollections();
                    ap.Customer.SetColors();
                    Mouse.OverrideCursor = Cursors.Arrow;
                }
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
                if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
                {
                    if (h.parent != null)
                    {
                        var t = h.parent.Days.FirstOrDefault(d => d.Date.DayOfYear == h.Time.DayOfYear).Hours.OrderBy(h1 => h1.Time);
                        if (t.Any(tr => tr.SelectedF))
                        {
                            bool found = false;
                            foreach (var o in t)
                            {
                                if (o.SelectedF)
                                    found = true;
                                else if (o == h)
                                {
                                    o.SelectedF = true;
                                    break;
                                }
                                else if (found)
                                    o.SelectedF = true;
                            }
                        }
                    }
                }
                else if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
                    h.SelectedF = !h.SelectedF;
                else if (DataContext is Apointments_ViewModel vm)
                    foreach (var hour in vm.Days.FirstOrDefault(d => d.Date.DayOfYear == h.Time.DayOfYear).Hours)
                        hour.DeselectAll();
        }

        private void Border_PreviewMouseLeftButtonUpR(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border b && b.DataContext is Hour h)
                if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
                {
                    if (h.parent != null)
                    {
                        var t = h.parent.Days.FirstOrDefault(d => d.Date.DayOfYear == h.Time.DayOfYear).Hours.OrderBy(h1 => h1.Time);
                        if (t.Any(tr => tr.SelectedR))
                        {
                            bool found = false;
                            foreach (var o in t)
                            {
                                if (o.SelectedR)
                                    found = true;
                                else if (o == h)
                                {
                                    o.SelectedR = true;
                                    break;
                                }
                                else if (found)
                                    o.SelectedR = true;
                            }
                        }
                    }
                }
                else if ((Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)))
                    h.SelectedR = !h.SelectedR;
                else if (DataContext is Apointments_ViewModel vm)
                    foreach (var hour in vm.Days.FirstOrDefault(d => d.Date.DayOfYear == h.Time.DayOfYear).Hours)
                        hour.DeselectAll();
        }

        private void Border_PreviewMouseLeftButtonUpFB(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border b && b.DataContext is Hour h)
                if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
                {
                    if (h.parent != null)
                    {
                        var t = h.parent.Days.FirstOrDefault(d => d.Date.DayOfYear == h.Time.DayOfYear).Hours.OrderBy(h1 => h1.Time);
                        if (t.Any(tr => tr.SelectedFB))
                        {
                            bool found = false;
                            foreach (var o in t)
                            {
                                if (o.SelectedFB)
                                    found = true;
                                else if (o == h)
                                {
                                    o.SelectedFB = true;
                                    break;
                                }
                                else if (found)
                                    o.SelectedFB = true;
                            }
                        }
                    }
                }
                else if ((Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)))
                    h.SelectedFB = !h.SelectedFB;
                else if (DataContext is Apointments_ViewModel vm)
                    foreach (var hour in vm.Days.FirstOrDefault(d => d.Date.DayOfYear == h.Time.DayOfYear).Hours)
                        hour.DeselectAll();
        }

        private void Border_PreviewMouseLeftButtonUpM(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border b && b.DataContext is Hour h)
                if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
                {
                    if (h.parent != null)
                    {
                        var t = h.parent.Days.FirstOrDefault(d => d.Date.DayOfYear == h.Time.DayOfYear).Hours.OrderBy(h1 => h1.Time);
                        if (t.Any(tr => tr.SelectedM))
                        {
                            bool found = false;
                            foreach (var o in t)
                            {
                                if (o.SelectedM)
                                    found = true;
                                else if (o == h)
                                {
                                    o.SelectedM = true;
                                    break;
                                }
                                else if (found)
                                    o.SelectedM = true;
                            }
                        }
                    }
                }
                else if ((Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)))
                    h.SelectedM = !h.SelectedM;
                else if (DataContext is Apointments_ViewModel vm)
                    foreach (var hour in vm.Days.FirstOrDefault(d => d.Date.DayOfYear == h.Time.DayOfYear).Hours)
                        hour.DeselectAll();
        }

        private void Border_PreviewMouseLeftButtonUpP(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border b && b.DataContext is Hour h)
                if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
                {
                    if (h.parent != null)
                    {
                        var t = h.parent.Days.FirstOrDefault(d => d.Date.DayOfYear == h.Time.DayOfYear).Hours.OrderBy(h1 => h1.Time);
                        if (t.Any(tr => tr.SelectedP))
                        {
                            bool found = false;
                            foreach (var o in t)
                            {
                                if (o.SelectedP)
                                    found = true;
                                else if (o == h)
                                {
                                    o.SelectedP = true;
                                    break;
                                }
                                else if (found)
                                    o.SelectedP = true;
                            }
                        }
                    }
                }
                else if ((Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)))
                    h.SelectedP = !h.SelectedP;
                else if (DataContext is Apointments_ViewModel vm)
                    foreach (var hour in vm.Days.FirstOrDefault(d => d.Date.DayOfYear == h.Time.DayOfYear).Hours)
                        hour.DeselectAll();
        }

        private void FunctionaControl_SizeChanged(object sender, System.Windows.SizeChangedEventArgs e)
        {
            if (sender is ItemsControl ic && ic.DataContext is Day d)
            {
                d.HeightFunctional = ic.ActualHeight;
            }
        }

        private void FunctionalBControl_SizeChanged(object sender, System.Windows.SizeChangedEventArgs e)
        {
            if (sender is ItemsControl ic && ic.DataContext is Day d)
            {
                d.HeightFunctionalB = ic.ActualHeight;
            }
        }

        private void ReformerControl_SizeChanged(object sender, System.Windows.SizeChangedEventArgs e)
        {
            if (sender is ItemsControl ic && ic.DataContext is Day d)
            {
                d.HeightPilates = ic.ActualHeight;
            }
        }

        private void MassageControl_SizeChanged(object sender, System.Windows.SizeChangedEventArgs e)
        {
            if (sender is ItemsControl ic && ic.DataContext is Day d)
            {
                d.HeightMassage = ic.ActualHeight;
            }
        }

        private void PersonalControl_SizeChanged(object sender, System.Windows.SizeChangedEventArgs e)
        {
            if (sender is ItemsControl ic && ic.DataContext is Day d)
            {
                d.HeightPersonal = ic.ActualHeight;
            }
        }
    }
}