using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using BubbleStart.Helpers;
using BubbleStart.Messages;
using BubbleStart.Model;
using BubbleStart.Views;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;

namespace BubbleStart.ViewModels
{
    public class Apointments_ViewModel : MyViewModelBase
    {

        #region Constructors

        public Apointments_ViewModel(BasicDataManager basicDataManager)
        {
            Days = new ObservableCollection<Day>();
            NextWeekCommand = new RelayCommand(async () => { await NextWeek(); });
            PreviousWeekCommand = new RelayCommand(async () => { await PreviousWeek(); });
            ShowProgramCommand = new RelayCommand(async () => { await CreateProgram(); });
            ReformerVisible = FunctionalVisible = true;
            BasicDataManager = basicDataManager;
            Messenger.Default.Register<BasicDataManagerRefreshedMessage>(this, msg => Load());

        }

        public bool HasDays => Days != null && Days.Count > 0;

        public RelayCommand ShowProgramCommand { get; set; }

        #endregion Constructors

        #region Fields

        private ObservableCollection<Day> _Days;
        private bool _FunctionalVisible;
        private int _GymIndex;

        private bool _ReformerVisible;

        private int _RoomIndex;

        private DateTime _StartDate;

        #endregion Fields

        #region Properties

        public BasicDataManager BasicDataManager { get; }

