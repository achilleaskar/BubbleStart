using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using BubbleStart.Model;
using BubbleStart.ViewModels;
using Org.BouncyCastle.Asn1.Ocsp;

namespace BubbleStart.Views
{
    /// <summary>
    /// Interaction logic for CustomerManagement.xaml
    /// </summary>
    public partial class CustomerManagement : Window
    {
        public CustomerManagement()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (DataContext is Customer c && c.BasicDataManager.HasChanges())
            {
                MessageBoxResult result = MessageBox.Show("Υπάρχουν μη αποθηκευμένες αλλαγές, θέλετε σίγουρα να κλείσετε?", "Προσοχή", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    c.BasicDataManager.RollBack();
                }
                else
                    e.Cancel = true;

            }

        }

        private void DataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {

        }

        private void DataGridRow_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is DataGridRow dgr && DataContext is Customer c)
            {
                c.UpdateSelections(dgr.DataContext);
            }
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                if (DataContext is Customer sc && sc.Popup1Open)
                {
                    sc.Popup1Open = false;
                }
            }
        }

        private void Window_ContentRendered(object sender, System.EventArgs e)
        {
            var childs = functionall.Children;

            Dictionary<UIElement, int> dict = new Dictionary<UIElement, int>();

            foreach (UIElement child in childs)
            {
                if (child is StackPanel sp && sp.Children[0] is GroupBox gb && gb.Content is DataGrid dg)
                {
                    dict.Add(child, dg.Items.Count);
                }
            }

            if (dict.Count == 3)
            {
                childs.Clear();
                foreach (var item in dict.OrderByDescending(d => d.Value))
                {
                    childs.Add(item.Key);
                    if (item.Key is StackPanel sp && sp.Children[0] is GroupBox gb && gb.Content is DataGrid dg && dg.ItemsSource is ListCollectionView lcv && lcv.SortDescriptions.Count == 0)
                        lcv.SortDescriptions.Add(new SortDescription("Arrived", ListSortDirection.Descending));
                    if (item.Key is StackPanel sp1 && sp1.Children[1] is GroupBox gb1 && gb1.Content is DataGrid dg1 && dg1.ItemsSource is ListCollectionView lcv1 && lcv1.SortDescriptions.Count == 0)
                        lcv1.SortDescriptions.Add(new SortDescription("StartDay", ListSortDirection.Descending));
                    if (item.Key is StackPanel sp2 && sp2.Children[2] is GroupBox gb2 && gb2.Content is DataGrid dg2 && dg2.ItemsSource is ListCollectionView lcv2 && lcv2.SortDescriptions.Count == 0)
                        lcv2.SortDescriptions.Add(new SortDescription("Date", ListSortDirection.Descending));
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ResetDayPicker.SelectedDate = System.DateTime.Today;

        }
    }
}