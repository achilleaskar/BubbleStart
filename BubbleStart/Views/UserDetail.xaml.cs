using BubbleStart.Model;
using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace BubbleStart.Views
{
    /// <summary>
    /// Interaction logic for UserDetail.xaml
    /// </summary>
    public partial class UserDetail : UserControl
    {
        private bool processing;

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

        private DateTime clicked;
        private void Button_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if ((DateTime.Now - clicked).TotalSeconds < 3)
                e.Handled = true;
            else
                clicked = DateTime.Now;
        }
    }
}