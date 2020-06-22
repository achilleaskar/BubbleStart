using System.Windows;
using System.Windows.Controls;
using BubbleStart.AttachedProperties;

namespace BubbleStart.Views
{
    /// <summary>
    /// Interaction logic for UsersManagement_Window.xaml
    /// </summary>
    public partial class UsersManagement_Window : Window
    {
        public UsersManagement_Window()
        {
            InitializeComponent();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            // Cast the 'sender' to a PasswordBox
            PasswordBox pBox = sender as PasswordBox;

            //Set this "EncryptedPassword" dependency property to the "SecurePassword"
            //of the PasswordBox.
            PasswordBoxMVVMAttachedProperties.SetEncryptedPassword(pBox, pBox.SecurePassword);
        }
    }
}