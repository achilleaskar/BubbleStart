using System.Windows.Controls;
using System.Windows.Input;
using BubbleStart.Model;

namespace BubbleStart.Views
{
    /// <summary>
    /// Interaction logic for UserDetail.xaml
    /// </summary>
    public partial class UserDetail : UserControl
    {
        public UserDetail()
        {
            InitializeComponent();
        }

        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (DataContext is Customer c)
                {
                    if (c.NewWeight > 0)
                    {
                        c.WeightHistory.Add(new Weight { WeightValue = c.NewWeight, Height = c.Height });
                        c.NewWeight = 0;
                    }
                }
            }
        }
    }
}