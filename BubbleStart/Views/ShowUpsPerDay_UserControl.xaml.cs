using BubbleStart.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
