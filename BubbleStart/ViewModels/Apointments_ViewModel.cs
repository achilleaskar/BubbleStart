using BubbleStart.Helpers;
using BubbleStart.Messages;
using BubbleStart.Migrations;
using BubbleStart.Model;
using BubbleStart.Views;
using DocumentFormat.OpenXml.Office2016.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

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
            Messenger.Default.Register<UpdateProgramMessage>(this, async (msg) => await CreateProgram(false));
            Messenger.Default.Register<UpdateClosedHoursMessage>(this, msg => RefreshProgram());
        }

        public bool HasDays => Days != null && Days.Count > 0;

        private DateTime _SelectedDayToGo;

        public DateTime SelectedDayToGo
        {
            get
            {
                return _SelectedDayToGo;
            }

            set
            {
                if (_SelectedDayToGo == value)
                {
                    return;
                }

                _SelectedDayToGo = value;

                RaisePropertyChanged();
            }
        }

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

        public void OpenCustomerManagement(Customer c)
        {
            c.BasicDataManager = BasicDataManager;
            c.UpdateCollections();
            Window window = new CustomerManagement
            {
                DataContext = c
            };
            Messenger.Default.Send(new OpenChildWindowCommand(window));
        }

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


        public void RefreshProgram()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            StartDate = SelectedDayToGo.AddDays(-((int)SelectedDayToGo.DayOfWeek + 6) % 7);
            DateTime tmpdate = StartDate.AddDays(6);

            List<ClosedHour> closedHours = BasicDataManager.Context.Context.ClosedHours.Local.Where(a => a.Date >= StartDate && a.Date < tmpdate && a.Date >= BasicDataManager.Context.Limit).ToList();

            DateTime tmpDate = StartDate;

            int numOfDay;

            foreach (var ch in closedHours)
            {
                numOfDay = ((int)ch.Date.DayOfWeek + 6) % 7;
                if (numOfDay < 6 && ch.Date.Hour >= 8 && ch.Date.Hour <= 21)
                {
                    if (ch.Room == 0)
                    {
                        Days[numOfDay].Hours[ch.Date.Hour - 8].ClosedHour0 = ch;
                    }
                    else
                    {
                        Days[numOfDay].Hours[ch.Date.Hour - 8].ClosedHour1 = ch;
                    }
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


        public async Task CreateProgram(bool refresh = true)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            StartDate = SelectedDayToGo.AddDays(-((int)SelectedDayToGo.DayOfWeek + 6) % 7);
            DateTime tmpdate = StartDate.AddDays(6);

            List<Apointment> apointments = refresh ? await BasicDataManager.Context.Context.Apointments.Where(a => a.DateTime >= StartDate && a.DateTime < tmpdate && a.DateTime >= BasicDataManager.Context.Limit).ToListAsync() :
               BasicDataManager.Context.Context.Apointments.Local.Where(a => a.DateTime >= StartDate && a.DateTime < tmpdate && a.DateTime >= BasicDataManager.Context.Limit).ToList();

            List<ClosedHour> closedHours = refresh ? await BasicDataManager.Context.Context.ClosedHours.Where(a => a.Date >= StartDate && a.Date < tmpdate && a.Date >= BasicDataManager.Context.Limit).ToListAsync() :
              BasicDataManager.Context.Context.ClosedHours.Local.Where(a => a.Date >= StartDate && a.Date < tmpdate && a.Date >= BasicDataManager.Context.Limit).ToList();

            DateTime tmpDate = StartDate;
            Days.Clear();
            for (int i = 0; i < 6; i++)
            {
                Days.Add(new Day(BasicDataManager, tmpDate));
                tmpDate = tmpDate.AddDays(1);
            }

            int numOfDay;

            foreach (var ap in closedHours)
            {
                numOfDay = ((int)ap.Date.DayOfWeek + 6) % 7;
                if (numOfDay < 6 && ap.Date.Hour >= 8 && ap.Date.Hour <= 21)
                {
                    if (ap.Room == 0)
                    {
                        Days[numOfDay].Hours[ap.Date.Hour - 8].ClosedHour0 = ap;
                    }
                    else
                    {
                        Days[numOfDay].Hours[ap.Date.Hour - 8].ClosedHour1 = ap;
                    }
                }
                else
                {
                    MessageBox.Show("Ραντεβού εκτός εβδομάδας");
                }
            }

            foreach (var ap in apointments)
            {
                numOfDay = ((int)ap.DateTime.DayOfWeek + 6) % 7;
                if (numOfDay < 6 && ap.DateTime.Hour >= 8 && ap.DateTime.Hour <= 21)
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
            SelectedDayToGo = DateTime.Today;
            Days = new ObservableCollection<Day>();
        }

        public override void Reload()
        {
            throw new NotImplementedException();
        }

        private async Task NextWeek()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            SelectedDayToGo = SelectedDayToGo.AddDays(-((int)SelectedDayToGo.DayOfWeek + 6) % 7 + 7);
            await CreateProgram();
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private async Task PreviousWeek()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            SelectedDayToGo = SelectedDayToGo.AddDays(-((int)SelectedDayToGo.DayOfWeek + 6) % 7 - 7);

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
            ToggleEnabled0Command = new RelayCommand(async () => await ToggleEnabled(0));
            ToggleEnabled1Command = new RelayCommand(async () => await ToggleEnabled(1));
            ToggleEnabled0ForEverCommand = new RelayCommand(async () => await ToggleEnabledForEver(0));
            ToggleEnabled1ForEverCommand = new RelayCommand(async () => await ToggleEnabledForEver(1));
            Enable0ForEverCommand = new RelayCommand(async () => await EnableForEver(0));
            Enable1ForEverCommand = new RelayCommand(async () => await EnableForEver(1));
            AppointmentsFunctional = new ObservableCollection<Apointment>();
            AppointmentsReformer = new ObservableCollection<Apointment>();
        }

        private async Task EnableForEver(int room)
        {
            List<ClosedHour> ClosedHours = await BasicDataManager.Context.GetAllClosedHoursAsync(room, Time);

            foreach (var item in ClosedHours)
            {
                BasicDataManager.Delete(item);
            }
            await BasicDataManager.SaveAsync();
            ClosedHour0 = ClosedHour1 = null;
            Messenger.Default.Send(new UpdateClosedHoursMessage());
            RaisePropertyChanged(nameof(ClosedColor1));
            RaisePropertyChanged(nameof(ClosedColor0));
        }

        private async Task ToggleEnabledForEver(int room)
        {

            List<ClosedHour> ClosedHours = await BasicDataManager.Context.GetAllClosedHoursAsync(room, Time);
            var limit = Time.AddMonths(3);
            var tmpTime = Time;
            while (tmpTime < limit)
            {
                if (!ClosedHours.Any(c => c.Date == tmpTime))
                {
                    BasicDataManager.Add(new ClosedHour { Date = tmpTime, Room = room });
                }
                tmpTime = tmpTime.AddDays(7);
            }

            await BasicDataManager.SaveAsync();
            Messenger.Default.Send(new UpdateClosedHoursMessage());
            RaisePropertyChanged(nameof(ClosedColor1));
            RaisePropertyChanged(nameof(ClosedColor0));
        }

        public SolidColorBrush ClosedColor0 => GetClosedColor(0);

        private SolidColorBrush GetClosedColor(int v)
        {
            if (v == 0)
            {
                if (ClosedHour0 != null)
                {
                    return new SolidColorBrush(Colors.Red);
                }
                else
                {
                    return new SolidColorBrush(Colors.LightYellow);
                }
            }
            else if (v == 1)
            {
                if (ClosedHour1 != null)
                {
                    return new SolidColorBrush(Colors.Red);
                }
                else
                {
                    return new SolidColorBrush(Colors.BlanchedAlmond);
                }
            }
            return new SolidColorBrush(Colors.BlanchedAlmond);
        }

        public SolidColorBrush ClosedColor1 => GetClosedColor(1);

        private async Task ToggleEnabled(int room)
        {
            if (room == 0)
            {
                if (ClosedHour0 != null)
                {
                    BasicDataManager.Context.Delete(ClosedHour0);
                    ClosedHour0 = null;
                }
                else
                {
                    ClosedHour0 = new ClosedHour { Date = Time, Room = room };
                    BasicDataManager.Add(ClosedHour0);
                }
            }
            else if (room == 1)
            {
                if (ClosedHour1 != null)
                {
                    BasicDataManager.Context.Delete(ClosedHour1);
                    ClosedHour1 = null;
                }
                else
                {
                    ClosedHour1 = new ClosedHour { Date = Time, Room = room };
                    BasicDataManager.Add(ClosedHour1);
                }
            }
            await BasicDataManager.SaveAsync();
            RaisePropertyChanged(nameof(ClosedColor1));
            RaisePropertyChanged(nameof(ClosedColor0));
        }

        public Hour Self => this;

        private ClosedHour _ClosedHour0;

        public ClosedHour ClosedHour0
        {
            get
            {
                return _ClosedHour0;
            }

            set
            {
                if (_ClosedHour0 == value)
                {
                    return;
                }

                _ClosedHour0 = value;
                RaisePropertyChanged();
            }
        }

        private ClosedHour _ClosedHour1;

        public ClosedHour ClosedHour1
        {
            get
            {
                return _ClosedHour1;
            }

            set
            {
                if (_ClosedHour1 == value)
                {
                    return;
                }

                _ClosedHour1 = value;
                RaisePropertyChanged();
            }
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
        public RelayCommand<int> CLoseHourCommand { get; set; }
        public RelayCommand ToggleEnabled1Command { get; set; }
        public RelayCommand ToggleEnabled0Command { get; set; }
        public RelayCommand ToggleEnabled0ForEverCommand { get; set; }
        public RelayCommand ToggleEnabled1ForEverCommand { get; set; }
        public RelayCommand Enable0ForEverCommand { get; set; }
        public RelayCommand Enable1ForEverCommand { get; set; }

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
                FunctionalCV.SortDescriptions.Add(new SortDescription(nameof(Apointment.Person), ListSortDirection.Ascending));
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

        public async Task AddCustomer(Customer customer, SelectedPersonEnum selectedPerson, int type, bool forever = false)
        {
            var tmpTime = Time;
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
            Time = tmpTime;
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
            if (obj is Apointment a && (GymIndex == 0 || (GymIndex == (int)a.Person + 1)))
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