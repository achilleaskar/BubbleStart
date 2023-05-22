using BubbleStart.Database;
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
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

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
            ReformerVisible = FunctionalVisible = OutdoorVisible = MassageVisible = MassageHalfVisible = PersonalVisible = true;
            BasicDataManager = basicDataManager;
            Messenger.Default.Register<BasicDataManagerRefreshedMessage>(this, msg => Load());
            Messenger.Default.Register<UpdateProgramMessage>(this, async (msg) => await CreateProgram(false));
            Messenger.Default.Register<UpdateClosedHoursMessage>(this, msg => RefreshProgram());
            Messenger.Default.Register<OpenPopupUpMessage>(this, msg => ChangeTime(msg.Hour, msg.Room));

            EventManager.RegisterClassHandler(typeof(Window), UIElement.PreviewMouseMoveEvent,
               new MouseEventHandler(OnPreviewMouseMove));
            EventManager.RegisterClassHandler(typeof(Window), UIElement.PreviewKeyDownEvent,
                new KeyEventHandler(OnPreviewKeyDown));

            timer.Interval = new TimeSpan(0, 0, 10);
            timer.Tick += timer_Tick;
        }

        #endregion Constructors

        #region Fields

        private string _CustomTime;

        private ObservableCollection<Day> _Days;

        private bool _FunctionalVisible;

        private int _GymIndex;

        private ObservableCollection<User> _Gymnasts;

        private bool _HasChanges;

        private bool _IsGymChecked;

        private bool _MassageHalfVisible;

        private bool _MassageVisible;

        private bool _OutdoorVisible;

        private bool _PersonalVisible;

        private bool _ReformerVisible;

        private int _RoomIndex;

        private DateTime _SelectedDayToGo;

        private DateTime _StartDate;

        private bool _TimePopupOpen;

        private MainDatabase db = new MainDatabase();

        private Stopwatch stopWatch = new Stopwatch();

        private Stopwatch stopWatchLE = new Stopwatch();

        private DispatcherTimer timer = new DispatcherTimer();

        private DateTime To;

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

        public bool HasChanges
        {
            get
            {
                return _HasChanges;
            }

            set
            {
                if (_HasChanges == value)
                {
                    return;
                }

                _HasChanges = value;
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

        public DateTime LastExecuted { get; set; }

        public bool MassageHalfVisible
        {
            get
            {
                return _MassageHalfVisible;
            }

            set
            {
                if (_MassageHalfVisible == value)
                {
                    return;
                }

                _MassageHalfVisible = value;
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

        public bool PersonalVisible
        {
            get
            {
                return _PersonalVisible;
            }

            set
            {
                if (_PersonalVisible == value)
                {
                    return;
                }

                _PersonalVisible = value;
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
                    FunctionalVisible = ReformerVisible = MassageVisible = MassageHalfVisible = PersonalVisible = OutdoorVisible = true;
                }
                else if (RoomIndex == 1)
                {
                    ReformerVisible = MassageVisible = MassageHalfVisible = PersonalVisible = OutdoorVisible = false;
                    FunctionalVisible = true;
                }
                else if (RoomIndex == 2)
                {
                    FunctionalVisible = MassageVisible = MassageHalfVisible = PersonalVisible = OutdoorVisible = false;
                    ReformerVisible = true;
                }
                else if (RoomIndex == 3)
                {
                    FunctionalVisible = ReformerVisible = MassageHalfVisible = PersonalVisible = OutdoorVisible = false;
                    MassageVisible = true;
                }
                else if (RoomIndex == 4)
                {
                    FunctionalVisible = ReformerVisible = MassageVisible = PersonalVisible = OutdoorVisible = false;
                    MassageHalfVisible = true;
                }
                else if (RoomIndex == 5)
                {
                    FunctionalVisible = ReformerVisible = MassageVisible = MassageHalfVisible = OutdoorVisible = false;
                    PersonalVisible = true;
                }
                else if (RoomIndex == 6)
                {
                    FunctionalVisible = ReformerVisible = MassageVisible = MassageHalfVisible = PersonalVisible = false;
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
                To = StartDate.AddDays(6);
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
            stopWatchLE.Start();
            timer.Start();
            stopWatch.Start();
            HasChanges = false;
            LastExecuted = DateTime.Now;

            StartDate = SelectedDayToGo.AddDays(-((int)SelectedDayToGo.DayOfWeek + 6) % 7);
            DateTime tmpdate = StartDate.AddDays(6);

            if (refresh)

                await BasicDataManager.Context.Context.ShowUps.Where(a => a.Arrived >= StartDate && a.Arrived < tmpdate)
                       .Include(c => c.Customer)
                       .ToListAsync();

            List<Apointment> apointments = refresh ? await BasicDataManager.Context.Context.Apointments.Where(a => a.DateTime >= StartDate && a.DateTime < tmpdate)
                .Include(a => a.Customer)
                .ToListAsync() :
               BasicDataManager.Context.Context.Apointments.Local.Where(a => a.DateTime >= StartDate && a.DateTime < tmpdate).ToList();

            List<CustomeTime> customTimes = refresh ? await BasicDataManager.Context.Context.CustomeTimes.Where(a => a.Datetime >= StartDate && a.Datetime < tmpdate).ToListAsync() :
              BasicDataManager.Context.Context.CustomeTimes.Local.Where(a => a.Datetime >= StartDate && a.Datetime < tmpdate).ToList();

            List<GymnastHour> gymnasts = refresh ? await BasicDataManager.Context.Context.GymnastHours.Where(a => a.Datetime >= StartDate && a.Datetime < tmpdate).ToListAsync() :
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
                if (numOfDay < 6 && ch.Date.Hour >= 8 && ch.Date.Hour <= 22)
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
                    else if (ch.Room == RoomEnum.MassageHalf)
                    {
                        Days[numOfDay].Hours[ch.Date.Hour - 8].ClosedHourMassageHalf = ch;
                    }
                    else if (ch.Room == RoomEnum.Personal)
                    {
                        Days[numOfDay].Hours[ch.Date.Hour - 8].ClosedHourPersonal = ch;
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

            foreach (var ap in apointments.OrderBy(a => a.Waiting))
            {
                numOfDay = ((int)ap.DateTime.DayOfWeek + 6) % 7;
                if (numOfDay < 6 && ap.DateTime.Hour >= 8 && ap.DateTime.Hour <= 22)
                {
                    if (ap.Room == RoomEnum.Functional)
                        Days[numOfDay].Hours[ap.DateTime.Hour - 8].AppointmentsFunctional.Add(ap);
                    else if (ap.Room == RoomEnum.Pilates)
                        Days[numOfDay].Hours[ap.DateTime.Hour - 8].AppointmentsReformer.Add(ap);
                    else if (ap.Room == RoomEnum.Massage)
                        Days[numOfDay].Hours[ap.DateTime.Hour - 8].AppointmentsMassage.Add(ap);
                    else if (ap.Room == RoomEnum.Outdoor)
                        Days[numOfDay].Hours[ap.DateTime.Hour - 8].AppointemntsOutdoor.Add(ap);
                    else if (ap.Room == RoomEnum.MassageHalf)
                        Days[numOfDay].Hours[ap.DateTime.Hour - 8].AppointmentsMassageHalf.Add(ap);
                    else if (ap.Room == RoomEnum.Personal)
                        Days[numOfDay].Hours[ap.DateTime.Hour - 8].AppointmentsPersonal.Add(ap);
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

                        case RoomEnum.MassageHalf:
                            t.CustomTime5 = ct;
                            break;

                        case RoomEnum.Personal:
                            t.CustomTime6 = ct;
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

                        case RoomEnum.MassageHalf:
                            t.GymnastMassageHalf = ct;
                            break;

                        case RoomEnum.Personal:
                            t.GymnastPersonal = ct;
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
            LastExecuted = DateTime.Now;
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        public override void Load(int id = 0, MyViewModelBaseAsync previousViewModel = null)
        {
            StartDate = DateTime.Today.AddDays(-((int)DateTime.Today.DayOfWeek + 6) % 7);
            Gymnasts = new ObservableCollection<User>(BasicDataManager.Users.Where(u => u.Id == 4 || u.Level == 4));
            SelectedDayToGo = DateTime.Today;
            Days = new ObservableCollection<Day>();
            stopWatch.Stop();
            stopWatchLE.Stop();
            timer.Stop();
        }

        public void OpenCustomerManagement(Customer c)
        {
            c.EditedInCustomerManagement = true;
            c.BasicDataManager = BasicDataManager;
            c.UpdateCollections();
            c.FillDefaultProframs();
            Window window = new CustomerManagement
            {
                DataContext = c
            };
            Messenger.Default.Send(new OpenChildWindowCommand(window));
        }

        public void RefreshProgram()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                StartDate = SelectedDayToGo.AddDays(-((int)SelectedDayToGo.DayOfWeek + 6) % 7);
                DateTime tmpdate = StartDate.AddDays(6);

                var closedHours = BasicDataManager.Context.Context.ClosedHours.Local?.Where(a => a.Date >= StartDate && a.Date < tmpdate && a.Date >= BasicDataManager.Context.Limit).ToList() ?? new List<ClosedHour>();

                DateTime tmpDate = StartDate;

                int numOfDay;

                foreach (var ch in closedHours)
                {
                    numOfDay = ((int)ch.Date.DayOfWeek + 6) % 7;
                    if (numOfDay < 6 && ch.Date.Hour >= 8 && ch.Date.Hour <= 22)
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
                        else if (ch.Room == RoomEnum.MassageHalf)
                        {
                            Days[numOfDay].Hours[ch.Date.Hour - 8].ClosedHourMassageHalf = ch;
                        }
                        else if (ch.Room == RoomEnum.Personal)
                        {
                            Days[numOfDay].Hours[ch.Date.Hour - 8].ClosedHourPersonal = ch;
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
            }
            catch (Exception ex)
            {
                MessengerInstance.Send(new ShowExceptionMessage_Message("Σφάλμα κατα την φόρτωση του προγράμματος" + ex.Message));
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

        private void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            stopWatch.Restart();
        }

        private void OnPreviewMouseMove(object sender, MouseEventArgs e)
        {
            stopWatch.Restart();
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
            else if (selectedRoom == RoomEnum.MassageHalf)
            {
                if (selectedHour.CustomTime5 != null)
                {
                    selectedHour.CustomTime5.Time = CustomTime;
                }
                else
                {
                    selectedHour.CustomTime5 = new CustomeTime { Datetime = selectedHour.Time, Room = selectedRoom, Time = CustomTime };

                    BasicDataManager.Add(selectedHour.CustomTime5);
                }
                selectedHour.RaisePropertyChanged(nameof(Hour.TimeString5));
            }
            else if (selectedRoom == RoomEnum.Personal)
            {
                if (selectedHour.CustomTime6 != null)
                {
                    selectedHour.CustomTime6.Time = CustomTime;
                }
                else
                {
                    selectedHour.CustomTime6 = new CustomeTime { Datetime = selectedHour.Time, Room = selectedRoom, Time = CustomTime };

                    BasicDataManager.Add(selectedHour.CustomTime6);
                }
                selectedHour.RaisePropertyChanged(nameof(Hour.TimeString6));
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

            BasicDataManager.Add(new ProgramChange
            {
                Date = DateTime.Now,
                InstanceGuid = StaticResources.Guid,
                From = selectedHour.Time,
                To = selectedHour.Time.AddHours(1)
            });

            await BasicDataManager.SaveAsync();
            Mouse.OverrideCursor = Cursors.Arrow;
            TimePopupOpen = false;
        }

        private async void timer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (!HasChanges && stopWatch.Elapsed.TotalSeconds >= 3 && stopWatchLE.Elapsed.TotalSeconds >= 60)
                {
                    Mouse.OverrideCursor = Cursors.Wait;

                    HasChanges = await db.ProgramChanges.AnyAsync(c => c.InstanceGuid != StaticResources.Guid && c.Date > LastExecuted && c.To >= StartDate && c.From <= To);

                    Mouse.OverrideCursor = Cursors.Arrow;
                    if (HasChanges)
                    {
                    }
                    //if (hasChange)
                    //{
                    //    HasChanges = true;
                    //    //stopWatchLE.Stop();
                    //    //timer.Stop();
                    //    //stopWatch.Stop();
                    //    //if (MessageBox.Show("Έχουν γίνει αλλαγές απο άλλον χρήστη για την εβδομάδα που βλέπετε. Παρακαλώ πατήστε ξανά φόρτωση.", "Προσοχη") != MessageBoxResult.Yes)
                    //    //{
                    //    //    stopWatchLE.Start();
                    //    //    timer.Start();
                    //    //    stopWatch.Start();
                    //    //}
                    //}
                }
            }
            catch (Exception)
            {
                db.Dispose();
                db = new MainDatabase();
            }
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
                new Hour(new DateTime(Date.Year,Date.Month,Date.Day,21,0,0),basicDataManager),
                new Hour(new DateTime(Date.Year,Date.Month,Date.Day,22,0,0),basicDataManager)
            };
        }

        #endregion Constructors

        #region Fields

        private DateTime _date;
        private double _HeightFunctional;

        private double _HeightMassage;

        private double _HeightMassageHalf;

        private double _HeightOutdoor;

        private double _HeightPersonal;

        private double _HeightPilates;

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

        public double HeightFunctional
        {
            get
            {
                return _HeightFunctional;
            }

            set
            {
                if (_HeightFunctional == value)
                {
                    return;
                }

                _HeightFunctional = value;
                RaisePropertyChanged();
            }
        }

        public double HeightMassage
        {
            get
            {
                return _HeightMassage;
            }

            set
            {
                if (_HeightMassage == value)
                {
                    return;
                }

                _HeightMassage = value;
                RaisePropertyChanged();
            }
        }

        public double HeightMassageHalf
        {
            get
            {
                return _HeightMassageHalf;
            }

            set
            {
                if (_HeightMassageHalf == value)
                {
                    return;
                }

                _HeightMassageHalf = value;
                RaisePropertyChanged();
            }
        }

        public double HeightOutdoor
        {
            get
            {
                return _HeightOutdoor;
            }

            set
            {
                if (_HeightOutdoor == value)
                {
                    return;
                }

                _HeightOutdoor = value;
                RaisePropertyChanged();
            }
        }

        public double HeightPersonal
        {
            get
            {
                return _HeightPersonal;
            }

            set
            {
                if (_HeightPersonal == value)
                {
                    return;
                }

                _HeightPersonal = value;
                RaisePropertyChanged();
            }
        }

        public double HeightPilates
        {
            get
            {
                return _HeightPilates;
            }

            set
            {
                if (_HeightPilates == value)
                {
                    return;
                }

                _HeightPilates = value;
                RaisePropertyChanged();
            }
        }

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
            ToggleWaitingCommand = new RelayCommand<string>(async (obj) => await ToggleWaiting(obj));
            ToggleCanceledCommand = new RelayCommand<string>(async (obj) => await ToggleCanceled(obj));
            ToggleEnabledCommand = new RelayCommand<int>(async (par) => await ToggleEnabled((RoomEnum)par));
            ToggleEnabledForEverCommand = new RelayCommand<int>(async (par) => await ToggleEnabledForEver((RoomEnum)par));
            EnableForEverCommand = new RelayCommand<int>(async (par) => await EnableForEver((RoomEnum)par));

            ChangeTimeCommand = new RelayCommand<int>(ChangeTime);

            AppointmentsFunctional = new ObservableCollection<Apointment>();
            AppointmentsReformer = new ObservableCollection<Apointment>();
            AppointmentsMassage = new ObservableCollection<Apointment>();
            AppointmentsMassageHalf = new ObservableCollection<Apointment>();
            AppointmentsPersonal = new ObservableCollection<Apointment>();
            AppointemntsOutdoor = new ObservableCollection<Apointment>();
        }

        #endregion Constructors

        #region Fields

        private ObservableCollection<Apointment> _ApointmentsFunctional;
        private ObservableCollection<Apointment> _AppointemntsOutdoor;
        private ObservableCollection<Apointment> _AppointmentsMassage;
        private ObservableCollection<Apointment> _AppointmentsMassageHalf;
        private ObservableCollection<Apointment> _AppointmentsPresonal;
        private ObservableCollection<Apointment> _AppointmentsReformer;
        private ClosedHour _ClosedHour0;
        private ClosedHour _ClosedHour1;
        private ClosedHour _ClosedHourMassage;
        private ClosedHour _ClosedHourMassageHalf;
        private ClosedHour _ClosedHourOutdoor;
        private ClosedHour _ClosedHourPersonal;
        private ICollectionView _FunctionalCV;
        private int _GymIndex;
        private GymnastHour _GymnastFunctional;
        private GymnastHour _GymnastMassage;
        private GymnastHour _GymnastMassageHalf;
        private GymnastHour _GymnastOutdoor;
        private GymnastHour _GymnastPersonal;
        private GymnastHour _GymnastReformer;
        private string _GymnastsWorking;
        private ICollectionView _MassageCV;
        private ICollectionView _MassageHalfCV;
        private ICollectionView _OutdoorCv;
        private ICollectionView _PersonalCV;
        private ICollectionView _ReformerCV;
        private bool _SelectedF;
        private bool _SelectedM;
        private bool _SelectedMH;
        private bool _SelectedO;
        private bool _SelectedP;
        private bool _SelectedR;

        #endregion Fields

        #region Properties

        public RelayCommand<int> AddApointmentCommand { get; set; }

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

        public ObservableCollection<Apointment> AppointmentsMassageHalf
        {
            get => _AppointmentsMassageHalf;

            set
            {
                if (_AppointmentsMassageHalf == value)
                {
                    return;
                }

                _AppointmentsMassageHalf = value;
                RaisePropertyChanged();
                MassageHalfCV = (CollectionView)CollectionViewSource.GetDefaultView(_AppointmentsMassageHalf);
                MassageHalfCV.Filter = AppointmensFilter;
                MassageHalfCV.SortDescriptions.Add(new SortDescription(nameof(Apointment.Person), ListSortDirection.Ascending));
                MassageHalfCV.Refresh();
            }
        }

        public ObservableCollection<Apointment> AppointmentsPersonal
        {
            get => _AppointmentsPresonal;

            set
            {
                if (_AppointmentsPresonal == value)
                {
                    return;
                }

                _AppointmentsPresonal = value;
                RaisePropertyChanged();
                PersonalCV = (CollectionView)CollectionViewSource.GetDefaultView(_AppointmentsPresonal);
                PersonalCV.Filter = AppointmensFilter;
                PersonalCV.SortDescriptions.Add(new SortDescription(nameof(Apointment.Person), ListSortDirection.Ascending));
                PersonalCV.Refresh();
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

        public SolidColorBrush ClosedColorMassageHalf => GetClosedColor(4);

        public SolidColorBrush ClosedColorOutdoor => GetClosedColor(3);

        public SolidColorBrush ClosedColorPersonal => GetClosedColor(5);

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

        public ClosedHour ClosedHourMassageHalf
        {
            get
            {
                return _ClosedHourMassageHalf;
            }

            set
            {
                if (_ClosedHourMassageHalf == value)
                {
                    return;
                }

                _ClosedHourMassageHalf = value;
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

        public ClosedHour ClosedHourPersonal
        {
            get
            {
                return _ClosedHourPersonal;
            }

            set
            {
                if (_ClosedHourPersonal == value)
                {
                    return;
                }

                _ClosedHourPersonal = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand<int> CLoseHourCommand { get; set; }

        public CustomeTime CustomTime1 { get; set; }

        public CustomeTime CustomTime2 { get; set; }

        public CustomeTime CustomTime3 { get; set; }

        public CustomeTime CustomTime4 { get; set; }

        public CustomeTime CustomTime5 { get; set; }

        public CustomeTime CustomTime6 { get; set; }

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
                MassageHalfCV.Refresh();
                PersonalCV.Refresh();
            }
        }

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

        public GymnastHour GymnastMassageHalf
        {
            get
            {
                return _GymnastMassageHalf;
            }

            set
            {
                if (_GymnastMassageHalf == value)
                {
                    return;
                }

                _GymnastMassageHalf = value;
                RaisePropertyChanged();
            }
        }

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

        public GymnastHour GymnastPersonal
        {
            get
            {
                return _GymnastPersonal;
            }

            set
            {
                if (_GymnastPersonal == value)
                {
                    return;
                }

                _GymnastPersonal = value;
                RaisePropertyChanged();
            }
        }

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

        public ICollectionView MassageHalfCV
        {
            get => _MassageHalfCV;

            set
            {
                if (_MassageHalfCV == value)
                {
                    return;
                }

                _MassageHalfCV = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand<int> NoGymnastCommand { get; set; }

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

        public Apointments_ViewModel parent { get; set; }

        public ICollectionView PersonalCV
        {
            get => _PersonalCV;

            set
            {
                if (_PersonalCV == value)
                {
                    return;
                }

                _PersonalCV = value;
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

        public Apointment SelectedAppointmentMassageHalf { get; set; }

        public Apointment SelectedAppointmentOutdoor { get; set; }

        public Apointment SelectedAppointmentPersonal { get; set; }

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

        public bool SelectedMH
        {
            get
            {
                return _SelectedMH;
            }

            set
            {
                if (_SelectedMH == value)
                {
                    return;
                }

                _SelectedMH = value;
                RaisePropertyChanged();
            }
        }

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

        public bool SelectedP
        {
            get
            {
                return _SelectedP;
            }

            set
            {
                if (_SelectedP == value)
                {
                    return;
                }

                _SelectedP = value;
                RaisePropertyChanged();
            }
        }

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

        public Hour Self => this;

        public DateTime Time { get; set; }

        public string TimeString1 => CustomTime1 != null ? CustomTime1.Time : Time.ToString("HH:mm");

        public string TimeString2 => CustomTime2 != null ? CustomTime2.Time : Time.ToString("HH:mm");

        public string TimeString3 => CustomTime3 != null ? CustomTime3.Time : Time.ToString("HH:mm");

        public string TimeString4 => CustomTime4 != null ? CustomTime4.Time : Time.ToString("HH:mm");

        public string TimeString5 => CustomTime5 != null ? CustomTime5.Time : Time.AddMinutes(30).ToString("HH:mm");

        public string TimeString6 => CustomTime6 != null ? CustomTime6.Time : Time.ToString("HH:mm");

        public RelayCommand<string> ToggleCanceledCommand { get; set; }

        public RelayCommand<int> ToggleEnabledCommand { get; set; }

        public RelayCommand<int> ToggleEnabledForEverCommand { get; set; }

        public RelayCommand<string> ToggleWaitingCommand { get; set; }

        #endregion Properties

        #region Methods

        public async Task AddCustomer(Customer customer, SelectedPersonEnum selectedPerson, RoomEnum room, User SelectedGymnast, bool forever = false, bool waiting = false)
        {
            var tmpTime = Time;
            if (customer != null)
            {
                Mouse.OverrideCursor = Cursors.Wait;

                Apointment ap = new Apointment { Customer = customer, DateTime = Time, Person = selectedPerson, Room = room, Gymnast = SelectedGymnast, Waiting = waiting };
                if (forever)
                {
                    BasicDataManager.Add(new ProgramChange
                    {
                        Date = DateTime.Now,
                        InstanceGuid = StaticResources.Guid,
                        From = Time,
                        To = Time.AddYears(1)
                    });
                    List<Apointment> nextAppoitments = await BasicDataManager.Context.GetAllAppointmentsThisDayAsync(customer.Id, Time, room);

                    DateTime tmpdate = Time;
                    while (Time < DateTime.Today.AddYears(1))
                    {
                        Time = Time.AddDays(7);
                        if (!nextAppoitments.Any(c => c.DateTime == Time))
                        {
                            BasicDataManager.Add(new Apointment { Customer = customer, DateTime = Time, Person = selectedPerson, Room = room, Gymnast = SelectedGymnast });
                        }
                    }
                }
                else
                    BasicDataManager.Add(new ProgramChange
                    {
                        Date = DateTime.Now,
                        InstanceGuid = StaticResources.Guid,
                        From = Time,
                        To = Time.AddHours(1)
                    });
                if ((room == RoomEnum.Functional && !AppointmentsFunctional.Any(a => a.Customer.Id == ap.Customer.Id)) ||
                    (room == RoomEnum.Pilates && !AppointmentsReformer.Any(api => api.Customer.Id == ap.Customer.Id)) ||
                    (room == RoomEnum.Massage && !AppointmentsMassage.Any(api => api.Customer.Id == ap.Customer.Id)) ||
                    (room == RoomEnum.MassageHalf && !AppointmentsMassageHalf.Any(api => api.Customer.Id == ap.Customer.Id)) ||
                    (room == RoomEnum.Personal && !AppointmentsPersonal.Any(api => api.Customer.Id == ap.Customer.Id)) ||
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
                    else if (room == RoomEnum.MassageHalf)
                    {
                        AppointmentsMassageHalf.Add(ap);
                        BasicDataManager.Add(ap);
                    }
                    else if (room == RoomEnum.Personal)
                    {
                        AppointmentsPersonal.Add(ap);
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

        internal async Task ChangeGymnast(object[] obj)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            if (obj[0] is User u && obj[1] is string st && int.TryParse(st, out int v))
            {
                if (v == 0)
                {
                    if (GymnastFunctional == null)
                    {
                        GymnastFunctional = new GymnastHour { Datetime = Time, Gymnast = u, Gymnast_Id = u.Id, Room = (RoomEnum)v };
                        BasicDataManager.Add(GymnastFunctional);
                    }
                    else
                    {
                        GymnastFunctional.Gymnast = u;
                        GymnastFunctional.Gymnast_Id = u.Id;
                    }
                }
                else if (v == 1)
                {
                    if (GymnastReformer == null)
                    {
                        GymnastReformer = new GymnastHour { Datetime = Time, Gymnast = u, Gymnast_Id = u.Id, Room = (RoomEnum)v };
                        BasicDataManager.Add(GymnastReformer);
                    }
                    else
                    {
                        GymnastReformer.Gymnast = u;
                        GymnastReformer.Gymnast_Id = u.Id;
                    }
                }
                else if (v == 2)
                {
                    if (GymnastMassage == null)
                    {
                        GymnastMassage = new GymnastHour { Datetime = Time, Gymnast = u, Gymnast_Id = u.Id, Room = (RoomEnum)v };
                        BasicDataManager.Add(GymnastMassage);
                    }
                    else
                    {
                        GymnastMassage.Gymnast = u;
                        GymnastMassage.Gymnast_Id = u.Id;
                    }
                }
                else if (v == 3)
                {
                    if (GymnastOutdoor == null)
                    {
                        GymnastOutdoor = new GymnastHour { Datetime = Time, Gymnast = u, Gymnast_Id = u.Id, Room = (RoomEnum)v };
                        BasicDataManager.Add(GymnastOutdoor);
                    }
                    else
                    {
                        GymnastOutdoor.Gymnast = u;
                        GymnastOutdoor.Gymnast_Id = u.Id;
                    }
                }
                else if (v == 4)
                {
                    if (GymnastMassageHalf == null)
                    {
                        GymnastMassageHalf = new GymnastHour { Datetime = Time, Gymnast = u, Gymnast_Id = u.Id, Room = (RoomEnum)v };
                        BasicDataManager.Add(GymnastMassageHalf);
                    }
                    else
                    {
                        GymnastMassageHalf.Gymnast = u;
                        GymnastMassageHalf.Gymnast_Id = u.Id;
                    }
                }
                else if (v == 5)
                {
                    if (GymnastPersonal == null)
                    {
                        GymnastPersonal = new GymnastHour { Datetime = Time, Gymnast = u, Gymnast_Id = u.Id, Room = (RoomEnum)v };
                        BasicDataManager.Add(GymnastPersonal);
                    }
                    else
                    {
                        GymnastPersonal.Gymnast = u;
                        GymnastPersonal.Gymnast_Id = u.Id;
                    }
                }
                if (parent != null)
                {
                    foreach (var h in parent.Days.FirstOrDefault(d => d.Date.DayOfYear == Time.DayOfYear).Hours.Where(h => h.SelectedF || h.SelectedR || h.SelectedM || h.SelectedMH || h.SelectedP || h.SelectedO))
                    {
                        if (h.SelectedF)
                        {
                            if (h.GymnastFunctional == null)
                            {
                                h.GymnastFunctional = new GymnastHour { Datetime = h.Time, Gymnast = u, Gymnast_Id = u.Id, Room = RoomEnum.Functional };
                                BasicDataManager.Add(h.GymnastFunctional);
                            }
                            {
                                h.GymnastFunctional.Gymnast = u;
                                h.GymnastFunctional.Gymnast_Id = u.Id;
                            }
                        }
                        if (h.SelectedR)
                        {
                            if (h.GymnastReformer == null)
                            {
                                h.GymnastReformer = new GymnastHour { Datetime = h.Time, Gymnast = u, Gymnast_Id = u.Id, Room = RoomEnum.Pilates };
                                BasicDataManager.Add(h.GymnastReformer);
                            }
                            {
                                h.GymnastReformer.Gymnast = u;
                                h.GymnastReformer.Gymnast_Id = u.Id;
                            }
                        }
                        if (h.SelectedM)
                        {
                            if (h.GymnastMassage == null)
                            {
                                h.GymnastMassage = new GymnastHour { Datetime = h.Time, Gymnast = u, Gymnast_Id = u.Id, Room = RoomEnum.Massage };
                                BasicDataManager.Add(h.GymnastMassage);
                            }
                            {
                                h.GymnastMassage.Gymnast = u;
                                h.GymnastMassage.Gymnast_Id = u.Id;
                            }
                        }
                        if (h.SelectedMH)
                        {
                            if (h.GymnastMassageHalf == null)
                            {
                                h.GymnastMassageHalf = new GymnastHour { Datetime = h.Time, Gymnast = u, Gymnast_Id = u.Id, Room = RoomEnum.MassageHalf };
                                BasicDataManager.Add(h.GymnastMassageHalf);
                            }
                            {
                                h.GymnastMassageHalf.Gymnast = u;
                                h.GymnastMassageHalf.Gymnast_Id = u.Id;
                            }
                        }
                        if (h.SelectedP)
                        {
                            if (h.GymnastPersonal == null)
                            {
                                h.GymnastPersonal = new GymnastHour { Datetime = h.Time, Gymnast = u, Gymnast_Id = u.Id, Room = RoomEnum.Personal };
                                BasicDataManager.Add(h.GymnastPersonal);
                            }
                            {
                                h.GymnastPersonal.Gymnast = u;
                                h.GymnastPersonal.Gymnast_Id = u.Id;
                            }
                        }
                        if (h.SelectedO)
                        {
                            if (h.GymnastOutdoor == null)
                            {
                                h.GymnastOutdoor = new GymnastHour { Datetime = h.Time, Gymnast = u, Gymnast_Id = u.Id, Room = RoomEnum.Outdoor };
                                BasicDataManager.Add(h.GymnastOutdoor);
                            }
                            {
                                h.GymnastOutdoor.Gymnast = u;
                                h.GymnastOutdoor.Gymnast_Id = u.Id;
                            }
                        }
                    }
                }
            }

            BasicDataManager.Add(new ProgramChange
            {
                Date = DateTime.Now,
                InstanceGuid = StaticResources.Guid,
                From = Time,
                To = Time.AddHours(1)
            });
            await BasicDataManager.SaveAsync();
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        internal void DeselectAll()
        {
            SelectedF = SelectedM = SelectedMH = SelectedP = SelectedO = SelectedR = false;
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
            return obj is Apointment a && (GymIndex == 0 || (GymIndex == (int)a.Person + 1));
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

                case "4":
                    return SelectedAppointmentMassageHalf != null;

                case "5":
                    return SelectedAppointmentPersonal != null;
            }
            return false;
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
                    if (MessageBox.Show($"Θέλετε σίγουρα να ΔΙΑΓΡΑΨΕΤΕ το επιλεγμένο ραντεβού? {SelectedApointmentFunctional.Description}", "Προσοχή", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        BasicDataManager.Add(new ProgramChange
                        {
                            Date = DateTime.Now,
                            InstanceGuid = StaticResources.Guid,
                            From = SelectedApointmentFunctional.DateTime,
                            To = SelectedApointmentFunctional.DateTime.AddHours(1)
                        });
                        BasicDataManager.Delete(SelectedApointmentFunctional);
                        AppointmentsFunctional.Remove(SelectedApointmentFunctional);
                    }
                    break;

                case "1":
                    if (MessageBox.Show($"Θέλετε σίγουρα να ΔΙΑΓΡΑΨΕΤΕ το επιλεγμένο ραντεβού? {SelectedApointmentReformer.Description}", "Προσοχή", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        BasicDataManager.Add(new ProgramChange
                        {
                            Date = DateTime.Now,
                            InstanceGuid = StaticResources.Guid,
                            From = SelectedApointmentReformer.DateTime,
                            To = SelectedApointmentReformer.DateTime.AddHours(1)
                        });
                        BasicDataManager.Delete(SelectedApointmentReformer);
                        AppointmentsReformer.Remove(SelectedApointmentReformer);
                    }
                    break;

                case "2":
                    if (MessageBox.Show($"Θέλετε σίγουρα να ΔΙΑΓΡΑΨΕΤΕ το επιλεγμένο ραντεβού? {SelectedAppointmentMassage.Description}", "Προσοχή", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        BasicDataManager.Add(new ProgramChange
                        {
                            Date = DateTime.Now,
                            InstanceGuid = StaticResources.Guid,
                            From = SelectedAppointmentMassage.DateTime,
                            To = SelectedAppointmentMassage.DateTime.AddHours(1)
                        });
                        BasicDataManager.Delete(SelectedAppointmentMassage);
                        AppointmentsMassage.Remove(SelectedAppointmentMassage);
                    }
                    break;

                case "3":
                    if (MessageBox.Show($"Θέλετε σίγουρα να ΔΙΑΓΡΑΨΕΤΕ το επιλεγμένο ραντεβού? {SelectedAppointmentOutdoor.Description}", "Προσοχή", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        BasicDataManager.Add(new ProgramChange
                        {
                            Date = DateTime.Now,
                            InstanceGuid = StaticResources.Guid,
                            From = SelectedAppointmentOutdoor.DateTime,
                            To = SelectedAppointmentOutdoor.DateTime.AddHours(1)
                        });
                        BasicDataManager.Delete(SelectedAppointmentOutdoor);
                        AppointemntsOutdoor.Remove(SelectedAppointmentOutdoor);
                    }
                    break;

                case "4":
                    if (MessageBox.Show($"Θέλετε σίγουρα να ΔΙΑΓΡΑΨΕΤΕ το επιλεγμένο ραντεβού? {SelectedAppointmentMassageHalf.Description}", "Προσοχή", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        BasicDataManager.Add(new ProgramChange
                        {
                            Date = DateTime.Now,
                            InstanceGuid = StaticResources.Guid,
                            From = SelectedAppointmentMassageHalf.DateTime,
                            To = SelectedAppointmentMassageHalf.DateTime.AddHours(1)
                        });
                        BasicDataManager.Delete(SelectedAppointmentMassageHalf);
                        AppointmentsMassageHalf.Remove(SelectedAppointmentMassageHalf);
                    }
                    break;

                case "5":
                    if (MessageBox.Show($"Θέλετε σίγουρα να ΔΙΑΓΡΑΨΕΤΕ το επιλεγμένο ραντεβού? {SelectedAppointmentPersonal.Description}", "Προσοχή", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        BasicDataManager.Add(new ProgramChange
                        {
                            Date = DateTime.Now,
                            InstanceGuid = StaticResources.Guid,
                            From = SelectedAppointmentPersonal.DateTime,
                            To = SelectedAppointmentPersonal.DateTime.AddHours(1)
                        });
                        BasicDataManager.Delete(SelectedAppointmentPersonal);
                        AppointmentsPersonal.Remove(SelectedAppointmentPersonal);
                    }
                    break;
            }

            await BasicDataManager.SaveAsync();

            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private async Task EnableForEver(RoomEnum room)
        {
            if (Time == null)
            {
                MessageBox.Show("Σφάλμα πατήστε φόρτωση και δοκιμάστε ξανά");
                return;
            }
            Mouse.OverrideCursor = Cursors.Wait;

            try
            {
                List<ClosedHour> ClosedHours = await BasicDataManager.Context.GetAllClosedHoursAsync(room, Time);

                foreach (var item in ClosedHours)
                {
                    BasicDataManager.Delete(item);
                }
                if (ClosedHours.Any())
                {
                    BasicDataManager.Add(new ProgramChange
                    {
                        Date = DateTime.Now,
                        InstanceGuid = StaticResources.Guid,
                        From = ClosedHours.Min(d => d.Date),
                        To = ClosedHours.Max(d => d.Date)
                    });
                }

                await BasicDataManager.SaveAsync();
                ClosedHour0 = ClosedHour1 = ClosedHourMassage = ClosedHourMassageHalf = ClosedHourPersonal = ClosedHourOutdoor = null;
                Messenger.Default.Send(new UpdateClosedHoursMessage());
            }
            catch (Exception ex)
            {
                BasicDataManager.CurrentMessenger.Send(new ShowExceptionMessage_Message("Σφάλμα κατα το πρώτο στάδιο της ενεργοποίησης" + ex.Message));
            }
            Mouse.OverrideCursor = Cursors.Arrow;

            RaisePropertyChanged(nameof(ClosedColor1));
            RaisePropertyChanged(nameof(ClosedColor0));
            RaisePropertyChanged(nameof(ClosedColorMassage));
            RaisePropertyChanged(nameof(ClosedColorMassageHalf));
            RaisePropertyChanged(nameof(ClosedColorPersonal));
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
            else if (v == 4)
            {
                if (ClosedHourMassageHalf != null)
                {
                    return new SolidColorBrush(Colors.Red);
                }
                else
                {
                    return new SolidColorBrush(Colors.LightYellow);
                }
            }
            else if (v == 5)
            {
                if (ClosedHourPersonal != null)
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

        private async Task NoGymnast(int obj)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            switch (obj)
            {
                case 0:
                    if (GymnastFunctional != null)
                    {
                        BasicDataManager.Delete(GymnastFunctional);
                        GymnastFunctional = null;
                    }
                    break;

                case 1:
                    if (GymnastReformer != null)
                    {
                        BasicDataManager.Delete(GymnastReformer);
                        GymnastReformer = null;
                    }
                    break;

                case 2:
                    if (GymnastMassage != null)
                    {
                        BasicDataManager.Delete(GymnastMassage);
                        GymnastMassage = null;
                    }
                    break;

                case 3:
                    if (GymnastOutdoor != null)
                    {
                        BasicDataManager.Delete(GymnastOutdoor);
                        GymnastOutdoor = null;
                    }
                    break;
                case 4:
                    if (GymnastMassageHalf != null)
                    {
                        BasicDataManager.Delete(GymnastMassageHalf);
                        GymnastMassageHalf = null;
                    }
                    break;

                case 5:
                    if (GymnastPersonal != null)
                    {
                        BasicDataManager.Delete(GymnastPersonal);
                        GymnastPersonal = null;
                    }
                    break;

            }
            if (parent != null)
            {
                var t = parent.Days.FirstOrDefault(d => d.Date.DayOfYear == Time.DayOfYear).Hours.Where(h => (h.SelectedF || h.SelectedR || h.SelectedM || h.SelectedMH || h.SelectedP || h.SelectedO));
                foreach (var h in t)
                {
                    if (h.SelectedF && !(h == this && obj == 0))
                    {
                        if (h.GymnastFunctional != null)
                        {
                            if (h.GymnastFunctional.Id > 0)
                                BasicDataManager.Delete(h.GymnastFunctional);
                            h.GymnastFunctional = null;
                        }
                    }
                    if (h.SelectedR && !(h == this && obj == 1))
                    {
                        if (h.GymnastReformer != null)
                        {
                            if (h.GymnastReformer.Id > 0)
                                BasicDataManager.Delete(h.GymnastReformer);
                            h.GymnastReformer = null;
                        }
                    }
                    if (h.SelectedM && !(h == this && obj == 2))
                    {
                        if (h.GymnastMassage != null)
                        {
                            if (h.GymnastMassage.Id > 0)
                                BasicDataManager.Delete(h.GymnastMassage);
                            h.GymnastMassage = null;
                        }
                    }
                    if (h.SelectedMH && !(h == this && obj == 4))
                    {
                        if (h.GymnastMassageHalf != null)
                        {
                            if (h.GymnastMassageHalf.Id > 0)
                                BasicDataManager.Delete(h.GymnastMassageHalf);
                            h.GymnastMassageHalf = null;
                        }
                    }
                    if (h.SelectedP && !(h == this && obj == 5))
                    {
                        if (h.GymnastPersonal!= null)
                        {
                            if (h.GymnastPersonal.Id > 0)
                                BasicDataManager.Delete(h.GymnastPersonal);
                            h.GymnastPersonal = null;
                        }
                    }
                    if (h.SelectedO && !(h == this && obj == 3))
                    {
                        if (h.GymnastOutdoor != null)
                        {
                            if (h.GymnastOutdoor.Id > 0)
                                BasicDataManager.Delete(h.GymnastOutdoor);
                            h.GymnastOutdoor = null;
                        }
                    }
                }
            }

            BasicDataManager.Add(new ProgramChange
            {
                Date = DateTime.Now,
                InstanceGuid = StaticResources.Guid,
                From = Time,
                To = Time.AddHours(1)
            });
            await BasicDataManager.SaveAsync();
            Mouse.OverrideCursor = Cursors.Arrow;
        }


        private async Task ToggleCanceled(string obj)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            switch (obj)
            {
                case "0":
                    SelectedApointmentFunctional.Canceled = !SelectedApointmentFunctional.Canceled;
                    SelectedApointmentFunctional.RaisePropertyChanged(nameof(Apointment.ApColor));
                    TryCancelForUser(SelectedApointmentFunctional, SelectedApointmentFunctional.Canceled);
                    break;

                case "1":
                    SelectedApointmentReformer.Canceled = !SelectedApointmentReformer.Canceled;
                    SelectedApointmentReformer.RaisePropertyChanged(nameof(Apointment.ApColor));
                    TryCancelForUser(SelectedApointmentReformer, SelectedApointmentReformer.Canceled);

                    break;

                case "2":
                    SelectedAppointmentMassage.Canceled = !SelectedAppointmentMassage.Canceled;
                    SelectedAppointmentMassage.RaisePropertyChanged(nameof(Apointment.ApColor));
                    TryCancelForUser(SelectedAppointmentMassage, SelectedAppointmentMassage.Canceled);

                    break;

                case "3":
                    SelectedAppointmentOutdoor.Canceled = !SelectedAppointmentOutdoor.Canceled;
                    SelectedAppointmentOutdoor.RaisePropertyChanged(nameof(Apointment.ApColor));
                    TryCancelForUser(SelectedAppointmentOutdoor, SelectedAppointmentOutdoor.Canceled);
                    break;
                case "4":
                    SelectedAppointmentMassageHalf.Canceled = !SelectedAppointmentMassageHalf.Canceled;
                    SelectedAppointmentMassageHalf.RaisePropertyChanged(nameof(Apointment.ApColor));
                    TryCancelForUser(SelectedAppointmentMassageHalf, SelectedAppointmentMassageHalf.Canceled);
                    break;
                case "5":
                    SelectedAppointmentPersonal.Canceled = !SelectedAppointmentPersonal.Canceled;
                    SelectedAppointmentPersonal.RaisePropertyChanged(nameof(Apointment.ApColor));
                    TryCancelForUser(SelectedAppointmentPersonal, SelectedAppointmentPersonal.Canceled);

                    break;
            }

            await BasicDataManager.SaveAsync();
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private async Task ToggleEnabled(RoomEnum room)
        {
            bool disable = false;
            if (room == RoomEnum.Functional)
            {
                if (ClosedHour0 != null)
                {
                    disable = false;
                    BasicDataManager.Context.Delete(ClosedHour0);
                    ClosedHour0 = null;
                }
                else
                {
                    disable = true;
                    ClosedHour0 = new ClosedHour { Date = Time, Room = room };
                    BasicDataManager.Add(ClosedHour0);
                }
                RaisePropertyChanged(nameof(ClosedColor0));
            }
            else if (room == RoomEnum.Pilates)
            {
                if (ClosedHour1 != null)
                {
                    disable = false;
                    BasicDataManager.Context.Delete(ClosedHour1);
                    ClosedHour1 = null;
                }
                else
                {
                    disable = true;
                    ClosedHour1 = new ClosedHour { Date = Time, Room = room };
                    BasicDataManager.Add(ClosedHour1);
                }
                RaisePropertyChanged(nameof(ClosedColor1));
            }
            else if (room == RoomEnum.Massage)
            {
                if (ClosedHourMassage != null)
                {
                    disable = false;
                    BasicDataManager.Context.Delete(ClosedHourMassage);
                    ClosedHourMassage = null;
                }
                else
                {
                    disable = true;
                    ClosedHourMassage = new ClosedHour { Date = Time, Room = room };
                    BasicDataManager.Add(ClosedHourMassage);
                }

                RaisePropertyChanged(nameof(ClosedColorMassage));
            }
            else if (room == RoomEnum.MassageHalf)
            {
                if (ClosedHourMassageHalf != null)
                {
                    disable = false;
                    BasicDataManager.Context.Delete(ClosedHourMassageHalf);
                    ClosedHourMassageHalf = null;
                }
                else
                {
                    disable = true;
                    ClosedHourMassageHalf = new ClosedHour { Date = Time, Room = room };
                    BasicDataManager.Add(ClosedHourMassageHalf);
                }

                RaisePropertyChanged(nameof(ClosedColorMassageHalf));
            }
            else if (room == RoomEnum.Personal)
            {
                if (ClosedHourPersonal != null)
                {
                    disable = false;
                    BasicDataManager.Context.Delete(ClosedHourPersonal);
                    ClosedHourPersonal = null;
                }
                else
                {
                    disable = true;
                    ClosedHourPersonal = new ClosedHour { Date = Time, Room = room };
                    BasicDataManager.Add(ClosedHourPersonal);
                }

                RaisePropertyChanged(nameof(ClosedColorPersonal));
            }
            else if (room == RoomEnum.Outdoor)
            {
                if (ClosedHourOutdoor != null)
                {
                    disable = false;
                    BasicDataManager.Context.Delete(ClosedHourOutdoor);
                    ClosedHourOutdoor = null;
                }
                else
                {
                    disable = true;
                    ClosedHourOutdoor = new ClosedHour { Date = Time, Room = room };
                    BasicDataManager.Add(ClosedHourOutdoor);
                }

                RaisePropertyChanged(nameof(ClosedColorOutdoor));
            }

            if (parent != null)
            {
                var t = parent.Days.FirstOrDefault(d => d.Date.DayOfYear == Time.DayOfYear).Hours.Where(h => (h.SelectedF || h.SelectedR || h.SelectedM || h.SelectedMH || h.SelectedP || h.SelectedO));
                foreach (var h in t)
                {
                    if (h.SelectedF && !(h == this && room == RoomEnum.Functional))
                    {
                        if (h.ClosedHour0 != null && !disable)
                        {
                            BasicDataManager.Context.Delete(h.ClosedHour0);
                            h.ClosedHour0 = null;
                        }
                        else if (h.ClosedHour0 == null && disable)
                        {
                            h.ClosedHour0 = new ClosedHour { Date = h.Time, Room = RoomEnum.Functional };
                            BasicDataManager.Add(h.ClosedHour0);
                        }
                        h.RaisePropertyChanged(nameof(ClosedColor0));
                    }
                    if (h.SelectedR && !(h == this && room == RoomEnum.Pilates))
                    {
                        if (h.ClosedHour1 != null && !disable)
                        {
                            BasicDataManager.Context.Delete(h.ClosedHour1);
                            h.ClosedHour1 = null;
                        }
                        else if (h.ClosedHour1 == null && disable)
                        {
                            h.ClosedHour1 = new ClosedHour { Date = h.Time, Room = RoomEnum.Pilates };
                            BasicDataManager.Add(h.ClosedHour1);
                        }
                        h.RaisePropertyChanged(nameof(ClosedColor1));
                    }
                    if (h.SelectedM && !(h == this && room == RoomEnum.Massage))
                    {
                        if (h.ClosedHourMassage != null && !disable)
                        {
                            BasicDataManager.Context.Delete(h.ClosedHourMassage);
                            h.ClosedHourMassage = null;
                        }
                        else if (h.ClosedHourMassage == null && disable)
                        {
                            h.ClosedHourMassage = new ClosedHour { Date = h.Time, Room = RoomEnum.Massage };
                            BasicDataManager.Add(h.ClosedHourMassage);
                        }
                        h.RaisePropertyChanged(nameof(ClosedColorMassage));
                    }
                    if (h.SelectedMH && !(h == this && room == RoomEnum.MassageHalf))
                    {
                        if (h.ClosedHourMassageHalf != null && !disable)
                        {
                            BasicDataManager.Context.Delete(h.ClosedHourMassageHalf);
                            h.ClosedHourMassageHalf = null;
                        }
                        else if (h.ClosedHourMassageHalf == null && disable)
                        {
                            h.ClosedHourMassageHalf = new ClosedHour { Date = h.Time, Room = RoomEnum.MassageHalf };
                            BasicDataManager.Add(h.ClosedHourMassageHalf);
                        }
                        h.RaisePropertyChanged(nameof(ClosedColorMassageHalf));
                    }
                    if (h.SelectedP && !(h == this && room == RoomEnum.Personal))
                    {
                        if (h.ClosedHourPersonal != null && !disable)
                        {
                            BasicDataManager.Context.Delete(h.ClosedHourPersonal);
                            h.ClosedHourPersonal = null;
                        }
                        else if (h.ClosedHourPersonal == null && disable)
                        {
                            h.ClosedHourPersonal = new ClosedHour { Date = h.Time, Room = RoomEnum.Personal };
                            BasicDataManager.Add(h.ClosedHourPersonal);
                        }
                        h.RaisePropertyChanged(nameof(ClosedColorPersonal));
                    }
                    if (h.SelectedO && !(h == this && room == RoomEnum.Outdoor))
                    {
                        if (h.ClosedHourOutdoor != null && !disable)
                        {
                            BasicDataManager.Context.Delete(h.ClosedHourOutdoor);
                            h.ClosedHourOutdoor = null;
                        }
                        else if (h.ClosedHourOutdoor == null && disable)
                        {
                            h.ClosedHourOutdoor = new ClosedHour { Date = h.Time, Room = RoomEnum.Outdoor };
                            BasicDataManager.Add(h.ClosedHourOutdoor);
                        }
                        h.RaisePropertyChanged(nameof(ClosedColorOutdoor));
                    }
                }
            }
            BasicDataManager.Add(new ProgramChange
            {
                Date = DateTime.Now,
                InstanceGuid = StaticResources.Guid,
                From = Time,
                To = Time.AddHours(1)
            });
            await BasicDataManager.SaveAsync();
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

            BasicDataManager.Add(new ProgramChange
            {
                Date = DateTime.Now,
                InstanceGuid = StaticResources.Guid,
                From = Time,
                To = limit
            });

            await BasicDataManager.SaveAsync();
            Messenger.Default.Send(new UpdateClosedHoursMessage());
            Mouse.OverrideCursor = Cursors.Arrow;
            RaisePropertyChanged(nameof(ClosedColor1));
            RaisePropertyChanged(nameof(ClosedColor0));
            RaisePropertyChanged(nameof(ClosedColorMassage));
            RaisePropertyChanged(nameof(ClosedColorMassageHalf));
            RaisePropertyChanged(nameof(ClosedColorPersonal));
            RaisePropertyChanged(nameof(ClosedColorOutdoor));
        }

        private async Task ToggleWaiting(string obj)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            switch (obj)
            {
                case "0":
                    SelectedApointmentFunctional.Waiting = !SelectedApointmentFunctional.Waiting;
                    SelectedApointmentFunctional.RaisePropertyChanged(nameof(Apointment.ApColor));
                    BasicDataManager.Add(new ProgramChange
                    {
                        Date = DateTime.Now,
                        InstanceGuid = StaticResources.Guid,
                        From = SelectedApointmentFunctional.DateTime,
                        To = SelectedApointmentFunctional.DateTime.AddHours(1)
                    });
                    break;

                case "1":
                    SelectedApointmentReformer.Waiting = !SelectedApointmentReformer.Waiting;
                    SelectedApointmentReformer.RaisePropertyChanged(nameof(Apointment.ApColor));
                    BasicDataManager.Add(new ProgramChange
                    {
                        Date = DateTime.Now,
                        InstanceGuid = StaticResources.Guid,
                        From = SelectedApointmentReformer.DateTime,
                        To = SelectedApointmentReformer.DateTime.AddHours(1)
                    });
                    break;

                case "2":
                    SelectedAppointmentMassage.Waiting = !SelectedAppointmentMassage.Waiting;
                    SelectedAppointmentMassage.RaisePropertyChanged(nameof(Apointment.ApColor));
                    BasicDataManager.Add(new ProgramChange
                    {
                        Date = DateTime.Now,
                        InstanceGuid = StaticResources.Guid,
                        From = SelectedAppointmentMassage.DateTime,
                        To = SelectedAppointmentMassage.DateTime.AddHours(1)
                    });
                    break;

                case "3":
                    SelectedAppointmentOutdoor.Waiting = !SelectedAppointmentOutdoor.Waiting;
                    SelectedAppointmentOutdoor.RaisePropertyChanged(nameof(Apointment.ApColor));
                    BasicDataManager.Add(new ProgramChange
                    {
                        Date = DateTime.Now,
                        InstanceGuid = StaticResources.Guid,
                        From = SelectedAppointmentOutdoor.DateTime,
                        To = SelectedAppointmentOutdoor.DateTime.AddHours(1)
                    });
                    break;
                case "4":
                    SelectedAppointmentMassageHalf.Waiting = !SelectedAppointmentMassageHalf.Waiting;
                    SelectedAppointmentMassageHalf.RaisePropertyChanged(nameof(Apointment.ApColor));
                    BasicDataManager.Add(new ProgramChange
                    {
                        Date = DateTime.Now,
                        InstanceGuid = StaticResources.Guid,
                        From = SelectedAppointmentMassageHalf.DateTime,
                        To = SelectedAppointmentMassageHalf.DateTime.AddHours(1)
                    });
                    break;
                case "5":
                    SelectedAppointmentPersonal.Waiting = !SelectedAppointmentPersonal.Waiting;
                    SelectedAppointmentPersonal.RaisePropertyChanged(nameof(Apointment.ApColor));
                    BasicDataManager.Add(new ProgramChange
                    {
                        Date = DateTime.Now,
                        InstanceGuid = StaticResources.Guid,
                        From = SelectedAppointmentPersonal.DateTime,
                        To = SelectedAppointmentPersonal.DateTime.AddHours(1)
                    });
                    break;
            }

            await BasicDataManager.SaveAsync();
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void TryCancelForUser(Apointment appo, bool canceled)
        {
            var showups = appo.Customer?.ShowUps.Where(s => s.Arrived.Date == appo.DateTime.Date);
            if (showups.Count() == 1)
            {
                showups.First().Real = !canceled;
                MessageBox.Show($"Βρεθηκε μία παρουσία για την ίδια μέρα η οποία ορίστηκε επίσης " +
                    $"σε {(!canceled ? "'ΗΡΘΕ'" : "ΔΕΝ ΗΡΘΕ'")}");
                return;
            }
            if (showups.Count() > 1)
            {
                MessageBox.Show("Βρεθηκαν περισσότερες απο μία παρουσίες." +
                    " Θα πρέπει να ορίσετε χειροκίνητα το 'Ηρθε/Δεν Ήρθε'");
                return;
            }
            MessageBox.Show("Δεν βρέθηκαν παρουσίες για να οριστεί το 'Ηρθε/Δεν Ήρθε'");
        }

        #endregion Methods
    }
}