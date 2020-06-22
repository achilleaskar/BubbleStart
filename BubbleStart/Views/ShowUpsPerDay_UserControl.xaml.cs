using System.Windows.Controls;
using System.Windows.Input;
using BubbleStart.ViewModels;

namespace BubbleStart.Views
{
    /// <summary>
    /// Interaction logic for ShowUpsPerDay_UserControl.xaml
    /// </summary>
    public partial class ShowUpsPerDay_UserControl : UserControl
    {
        public ShowUpsPerDay_UserControl()
        {
            InitializeComponent();
        }

        private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ((ShowUpsPerDay_ViewModel)DataContext).OpenActiveCustomerManagementCommand.Execute(null);
        }
    }
}
