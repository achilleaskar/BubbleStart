using BubbleStart.Helpers;
using BubbleStart.Messages;
using BubbleStart.Model;
using BubbleStart.Views;
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
            NoGymnastCommand = new RelayCommand<Hour>(async (obj) => await NoGymnast(obj));
            SetCustomTimeCommand = new RelayCommand(async () => { await (SetCustomTime()); });
            ReformerVisible = FunctionalVisible = OutdoorVisible = MassageVisible = true;
            BasicDataManager = basicDataManager;
            Messenger.Default.Register<BasicDataManagerRefreshedMessage>(this, msg => Load());
            Messenger.Default.Register<UpdateProgramMessage>(this, async (msg) => await CreateProgram(false));
            Messenger.Default.Register<UpdateClosedHoursMessage>(this, msg => RefreshProgram());
            Messenger.Default.Register<OpenPopupUpMessage>(this, msg => ChangeTime(msg.Hour, msg.Room));
        }

        #endregion Constructors

        #region Fields

        private string _CustomTime;
        private ObservableCollection<Day> _Days;
        private bool _FunctionalVisible;
        private int _GymIndex;
        private ObservableCollection<User> _Gymnasts;
        private bool _IsGymChecked;
        private bool _MassageVisible;
        private bool _OutdoorVisible;
        private bool _ReformerVisible;
        private int _RoomIndex;
        private DateTime _SelectedDayToGo;
        private DateTime _StartDate;
        private bool _TimePopupOpen;

        #endregion Fields

        #region Properties

        public BasicDataManager BasicDataManager { get; }

        public string CustomTime
        {
            get
            {
                return _CustomTime;
            }

            set
            {
                if (_CustomTime == value)
                {
                    return;
                }

                _CustomTime = value;
                RaisePropertyChanged();
            }
        }

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

        public ObservableCollection<User> Gymnasts
        {
            get => _Gymnasts;

            set
            {
                if (_Gymnasts == value)
                {
                    return;
                }

                _Gymnasts = value;
                RaisePropertyChanged();
            }
        }

        public bool HasDays => Days != null && Days.Count > 0;

        public bool IsGymChecked
        {
            get => _IsGymChecked;

            set
            {
                if (_IsGymChecked == value)
                {
                    return;
                }

                _IsGymChecked = value;
                RaisePropertyChanged();
            }
        }

        public bool MassageVisible
        {
            get => _MassageVisible;

            set
            {
                if (_MassageVisible == value)
                {
                    return;
                }

                _MassageVisible = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand NextWeekCommand { get; set; }
        public RelayCommand<Hour> NoGymnastCommand { get; set; }

        public bool OutdoorVisible
        {
            get => _OutdoorVisible;

            set
            {
                if (_OutdoorVisible == value)
                {
                    return;
                }

                _OutdoorVisible = value;
                RaisePropertyChanged();
            }
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
                    FunctionalVisible = ReformerVisible = MassageVisible = OutdoorVisible = true;
                }
                else if (RoomIndex == 1)
                {
                    ReformerVisible = MassageVisible = OutdoorVisible = false;
                    FunctionalVisible = true;
                }
                else if (RoomIndex == 2)
                {
                    FunctionalVisible = MassageVisible = OutdoorVisible = false;
                    ReformerVisible = true;
                }
                else if (RoomIndex == 3)
                {
                    FunctionalVisible = ReformerVisible = OutdoorVisible = false;
                    MassageVisible = true;
                }
                else if (RoomIndex == 4)
                {
                    FunctionalVisible = ReformerVisible = MassageVisible = false;
                    OutdoorVisible = true;
                }
            }
        }

        public DateTime SelectedDayToGo
        {
            get => _SelectedDayToGo;

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

        public RelayCommand SetCustomTimeCommand { get; set; }
        public RelayCommand ShowProgramCommand { get; set; }

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

        public bool TimePopupOpen
        {
            get
            {
                return _TimePopupOpen;
            }

            set
            {
                if (_TimePopupOpen == value)
                {
                    return;
                }

                _TimePopupOpen = value;
                RaisePropertyChanged();
            }
        }

        private Hour selectedHour { get; set; }
        private RoomEnum selectedRoom { get; set; }

        #endregion Properties

        #region Methods

        public async Task CreateProgram(bool refresh = true)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            StartDate = SelectedDayToGo.AddDays(-((int)SelectedDayToGo.DayOfWeek + 6) % 7);
            DateTime tmpdate = StartDate.AddDays(6);

            List<Apointment> apointments = refresh ? await BasicDataManager.Context.Context.Apointments.Where(a => a.DateTime >= StartDate && a.DateTime < tmpdate).ToListAsync() :
               BasicDataManager.Context.Context.Apointments.Local.Where(a => a.DateTime >= StartDate && a.DateTime < tmpdate).ToList();

            if (refresh)
                await BasicDataManager.Context.Context.ShowUps.Where(a => a.Arrived >= StartDate && a.Arrived < tmpdate && a.Arrived < a.Customer.ResetDate).ToListAsync();
            else
                BasicDataManager.Context.Context.ShowUps.Local.Where(a => a.Arrived >= StartDate && a.Arrived < tmpdate && a.Arrived < a.Customer.ResetDate).ToList();

            List<CustomeTime> customTimes = refresh ? await BasicDataManager.Context.Context.CustomeTimes.Where(a => a.Datetime >= StartDate && a.Datetime < tmpdate).ToListAsync() :
              BasicDataManager.Context.Context.CustomeTimes.Local.Where(a => a.Datetime >= StartDate && a.Datetime < tmpdate).ToList();

            List<GymnastHour> gymnasts = StaticResources.User.Level > 1 ? new List<GymnastHour>() : refresh ? await BasicDataManager.Context.Context.GymnastHours.Where(a => a.Datetime >= StartDate && a.Datetime < tmpdate).ToListAsync() :
             BasicDataManager.Context.Context.GymnastHours.Local.Where(a => a.Datetime >= StartDate && a.Datetime < tmpdate).ToList();

            List<ClosedHour> closedHours = refresh ? await BasicDataManager.Context.Context.ClosedHours.Where(a => a.Date >= StartDate && a.Date < tmpdate).ToListAsync() :
              BasicDataManager.Context.Context.ClosedHours.Local.Where(a => a.Date >= StartDate && a.Date < tmpdate).ToList();

            DateTime tmpDate = StartDate;
            Days.Clear();
            for (int i = 0; i < 6; i++)
            {
                Days.Add(new Day(BasicDataManager, tmpDate));
                tmpDate = tmpDate.AddDays(1);
            }
            foreach (var d in Days)
            {
                foreach (var h in d.Hours)
                {
                    h.parent = this;
                }
            }
            int numOfDay;

            foreach (var ch in closedHours)
            {
                numOfDay = ((int)ch.Date.DayOfWeek + 6) % 7;
                if (numOfDay < 6 && ch.Date.Hour >= 8 && ch.Date.Hour <= 21)
                {
                    if (ch.Room == RoomEnum.Functional)
                    {
                        Days[numOfDay].Hours[ch.Date.Hour - 8].ClosedHour0 = ch;
                    }
                    else if (ch.Room == RoomEnum.Pilates)
                    {
                        Days[numOfDay].Hours[ch.Date.Hour - 8].ClosedHour1 = ch;
                    }
                    else if (ch.Room == RoomEnum.Massage)
                    {
                        Days[numOfDay].Hours[ch.Date.Hour - 8].ClosedHourMassage = ch;
                    }
                    else if (ch.Room == RoomEnum.Outdoor)
                    {
                        Days[numOfDay].Hours[ch.Date.Hour - 8].ClosedHourOutdoor = ch;
                    }
                    else
                    {
                        MessageBox.Show("Σφάλμα στην κλειστή ώρα");
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
                    if (ap.Room == RoomEnum.Functional)
                        Days[numOfDay].Hours[ap.DateTime.Hour - 8].AppointmentsFunctional.Add(ap);
                    else if (ap.Room == RoomEnum.Pilates)
                        Days[numOfDay].Hours[ap.DateTime.Hour - 8].AppointmentsReformer.Add(ap);
                    else if (ap.Room == RoomEnum.Massage)
                        Days[numOfDay].Hours[ap.DateTime.Hour - 8].AppointmentsMassage.Add(ap);
                    else if (ap.Room == RoomEnum.Outdoor)
                        Days[numOfDay].Hours[ap.DateTime.Hour - 8].AppointemntsOutdoor.Add(ap);
                    else
                    {
                        MessageBox.Show("Σφάλμα στην επιλογή αίθουσας");
                    }
                }
                else
                {
                    MessageBox.Show("Ραντεβού εκτός εβδομάδας");
                }
            }
            Hour t;
            foreach (var ct in customTimes)
            {
                t = Days.FirstOrDefault(d => d.Date == ct.Datetime.Date)?.Hours.FirstOrDefault(h => h.Time.Hour == ct.Datetime.Hour && ct.Datetime.Minute == h.Time.Minute);
                if (t != null)
                    switch (ct.Room)
                    {
                        case RoomEnum.Functional:
                            t.CustomTime1 = ct;
                            break;

                        case RoomEnum.Pilates:
                            t.CustomTime2 = ct;
                            break;

                        case RoomEnum.Massage:
                            t.CustomTime3 = ct;
                            break;

                        case RoomEnum.Outdoor:
                            t.CustomTime4 = ct;
                            break;
                    }
            }
            foreach (var ct in gymnasts)
            {
                t = Days.FirstOrDefault(d => d.Date == ct.Datetime.Date)?.Hours.FirstOrDefault(h => h.Time.Hour == ct.Datetime.Hour && ct.Datetime.Minute == h.Time.Minute);
                if (t != null)
                    switch (ct.Room)
                    {
                        case RoomEnum.Functional:
                            t.GymnastFunctional = ct;
                            break;

                        case RoomEnum.Pilates:
                            t.GymnastReformer = ct;
                            break;

                        case RoomEnum.Massage:
                            t.GymnastMassage = ct;
                            break;

                        case RoomEnum.Outdoor:
                            t.GymnastOutdoor = ct;
                            break;
                    }
            }

            var Wrules = new List<WorkingRule>();
            WorkingRule rule;
            DayWorkingShift dayWR;

            foreach (var day in Days)
            {
                Wrules = new List<WorkingRule>();
                foreach (var u in BasicDataManager.Users)
                {
                    rule = u.WorkingRules.OrderByDescending(w => w.Id).FirstOrDefault(r => r.From <= day.Date && r.To >= day.Date);
                    if (rule != null)
                    {
                        Wrules.Add(rule);
                    }
                }

                foreach (var hour in day.Hours)
                {
                    hour.GymnastsWorking = "";
                    foreach (var wr in Wrules)
                    {
                        dayWR = wr.DailyWorkingShifts.FirstOrDefault(e => e.NumOfDay == (int)day.Date.DayOfWeek);
                        if (dayWR != null &&
                            ((dayWR.Shift.From.TimeOfDay <= hour.Time.TimeOfDay && dayWR.Shift.To.TimeOfDay >= hour.Time.AddHours(1).TimeOfDay) ||
                            (dayWR.Shift.FromB?.TimeOfDay <= hour.Time.TimeOfDay && dayWR.Shift.ToB?.TimeOfDay >= hour.Time.AddHours(1).TimeOfDay)))
                        {
                            hour.GymnastsWorking += wr.User.Name + ", ";
                        }
                    }
                    hour.GymnastsWorking = hour.GymnastsWorking.TrimEnd(' ').TrimEnd(',');
                }
            }

            RaisePropertyChanged(nameof(HasDays));
            RaisePropertyChanged(nameof(Days));
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        public override void Load(int id = 0, MyViewModelBaseAsync previousViewModel = null)
        {
            StartDate = DateTime.Today.AddDays(-((int)DateTime.Today.DayOfWeek + 6) % 7);
            Gymnasts = new ObservableCollection<User>(BasicDataManager.Users.Where(u => u.Id == 4 || u.Level == 4));
            SelectedDayToGo = DateTime.Today;
            Days = new ObservableCollection<Day>();
        }

        public void OpenCustomerManagement(Customer c)
        {
            c.EditedInCustomerManagement=true;
            c.BasicDataManager = BasicDataManager;
            c.UpdateCollections();
            Window window = new CustomerManagement
            {
                DataContext = c
            };
            Messenger.Default.Send(new OpenChildWindowCommand(window));
        }

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
                    if (ch.Room == RoomEnum.Functional)
                    {
                        Days[numOfDay].Hours[ch.Date.Hour - 8].ClosedHour0 = ch;
                    }
                    else if (ch.Room == RoomEnum.Pilates)
                    {
                        Days[numOfDay].Hours[ch.Date.Hour - 8].ClosedHour1 = ch;
                    }
                    else if (ch.Room == RoomEnum.Massage)
                    {
                        Days[numOfDay].Hours[ch.Date.Hour - 8].ClosedHourMassage = ch;
                    }
                    else if (ch.Room == RoomEnum.Outdoor)
                    {
                        Days[numOfDay].Hours[ch.Date.Hour - 8].ClosedHourOutdoor = ch;
                    }
                    else
                    {
                        MessageBox.Show("Σφάλμα στην κλειστή ώρα");
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

        public override void Reload()
        {
            throw new NotImplementedException();
        }

        private void ChangeTime(Hour h, RoomEnum room)
        {
            selectedHour = h;
            selectedRoom = room;
            CustomTime = selectedHour.Time.ToString("HH:mm");
            TimePopupOpen = true;
        }

        private async Task NextWeek()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            SelectedDayToGo = SelectedDayToGo.AddDays(-((int)SelectedDayToGo.DayOfWeek + 6) % 7 + 7);
            await CreateProgram();
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private async Task NoGymnast(Hour apo)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            //if (apo.Gymnast != null)
            //{
            //    apo.Gymnast = null;
            //    apo.RaisePropertyChanged(nameof(apo.GymColor));
            //    await BasicDataManager.SaveAsync();
            //}
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private async Task PreviousWeek()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            SelectedDayToGo = SelectedDayToGo.AddDays(-((int)SelectedDayToGo.DayOfWeek + 6) % 7 - 7);

            await CreateProgram();
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private async Task SetCustomTime()
        {
            if (selectedHour == null)
                return;
            Mouse.OverrideCursor = Cursors.Wait;
            if (selectedRoom == RoomEnum.Functional)
            {
                if (selectedHour.CustomTime1 != null)
                {
                    selectedHour.CustomTime1.Time = CustomTime;
                }
                else
                {
                    selectedHour.CustomTime1 = new CustomeTime { Datetime = selectedHour.Time, Room = selectedRoom, Time = CustomTime };

                    BasicDataManager.Add(selectedHour.CustomTime1);
                }
                selectedHour.RaisePropertyChanged(nameof(Hour.TimeString1));
            }
            else if (selectedRoom == RoomEnum.Pilates)
            {
                if (selectedHour.CustomTime2 != null)
                {
                    selectedHour.CustomTime2.Time = CustomTime;
                }
                else
                {
                    selectedHour.CustomTime2 = new CustomeTime { Datetime = selectedHour.Time, Room = selectedRoom, Time = CustomTime };

                    BasicDataManager.Add(selectedHour.CustomTime2);
                }
                selectedHour.RaisePropertyChanged(nameof(Hour.TimeString2));
            }
            else if (selectedRoom == RoomEnum.Massage)
            {
                if (selectedHour.CustomTime3 != null)
                {
                    selectedHour.CustomTime3.Time = CustomTime;
                }
                else
                {
                    selectedHour.CustomTime3 = new CustomeTime { Datetime = selectedHour.Time, Room = selectedRoom, Time = CustomTime };

                    BasicDataManager.Add(selectedHour.CustomTime3);
                }
                selectedHour.RaisePropertyChanged(nameof(Hour.TimeString3));
            }
            else if (selectedRoom == RoomEnum.Outdoor)
            {
                if (selectedHour.CustomTime4 != null)
                {
                    selectedHour.CustomTime4.Time = CustomTime;
                }
                else
                {
                    selectedHour.CustomTime4 = new CustomeTime { Datetime = selectedHour.Time, Room = selectedRoom, Time = CustomTime };

                    BasicDataManager.Add(selectedHour.CustomTime4);
                }
                selectedHour.RaisePropertyChanged(nameof(Hour.TimeString4));
            }

            await BasicDataManager.SaveAsync();
            Mouse.OverrideCursor = Cursors.Arrow;
            TimePopupOpen = false;
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

        private bool _SelectedR;

        public bool SelectedR
        {
            get
            {
                return _SelectedR;
            }

            set
            {
                if (_SelectedR == value)
                {
                    return;
                }

                _SelectedR = value;
                RaisePropertyChanged();
            }
        }

        private bool _SelectedM;

        public bool SelectedM
        {
            get
            {
                return _SelectedM;
            }

            set
            {
                if (_SelectedM == value)
                {
                    return;
                }

                _SelectedM = value;
                RaisePropertyChanged();
            }
        }

        private bool _SelectedO;

        public bool SelectedO
        {
            get
            {
                return _SelectedO;
            }

            set
            {
                if (_SelectedO == value)
                {
                    return;
                }

                _SelectedO = value;
                RaisePropertyChanged();
            }
        }

        private bool _SelectedF;

        public bool SelectedF
        {
            get
            {
                return _SelectedF;
            }

            set
            {
                if (_SelectedF == value)
                {
                    return;
                }

                _SelectedF = value;
                RaisePropertyChanged();
            }
        }

        public Hour(DateTime time, BasicDataManager basicDataManager)
        {
            //AddApointmentCommand = new RelayCommand<int>(AddApointment);
            //DeleteFunctionalApointmentCommand = new RelayCommand(async () => { await DeleteApointment(0); }, CanDeleteFunctionalApointment);
            //ChangeGymnastFunctionalCommand = new RelayCommand<object>(async (obj) => await ChangeGymnast(obj, 0));
            //ChangeGymnastReformerCommand = new RelayCommand<object>(async (obj) => await ChangeGymnast(obj, 1));
            //DeleteReformerApointmentCommand = new RelayCommand(async () => { await DeleteApointment(1); }, CanDeleteReformerApointment);
            //ToggleEnabled0Command = new RelayCommand(async () => await ToggleEnabled(0));
            //ToggleEnabled1Command = new RelayCommand(async () => await ToggleEnabled(1));
            //ToggleEnabled0ForEverCommand = new RelayCommand(async () => await ToggleEnabledForEver(0));
            //ToggleEnabled1ForEverCommand = new RelayCommand(async () => await ToggleEnabledForEver(1));
            //Enable0ForEverCommand = new RelayCommand(async () => await EnableForEver(0));
            //Enable1ForEverCommand = new RelayCommand(async () => await EnableForEver(1));

            Time = time;
            BasicDataManager = basicDataManager;
            AddApointmentCommand = new RelayCommand<int>(AddApointment);
            DeleteApointmentCommand = new RelayCommand<object>(async (par) => { await DeleteApointment(par); }, CanDeleteApointment);
            ChangeGymnastCommand = new RelayCommand<object[]>(async (obj) => await ChangeGymnast(obj));
            NoGymnastCommand = new RelayCommand<int>(async (obj) => await NoGymnast(obj));
            ToggleEnabledCommand = new RelayCommand<int>(async (par) => await ToggleEnabled((RoomEnum)par));
            ToggleEnabledForEverCommand = new RelayCommand<int>(async (par) => await ToggleEnabledForEver((RoomEnum)par));
            EnableForEverCommand = new RelayCommand<int>(async (par) => await EnableForEver((RoomEnum)par));

            ChangeTimeCommand = new RelayCommand<int>(ChangeTime);

            AppointmentsFunctional = new ObservableCollection<Apointment>();
            AppointmentsReformer = new ObservableCollection<Apointment>();
            AppointmentsMassage = new ObservableCollection<Apointment>();
            AppointemntsOutdoor = new ObservableCollection<Apointment>();
        }

        private async Task NoGymnast(int obj)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            switch (obj)
            {
                case 0:
                    if (GymnastFunctional != null)
                    {
                        BasicDataManager.Delete(GymnastFunctional);
                    }
                    break;

                case 1:
                    if (GymnastReformer != null)
                    {
                        BasicDataManager.Delete(GymnastReformer);
                    }
                    break;

                case 2:
                    if (GymnastMassage != null)
                    {
                        BasicDataManager.Delete(GymnastMassage);
                    }
                    break;

                case 3:
                    if (GymnastOutdoor != null)
                    {
                        BasicDataManager.Delete(GymnastOutdoor);
                    }
                    break;
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        #endregion Constructors

        #region Fields

        private ObservableCollection<Apointment> _ApointmentsFunctional;
        private ObservableCollection<Apointment> _AppointemntsOutdoor;
        private ObservableCollection<Apointment> _AppointmentsMassage;
        private ObservableCollection<Apointment> _AppointmentsReformer;
        private ClosedHour _ClosedHour0;
        private ClosedHour _ClosedHour1;
        private ClosedHour _ClosedHourMassage;
        private ClosedHour _ClosedHourOutdoor;
        private ICollectionView _FunctionalCV;
        private int _GymIndex;

        private string _GymnastsWorking;

        private ICollectionView _MassageCV;
        private ICollectionView _OutdoorCv;
        private ICollectionView _ReformerCV;

        #endregion Fields

        #region Properties

        public RelayCommand<int> AddApointmentCommand { get; set; }
        public RelayCommand<int> NoGymnastCommand { get; set; }

        public ObservableCollection<Apointment> AppointemntsOutdoor
        {
            get => _AppointemntsOutdoor;

            set
            {
                if (_AppointemntsOutdoor == value)
                {
                    return;
                }

                _AppointemntsOutdoor = value;
                RaisePropertyChanged();
                OutdoorCv = (CollectionView)CollectionViewSource.GetDefaultView(_AppointemntsOutdoor);
                OutdoorCv.Filter = AppointmensFilter;
                OutdoorCv.SortDescriptions.Add(new SortDescription(nameof(Apointment.Person), ListSortDirection.Ascending));
                OutdoorCv.Refresh();
            }
        }

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

        public ObservableCollection<Apointment> AppointmentsMassage
        {
            get => _AppointmentsMassage;

            set
            {
                if (_AppointmentsMassage == value)
                {
                    return;
                }

                _AppointmentsMassage = value;
                RaisePropertyChanged();
                MassageCV = (CollectionView)CollectionViewSource.GetDefaultView(_AppointmentsMassage);
                MassageCV.Filter = AppointmensFilter;
                MassageCV.SortDescriptions.Add(new SortDescription(nameof(Apointment.Person), ListSortDirection.Ascending));
                MassageCV.Refresh();
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

        public BasicDataManager BasicDataManager { get; }
        public RelayCommand<object[]> ChangeGymnastCommand { get; set; }
        public RelayCommand<int> ChangeTimeCommand { get; set; }
        public SolidColorBrush ClosedColor0 => GetClosedColor(0);
        public SolidColorBrush ClosedColor1 => GetClosedColor(1);
        public SolidColorBrush ClosedColorMassage => GetClosedColor(2);
        public SolidColorBrush ClosedColorOutdoor => GetClosedColor(3);

        public ClosedHour ClosedHour0
        {
            get => _ClosedHour0;

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

        public ClosedHour ClosedHour1
        {
            get => _ClosedHour1;

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

        public ClosedHour ClosedHourMassage
        {
            get => _ClosedHourMassage;

            set
            {
                if (_ClosedHourMassage == value)
                {
                    return;
                }

                _ClosedHourMassage = value;
                RaisePropertyChanged();
            }
        }

        public ClosedHour ClosedHourOutdoor
        {
            get => _ClosedHourOutdoor;

            set
            {
                if (_ClosedHourOutdoor == value)
                {
                    return;
                }

                _ClosedHourOutdoor = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand<int> CLoseHourCommand { get; set; }
        public CustomeTime CustomTime1 { get; set; }
        public CustomeTime CustomTime2 { get; set; }
        public CustomeTime CustomTime3 { get; set; }
        public CustomeTime CustomTime4 { get; set; }
        public RelayCommand<object> DeleteApointmentCommand { get; set; }
        public RelayCommand<int> EnableForEverCommand { get; set; }

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
                MassageCV.Refresh();
                OutdoorCv.Refresh();
            }
        }

        private GymnastHour _GymnastReformer;

        public GymnastHour GymnastReformer
        {
            get
            {
                return _GymnastReformer;
            }

            set
            {
                if (_GymnastReformer == value)
                {
                    return;
                }

                _GymnastReformer = value;
                RaisePropertyChanged();
            }
        }

        private GymnastHour _GymnastFunctional;

        public GymnastHour GymnastFunctional
        {
            get
            {
                return _GymnastFunctional;
            }

            set
            {
                if (_GymnastFunctional == value)
                {
                    return;
                }

                _GymnastFunctional = value;
                RaisePropertyChanged();
            }
        }

        private GymnastHour _GymnastMassage;

        public GymnastHour GymnastMassage
        {
            get
            {
                return _GymnastMassage;
            }

            set
            {
                if (_GymnastMassage == value)
                {
                    return;
                }

                _GymnastMassage = value;
                RaisePropertyChanged();
            }
        }

        private GymnastHour _GymnastOutdoor;

        public GymnastHour GymnastOutdoor
        {
            get
            {
                return _GymnastOutdoor;
            }

            set
            {
                if (_GymnastOutdoor == value)
                {
                    return;
                }

                _GymnastOutdoor = value;
                RaisePropertyChanged();
            }
        }

        public string GymnastsWorking
        {
            get
            {
                return _GymnastsWorking;
            }

            set
            {
                if (_GymnastsWorking == value)
                {
                    return;
                }

                _GymnastsWorking = value;
                RaisePropertyChanged();
            }
        }

        public ICollectionView MassageCV
        {
            get => _MassageCV;

            set
            {
                if (_MassageCV == value)
                {
                    return;
                }

                _MassageCV = value;
                RaisePropertyChanged();
            }
        }

        public ICollectionView OutdoorCv
        {
            get => _OutdoorCv;

            set
            {
                if (_OutdoorCv == value)
                {
                    return;
                }

                _OutdoorCv = value;
                RaisePropertyChanged();
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
        public Apointment SelectedAppointmentMassage { get; set; }
        public Apointment SelectedAppointmentOutdoor { get; set; }
        public Hour Self => this;
        public DateTime Time { get; set; }
        public string TimeString1 => CustomTime1 != null ? CustomTime1.Time : Time.ToString("HH:mm");
        public string TimeString2 => CustomTime2 != null ? CustomTime2.Time : Time.ToString("HH:mm");
        public string TimeString3 => CustomTime3 != null ? CustomTime3.Time : Time.ToString("HH:mm");
        public string TimeString4 => CustomTime4 != null ? CustomTime4.Time : Time.ToString("HH:mm");
        public RelayCommand<int> ToggleEnabledCommand { get; set; }
        public RelayCommand<int> ToggleEnabledForEverCommand { get; set; }

        public Apointments_ViewModel parent { get; set; }

        #endregion Properties

        #region Methods

        public async Task AddCustomer(Customer customer, SelectedPersonEnum selectedPerson, RoomEnum room, User SelectedGymnast, bool forever = false)
        {
            var tmpTime = Time;
            if (customer != null)
            {
                Mouse.OverrideCursor = Cursors.Wait;

                Apointment ap = new Apointment { Customer = customer, DateTime = Time, Person = selectedPerson, Room = room, Gymnast = SelectedGymnast };
                if (forever)
                {
                    List<Apointment> nextAppoitments = await BasicDataManager.Context.GetAllAppointmentsThisDayAsync(customer.Id, Time, room);

                    DateTime tmpdate = Time;
                    while (Time.Month != 8)
                    {
                        Time = Time.AddDays(7);
                        if (!nextAppoitments.Any(c => c.DateTime == Time))
                        {
                            BasicDataManager.Add(new Apointment { Customer = customer, DateTime = Time, Person = selectedPerson, Room = room, Gymnast = SelectedGymnast });
                        }
                    }
                }
                if ((room == RoomEnum.Functional && !AppointmentsFunctional.Any(a => a.Customer.Id == ap.Customer.Id)) ||
                    (room == RoomEnum.Pilates && !AppointmentsReformer.Any(api => api.Customer.Id == ap.Customer.Id)) ||
                    (room == RoomEnum.Massage && !AppointmentsMassage.Any(api => api.Customer.Id == ap.Customer.Id)) ||
                    (room == RoomEnum.Outdoor && !AppointemntsOutdoor.Any(api => api.Customer.Id == ap.Customer.Id)))
                {
                    if (room == RoomEnum.Functional)
                    {
                        AppointmentsFunctional.Add(ap);
                        BasicDataManager.Add(ap);
                    }
                    else if (room == RoomEnum.Pilates)
                    {
                        AppointmentsReformer.Add(ap);
                        BasicDataManager.Add(ap);
                    }
                    else if (room == RoomEnum.Massage)
                    {
                        AppointmentsMassage.Add(ap);
                        BasicDataManager.Add(ap);
                    }
                    else if (room == RoomEnum.Outdoor)
                    {
                        AppointemntsOutdoor.Add(ap);
                        BasicDataManager.Add(ap);
                    }
                }
                await BasicDataManager.SaveAsync();
                Mouse.OverrideCursor = Cursors.Arrow;
            }
            Time = tmpTime;
        }

        private void AddApointment(int room)
        {
            CustomersWindow_Viewmodel vm = new CustomersWindow_Viewmodel(BasicDataManager, (RoomEnum)room, this);
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

        private bool CanDeleteApointment(object room)
        {
            switch (room)
            {
                case "0":
                    return SelectedApointmentFunctional != null;

                case "1":
                    return SelectedApointmentReformer != null;

                case "2":
                    return SelectedAppointmentMassage != null;

                case "3":
                    return SelectedAppointmentOutdoor != null;
            }
            return false;
        }

        internal async Task ChangeGymnast(object[] obj)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            if (obj[0] is User u && obj[1] is string st && int.TryParse(st, out int v))
            {
                if (v == 0)
                {
                    if (GymnastFunctional == null)
                    {
                        GymnastFunctional = new GymnastHour { Datetime = Time, Gymnast = u, Room = (RoomEnum)v };
                        BasicDataManager.Add(GymnastFunctional);
                    }
                    else
                        GymnastFunctional.Gymnast = u;
                }
                else if (v == 1)
                {
                    if (GymnastReformer == null)
                    {
                        GymnastReformer = new GymnastHour { Datetime = Time, Gymnast = u, Room = (RoomEnum)v };
                        BasicDataManager.Add(GymnastReformer);
                    }
                    else
                        GymnastReformer.Gymnast = u;
                }
                else if (v == 2)
                {
                    if (GymnastMassage == null)
                    {
                        GymnastMassage = new GymnastHour { Datetime = Time, Gymnast = u, Room = (RoomEnum)v };
                        BasicDataManager.Add(GymnastMassage);
                    }
                    else
                        GymnastMassage.Gymnast = u;
                }
                else if (v == 3)
                {
                    if (GymnastOutdoor == null)
                    {
                        GymnastOutdoor = new GymnastHour { Datetime = Time, Gymnast = u, Room = (RoomEnum)v };
                        BasicDataManager.Add(GymnastOutdoor);
                    }
                    else
                        GymnastOutdoor.Gymnast = u;
                }
                if (parent != null)
                {
                    foreach (var h in parent.Days.FirstOrDefault(d => d.Date.DayOfYear == Time.DayOfYear).Hours.Where(h => h.SelectedF || h.SelectedR || h.SelectedM || h.SelectedO))
                    {
                        if (h.SelectedF)
                        {
                            if (h.GymnastFunctional == null)
                            {
                                h.GymnastFunctional = new GymnastHour { Datetime = h.Time, Gymnast = u, Room = RoomEnum.Functional };
                                BasicDataManager.Add(h.GymnastFunctional);
                            }
                            else
                                h.GymnastFunctional.Gymnast = u;
                        }
                        if (h.SelectedR)
                        {
                            if (h.GymnastReformer == null)
                            {
                                h.GymnastReformer = new GymnastHour { Datetime = h.Time, Gymnast = u, Room = RoomEnum.Pilates };
                                BasicDataManager.Add(h.GymnastReformer);
                            }
                            else
                                h.GymnastReformer.Gymnast = u;
                        }
                        if (h.SelectedM)
                        {
                            if (h.GymnastMassage == null)
                            {
                                h.GymnastMassage = new GymnastHour { Datetime = h.Time, Gymnast = u, Room = RoomEnum.Massage };
                                BasicDataManager.Add(h.GymnastMassage);
                            }
                            else
                                h.GymnastMassage.Gymnast = u;
                        }
                        if (h.SelectedO)
                        {
                            if (h.GymnastOutdoor == null)
                            {
                                h.GymnastOutdoor = new GymnastHour { Datetime = h.Time, Gymnast = u, Room = RoomEnum.Outdoor };
                                BasicDataManager.Add(h.GymnastOutdoor);
                            }
                            else
                                h.GymnastOutdoor.Gymnast = u;
                        }
                    }
                }
            }
            await BasicDataManager.SaveAsync();
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void ChangeTime(int obj)
        {
            Messenger.Default.Send(new OpenPopupUpMessage(this, (RoomEnum)obj));
        }

        private async Task DeleteApointment(object type)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            switch (type)
            {
                case "0":
                    BasicDataManager.Delete(SelectedApointmentFunctional);
                    AppointmentsFunctional.Remove(SelectedApointmentFunctional);
                    break;

                case "1":
                    BasicDataManager.Delete(SelectedApointmentReformer);
                    AppointmentsReformer.Remove(SelectedApointmentReformer);
                    break;

                case "2":
                    BasicDataManager.Delete(SelectedAppointmentMassage);
                    AppointmentsMassage.Remove(SelectedAppointmentMassage);
                    break;

                case "3":
                    BasicDataManager.Delete(SelectedAppointmentOutdoor);
                    AppointemntsOutdoor.Remove(SelectedAppointmentOutdoor);
                    break;
            }

            await BasicDataManager.SaveAsync();
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private async Task EnableForEver(RoomEnum room)
        {
            Mouse.OverrideCursor = Cursors.Wait;

            List<ClosedHour> ClosedHours = await BasicDataManager.Context.GetAllClosedHoursAsync(room, Time);

            foreach (var item in ClosedHours)
            {
                BasicDataManager.Delete(item);
            }
            await BasicDataManager.SaveAsync();
            ClosedHour0 = ClosedHour1 = ClosedHourMassage = ClosedHourOutdoor = null;
            Messenger.Default.Send(new UpdateClosedHoursMessage());
            Mouse.OverrideCursor = Cursors.Arrow;

            RaisePropertyChanged(nameof(ClosedColor1));
            RaisePropertyChanged(nameof(ClosedColor0));
            RaisePropertyChanged(nameof(ClosedColorMassage));
            RaisePropertyChanged(nameof(ClosedColorOutdoor));
        }

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
            else if (v == 2)
            {
                if (ClosedHourMassage != null)
                {
                    return new SolidColorBrush(Colors.Red);
                }
                else
                {
                    return new SolidColorBrush(Colors.LightYellow);
                }
            }
            else if (v == 3)
            {
                if (ClosedHourOutdoor != null)
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

        private async Task ToggleEnabled(RoomEnum room)
        {
            if (room == RoomEnum.Functional)
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
            else if (room == RoomEnum.Pilates)
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
            else if (room == RoomEnum.Massage)
            {
                if (ClosedHourMassage != null)
                {
                    BasicDataManager.Context.Delete(ClosedHourMassage);
                    ClosedHourMassage = null;
                }
                else
                {
                    ClosedHourMassage = new ClosedHour { Date = Time, Room = room };
                    BasicDataManager.Add(ClosedHourMassage);
                }
            }
            else if (room == RoomEnum.Outdoor)
            {
                if (ClosedHourOutdoor != null)
                {
                    BasicDataManager.Context.Delete(ClosedHourOutdoor);
                    ClosedHourOutdoor = null;
                }
                else
                {
                    ClosedHourOutdoor = new ClosedHour { Date = Time, Room = room };
                    BasicDataManager.Add(ClosedHourOutdoor);
                }
            }
            await BasicDataManager.SaveAsync();
            RaisePropertyChanged(nameof(ClosedColor1));
            RaisePropertyChanged(nameof(ClosedColor0));
            RaisePropertyChanged(nameof(ClosedColorMassage));
            RaisePropertyChanged(nameof(ClosedColorOutdoor));
        }

        private async Task ToggleEnabledForEver(RoomEnum room)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            List<ClosedHour> ClosedHours = await BasicDataManager.Context.GetAllClosedHoursAsync(room, Time);
            var limit = Time.AddMonths(3);
            var tmpTime = Time;
            while (tmpTime < limit)
            {
                if (!ClosedHours.Any(c => c.Date == tmpTime))
                {
                    BasicDataManager.Add(new ClosedHour { Date = tmpTime, Room = (RoomEnum)room });
                }
                tmpTime = tmpTime.AddDays(7);
            }

            await BasicDataManager.SaveAsync();
            Messenger.Default.Send(new UpdateClosedHoursMessage());
            Mouse.OverrideCursor = Cursors.Arrow;
            RaisePropertyChanged(nameof(ClosedColor1));
            RaisePropertyChanged(nameof(ClosedColor0));
            RaisePropertyChanged(nameof(ClosedColorMassage));
            RaisePropertyChanged(nameof(ClosedColorOutdoor));
        }

        internal void DeselectAll()
        {
            SelectedF = SelectedM = SelectedO = SelectedR = false;
        }

        #endregion Methods
    }
}