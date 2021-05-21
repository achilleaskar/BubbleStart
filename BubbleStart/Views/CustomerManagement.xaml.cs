using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BubbleStart.Model;
using BubbleStart.ViewModels;

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
            //DatagridMassage.CommitEdit();
            //DatagridMassage.CommitEdit();
            //DatagridNormal. edit CommitEdit();
            //DatagridNormal.CommitEdit();
            //DatagridOnline.CommitEdit();
            //DatagridOnline.CommitEdit();
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
    }
}