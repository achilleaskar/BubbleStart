using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Threading;
using BubbleStart.Helpers;
using BubbleStart.Messages;
using BubbleStart.ViewModels;
using BubbleStart.Views;
using GalaSoft.MvvmLight.Messaging;

namespace BubbleStart
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        DispatcherTimer timer = new DispatcherTimer();
        Stopwatch stopWatch = new Stopwatch();
        protected override void OnStartup(StartupEventArgs e)
        {
            EventManager.RegisterClassHandler(typeof(TextBox), UIElement.PreviewMouseLeftButtonDownEvent,
               new MouseButtonEventHandler(SelectivelyHandleMouseButton), true);
            EventManager.RegisterClassHandler(typeof(TextBox), UIElement.GotKeyboardFocusEvent,
              new RoutedEventHandler(SelectAllText), true);

            EventManager.RegisterClassHandler(typeof(Window), UIElement.PreviewMouseMoveEvent,
                new MouseEventHandler(OnPreviewMouseMove));
            EventManager.RegisterClassHandler(typeof(Window), UIElement.PreviewKeyDownEvent,
                new KeyEventHandler(OnPreviewKeyDown));
            stopWatch.Start();

            timer.Interval = new TimeSpan(0, 0, 30);
            timer.Tick += timer_Tick;
            timer.Start();

            var vCulture = new CultureInfo("el-GR");

            Thread.CurrentThread.CurrentCulture = vCulture;
            Thread.CurrentThread.CurrentUICulture = vCulture;
            CultureInfo.DefaultThreadCurrentCulture = vCulture;
            CultureInfo.DefaultThreadCurrentUICulture = vCulture;

            FrameworkElement.LanguageProperty.OverrideMetadata(
            typeof(FrameworkElement),
            new FrameworkPropertyMetadata(
         XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));

            base.OnStartup(e);
        }

        void timer_Tick(object sender, EventArgs e)
        {
            if (StaticResources.User?.Id != 35 && stopWatch.Elapsed.TotalSeconds >= 180)
            {
                Messenger.Default.Send(new LoginLogOutMessage(false));
            }
        }
        private void OnPreviewMouseMove(object sender, MouseEventArgs e)
        {
            stopWatch.Restart();
        }

        private void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            stopWatch.Restart();
        }


        private static void SelectivelyHandleMouseButton(object sender, MouseButtonEventArgs e)
        {
            var textbox = sender as TextBox;
            if (textbox != null && !textbox.IsKeyboardFocusWithin)
            {
                if (e.OriginalSource.GetType().Name == "TextBoxView")
                {
                    e.Handled = true;
                    textbox.Focus();
                }
            }
        }

        private static void SelectAllText(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is TextBox textBox)
                textBox.SelectAll();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            MainViewModel mvm = new MainViewModel();
            MainWindow mw = new MainWindow(mvm);
            mw.Show();
        }

        private void Application_DispatcherUnhandledException(object sender,
          DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show("Unexpected error occured. Please inform the admin."
              + Environment.NewLine + e.Exception.Message, "Unexpected error");
            e.Handled = true;
        }
    }
}