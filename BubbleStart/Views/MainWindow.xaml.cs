using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Windows;
using BubbleStart.Database;
using BubbleStart.ViewModels;
using Squirrel;

namespace BubbleStart.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public GenericRepository StartingRepository;

        public MainWindow(MainViewModel viewModel)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            InitializeComponent();
            var viewModel1 = viewModel;
            DataContext = viewModel1;
            StartingRepository = new GenericRepository();
            viewModel1.LoadAsync(StartingRepository).ConfigureAwait(true);
        }

        private async void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var updateManager = await UpdateManager.GitHubUpdateManager("https://github.com/achilleaskar/BubbleStart"))
                {
                    ReleaseEntry releaseEntry = await updateManager.UpdateApp();
                    if (releaseEntry?.Version.ToString() != null)
                    {
                        MessageBoxResult dialogResult = MessageBox.Show("Εγκαταστάθηκε νέα ενημέρωση. Θέλετε να επανεκκινήσετε την εφαρμογή σας τώρα?", "", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (dialogResult == MessageBoxResult.Yes)
                            UpdateManager.RestartApp();
                    }
                }
            }
            catch (Exception ex)
            {
                if (!ex.Message.Contains(".exe"))
                    MessageBox.Show("Error updating:" + ex.Message + "   " + (ex.InnerException != null ? ex.InnerException.Message : ""));
                throw;
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();

            Environment.Exit(0);
        }

        private void Window_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is bool b && b && DataContext is MainViewModel m && m.SelectedViewmodel is MainUserControl_ViewModel mu)
            {
                foreach (var a in mu.SearchCustomer_ViewModel.TodaysApointments)
                    a.RaisePropertyChanged(nameof(a.ShowedUpToday));
                mu.SearchCustomer_ViewModel.SelectedApointment = null;
                foreach (var d in mu.Apointments_ViewModel.Days ?? new ObservableCollection<Day>())
                    foreach (var h in d.Hours)
                    {
                        foreach (var a in h.AppointemntsOutdoor)
                            a.RaisePropertyChanged(nameof(a.ApColor));
                        foreach (var a in h.AppointmentsFunctional)
                            a.RaisePropertyChanged(nameof(a.ApColor));
                        foreach (var a in h.AppointmentsMassage)
                            a.RaisePropertyChanged(nameof(a.ApColor));
                        foreach (var a in h.AppointmentsReformer)
                            a.RaisePropertyChanged(nameof(a.ApColor));
                    }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MessageBox.Show("Θέλετε σίγουρα να κλείσετε?", "Έξοδος", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                e.Cancel = false;
            else
                e.Cancel = true;
        }
    }
}