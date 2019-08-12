using BubbleStart.Database;
using BubbleStart.Model;
using BubbleStart.Views;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BubbleStart.ViewModels
{
    public class ShowUpsPerDay_ViewModel : ViewModelBase
    {
        public ShowUpsPerDay_ViewModel()
        {
            ShowShowUpsCommand = new RelayCommand(async () => { await ShowShowUps(); });
            StartDate = EndDate = DateTime.Today;
            OpenActiveCustomerManagementCommand = new RelayCommand(() => { OpenCustomerManagement(SelectedShowUp.Customer); });
        }

        private void OpenCustomerManagement(Customer c)
        {
            if (c != null)
            {
                if (Context.HasChanges())
                {
                    MessageBoxResult result = MessageBox.Show("Υπάρχουν μη αποθηκευμένες αλλαγές, θέλετε σίγουρα να συνεχίσετε?", "Προσοχή", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (result == MessageBoxResult.No)
                    {
                        return;
                    }
                }
                if (Context.HasChanges())
                {
                    Context.RollBack();
                }

                c.Context = Context;
                Window window = new CustomerManagement
                {
                    DataContext = c
                };
                Application.Current.MainWindow.Visibility = Visibility.Hidden;
                window.ShowDialog();
                if (Context.HasChanges())
                {
                    Context.RollBack();
                }
                Application.Current.MainWindow.Visibility = Visibility.Visible;
            }
        }

        GenericRepository Context;





        private ShowUp _SelectedShowUp;


        public ShowUp SelectedShowUp
        {
            get
            {
                return _SelectedShowUp;
            }

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

             Context = new GenericRepository();


                DailyShowUps = new ObservableCollection<ShowUp>((await Context.GetAllShowUpsInRangeAsyncsAsync(StartDate, EndDate.AddDays(1))));

        }




        private ObservableCollection<ShowUp> _DailyShowUps;


        public ObservableCollection<ShowUp> DailyShowUps
        {
            get
            {
                return _DailyShowUps;
            }

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
            get
            {
                return _EndDate;
            }

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
            get
            {
                return _StartDate;
            }

            set
            {
                if (_StartDate == value)
                {
                    return;
                }

                _StartDate = value;
                if (StartDate > EndDate)
                {
                    EndDate = StartDate;
                }
                RaisePropertyChanged();
            }
        }

        public RelayCommand OpenActiveCustomerManagementCommand { get; internal set; }
    }
}
