using BubbleStart.Messages;
using BubbleStart.ViewModels;
using GalaSoft.MvvmLight.Messaging;
using System.Windows;
using System.Windows.Controls;

namespace BubbleStart.Views
{
    /// <summary>
    /// Interaction logic for ExpenseCategories_ManagementWindow.xaml
    /// </summary>
    public partial class ExpenseCategories_ManagementWindow : Window
    {
        public ExpenseCategories_ManagementWindow()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (DataContext is ExpenseCategoriesManagement_Viewmodel u && u.Context.HasChanges())
            {
                var failed = false;
                MessageBoxResult result = MessageBox.Show("Υπάρχουν μη απόθηκευμένες αλλαγές, θέλετε σίγουρα να κλείσετε?", "Προσοχή", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.No)
                {
                    e.Cancel = true;
                }
                else
                {
                    failed = u.Context.Context.RollBack();
                }
                if (failed)
                    u.Context.RefreshCommand.Execute(null);
            }
            Messenger.Default.Send(new UpdateExpenseCategoriesMessage());
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl && DataContext is ExpenseCategoriesManagement_Viewmodel vm)
            {
                if (e.AddedItems.Count > 0 && e.AddedItems[0] is TabItem tabItem)
                {
                    vm.IsIncomeTabSelected = tabItem.Tag?.ToString() == "Income";
                }
            }
        }
    }
}