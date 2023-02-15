﻿using BubbleStart.Helpers;
using BubbleStart.Model;
using BubbleStart.Views;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BubbleStart.ViewModels
{
    public class ShowUpsPerDay_ViewModel : ViewModelBase
    {
        public ShowUpsPerDay_ViewModel(BasicDataManager basicDataManager)
        {
            ShowShowUpsCommand = new RelayCommand(async () => { await ShowShowUps(); });
            StartDate = EndDate = DateTime.Today;
            OpenActiveCustomerManagementCommand = new RelayCommand(() => { OpenCustomerManagement(SelectedShowUp.Customer); });
            BasicDataManager = basicDataManager;
        }

        private void OpenCustomerManagement(Customer c)
        {
            if (c != null)
            {
                if (BasicDataManager.HasChanges())
                {
                    MessageBoxResult result = MessageBox.Show("Υπάρχουν μη αποθηκευμένες αλλαγές, θέλετε σίγουρα να συνεχίσετε?", "Προσοχή", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (result == MessageBoxResult.No)
                    {
                        return;
                    }
                }
                if (BasicDataManager.HasChanges())
                {
                    BasicDataManager.RollBack();
                }
                c.EditedInCustomerManagement = true;
                c.BasicDataManager = BasicDataManager;
                c.FillDefaultProframs();

                Window window = new CustomerManagement
                {
                    DataContext = c
                };
                Application.Current.MainWindow.Visibility = Visibility.Hidden;
                window.ShowDialog();
                Application.Current.MainWindow.Visibility = Visibility.Visible;
            }
        }

        private ShowUp _SelectedShowUp;

        public ShowUp SelectedShowUp
        {
            get => _SelectedShowUp;

            set
            {
                if (_SelectedShowUp == value)
                {
                    return;
                }

                _SelectedShowUp = value;
                RaisePropertyChanged();
            }
        }

        private async Task ShowShowUps()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            DailyShowUps = new ObservableCollection<ShowUp>(await BasicDataManager.Context.GetAllShowUpsInRangeAsyncsAsync(StartDate, EndDate.AddDays(1)));
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private ObservableCollection<ShowUp> _DailyShowUps;

        public ObservableCollection<ShowUp> DailyShowUps
        {
            get => _DailyShowUps;

            set
            {
                if (_DailyShowUps == value)
                {
                    return;
                }

                _DailyShowUps = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand ShowShowUpsCommand { get; set; }

        private DateTime _EndDate;

        public DateTime EndDate
        {
            get => _EndDate;

            set
            {
                if (_EndDate == value)
                {
                    return;
                }

                _EndDate = value;
                if (EndDate < StartDate)
                {
                    StartDate = EndDate;
                }
                RaisePropertyChanged();
            }
        }

        private DateTime _StartDate;

        public DateTime StartDate
        {
            get => _StartDate;

            set
            {
                if (_StartDate == value)
                {
                    return;
                }

                _StartDate = value;
                EndDate = StartDate;
                RaisePropertyChanged();
            }
        }

        public RelayCommand OpenActiveCustomerManagementCommand { get; internal set; }
        public BasicDataManager BasicDataManager { get; }
    }
}