        public ObservableCollection<Day> Days
        {
            get => _Days;

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

        public bool FunctionalVisible
        {
            get => _FunctionalVisible;

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

        public int GymIndex
        {
            get => _GymIndex;

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
        public RelayCommand NextWeekCommand { get; set; }

        public RelayCommand PreviousWeekCommand { get; set; }

        public bool ReformerVisible
        {
            get => _ReformerVisible;

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

        public int RoomIndex
        {
            get => _RoomIndex;

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
                RaisePropertyChanged();
            }
        }

        #endregion Properties

        #region Methods

        public async Task CreateProgram()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            DateTime tmpdate = StartDate.AddDays(6);
            List<Apointment> apointments = BasicDataManager.Context.Context.Apointments.Where(a => a.DateTime >= StartDate && a.DateTime < tmpdate && a.DateTime >= BasicDataManager.Context.Limit).ToList();

            DateTime tmpDate = StartDate;
            Days.Clear();
            for (int i = 0; i < 6; i++)
            {
                Days.Add(new Day(BasicDataManager, tmpDate));
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
            RaisePropertyChanged(nameof(HasDays));
            RaisePropertyChanged(nameof(Days));
            Mouse.OverrideCursor = Cursors.Arrow;

        }

        public override void Load(int id = 0, MyViewModelBaseAsync previousViewModel = null)
        {
            StartDate = DateTime.Today.AddDays(-((int)DateTime.Today.DayOfWeek + 6) % 7);
        }

        public override void Reload()
        {
            throw new NotImplementedException();
        }


        private async Task NextWeek()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            StartDate = StartDate.AddDays(7);
            await CreateProgram();
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private async Task PreviousWeek()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            StartDate = StartDate.AddDays(-7);
            await CreateProgram();
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        #endregion Methods
    }

    public class Day : BaseModel
    {

        #region Constructors

        public Day(BasicDataManager basicDataManager, DateTime date)
        {
            Date = date;
            Hours = new ObservableCollection<Hour>
            {
                new Hour(new DateTime(Date.Year,Date.Month,Date.Day,8,0,0),basicDataManager),
                new Hour(new DateTime(Date.Year,Date.Month,Date.Day,9,0,0),basicDataManager),
                new Hour(new DateTime(Date.Year,Date.Month,Date.Day,10,0,0),basicDataManager),
                new Hour(new DateTime(Date.Year,Date.Month,Date.Day,11,0,0),basicDataManager),
                new Hour(new DateTime(Date.Year,Date.Month,Date.Day,12,0,0),basicDataManager),
                new Hour(new DateTime(Date.Year,Date.Month,Date.Day,13,0,0),basicDataManager),
                new Hour(new DateTime(Date.Year,Date.Month,Date.Day,14,0,0),basicDataManager),
                new Hour(new DateTime(Date.Year,Date.Month,Date.Day,15,0,0),basicDataManager),
                new Hour(new DateTime(Date.Year,Date.Month,Date.Day,16,0,0),basicDataManager),
                new Hour(new DateTime(Date.Year,Date.Month,Date.Day,17,0,0),basicDataManager),
                new Hour(new DateTime(Date.Year,Date.Month,Date.Day,18,0,0),basicDataManager),
                new Hour(new DateTime(Date.Year,Date.Month,Date.Day,19,0,0),basicDataManager),
                new Hour(new DateTime(Date.Year,Date.Month,Date.Day,20,0,0),basicDataManager),
                new Hour(new DateTime(Date.Year,Date.Month,Date.Day,21,0,0),basicDataManager)
            };
        }

        #endregion Constructors

        #region Fields

        private DateTime _date;
        private ObservableCollection<Hour> _Hours;

        #endregion Fields

        #region Properties

        public DateTime Date
        {
            get => _date;

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
            get => _Hours;

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

        public Hour(DateTime time, BasicDataManager basicDataManager)
        {
            Time = time;
            BasicDataManager = basicDataManager;
            AddApointmentCommand = new RelayCommand<int>(AddApointment);
            DeleteFunctionalApointmentCommand = new RelayCommand(async () => { await DeleteApointment(0); }, CanDeleteFunctionalApointment);
            DeleteReformerApointmentCommand = new RelayCommand(async () => { await DeleteApointment(1); }, CanDeleteReformerApointment);
            AppointmentsFunctional = new ObservableCollection<Apointment>();
            AppointmentsReformer = new ObservableCollection<Apointment>();
        }

        #endregion Constructors

        #region Fields

        private ObservableCollection<Apointment> _ApointmentsFunctional;

        private ObservableCollection<Apointment> _AppointmentsReformer;

        private ICollectionView _FunctionalCV;

        private int _GymIndex;

        private ICollectionView _ReformerCV;

        #endregion Fields

        #region Properties

        public RelayCommand<int> AddApointmentCommand { get; set; }

        public ObservableCollection<Apointment> AppointmentsFunctional
        {
            get => _ApointmentsFunctional;

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
                FunctionalCV.SortDescriptions.Add(new SortDescription(nameof(Apointment.Person),ListSortDirection.Ascending));
                FunctionalCV.Refresh();
            }
        }

        public ObservableCollection<Apointment> AppointmentsReformer
        {
            get => _AppointmentsReformer;

            set
            {
                if (_AppointmentsReformer == value)
                {
                    return;
                }

                _AppointmentsReformer = value;
                RaisePropertyChanged();
                ReformerCV = (CollectionView)CollectionViewSource.GetDefaultView(_AppointmentsReformer);
                ReformerCV.SortDescriptions.Add(new SortDescription(nameof(Apointment.Person), ListSortDirection.Ascending));
                ReformerCV.Filter = AppointmensFilter;
                ReformerCV.Refresh();
            }
        }

        public RelayCommand DeleteFunctionalApointmentCommand { get; set; }

        public RelayCommand DeleteReformerApointmentCommand { get; set; }

        public ICollectionView FunctionalCV
        {
            get => _FunctionalCV;

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

        public int GymIndex
        {
            get => _GymIndex;

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

        public ICollectionView ReformerCV
        {
            get => _ReformerCV;

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

        public Apointment SelectedApointmentFunctional { get; set; }

        public Apointment SelectedApointmentReformer { get; set; }

        public DateTime Time { get; set; }
        public BasicDataManager BasicDataManager { get; }

        #endregion Properties

        #region Methods

        public async Task AddCustomer(Customer customer, int selectedPerson, int type, bool forever = false)
        {
            if (customer != null)
            {
                Mouse.OverrideCursor = Cursors.Wait;
                Apointment ap = new Apointment { Customer = customer, DateTime = Time, Person = selectedPerson, Room = type };
                if (forever)
                {
                    DateTime tmpdate = Time;
                    while (Time.Month != 8)
                    {
                        Time = Time.AddDays(7);
                        BasicDataManager.Add(new Apointment { Customer = customer, DateTime = Time, Person = selectedPerson, Room = type });
                    }
                }
                if ((type == 0 && !AppointmentsFunctional.Any(a => a.Customer.Id == ap.Customer.Id)) || (type == 1 && !AppointmentsReformer.Any(api => api.Customer.Id == ap.Customer.Id)))
                {
                    if (type == 0)
                    {
                        AppointmentsFunctional.Add(ap);
                        BasicDataManager.Add(ap);

                    }
                    else
                    {
                        AppointmentsReformer.Add(ap);
                        BasicDataManager.Add(ap);

                    }
                }
                await BasicDataManager.SaveAsync();
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        private void AddApointment(int type)
        {
            CustomersWindow_Viewmodel vm = new CustomersWindow_Viewmodel(BasicDataManager, type, this);
            vm.Load();
            Window window = new FindCustomerWidnow
            {
                DataContext = vm
            };
            window.ShowDialog();
        }

        private bool AppointmensFilter(object obj)
        {
            if (obj is Apointment a && (GymIndex == 0 || (GymIndex == a.Person + 1)))
            {
                return true;
            }
            return false;
        }

        private bool CanDeleteFunctionalApointment()
        {
            return SelectedApointmentFunctional != null;
        }

        private bool CanDeleteReformerApointment()
        {
            return SelectedApointmentReformer != null;
        }
        private async Task DeleteApointment(int type)
        {
            Mouse.OverrideCursor = Cursors.Wait;

            if (type == 0)
            {
                BasicDataManager.Delete(SelectedApointmentFunctional);
                AppointmentsFunctional.Remove(SelectedApointmentFunctional);
            }
            else
            {
                BasicDataManager.Delete(SelectedApointmentReformer);
                AppointmentsReformer.Remove(SelectedApointmentReformer);
            }
            await BasicDataManager.SaveAsync();
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        #endregion Methods

    }
}