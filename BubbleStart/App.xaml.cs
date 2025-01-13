using BubbleStart.Helpers;
using BubbleStart.Messages;
using BubbleStart.Model;
using BubbleStart.ViewModels;
using BubbleStart.Views;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Threading;

namespace BubbleStart
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IHost _host;
        private DispatcherTimer timer = new DispatcherTimer();
        private Stopwatch stopWatch = new Stopwatch();

        protected override void OnStartup(StartupEventArgs e)
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Bubble Logs\Bubble.txt";

            Log.Logger = new LoggerConfiguration()
               .MinimumLevel.Debug()
               .WriteTo.File(path, rollingInterval: RollingInterval.Day)
               .CreateLogger();

            _host = Host.CreateDefaultBuilder()
                 .ConfigureLogging(logging =>
                 {
                     logging.SetMinimumLevel(LogLevel.Information);
                 })
                 .ConfigureServices(services =>
                 {
                     services.AddSingleton<MainWindow>();
                     services.AddSingleton<MainViewModel>();
                 })
                 .Build();

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

        private void timer_Tick(object sender, EventArgs e)
        {
            if (StaticResources.User?.Id != 35 && StaticResources.User?.Id != 3 && stopWatch.Elapsed.TotalSeconds >= 180)
            {
                if (StaticResources.OpenWindow != null)
                {
                    if (StaticResources.OpenWindow.DataContext is Customer c && c.BasicDataManager.HasChanges())
                    {
                        c.BasicDataManager.RollBack();
                    }
                    StaticResources.OpenWindow.Close();
                }
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
            if (sender is TextBox textbox && !textbox.IsKeyboardFocusWithin)
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
            if (e.OriginalSource is TextBox textBox && !(textBox.DataContext is Expense) )
                textBox.SelectAll();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var mw = _host.Services.GetService<MainWindow>();
            this.MainWindow = mw;
            mw.Show();
        }

        private readonly Serilog.ILogger logger = Log.ForContext<App>();

        private void Application_DispatcherUnhandledException(object sender,
          DispatcherUnhandledExceptionEventArgs e)
        {
            Messenger.Default.Send(new ErrorMessage(e.Exception.ToString()));

            MessageBox.Show("Unexpected error occured. Please inform the admin."
              + Environment.NewLine + e.Exception.Message, "Unexpected error");
            e.Handled = true;
        }
    }
}