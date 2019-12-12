using BubbleStart.Database;
using BubbleStart.Model;
using BubbleStart.Views;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
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
            ReformerVisible = FunctionalVisible = true;
        }

        private int _GymIndex;

        public int GymIndex
        {
            get
            {
                return _GymIndex;
            }

            set
            {
                if (_GymIndex == value)
                {
                    return;
                }

                _GymIndex = value;
                RaisePropertyChanged();
                foreach (var day in Days)
                {
                    foreach (var hour in day.Hours)
                    {
                        hour.GymIndex = value;
                    }
                }
            }
        }

        private int _RoomIndex;

        public int RoomIndex
        {
            get
            {
                return _RoomIndex;
            }

            set
            {
                if (_RoomIndex == value)
                {
                    return;
                }

                _RoomIndex = value;
                RaisePropertyChanged();
                if (RoomIndex == 0)
                {
                    FunctionalVisible = ReformerVisible = true;
                }
                else if (RoomIndex == 1)
                {
                    FunctionalVisible = true;
                    ReformerVisible = false;
                }
                else
                {
                    FunctionalVisible = false;
                    ReformerVisible = true;
                }
            }
        }

        private bool _ReformerVisible;

        public bool ReformerVisible
        {
            get
            {
                return _ReformerVisible;
            }

            set
            {
                if (_ReformerVisible == value)
                {
                    return;
                }

                _ReformerVisible = value;
                RaisePropertyChanged();
            }
        }

        private bool _FunctionalVisible;

        public bool FunctionalVisible
        {
            get
            {
                return _FunctionalVisible;
            }

            set
            {
                if (_FunctionalVisible == value)
                {
                    return;
                }

                _FunctionalVisible = value;
                RaisePropertyChanged();
            }
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
            for (int i = 0; i < 6; i++)
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
                    if (ap.Room == 0)
                        Days[numOfDay].Hours[ap.DateTime.Hour - 8].AppointmentsFunctional.Add(ap);
                    else
                        Days[numOfDay].Hours[ap.DateTime.Hour - 8].AppointmentsReformer.Add(ap);
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
                new Hour(new DateTime(Date.Year,Date.Month,Date.Day,20,0,0),context),
                new Hour(new DateTime(Date.Year,Date.Month,Date.Day,21,0,0),context)
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
            AddApointmentCommand = new RelayCommand<int>(async (obj) => { await AddApointment(obj); });
            DeleteFunctionalApointmentCommand = new RelayCommand(async () => { await DeleteApointment(0); }, CanDeleteFunctionalApointment);
            DeleteReformerApointmentCommand = new RelayCommand(async () => { await DeleteApointment(1); }, CanDeleteReformerApointment);
            Context = context;
            AppointmentsFunctional = new ObservableCollection<Apointment>();
            AppointmentsReformer = new ObservableCollection<Apointment>();
        }

        private bool CanDeleteReformerApointment()
        {
            return SelectedApointmentReformer != null;
        }

        #endregion Constructors

        #region Fields

        private ObservableCollection<Apointment> _ApointmentsFunctional;

        #endregion Fields

        #region Properties

        public RelayCommand<int> AddApointmentCommand { get; set; }

        public ObservableCollection<Apointment> AppointmentsFunctional
        {
            get
            {
                return _ApointmentsFunctional;
            }

            set
            {
                if (_ApointmentsFunctional == value)
                {
                    return;
                }

                _ApointmentsFunctional = value;
                RaisePropertyChanged();
                FunctionalCV = (CollectionView)CollectionViewSource.GetDefaultView(_ApointmentsFunctional);
                FunctionalCV.Filter = AppointmensFilter;
                FunctionalCV.Refresh();
            }
        }

        private ICollectionView _ReformerCV;

        public ICollectionView ReformerCV
        {
            get
            {
                return _ReformerCV;
            }

            set
            {
                if (_ReformerCV == value)
                {
                    return;
                }

                _ReformerCV = value;
                RaisePropertyChanged();
            }
        }

        private ICollectionView _FunctionalCV;

        public ICollectionView FunctionalCV
        {
            get
            {
                return _FunctionalCV;
            }

            set
            {
                if (_FunctionalCV == value)
                {
                    return;
                }

                _FunctionalCV = value;
                RaisePropertyChanged();
            }
        }

        private bool AppointmensFilter(object obj)
        {
            if (obj is Apointment a && (GymIndex == 0 || (GymIndex == a.Person + 1)))
            {
                return true;
            }
            return false;
        }

        private int _GymIndex;

        public int GymIndex
        {
            get
            {
                return _GymIndex;
            }

            set
            {
                if (_GymIndex == value)
                {
                    return;
                }

                _GymIndex = value;
                RaisePropertyChanged();
                FunctionalCV.Refresh();
                ReformerCV.Refresh();
            }
        }

        private ObservableCollection<Apointment> _AppointmentsReformer;

        public ObservableCollection<Apointment> AppointmentsReformer
        {
            get
            {
                return _AppointmentsReformer;
            }

            set
            {
                if (_AppointmentsReformer == value)
                {
                    return;
                }

                _AppointmentsReformer = value;
                RaisePropertyChanged();
                ReformerCV = (CollectionView)CollectionViewSource.GetDefaultView(_AppointmentsReformer);
                ReformerCV.Filter = AppointmensFilter;
                ReformerCV.Refresh();
            }
        }

        public GenericRepository Context { get; }

        public RelayCommand DeleteFunctionalApointmentCommand { get; set; }
        public RelayCommand DeleteReformerApointmentCommand { get; set; }

        public Apointment SelectedApointmentFunctional { get; set; }
        public Apointment SelectedApointmentReformer { get; set; }

        public DateTime Time { get; set; }

        #endregion Properties

        #region Methods

        public async Task AddCustomer(Customer customer, int selectedPerson, int type, bool forever = false)
        {
            if (customer != null)
            {
                Mouse.OverrideCursor = Cursors.Wait;
                Apointment ap = new Apointment { Customer = customer, DateTime = Time, Person = selectedPerson, Room = type };
                Context.Add(ap);
                if (forever)
                {
                    DateTime tmpdate = Time;
                    while (Time.Month != 8)
                    {
                        Time = Time.AddDays(7);
                        Context.Add(new Apointment { Customer = customer, DateTime = Time, Person = selectedPerson, Room = type });
                    }
                }
                if (!AppointmentsFunctional.Any(a => a.Customer.Id == ap.Customer.Id) && !AppointmentsReformer.Any(api => api.Customer.Id == ap.Customer.Id))
                {
                    if (type == 0)
                    {
                        AppointmentsFunctional.Add(ap);
                    }
                    else
                    {
                        AppointmentsReformer.Add(ap);
                    }
                }
                await Context.SaveAsync();
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        private async Task AddApointment(int type)
        {
            CustomersWindow_Viewmodel vm = new CustomersWindow_Viewmodel(Context, type, this);
            await vm.LoadAsync();
            Window window = new FindCustomerWidnow
            {
                DataContext = vm
            };
            window.ShowDialog();
        }

        private bool CanDeleteFunctionalApointment()
        {
            return SelectedApointmentFunctional != null;
        }

        private async Task DeleteApointment(int type)
        {
            Mouse.OverrideCursor = Cursors.Wait;

            if (type == 0)
            {
                Context.Delete(SelectedApointmentFunctional);
                AppointmentsFunctional.Remove(SelectedApointmentFunctional);
            }
            else
            {
                Context.Delete(SelectedApointmentReformer);
                AppointmentsReformer.Remove(SelectedApointmentReformer);
            }
            await Context.SaveAsync();
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        #endregion Methods
    }
}