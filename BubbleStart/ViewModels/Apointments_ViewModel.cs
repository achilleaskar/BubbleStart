using BubbleStart.Database;
using BubbleStart.Model;
using BubbleStart.Views;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BubbleStart.ViewModels
{
    public class Apointments_ViewModel : MyViewModelBase
    {
        #region Constructors

        public Apointments_ViewModel()
        {
            Days = new ObservableCollection<Day>();
            Context = new GenericRepository();
            NextWeekCommand = new RelayCommand(async () => { await NextWeek(); });
            PreviousWeekCommand = new RelayCommand(async () => { await PreviousWeek(); });
        }

        private async Task PreviousWeek()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            StartDate = StartDate.AddDays(-7);
            await CreateProgram();
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private async Task NextWeek()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            StartDate = StartDate.AddDays(7);
            await CreateProgram();
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        #endregion Constructors

        #region Fields

        private ObservableCollection<Day> _Days;

        private DateTime _StartDate;

        #endregion Fields

        #region Properties

        public RelayCommand PreviousWeekCommand { get; set; }
        public RelayCommand NextWeekCommand { get; set; }

        public GenericRepository Context { get; }

        public ObservableCollection<Day> Days
        {
            get
            {
                return _Days;
            }

            set
            {
                if (_Days == value)
                {
                    return;
                }

                _Days = value;
                RaisePropertyChanged();
            }
        }

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
                RaisePropertyChanged();
            }
        }

        #endregion Properties

        #region Methods

        public async Task CreateProgram()
        {
            List<Apointment> apointments = (await Context.GetApointmentsAsync(StartDate)).ToList();
            DateTime tmpDate = StartDate;
            Days.Clear();
            for (int i = 0; i < 5; i++)
            {
                Days.Add(new Day(Context, tmpDate));
                tmpDate = tmpDate.AddDays(1);
            }

            int numOfDay;
            foreach (var ap in apointments)
            {
                numOfDay = ((int)ap.DateTime.DayOfWeek + 6) % 7;
                if (numOfDay < 5 && ap.DateTime.Hour >= 8 || ap.DateTime.Hour <= 20)
                {
                    Days[numOfDay].Hours[ap.DateTime.Hour - 8].Apointments.Add(ap);
                }
                else
                {
                    MessageBox.Show("Ραντεβού εκτός εβδομάδας");
                }
            }
            RaisePropertyChanged(nameof(Days));
        }

        public override async Task LoadAsync(int id = 0, MyViewModelBase previousViewModel = null)
        {
            StartDate = DateTime.Today.AddDays(-((int)DateTime.Today.DayOfWeek + 6) % 7);
            await CreateProgram();
        }

        public override Task ReloadAsync()
        {
            throw new NotImplementedException();
        }

        #endregion Methods
    }

    public class Day : BaseModel
    {
        #region Constructors

        public Day(GenericRepository context, DateTime date)
        {
            Date = date;
            Hours = new ObservableCollection<Hour>
            {
                new Hour(new DateTime(Date.Year,Date.Month,Date.Day,8,0,0),context),
                new Hour(new DateTime(Date.Year,Date.Month,Date.Day,9,0,0),context),
                new Hour(new DateTime(Date.Year,Date.Month,Date.Day,10,0,0),context),
                new Hour(new DateTime(Date.Year,Date.Month,Date.Day,11,0,0),context),
                new Hour(new DateTime(Date.Year,Date.Month,Date.Day,12,0,0),context),
                new Hour(new DateTime(Date.Year,Date.Month,Date.Day,13,0,0),context),
                new Hour(new DateTime(Date.Year,Date.Month,Date.Day,14,0,0),context),
                new Hour(new DateTime(Date.Year,Date.Month,Date.Day,15,0,0),context),
                new Hour(new DateTime(Date.Year,Date.Month,Date.Day,16,0,0),context),
                new Hour(new DateTime(Date.Year,Date.Month,Date.Day,17,0,0),context),
                new Hour(new DateTime(Date.Year,Date.Month,Date.Day,18,0,0),context),
                new Hour(new DateTime(Date.Year,Date.Month,Date.Day,19,0,0),context),
                new Hour(new DateTime(Date.Year,Date.Month,Date.Day,20,0,0),context)
            };
            Context = context;
        }

        #endregion Constructors

        #region Fields

        private DateTime _date;
        private ObservableCollection<Hour> _Hours;

        #endregion Fields

        #region Properties

        public GenericRepository Context { get; }

        public DateTime Date
        {
            get
            {
                return _date;
            }

            set
            {
                if (_date == value)
                {
                    return;
                }

                _date = value;
                RaisePropertyChanged();
            }
        }

        public string DayName => Date.ToString("ddd");

        public ObservableCollection<Hour> Hours
        {
            get
            {
                return _Hours;
            }

            set
            {
                if (_Hours == value)
                {
                    return;
                }

                _Hours = value;
                RaisePropertyChanged();
            }
        }

        #endregion Properties
    }

    public class Hour : BaseModel
    {
        #region Constructors

        public Hour(DateTime time, GenericRepository context)
        {
            Time = time;
            AddApointmentCommand = new RelayCommand(async () => { await AddApointment(); });
            DeleteApointmentCommand = new RelayCommand(async () => { await DeleteApointment(); }, CanDeleteApointment);
            Context = context;
            Apointments = new ObservableCollection<Apointment>();
        }

        #endregion Constructors

        #region Fields

        private ObservableCollection<Apointment> _Apointments;

        #endregion Fields

        #region Properties

        public RelayCommand AddApointmentCommand { get; set; }

        public ObservableCollection<Apointment> Apointments
        {
            get
            {
                return _Apointments;
            }

            set
            {
                if (_Apointments == value)
                {
                    return;
                }

                _Apointments = value;
                RaisePropertyChanged();
            }
        }

        public GenericRepository Context { get; }

        public RelayCommand DeleteApointmentCommand { get; set; }

        public Apointment SelectedApointment { get; set; }

        public DateTime Time { get; set; }

        #endregion Properties

        #region Methods

        private async Task AddApointment()
        {
            CustomersWindow_Viewmodel vm = new CustomersWindow_Viewmodel(Context);
            await vm.LoadAsync();
            Window window = new FindCustomerWidnow
            {
                DataContext = vm
            };
            window.ShowDialog();
            Apointment ap = new Apointment { Customer = vm.SelectedCustomer, DateTime = Time };
            if (vm.SelectedCustomer != null)
            {
                Context.Add(ap);
                await Context.SaveAsync();
                Apointments.Add(ap);
            }
        }

        private bool CanDeleteApointment()
        {
            return SelectedApointment != null;
        }

        private async Task DeleteApointment()
        {
            Mouse.OverrideCursor = Cursors.Wait;

            Context.Delete(SelectedApointment);
            Apointments.Remove(SelectedApointment);
            await Context.SaveAsync();
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        #endregion Methods
    }
}