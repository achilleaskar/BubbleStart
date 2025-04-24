﻿using System;
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
using BubbleStart.Database;
using BubbleStart.Helpers;
using BubbleStart.Messages;
using BubbleStart.Model;
using BubbleStart.Views;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using Serilog;

namespace BubbleStart.ViewModels
{
    public class Apointments_ViewModel : MyViewModelBase
    {
        #region Constructors

        private readonly ILogger logger = Log.ForContext<Apointments_ViewModel>();

        public Apointments_ViewModel(BasicDataManager basicDataManager, int GymNum = 0)
        {
            Days = new ObservableCollection<Day>();
            NextWeekCommand = new RelayCommand(async () => { await NextWeek(); }, CanRun);
            PreviousWeekCommand = new RelayCommand(async () => { await PreviousWeek(); }, CanRun);
            CurrentWeekCommand = new RelayCommand(async () => { await CurrentWeek(); }, CanRun);
            ShowProgramCommand = new RelayCommand(async () => { await CreateProgram(); }, CanRun);
            NoGymnastCommand = new RelayCommand<Hour>(async (obj) => await NoGymnast(obj));
            SetCustomTimeCommand = new RelayCommand(async () => { await (SetCustomTime()); }, CanRun);
            ReformerVisible = FunctionalVisible = FunctionalBVisible = OutdoorVisible = MassageVisible = MassageHalfVisible = PersonalVisible = true;
            BasicDataManager = basicDataManager;
            gymNum = GymNum;
            RoomFrom = gymNum * 10;
            RoomTo = RoomFrom + 9;
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

        private async Task CurrentWeek()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            SelectedDayToGo = DateTime.Today;
            await CreateProgram();
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private bool CanRun()
        {
            return !Loading;
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

        private bool _FunctionalBVisible;

        public bool FunctionalBVisible
        {
            get
            {
                return _FunctionalBVisible;
            }

            set
            {
                if (_FunctionalBVisible == value)
                {
                    return;
                }

                _FunctionalBVisible = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand PreviousWeekCommand { get; set; }
        public RelayCommand CurrentWeekCommand { get; set; }

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
                    FunctionalVisible = FunctionalBVisible = ReformerVisible = MassageVisible = MassageHalfVisible = PersonalVisible = OutdoorVisible = true;
                }
                else if (RoomIndex == 1)
                {
                    ReformerVisible = FunctionalBVisible = MassageVisible = MassageHalfVisible = PersonalVisible = OutdoorVisible = false;
                    FunctionalVisible = true;
                }
                else if (RoomIndex == 2)
                {
                    ReformerVisible = FunctionalVisible = MassageVisible = MassageHalfVisible = PersonalVisible = OutdoorVisible = false;
                    FunctionalBVisible = true;
                }
                else if (RoomIndex == 3)
                {
                    FunctionalVisible = FunctionalBVisible = MassageVisible = MassageHalfVisible = PersonalVisible = OutdoorVisible = false;
                    ReformerVisible = true;
                }
                else if (RoomIndex == 4)
                {
                    FunctionalVisible = FunctionalBVisible = ReformerVisible = MassageHalfVisible = PersonalVisible = OutdoorVisible = false;
                    MassageVisible = true;
                }
                else if (RoomIndex == 5)
                {
                    FunctionalVisible = FunctionalBVisible = ReformerVisible = MassageVisible = PersonalVisible = OutdoorVisible = false;
                    MassageHalfVisible = true;
                }
                else if (RoomIndex == 6)
                {
                    FunctionalVisible = FunctionalBVisible = ReformerVisible = MassageVisible = MassageHalfVisible = OutdoorVisible = false;
                    PersonalVisible = true;
                }
                else if (RoomIndex == 7)
                {
                    FunctionalVisible = FunctionalBVisible = ReformerVisible = MassageVisible = MassageHalfVisible = PersonalVisible = false;
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

        private bool _Loading;

        public bool Loading
        {
            get
            {
                return _Loading;
            }

            set
            {
                if (_Loading == value)
                {
                    return;
                }

                _Loading = value;
                RaisePropertyChanged();
            }
        }

        public List<GymnastHour> GymnastsLocal { get; set; }

        private readonly int RoomFrom;
        private readonly int RoomTo;
        private readonly int gymNum;

        public async Task CreateProgram(bool refresh = true)
        {
            if (Loading)
            {
                return;
            }
            Mouse.OverrideCursor = Cursors.Wait;
            Loading = true;
            stopWatchLE.Start();
            timer.Start();
            stopWatch.Start();
            HasChanges = false;
            LastExecuted = DateTime.Now;

            StartDate = SelectedDayToGo.AddDays(-((int)SelectedDayToGo.DayOfWeek + 6) % 7);
            DateTime tmpdate = StartDate.AddDays(7);

            if (refresh)
                await BasicDataManager.Context.Context.ShowUps.Where(a => a.Arrived >= StartDate && a.Arrived < tmpdate)
                       .Include(c => c.Customer).Distinct()
                       .ToListAsync();
            List<Apointment> apointments = refresh ? await BasicDataManager.Context.Context.Apointments.Where(a => (int)a.Room >= RoomFrom && (int)a.Room <= RoomTo && a.DateTime >= StartDate && a.DateTime < tmpdate)
                .Include(a => a.Customer).Distinct()
                .ToListAsync() :
               BasicDataManager.Context.Context.Apointments.Local.Where(a => (int)a.Room >= RoomFrom && (int)a.Room <= RoomTo && a.DateTime >= StartDate && a.DateTime < tmpdate).Distinct().ToList();

            List<CustomeTime> customTimes = refresh ? await BasicDataManager.Context.Context.CustomeTimes.Where(a => (int)a.Room >= RoomFrom && (int)a.Room <= RoomTo && a.Datetime >= StartDate && a.Datetime < tmpdate).Distinct().ToListAsync() :
              BasicDataManager.Context.Context.CustomeTimes.Local.Where(a => a.Datetime >= StartDate && a.Datetime < tmpdate).Distinct().ToList();

            List<GymnastHour> gymnasts = refresh ? await BasicDataManager.Context.Context.GymnastHours.Where(a => (int)a.Room >= RoomFrom && (int)a.Room <= RoomTo && a.Datetime >= StartDate && a.Datetime < tmpdate || a.Forever).ToListAsync() :
             BasicDataManager.Context.Context.GymnastHours.Local.Where(a => a.Datetime >= StartDate && a.Datetime < tmpdate || a.Forever).Distinct().ToList();
            GymnastsLocal = gymnasts;
            List<ClosedHour> closedHours = refresh ? await BasicDataManager.Context.Context.ClosedHours.Where(a => (int)a.Room >= RoomFrom && (int)a.Room <= RoomTo && a.Date >= StartDate && a.Date < tmpdate).Distinct().ToListAsync() :
             BasicDataManager.Context.Context.ClosedHours.Local.Where(a => a.Date >= StartDate && a.Date < tmpdate).Distinct().ToList();

            logger.Information($"Loaded {gymnasts.Count} gymnasts for gymNum {gymNum} : {string.Join(", ", gymnasts.OrderBy(d => d.Datetime).Select(g => $"{g.Gymnast_Id}-{g.Datetime.ToString("dd/MM hh:mm")}-id:{g.Id}-room:{g.Room}"))}");

            DateTime tmpDate = StartDate;
            Days.Clear();
            for (int i = 0; i < 7; i++)
            {
                Days.Add(new Day(BasicDataManager, tmpDate, gymNum));
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
                if (numOfDay < 7 && ch.Date.Hour >= 7 && ch.Date.Hour <= 22)
                {
                    if (ch.Room == RoomEnum.Functional || ch.Room == RoomEnum.Fitness)
                    {
                        Days[numOfDay].Hours[ch.Date.Hour - 7].ClosedHour0 = ch;
                    }
                    else if (ch.Room == RoomEnum.FunctionalB || ch.Room == RoomEnum.Strength)
                    {
                        Days[numOfDay].Hours[ch.Date.Hour - 7].ClosedHourFB = ch;
                    }
                    else if (ch.Room == RoomEnum.Pilates || ch.Room == RoomEnum.Personal2)
                    {
                        Days[numOfDay].Hours[ch.Date.Hour - 7].ClosedHour1 = ch;
                    }
                    else if (ch.Room == RoomEnum.Personal || ch.Room == RoomEnum.FreeSpace)
                    {
                        Days[numOfDay].Hours[ch.Date.Hour - 7].ClosedHourPersonal = ch;
                    }
                    else if (ch.Room == RoomEnum.Massage || ch.Room == RoomEnum.Massage2)
                    {
                        Days[numOfDay].Hours[ch.Date.Hour - 7].ClosedHourMassage = ch;
                    }
                    else if (ch.Room == RoomEnum.Outdoor)
                    {
                        Days[numOfDay].Hours[ch.Date.Hour - 7].ClosedHourOutdoor = ch;
                    }
                    else if (ch.Room == RoomEnum.MassageHalf)
                    {
                        Days[numOfDay].Hours[ch.Date.Hour - 7].ClosedHourMassageHalf = ch;
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
                if (numOfDay < 7 && ap.DateTime.Hour >= 7 && ap.DateTime.Hour <= 22)
                {
                    switch (ap.Room)
                    {
                        case RoomEnum.Functional:
                        case RoomEnum.Fitness:
                            Days[numOfDay].Hours[ap.DateTime.Hour - 7].AppointmentsFunctional.Add(ap);
                            break;

                        case RoomEnum.FunctionalB:
                        case RoomEnum.Strength:
                            Days[numOfDay].Hours[ap.DateTime.Hour - 7].AppointmentsFB.Add(ap);
                            break;

                        case RoomEnum.Pilates:
                        case RoomEnum.Personal2:
                            Days[numOfDay].Hours[ap.DateTime.Hour - 7].AppointmentsReformer.Add(ap);
                            break;

                        case RoomEnum.Personal:
                        case RoomEnum.FreeSpace:
                            Days[numOfDay].Hours[ap.DateTime.Hour - 7].AppointmentsPersonal.Add(ap);
                            break;

                        case RoomEnum.Massage:
                        case RoomEnum.Massage2:
                            Days[numOfDay].Hours[ap.DateTime.Hour - 7].AppointmentsMassage.Add(ap);
                            break;

                        case RoomEnum.Outdoor:
                            Days[numOfDay].Hours[ap.DateTime.Hour - 7].AppointemntsOutdoor.Add(ap);
                            break;

                        case RoomEnum.MassageHalf:
                            Days[numOfDay].Hours[ap.DateTime.Hour - 7].AppointmentsMassageHalf.Add(ap);
                            break;

                        default:
                            MessageBox.Show("Σφάλμα στην επιλογή αίθουσας");
                            break;
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
                        case RoomEnum.Fitness:
                            t.CustomTime1 = ct;
                            break;

                        case RoomEnum.FunctionalB:
                        case RoomEnum.Strength:
                            t.CustomTime7 = ct;
                            break;

                        case RoomEnum.Pilates:
                        case RoomEnum.Personal2:
                            t.CustomTime2 = ct;
                            break;

                        case RoomEnum.Personal:
                        case RoomEnum.FreeSpace:
                            t.CustomTime6 = ct;
                            break;

                        case RoomEnum.Massage:
                        case RoomEnum.Massage2:
                            t.CustomTime3 = ct;
                            break;

                        case RoomEnum.Outdoor:
                            t.CustomTime4 = ct;
                            break;

                        case RoomEnum.MassageHalf:
                            t.CustomTime5 = ct;
                            break;
                    }
            }

            foreach (var d in Days)
            {
                if (d.Date.DayOfWeek != DayOfWeek.Saturday && gymNum == 1)
                    foreach (var h in d.Hours)
                    {
                        if (h.Time.Hour > 14 && h.CustomTime1 == null)
                            h.CustomTime1 = new CustomeTime
                            {
                                Time = h.Time.AddMinutes(-15).ToString("HH:mm")
                            };
                        if (h.Time.Hour > 14 && h.CustomTime7 == null)
                            h.CustomTime7 = new CustomeTime
                            {
                                Time = h.Time.AddMinutes(-15).ToString("HH:mm")
                            };
                    }
            }

            foreach (var gh in gymnasts.OrderByDescending(r => r.Forever).ThenBy(r => r.Id))
            {
                t = Days.FirstOrDefault(d => d.Date == gh.Datetime.Date || (gh.Forever && gh.Datetime <= d.Date && gh.Datetime.DayOfWeek == d.Date.DayOfWeek))?
                    .Hours.FirstOrDefault(h => h.Time.Hour == gh.Datetime.Hour && gh.Datetime.Minute == h.Time.Minute);
                if (t != null)
                    switch (gh.Room)
                    {
                        case RoomEnum.Functional:
                        case RoomEnum.Fitness:
                            t.GymnastFunctional = gh;
                            break;

                        case RoomEnum.FunctionalB:
                        case RoomEnum.Strength:
                            t.GymnastFunctionalB = gh;
                            break;

                        case RoomEnum.Pilates:
                        case RoomEnum.Personal2:
                            t.GymnastReformer = gh;
                            break;

                        case RoomEnum.Personal:
                        case RoomEnum.FreeSpace:
                            t.GymnastPersonal = gh;
                            break;

                        case RoomEnum.Massage:
                        case RoomEnum.Massage2:
                            t.GymnastMassage = gh;
                            break;

                        case RoomEnum.MassageHalf:
                            t.GymnastMassageHalf = gh;
                            break;

                        case RoomEnum.Outdoor:
                            t.GymnastOutdoor = gh;
                            break;
                    }
            }

            List<WorkingRule> Wrules;
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
            //var count = 0;
            //foreach (var day in Days)
            //{
            //    foreach (var hour in day.Hours)
            //    {
            //        if (hour.GymnastFunctional?.Forever == true)
            //        {
            //            BasicDataManager.Add(new GymnastHour
            //            {
            //                Datetime = hour.Time,
            //                Gymnast = hour.GymnastFunctional.Gymnast,
            //                Gymnast_Id = hour.GymnastFunctional.Gymnast.Id,
            //                Room = RoomEnum.Funct ional
            //            });
            //            count++;
            //            if (count > 30)
            //            {
            //                await BasicDataManager.SaveAsync();
            //                count = 0;
            //            }
            //        }
            //        if (hour.GymnastFunctionalB?.Forever == true)
            //        {
            //            BasicDataManager.Add(new GymnastHour
            //            {
            //                Datetime = hour.Time,
            //                Gymnast = hour.GymnastFunctionalB.Gymnast,
            //                Gymnast_Id = hour.GymnastFunctionalB.Gymnast.Id,
            //                Room = RoomEnum.Functio nalB
            //            });
            //            count++;
            //            if (count > 30)
            //            {
            //                await BasicDataManager.SaveAsync();
            //                count = 0;
            //            }
            //        }
            //        if (hour.GymnastReformer?.Forever == true)
            //        {
            //            BasicDataManager.Add(new GymnastHour
            //            {
            //                Datetime = hour.Time,
            //                Gymnast = hour.GymnastReformer.Gymnast,
            //                Gymnast_Id = hour.GymnastReformer.Gymnast.Id,
            //                Room = RoomEnum.Pil ates
            //            });
            //            count++;
            //            if (count > 30)
            //            {
            //                await BasicDataManager.SaveAsync();
            //                count = 0;
            //            }
            //        }
            //        if (hour.GymnastMassage?.Forever == true)
            //        {
            //            BasicDataManager.Add(new GymnastHour
            //            {
            //                Datetime = hour.Time,
            //                Gymnast = hour.GymnastMassage.Gymnast,
            //                Gymnast_Id = hour.GymnastMassage.Gymnast.Id,
            //                Room = RoomEnum.Ma ssage
            //            });
            //            count++;
            //            if (count > 30)
            //            {
            //                await BasicDataManager.SaveAsync();
            //                count = 0;
            //            }
            //        }
            //        if (hour.GymnastMassageHalf?.Forever == true)
            //        {
            //            BasicDataManager.Add(new GymnastHour
            //            {
            //                Datetime = hour.Time,
            //                Gymnast = hour.GymnastMassageHalf.Gymnast,
            //                Gymnast_Id = hour.GymnastMassageHalf.Gymnast.Id,
            //                Room = RoomEnum.MassageH alf
            //            });
            //            count++;
            //            if (count > 30)
            //            {
            //                await BasicDataManager.SaveAsync();
            //                count = 0;
            //            }
            //        }
            //        if (hour.GymnastOutdoor?.Forever == true)
            //        {
            //            BasicDataManager.Add(new GymnastHour
            //            {
            //                Datetime = hour.Time,
            //                Gymnast = hour.GymnastOutdoor.Gymnast,
            //                Gymnast_Id = hour.GymnastOutdoor.Gymnast.Id,
            //                Room = RoomEnum.Outd oor
            //            });
            //            count++;
            //            if (count > 30)
            //            {
            //                await BasicDataManager.SaveAsync();
            //                count = 0;
            //            }
            //        }
            //        if (hour.GymnastPersonal?.Forever == true)
            //        {
            //            BasicDataManager.Add(new GymnastHour
            //            {
            //                Datetime = hour.Time,
            //                Gymnast = hour.GymnastPersonal.Gymnast,
            //                Gymnast_Id = hour.GymnastPersonal.Gymnast.Id,
            //                Room = RoomEnum.Pers onal
            //            });
            //            count++;
            //            if (count > 30)
            //            {
            //                await BasicDataManager.SaveAsync();
            //                count = 0;
            //            }
            //        }
            //    }
            //}
            //await BasicDataManager.SaveAsync(false);

            RaisePropertyChanged(nameof(HasDays));
            RaisePropertyChanged(nameof(Days));
            LastExecuted = DateTime.Now;
            Mouse.OverrideCursor = Cursors.Arrow;
            Loading = false;
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

                var closedHours = BasicDataManager.Context.Context.ClosedHours.Local?.Where(a => (int)a.Room >= RoomFrom && (int)a.Room <= RoomTo && a.Date >= StartDate && a.Date < tmpdate && a.Date >= BasicDataManager.Context.Limit).ToList() ?? new List<ClosedHour>();

                DateTime tmpDate = StartDate;

                int numOfDay;

                foreach (var ch in closedHours)
                {
                    numOfDay = ((int)ch.Date.DayOfWeek + 6) % 7;
                    if (numOfDay < 6 && ch.Date.Hour >= 7 && ch.Date.Hour <= 22)
                    {
                        if (ch.Room == RoomEnum.Functional || ch.Room == RoomEnum.Fitness)
                        {
                            Days[numOfDay].Hours[ch.Date.Hour - 7].ClosedHour0 = ch;
                        }
                        else if (ch.Room == RoomEnum.FunctionalB || ch.Room == RoomEnum.Strength)
                        {
                            Days[numOfDay].Hours[ch.Date.Hour - 7].ClosedHourFB = ch;
                        }
                        else if (ch.Room == RoomEnum.Pilates || ch.Room == RoomEnum.Personal2)
                        {
                            Days[numOfDay].Hours[ch.Date.Hour - 7].ClosedHour1 = ch;
                        }
                        else if (ch.Room == RoomEnum.Personal || ch.Room == RoomEnum.FreeSpace)
                        {
                            Days[numOfDay].Hours[ch.Date.Hour - 7].ClosedHourPersonal = ch;
                        }
                        else if (ch.Room == RoomEnum.Massage || ch.Room == RoomEnum.Massage2)
                        {
                            Days[numOfDay].Hours[ch.Date.Hour - 7].ClosedHourMassage = ch;
                        }
                        else if (ch.Room == RoomEnum.MassageHalf)
                        {
                            Days[numOfDay].Hours[ch.Date.Hour - 7].ClosedHourMassageHalf = ch;
                        }
                        else if (ch.Room == RoomEnum.Outdoor)
                        {
                            Days[numOfDay].Hours[ch.Date.Hour - 7].ClosedHourOutdoor = ch;
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
            if ((int)room / 10 == gymNum)
            {
                selectedHour = h;
                selectedRoom = room;
                CustomTime = selectedHour.Time.ToString("HH:mm");
                TimePopupOpen = true;
            }
        }

        private async Task NextWeek()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            SelectedDayToGo = SelectedDayToGo.AddDays(-((int)SelectedDayToGo.DayOfWeek + 6) % 7 + 7);
            await CreateProgram();
            //if (SelectedDayToGo < new DateTime(2024, 8, 1))
            //    await NextWeek();
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
            Loading = true;
            if (selectedHour == null)
                return;
            Mouse.OverrideCursor = Cursors.Wait;
            if (selectedRoom == RoomEnum.Functional || selectedRoom == RoomEnum.Fitness)
            {
                if (selectedHour.CustomTime1?.Id > 0)
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
            if (selectedRoom == RoomEnum.FunctionalB || selectedRoom == RoomEnum.Strength)
            {
                if (selectedHour.CustomTime7?.Id > 0)
                {
                    selectedHour.CustomTime7.Time = CustomTime;
                }
                else
                {
                    selectedHour.CustomTime7 = new CustomeTime { Datetime = selectedHour.Time, Room = selectedRoom, Time = CustomTime };

                    BasicDataManager.Add(selectedHour.CustomTime7);
                }
                selectedHour.RaisePropertyChanged(nameof(Hour.TimeString7));
            }
            else if (selectedRoom == RoomEnum.Pilates || selectedRoom == RoomEnum.Personal2)
            {
                if (selectedHour.CustomTime2?.Id > 0)
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
            else if (selectedRoom == RoomEnum.Personal || selectedRoom == RoomEnum.FreeSpace)
            {
                if (selectedHour.CustomTime6?.Id > 0)
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
            else if (selectedRoom == RoomEnum.Massage || selectedRoom == RoomEnum.Massage2)
            {
                if (selectedHour.CustomTime3?.Id > 0)
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
                if (selectedHour.CustomTime5?.Id > 0)
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
            else if (selectedRoom == RoomEnum.Outdoor)
            {
                if (selectedHour.CustomTime4?.Id > 0)
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
                GymNum = gymNum,
                InstanceGuid = StaticResources.Guid,
                From = selectedHour.Time,
                To = selectedHour.Time.AddHours(1)
            });

            await BasicDataManager.SaveAsync();
            Mouse.OverrideCursor = Cursors.Arrow;
            TimePopupOpen = false;
            Loading = false;
        }

        private async void timer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (!HasChanges && stopWatch.Elapsed.TotalSeconds >= 3 && stopWatchLE.Elapsed.TotalSeconds >= 60)
                {
                    HasChanges = await db.ProgramChanges.AnyAsync(c => c.InstanceGuid != StaticResources.Guid && c.Date > LastExecuted && c.To >= StartDate && c.From <= To);
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

        public Day(BasicDataManager basicDataManager, DateTime date, int gymNum)
        {
            Date = date;
            Hours = new ObservableCollection<Hour>
            {
                new Hour(new DateTime(Date.Year,Date.Month,Date.Day,7,0,0),basicDataManager,gymNum),
                new Hour(new DateTime(Date.Year,Date.Month,Date.Day,8,0,0),basicDataManager,gymNum),
                new Hour(new DateTime(Date.Year,Date.Month,Date.Day,9,0,0),basicDataManager,gymNum),
                new Hour(new DateTime(Date.Year,Date.Month,Date.Day,10,0,0),basicDataManager,gymNum),
                new Hour(new DateTime(Date.Year,Date.Month,Date.Day,11,0,0),basicDataManager,gymNum),
                new Hour(new DateTime(Date.Year,Date.Month,Date.Day,12,0,0),basicDataManager,gymNum),
                new Hour(new DateTime(Date.Year,Date.Month,Date.Day,13,0,0),basicDataManager,gymNum),
                new Hour(new DateTime(Date.Year,Date.Month,Date.Day,14,0,0),basicDataManager,gymNum),
                new Hour(new DateTime(Date.Year,Date.Month,Date.Day,15,0,0),basicDataManager,gymNum),
                new Hour(new DateTime(Date.Year,Date.Month,Date.Day,16,0,0),basicDataManager,gymNum),
                new Hour(new DateTime(Date.Year,Date.Month,Date.Day,17,0,0),basicDataManager,gymNum),
                new Hour(new DateTime(Date.Year,Date.Month,Date.Day,18,0,0),basicDataManager,gymNum),
                new Hour(new DateTime(Date.Year,Date.Month,Date.Day,19,0,0),basicDataManager,gymNum),
                new Hour(new DateTime(Date.Year,Date.Month,Date.Day,20,0,0),basicDataManager,gymNum),
                new Hour(new DateTime(Date.Year,Date.Month,Date.Day,21,0,0),basicDataManager,gymNum),
                new Hour(new DateTime(Date.Year,Date.Month,Date.Day,22,0,0),basicDataManager,gymNum)
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

        private double _HeightFunctionalB;

        public double HeightFunctionalB
        {
            get
            {
                return _HeightFunctionalB;
            }

            set
            {
                if (_HeightFunctionalB == value)
                {
                    return;
                }

                _HeightFunctionalB = value;
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

        private double _RatingFunctional;

        public double RatingFunctional
        {
            get
            {
                return _RatingFunctional;
            }

            set
            {
                if (_RatingFunctional == value)
                {
                    return;
                }

                _RatingFunctional = value;
                RaisePropertyChanged();
            }
        }

        private double _RatingPilates;

        public double RatingPilates
        {
            get
            {
                return _RatingPilates;
            }

            set
            {
                if (_RatingPilates == value)
                {
                    return;
                }

                _RatingPilates = value;
                RaisePropertyChanged();
            }
        }

        private double _RatingMassage;

        public double RatingMassage
        {
            get
            {
                return _RatingMassage;
            }

            set
            {
                if (_RatingMassage == value)
                {
                    return;
                }

                _RatingMassage = value;
                RaisePropertyChanged();
            }
        }

        private double _RatingMassage30;

        public double RatingMassage30
        {
            get
            {
                return _RatingMassage30;
            }

            set
            {
                if (_RatingMassage30 == value)
                {
                    return;
                }

                _RatingMassage30 = value;
                RaisePropertyChanged();
            }
        }

        private double _RatingPersonal;

        public double RatingPersonal
        {
            get
            {
                return _RatingPersonal;
            }

            set
            {
                if (_RatingPersonal == value)
                {
                    return;
                }

                _RatingPersonal = value;
                RaisePropertyChanged();
            }
        }

        private double _RatingOutdoor;

        public double RatingOutdoor
        {
            get
            {
                return _RatingOutdoor;
            }

            set
            {
                if (_RatingOutdoor == value)
                {
                    return;
                }

                _RatingOutdoor = value;
                RaisePropertyChanged();
            }
        }

        private bool _ShowStars;

        public bool ShowStars
        {
            get
            {
                return _ShowStars;
            }

            set
            {
                if (_ShowStars == value)
                {
                    return;
                }

                _ShowStars = value;
                RaisePropertyChanged();
            }
        }

        public Hour(DateTime time, BasicDataManager basicDataManager, int GymNum)
        {
            Time = time;
            BasicDataManager = basicDataManager;
            gymNum = GymNum;
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
            AppointmentsFB = new ObservableCollection<Apointment>();
            AppointmentsReformer = new ObservableCollection<Apointment>();
            AppointmentsMassage = new ObservableCollection<Apointment>();
            AppointmentsMassageHalf = new ObservableCollection<Apointment>();
            AppointmentsPersonal = new ObservableCollection<Apointment>();
            AppointemntsOutdoor = new ObservableCollection<Apointment>();
        }

        #endregion Constructors

        #region Fields

        private ObservableCollection<Apointment> _AppointmentsFunctional;
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
            get => _AppointmentsFunctional;

            set
            {
                if (_AppointmentsFunctional == value)
                {
                    return;
                }

                _AppointmentsFunctional = value;
                RaisePropertyChanged();
                FunctionalCV = (CollectionView)CollectionViewSource.GetDefaultView(_AppointmentsFunctional);
                FunctionalCV.Filter = AppointmensFilter;
                FunctionalCV.SortDescriptions.Add(new SortDescription(nameof(Apointment.Person), ListSortDirection.Ascending));
                FunctionalCV.Refresh();
            }
        }

        private ObservableCollection<Apointment> _AppointmentsFB;

        public ObservableCollection<Apointment> AppointmentsFB
        {
            get
            {
                return _AppointmentsFB;
            }

            set
            {
                if (_AppointmentsFB == value)
                {
                    return;
                }

                _AppointmentsFB = value;
                RaisePropertyChanged();
                FunctionalBCV = (CollectionView)CollectionViewSource.GetDefaultView(_AppointmentsFB);
                FunctionalBCV.Filter = AppointmensFilter;
                FunctionalBCV.SortDescriptions.Add(new SortDescription(nameof(Apointment.Person), ListSortDirection.Ascending));
                FunctionalBCV.Refresh();
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

        public SolidColorBrush ClosedColorFB => GetClosedColor(6);

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
                RaisePropertyChanged(nameof(ClosedColor0));
            }
        }

        private ClosedHour _ClosedHourFB;

        public ClosedHour ClosedHourFB
        {
            get
            {
                return _ClosedHourFB;
            }

            set
            {
                if (_ClosedHourFB == value)
                {
                    return;
                }

                _ClosedHourFB = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(ClosedColorFB));
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
                RaisePropertyChanged(nameof(ClosedColor1));
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
                RaisePropertyChanged(nameof(ClosedColorMassage));
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
                RaisePropertyChanged(nameof(ClosedColorMassageHalf));
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
                RaisePropertyChanged(nameof(ClosedColorOutdoor));
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
                RaisePropertyChanged(nameof(ClosedColorPersonal));
            }
        }

        public RelayCommand<int> CLoseHourCommand { get; set; }

        public CustomeTime CustomTime1 { get; set; }

        public CustomeTime CustomTime2 { get; set; }

        public CustomeTime CustomTime3 { get; set; }

        public CustomeTime CustomTime4 { get; set; }

        public CustomeTime CustomTime5 { get; set; }

        public CustomeTime CustomTime6 { get; set; }

        public CustomeTime CustomTime7 { get; set; }

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
                FunctionalBCV.Refresh();
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

        private GymnastHour _GymnastFunctionalB;

        public GymnastHour GymnastFunctionalB
        {
            get
            {
                return _GymnastFunctionalB;
            }

            set
            {
                if (_GymnastFunctionalB == value)
                {
                    return;
                }

                _GymnastFunctionalB = value;
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

        private ICollectionView _FunctionalBCV;

        public ICollectionView FunctionalBCV
        {
            get
            {
                return _FunctionalBCV;
            }

            set
            {
                if (_FunctionalBCV == value)
                {
                    return;
                }

                _FunctionalBCV = value;
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
        public Apointment SelectedApointmentFB { get; set; }

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

        private bool _SelectedFB;
        private readonly int gymNum;

        public bool SelectedFB
        {
            get
            {
                return _SelectedFB;
            }

            set
            {
                if (_SelectedFB == value)
                {
                    return;
                }

                _SelectedFB = value;
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

        public string TimeString7 => CustomTime7 != null ? CustomTime7.Time : Time.ToString("HH:mm");

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

                Apointment ap = new Apointment
                {
                    Customer = customer,
                    DateTime = Time,
                    Person = selectedPerson,
                    Room = room,
                    Gymnast = SelectedGymnast,
                    Waiting = waiting
                };
                if (forever)
                {
                    BasicDataManager.Add(new ProgramChange
                    {
                        Date = DateTime.Now,
                        GymNum = gymNum,
                        InstanceGuid = StaticResources.Guid,
                        From = Time,
                        To = Time.AddYears(1)
                    });
                    List<Apointment> nextAppoitments = await BasicDataManager.Context.GetAllAppointmentsThisDayAsync(customer.Id, Time);
                    if (nextAppoitments.Any(a => a.DateTime.DayOfWeek == Time.DayOfWeek))
                    {
                        var ansr = MessageBox.Show("Βρέθηκαν ήδη μελλοντικά ραντεβού την ίδια μέρα στις: " +
                           string.Join("/n", nextAppoitments.Where(a => a.DateTime.DayOfWeek == Time.DayOfWeek)
                           .Select(a => a.DateTime.ToString("dd/MM/yy"))) +
                           " Θέλετε σίγουρα να συνεχίσετε?", "Προσοχή", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (ansr == MessageBoxResult.No)
                        {
                            Mouse.OverrideCursor = Cursors.Arrow;
                            return;
                        }
                    }
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
                {
                    if (customer.Apointments.Any(a => a.DateTime.Date == Time.Date))
                    {
                        var ansr = MessageBox.Show("Ο πελάτης έχει ήδη ραντεβού σήμερα Θέλετε σίγουρα να συνεχίσετε?", "Προσοχή", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (ansr == MessageBoxResult.No)
                        {
                            Mouse.OverrideCursor = Cursors.Arrow;
                            return;
                        }
                    }
                    BasicDataManager.Add(new ProgramChange
                    {
                        GymNum = gymNum,
                        Date = DateTime.Now,
                        InstanceGuid = StaticResources.Guid,
                        From = Time,
                        To = Time.AddHours(1)
                    });
                }

                if (((room == RoomEnum.Functional || room == RoomEnum.Fitness) && !AppointmentsFunctional.Any(a => a.Customer.Id == ap.Customer.Id)) ||
                    ((room == RoomEnum.FunctionalB || room == RoomEnum.Strength) && !AppointmentsFB.Any(api => api.Customer.Id == ap.Customer.Id)) ||
                    ((room == RoomEnum.Pilates || room == RoomEnum.Personal2) && !AppointmentsReformer.Any(api => api.Customer.Id == ap.Customer.Id)) ||
                    ((room == RoomEnum.Personal || room == RoomEnum.FreeSpace) && !AppointmentsPersonal.Any(api => api.Customer.Id == ap.Customer.Id)) ||
                    ((room == RoomEnum.Massage || room == RoomEnum.Massage2) && !AppointmentsMassage.Any(api => api.Customer.Id == ap.Customer.Id)) ||
                    (room == RoomEnum.MassageHalf && !AppointmentsMassageHalf.Any(api => api.Customer.Id == ap.Customer.Id)) ||
                    (room == RoomEnum.Outdoor && !AppointemntsOutdoor.Any(api => api.Customer.Id == ap.Customer.Id)))
                {
                    if (room == RoomEnum.Functional || room == RoomEnum.Fitness)
                    {
                        AppointmentsFunctional.Add(ap);
                        BasicDataManager.Add(ap);
                    }
                    else if (room == RoomEnum.FunctionalB || room == RoomEnum.Strength)
                    {
                        AppointmentsFB.Add(ap);
                        BasicDataManager.Add(ap);
                    }
                    else if (room == RoomEnum.Pilates || room == RoomEnum.Personal2)
                    {
                        AppointmentsReformer.Add(ap);
                        BasicDataManager.Add(ap);
                    }
                    else if (room == RoomEnum.Personal || room == RoomEnum.FreeSpace)
                    {
                        AppointmentsPersonal.Add(ap);
                        BasicDataManager.Add(ap);
                    }
                    else if (room == RoomEnum.Massage || room == RoomEnum.Massage2)
                    {
                        AppointmentsMassage.Add(ap);
                        BasicDataManager.Add(ap);
                    }
                    else if (room == RoomEnum.MassageHalf)
                    {
                        AppointmentsMassageHalf.Add(ap);
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
                bool forever = v >= 10;
                var selHours = parent.Days.FirstOrDefault(d => d.Date.DayOfYear == Time.DayOfYear).Hours.Where(h => h.SelectedF || h.SelectedFB || h.SelectedR || h.SelectedM || h.SelectedMH || h.SelectedP || h.SelectedO).ToList();
                //List<ClosedHour> ClosedHours = await BasicDataManager.Context.GetAllClosedHoursAsync(room, Time, selectedHours);

                //if (forever && selHours.Any(s =>
                //s.SelectedF && s.GymnastFunctional != null ||
                //s.SelectedFB && s.GymnastFunctionalB != null ||
                //s.SelectedM && s.GymnastMassage != null ||
                //s.SelectedMH && s.GymnastMassageHalf != null ||
                //s.SelectedO && s.GymnastOutdoor != null ||
                //s.SelectedP && s.GymnastPersonal != null ||
                //s.SelectedR && s.GymnastReformer != null
                //))
                //{
                //    MessageBox.Show("Αφαιρέστε τους υπάρχοντες γυμναστές πρίν βάλετε γυμναστές για πάντα στα επιλεγμένα κουτάκια");
                //    Mouse.OverrideCursor = Cursors.Arrow;
                //    return;
                //}

                //TODO
                v %= 10;
                int GymPlus = gymNum * 10;
                if (selHours == null || selHours.Count == 0)
                {
                    if (v == 0)
                    {
                        if (GymnastFunctional == null)
                        {
                            GymnastFunctional = new GymnastHour { Datetime = Time, Gymnast = u, Gymnast_Id = u.Id, Room = (RoomEnum)(v + GymPlus) };
                            BasicDataManager.Add(GymnastFunctional);
                            parent.GymnastsLocal.Add(GymnastFunctional);
                        }
                        else
                        {
                            if (ShowMessage(forever, GymnastFunctional, u))
                            {
                                GymnastFunctional.Gymnast = u;
                                GymnastFunctional.Gymnast_Id = u.Id;
                            }
                        }
                    }
                    else if (v == 6)
                    {
                        if (GymnastFunctionalB == null)
                        {
                            GymnastFunctionalB = new GymnastHour { Datetime = Time, Gymnast = u, Gymnast_Id = u.Id, Room = (RoomEnum)(v + GymPlus) };
                            BasicDataManager.Add(GymnastFunctionalB);
                            parent.GymnastsLocal.Add(GymnastFunctionalB);
                        }
                        else
                        {
                            if (ShowMessage(forever, GymnastFunctionalB, u))
                            {
                                GymnastFunctionalB.Gymnast = u;
                                GymnastFunctionalB.Gymnast_Id = u.Id;
                            }
                        }
                    }
                    else if (v == 1)
                    {
                        if (GymnastReformer == null)
                        {
                            GymnastReformer = new GymnastHour { Datetime = Time, Gymnast = u, Gymnast_Id = u.Id, Room = (RoomEnum)(v + GymPlus) };
                            BasicDataManager.Add(GymnastReformer);
                            parent.GymnastsLocal.Add(GymnastReformer);
                        }
                        else
                        {
                            if (ShowMessage(forever, GymnastReformer, u))
                            {
                                GymnastReformer.Gymnast = u;
                                GymnastReformer.Gymnast_Id = u.Id;
                            }
                        }
                    }
                    else if (v == 2)
                    {
                        if (GymnastMassage == null)
                        {
                            GymnastMassage = new GymnastHour { Datetime = Time, Gymnast = u, Gymnast_Id = u.Id, Room = (RoomEnum)(v + GymPlus) };
                            BasicDataManager.Add(GymnastMassage);
                            parent.GymnastsLocal.Add(GymnastMassage);
                        }
                        else
                        {
                            if (ShowMessage(forever, GymnastMassage, u))
                            {
                                GymnastMassage.Gymnast = u;
                                GymnastMassage.Gymnast_Id = u.Id;
                            }
                        }
                    }
                    else if (v == 3)
                    {
                        if (GymnastOutdoor == null)
                        {
                            GymnastOutdoor = new GymnastHour { Datetime = Time, Gymnast = u, Gymnast_Id = u.Id, Room = (RoomEnum)(v + GymPlus) };
                            BasicDataManager.Add(GymnastOutdoor);
                            parent.GymnastsLocal.Add(GymnastOutdoor);
                        }
                        else
                        {
                            if (ShowMessage(forever, GymnastOutdoor, u))
                            {
                                GymnastOutdoor.Gymnast = u;
                                GymnastOutdoor.Gymnast_Id = u.Id;
                            }
                        }
                    }
                    else if (v == 4)
                    {
                        if (GymnastMassageHalf == null)
                        {
                            GymnastMassageHalf = new GymnastHour { Datetime = Time, Gymnast = u, Gymnast_Id = u.Id, Room = (RoomEnum)(v + GymPlus) };
                            BasicDataManager.Add(GymnastMassageHalf);
                            parent.GymnastsLocal.Add(GymnastMassageHalf);
                        }
                        else
                        {
                            if (ShowMessage(forever, GymnastMassageHalf, u))
                            {
                                GymnastMassageHalf.Gymnast = u;
                                GymnastMassageHalf.Gymnast_Id = u.Id;
                            }
                        }
                    }
                    else if (v == 5)
                    {
                        if (GymnastPersonal == null)
                        {
                            GymnastPersonal = new GymnastHour { Datetime = Time, Gymnast = u, Gymnast_Id = u.Id, Room = (RoomEnum)(v + GymPlus) };
                            BasicDataManager.Add(GymnastPersonal);
                            parent.GymnastsLocal.Add(GymnastPersonal);
                        }
                        else
                        {
                            if (ShowMessage(forever, GymnastPersonal, u))
                            {
                                GymnastPersonal.Gymnast = u;
                                GymnastPersonal.Gymnast_Id = u.Id;
                            }
                        }
                    }
                }
                if (parent != null && (selHours.Any() || forever))
                {
                    bool solo = false;
                    if (selHours == null || !selHours.Any())
                    {
                        selHours = new List<Hour>
                        {
                            this
                        };
                        solo = true;
                    }

                    List<GymnastHour> NextGymnasts = await BasicDataManager.Context.GetAllNextGymnastsAsync(Time, selHours, forever, gymNum);
                    int counter = 0;
                    foreach (var h in selHours)
                    {
                        if (h.SelectedF || (solo && v == 0))
                        {
                            var limit = forever ? Time.AddMonths(3) : h.Time;
                            var tmpTime = !solo ? h.Time : h.Time.AddDays(7);
                            while (tmpTime <= limit)
                            {
                                var nextGym = NextGymnasts.Find(c => c.Datetime == tmpTime && (c.Room == RoomEnum.Functional || c.Room == RoomEnum.Fitness));
                                if (nextGym == null)
                                {
                                    nextGym = new GymnastHour { Datetime = tmpTime, Gymnast = u, Gymnast_Id = u.Id, Room = gymNum == 0 ? RoomEnum.Functional : RoomEnum.Fitness };
                                    if (tmpTime.Date == Time.Date)
                                    {
                                        h.GymnastFunctional = nextGym;
                                    }
                                    BasicDataManager.Add(nextGym);
                                    parent.GymnastsLocal.Add(nextGym);
                                    counter++;
                                    if (counter > 30)
                                    {
                                        await BasicDataManager.SaveAsync();
                                        counter = 0;
                                    }
                                }
                                else
                                {
                                    nextGym.Gymnast = u;
                                    nextGym.Gymnast_Id = u.Id;
                                }
                                tmpTime = tmpTime.AddDays(7);
                            }
                        }
                        if (h.SelectedFB || (solo && v == 6))
                        {
                            var limit = forever ? Time.AddMonths(3) : h.Time;
                            var tmpTime = !solo ? h.Time : h.Time.AddDays(7);
                            while (tmpTime <= limit)
                            {
                                var nextGym = NextGymnasts.FirstOrDefault(c => c.Datetime == tmpTime && (c.Room == RoomEnum.FunctionalB || c.Room == RoomEnum.Strength));
                                if (nextGym == null)
                                {
                                    nextGym = new GymnastHour { Datetime = tmpTime, Gymnast = u, Gymnast_Id = u.Id, Room = gymNum == 0 ? RoomEnum.FunctionalB : RoomEnum.Strength };
                                    if (tmpTime.Date == Time.Date)
                                    {
                                        h.GymnastFunctionalB = nextGym;
                                    }
                                    BasicDataManager.Add(nextGym);
                                    parent.GymnastsLocal.Add(nextGym);
                                    counter++;
                                    if (counter > 30)
                                    {
                                        await BasicDataManager.SaveAsync();
                                        counter = 0;
                                    }
                                }
                                else
                                {
                                    nextGym.Gymnast = u;
                                    nextGym.Gymnast_Id = u.Id;
                                }
                                tmpTime = tmpTime.AddDays(7);
                            }
                        }
                        if (h.SelectedR || (solo && v == 1))
                        {
                            var limit = forever ? Time.AddMonths(3) : h.Time;
                            var tmpTime = !solo ? h.Time : h.Time.AddDays(7);
                            while (tmpTime <= limit)
                            {
                                var nextGym = NextGymnasts.FirstOrDefault(c => c.Datetime == tmpTime && (c.Room == RoomEnum.Pilates || c.Room == RoomEnum.Personal2));
                                if (nextGym == null)
                                {
                                    nextGym = new GymnastHour { Datetime = tmpTime, Gymnast = u, Gymnast_Id = u.Id, Room = gymNum == 0 ? RoomEnum.Pilates : RoomEnum.Personal2 };
                                    if (tmpTime.Date == Time.Date)
                                    {
                                        h.GymnastReformer = nextGym;
                                    }
                                    BasicDataManager.Add(nextGym);
                                    parent.GymnastsLocal.Add(nextGym);
                                    counter++;
                                    if (counter > 30)
                                    {
                                        await BasicDataManager.SaveAsync();
                                        counter = 0;
                                    }
                                }
                                else
                                {
                                    nextGym.Gymnast = u;
                                    nextGym.Gymnast_Id = u.Id;
                                }
                                tmpTime = tmpTime.AddDays(7);
                            }
                        }
                        if (h.SelectedP || (solo && v == 5))
                        {
                            var limit = forever ? Time.AddMonths(3) : h.Time;
                            var tmpTime = !solo ? h.Time : h.Time.AddDays(7);
                            while (tmpTime <= limit)
                            {
                                var nextGym = NextGymnasts.FirstOrDefault(c => c.Datetime == tmpTime && (c.Room == RoomEnum.Personal || c.Room == RoomEnum.FreeSpace));
                                if (nextGym == null)
                                {
                                    nextGym = new GymnastHour { Datetime = tmpTime, Gymnast = u, Gymnast_Id = u.Id, Room = gymNum == 0 ? RoomEnum.Personal : RoomEnum.FreeSpace };
                                    if (tmpTime.Date == Time.Date)
                                    {
                                        h.GymnastPersonal = nextGym;
                                    }
                                    BasicDataManager.Add(nextGym);
                                    parent.GymnastsLocal.Add(nextGym);
                                    counter++;
                                    if (counter > 30)
                                    {
                                        await BasicDataManager.SaveAsync();
                                        counter = 0;
                                    }
                                }
                                else
                                {
                                    nextGym.Gymnast = u;
                                    nextGym.Gymnast_Id = u.Id;
                                }
                                tmpTime = tmpTime.AddDays(7);
                            }
                        }
                        if (h.SelectedM || (solo && v == 2))
                        {
                            var limit = forever ? Time.AddMonths(3) : h.Time;
                            var tmpTime = !solo ? h.Time : h.Time.AddDays(7);
                            while (tmpTime <= limit)
                            {
                                var nextGym = NextGymnasts.FirstOrDefault(c => c.Datetime == tmpTime && (c.Room == RoomEnum.Massage || c.Room == RoomEnum.Massage2));
                                if (nextGym == null)
                                {
                                    nextGym = new GymnastHour { Datetime = tmpTime, Gymnast = u, Gymnast_Id = u.Id, Room = gymNum == 0 ? RoomEnum.Massage : RoomEnum.Massage2 };
                                    if (tmpTime.Date == Time.Date)
                                    {
                                        h.GymnastMassage = nextGym;
                                    }
                                    BasicDataManager.Add(nextGym);
                                    parent.GymnastsLocal.Add(nextGym);
                                    counter++;
                                    if (counter > 30)
                                    {
                                        await BasicDataManager.SaveAsync();
                                        counter = 0;
                                    }
                                }
                                else
                                {
                                    nextGym.Gymnast = u;
                                    nextGym.Gymnast_Id = u.Id;
                                }
                                tmpTime = tmpTime.AddDays(7);
                            }
                        }
                        if (h.SelectedMH)
                        {
                            var limit = forever ? Time.AddMonths(3) : h.Time;
                            var tmpTime = h.Time;
                            while (tmpTime <= limit)
                            {
                                var nextGym = NextGymnasts.FirstOrDefault(c => c.Datetime == tmpTime && c.Room == RoomEnum.MassageHalf);
                                if (nextGym == null)
                                {
                                    nextGym = new GymnastHour { Datetime = tmpTime, Gymnast = u, Gymnast_Id = u.Id, Room = RoomEnum.MassageHalf };
                                    if (tmpTime.Date == Time.Date)
                                    {
                                        h.GymnastMassageHalf = nextGym;
                                    }
                                    BasicDataManager.Add(nextGym);
                                    parent.GymnastsLocal.Add(nextGym);
                                    counter++;
                                    if (counter > 30)
                                    {
                                        await BasicDataManager.SaveAsync();
                                        counter = 0;
                                    }
                                }
                                else
                                {
                                    nextGym.Gymnast = u;
                                    nextGym.Gymnast_Id = u.Id;
                                }
                                tmpTime = tmpTime.AddDays(7);
                            }
                        }
                        if (h.SelectedO)
                        {
                            var limit = forever ? h.Time.AddMonths(3) : h.Time;
                            var tmpTime = h.Time;
                            while (tmpTime <= limit)
                            {
                                var nextGym = NextGymnasts.FirstOrDefault(c => c.Datetime == tmpTime && c.Room == RoomEnum.Outdoor);
                                if (nextGym == null)
                                {
                                    nextGym = new GymnastHour { Datetime = tmpTime, Gymnast = u, Gymnast_Id = u.Id, Room = RoomEnum.Outdoor };
                                    if (tmpTime.Date == Time.Date)
                                    {
                                        h.GymnastOutdoor = nextGym;
                                    }
                                    BasicDataManager.Add(nextGym);
                                    parent.GymnastsLocal.Add(nextGym);
                                    counter++;
                                    if (counter > 30)
                                    {
                                        await BasicDataManager.SaveAsync();
                                        counter = 0;
                                    }
                                }
                                else
                                {
                                    nextGym.Gymnast = u;
                                    nextGym.Gymnast_Id = u.Id;
                                }
                                tmpTime = tmpTime.AddDays(7);
                            }
                        }
                    }
                }
            }

            BasicDataManager.Add(new ProgramChange
            {
                Date = DateTime.Now,
                InstanceGuid = StaticResources.Guid,
                GymNum = gymNum,
                From = Time,
                To = Time.AddHours(1)
            });
            await BasicDataManager.SaveAsync();
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private static bool ShowMessage(bool forever, GymnastHour gymnastHour, User u)
        {
            if (forever && gymnastHour.Forever && gymnastHour.Gymnast.Id == u.Id)
                return false;
            if (forever)
            {
                MessageBox.Show("Παρακαλώ αφάιρέστε τον υπάρχον γυμναστή πριν προσθέσετε κάποιον για πάντα");
                return false;
            }
            return true;
        }

        internal void DeselectAll()
        {
            SelectedF = SelectedFB = SelectedM = SelectedMH = SelectedP = SelectedO = SelectedR = false;
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
                case "10":
                    return SelectedApointmentFunctional != null;

                case "6":
                case "16":
                    return SelectedApointmentFB != null;

                case "1":
                case "11":
                    return SelectedApointmentReformer != null;

                case "2":
                case "12":
                    return SelectedAppointmentMassage != null;

                case "5":
                case "15":
                    return SelectedAppointmentPersonal != null;

                case "3":
                    return SelectedAppointmentOutdoor != null;

                case "4":
                    return SelectedAppointmentMassageHalf != null;
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
                case "10":
                    if (MessageBox.Show($"Θέλετε σίγουρα να ΔΙΑΓΡΑΨΕΤΕ το επιλεγμένο ραντεβού? {SelectedApointmentFunctional.Description}", "Προσοχή", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        BasicDataManager.Add(new ProgramChange
                        {
                            Date = DateTime.Now,
                            InstanceGuid = StaticResources.Guid,
                            GymNum = gymNum,
                            From = SelectedApointmentFunctional.DateTime,
                            To = SelectedApointmentFunctional.DateTime.AddHours(1)
                        });
                        BasicDataManager.Delete(SelectedApointmentFunctional);
                        AppointmentsFunctional.Remove(SelectedApointmentFunctional);
                    }
                    break;

                case "6":
                case "16":
                    if (MessageBox.Show($"Θέλετε σίγουρα να ΔΙΑΓΡΑΨΕΤΕ το επιλεγμένο ραντεβού? {SelectedApointmentFB.Description}", "Προσοχή", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        BasicDataManager.Add(new ProgramChange
                        {
                            Date = DateTime.Now,
                            InstanceGuid = StaticResources.Guid,
                            GymNum = gymNum,
                            From = SelectedApointmentFB.DateTime,
                            To = SelectedApointmentFB.DateTime.AddHours(1)
                        });
                        BasicDataManager.Delete(SelectedApointmentFB);
                        AppointmentsFB.Remove(SelectedApointmentFB);
                    }
                    break;

                case "1":
                case "11":
                    if (MessageBox.Show($"Θέλετε σίγουρα να ΔΙΑΓΡΑΨΕΤΕ το επιλεγμένο ραντεβού? {SelectedApointmentReformer.Description}", "Προσοχή", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        BasicDataManager.Add(new ProgramChange
                        {
                            Date = DateTime.Now,
                            InstanceGuid = StaticResources.Guid,
                            GymNum = gymNum,
                            From = SelectedApointmentReformer.DateTime,
                            To = SelectedApointmentReformer.DateTime.AddHours(1)
                        });
                        BasicDataManager.Delete(SelectedApointmentReformer);
                        AppointmentsReformer.Remove(SelectedApointmentReformer);
                    }
                    break;

                case "2":
                case "12":
                    if (MessageBox.Show($"Θέλετε σίγουρα να ΔΙΑΓΡΑΨΕΤΕ το επιλεγμένο ραντεβού? {SelectedAppointmentMassage.Description}", "Προσοχή", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        BasicDataManager.Add(new ProgramChange
                        {
                            Date = DateTime.Now,
                            GymNum = gymNum,
                            InstanceGuid = StaticResources.Guid,
                            From = SelectedAppointmentMassage.DateTime,
                            To = SelectedAppointmentMassage.DateTime.AddHours(1)
                        });
                        BasicDataManager.Delete(SelectedAppointmentMassage);
                        AppointmentsMassage.Remove(SelectedAppointmentMassage);
                    }
                    break;

                case "5":
                case "15":
                    if (MessageBox.Show($"Θέλετε σίγουρα να ΔΙΑΓΡΑΨΕΤΕ το επιλεγμένο ραντεβού? {SelectedAppointmentPersonal.Description}", "Προσοχή", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        BasicDataManager.Add(new ProgramChange
                        {
                            Date = DateTime.Now,
                            InstanceGuid = StaticResources.Guid,
                            GymNum = gymNum,
                            From = SelectedAppointmentPersonal.DateTime,
                            To = SelectedAppointmentPersonal.DateTime.AddHours(1)
                        });
                        BasicDataManager.Delete(SelectedAppointmentPersonal);
                        AppointmentsPersonal.Remove(SelectedAppointmentPersonal);
                    }
                    break;

                case "3":
                    if (MessageBox.Show($"Θέλετε σίγουρα να ΔΙΑΓΡΑΨΕΤΕ το επιλεγμένο ραντεβού? {SelectedAppointmentOutdoor.Description}", "Προσοχή", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        BasicDataManager.Add(new ProgramChange
                        {
                            Date = DateTime.Now,
                            InstanceGuid = StaticResources.Guid,
                            GymNum = gymNum,
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
                            GymNum = gymNum,
                            From = SelectedAppointmentMassageHalf.DateTime,
                            To = SelectedAppointmentMassageHalf.DateTime.AddHours(1)
                        });
                        BasicDataManager.Delete(SelectedAppointmentMassageHalf);
                        AppointmentsMassageHalf.Remove(SelectedAppointmentMassageHalf);
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
                List<ClosedHour> ClosedHours = new List<ClosedHour>();
                List<Hour> t = new List<Hour>();
                if (parent != null)
                {
                    t = parent.Days.FirstOrDefault(d => d.Date.DayOfYear == Time.DayOfYear).Hours.Where(h => (h.SelectedF || h.SelectedFB || h.SelectedR || h.SelectedM || h.SelectedMH || h.SelectedP || h.SelectedO)).ToList();
                    foreach (var h in t)
                    {
                        if (h.SelectedF)
                        {
                            ClosedHours.AddRange(await BasicDataManager.Context.GetAllClosedHoursAsync(gymNum == 0 ? RoomEnum.Functional : RoomEnum.Fitness, h.Time, null));
                        }
                        if (h.SelectedFB)
                        {
                            ClosedHours.AddRange(await BasicDataManager.Context.GetAllClosedHoursAsync(gymNum == 0 ? RoomEnum.FunctionalB : RoomEnum.Strength, h.Time, null));
                        }
                        if (h.SelectedR)
                        {
                            ClosedHours.AddRange(await BasicDataManager.Context.GetAllClosedHoursAsync(gymNum == 0 ? RoomEnum.Pilates : RoomEnum.Personal2, h.Time, null));
                        }
                        if (h.SelectedM)
                        {
                            ClosedHours.AddRange(await BasicDataManager.Context.GetAllClosedHoursAsync(gymNum == 0 ? RoomEnum.Massage : RoomEnum.Massage2, h.Time, null));
                        }
                        if (h.SelectedP)
                        {
                            ClosedHours.AddRange(await BasicDataManager.Context.GetAllClosedHoursAsync(gymNum == 0 ? RoomEnum.Personal : RoomEnum.FreeSpace, h.Time, null));
                        }
                        if (h.SelectedMH)
                        {
                            ClosedHours.AddRange(await BasicDataManager.Context.GetAllClosedHoursAsync(RoomEnum.MassageHalf, h.Time, null));
                        }
                        if (h.SelectedO)
                        {
                            ClosedHours.AddRange(await BasicDataManager.Context.GetAllClosedHoursAsync(RoomEnum.Outdoor, h.Time, null));
                        }
                    }
                }
                else
                {
                    ClosedHours = await BasicDataManager.Context.GetAllClosedHoursAsync(room, Time, null);
                }

                foreach (var item in ClosedHours)
                {
                    BasicDataManager.Delete(item);
                    var h = t.FirstOrDefault(hr => hr.Time.TimeOfDay == item.Date.TimeOfDay);
                    if (h != null)
                    {
                        if (h.SelectedF)
                        {
                            if (h.ClosedHour0 != null)
                            {
                                h.ClosedHour0 = null;
                            }

                            h.RaisePropertyChanged(nameof(ClosedColor0));
                        }
                        if (h.SelectedFB)
                        {
                            if (h.ClosedHourFB != null)
                            {
                                h.ClosedHourFB = null;
                            }

                            h.RaisePropertyChanged(nameof(ClosedColorFB));
                        }
                        if (h.SelectedR)
                        {
                            if (h.ClosedHour1 != null)
                            {
                                h.ClosedHour1 = null;
                            }

                            h.RaisePropertyChanged(nameof(ClosedColor1));
                        }
                        if (h.SelectedM)
                        {
                            if (h.ClosedHourMassage != null)
                            {
                                h.ClosedHourMassage = null;
                            }

                            h.RaisePropertyChanged(nameof(ClosedColorMassage));
                        }
                        if (h.SelectedMH)
                        {
                            if (h.ClosedHourMassageHalf != null)
                            {
                                h.ClosedHourMassageHalf = null;
                            }

                            h.RaisePropertyChanged(nameof(ClosedColorMassageHalf));
                        }
                        if (h.SelectedP)
                        {
                            if (h.ClosedHourPersonal != null)
                            {
                                h.ClosedHourPersonal = null;
                            }

                            h.RaisePropertyChanged(nameof(ClosedColorPersonal));
                        }
                        if (h.SelectedO)
                        {
                            if (h.ClosedHourOutdoor != null)
                            {
                                h.ClosedHourOutdoor = null;
                            }

                            h.RaisePropertyChanged(nameof(ClosedColorOutdoor));
                        }
                    }
                }

                if (ClosedHours.Any())
                {
                    BasicDataManager.Add(new ProgramChange
                    {
                        Date = DateTime.Now,
                        InstanceGuid = StaticResources.Guid,
                        GymNum = gymNum,
                        From = ClosedHours.Min(d => d.Date),
                        To = ClosedHours.Max(d => d.Date)
                    });
                }

                await BasicDataManager.SaveAsync();

                ClosedHour0 = ClosedHourFB = ClosedHour1 = ClosedHourMassage = ClosedHourMassageHalf = ClosedHourPersonal = ClosedHourOutdoor = null;
                Messenger.Default.Send(new UpdateClosedHoursMessage());
            }
            catch (Exception ex)
            {
                BasicDataManager.CurrentMessenger.Send(new ShowExceptionMessage_Message("Σφάλμα κατα το πρώτο στάδιο της ενεργοποίησης" + ex.Message));
            }
            Mouse.OverrideCursor = Cursors.Arrow;

            RaisePropertyChanged(nameof(ClosedColor1));
            RaisePropertyChanged(nameof(ClosedColorFB));
            RaisePropertyChanged(nameof(ClosedColor0));
            RaisePropertyChanged(nameof(ClosedColorMassage));
            RaisePropertyChanged(nameof(ClosedColorMassageHalf));
            RaisePropertyChanged(nameof(ClosedColorPersonal));
            RaisePropertyChanged(nameof(ClosedColorOutdoor));
        }

        private SolidColorBrush GetClosedColor(int v)
        {
            if (v == 0 || v == 10)
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
            else if (v == 6 || v == 16)
            {
                if (ClosedHourFB != null)
                {
                    return new SolidColorBrush(Colors.Red);
                }
                else
                {
                    return new SolidColorBrush(Colors.LightYellow);
                }
            }
            else if (v == 1 || v == 11)
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
            else if (v == 2 || v == 12)
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
            else if (v == 5 || v == 15)
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

            return new SolidColorBrush(Colors.BlanchedAlmond);
        }

        private async Task NoGymnast(int obj)
        {
            //kai edw lathos mallon
            Mouse.OverrideCursor = Cursors.Wait;
            var forevers = parent.GymnastsLocal.Where(te => te?.Forever == true).ToList();
            switch (obj)
            {
                case 0:
                case 10:
                    if (GymnastFunctional != null)
                    {
                        if (parent != null && GymnastFunctional.Forever)
                        {
                            forevers.Remove(GymnastFunctional);
                            parent?.GymnastsLocal.Remove(GymnastFunctional);
                            GymnastFunctional.Forever = false;
                        }
                        BasicDataManager.Delete(GymnastFunctional);
                        GymnastFunctional = forevers.FirstOrDefault(g => g.Datetime.DayOfWeek == Time.DayOfWeek && g.Datetime.Hour == Time.Hour && g.Datetime.Minute == Time.Minute && (g.Room == RoomEnum.Functional || g.Room == RoomEnum.Fitness));
                    }
                    break;

                case 6:
                case 16:
                    if (GymnastFunctionalB != null)
                    {
                        if (parent != null && GymnastFunctionalB.Forever)
                        {
                            forevers.Remove(GymnastFunctionalB);
                            parent?.GymnastsLocal.Remove(GymnastFunctionalB);
                            GymnastFunctionalB.Forever = false;
                        }
                        BasicDataManager.Delete(GymnastFunctionalB);
                        GymnastFunctionalB = forevers.FirstOrDefault(g => g.Datetime.DayOfWeek == Time.DayOfWeek && g.Datetime.Hour == Time.Hour && g.Datetime.Minute == Time.Minute && (g.Room == RoomEnum.FunctionalB || g.Room == RoomEnum.Strength));
                    }
                    break;

                case 1:
                case 11:
                    if (GymnastReformer != null)
                    {
                        if (parent != null && GymnastReformer.Forever)
                        {
                            forevers.Remove(GymnastReformer);
                            parent?.GymnastsLocal.Remove(GymnastReformer);
                            GymnastReformer.Forever = false;
                        }
                        BasicDataManager.Delete(GymnastReformer);
                        GymnastReformer = forevers.FirstOrDefault(g => g.Datetime.DayOfWeek == Time.DayOfWeek && g.Datetime.Hour == Time.Hour && g.Datetime.Minute == Time.Minute && (g.Room == RoomEnum.Pilates || g.Room == RoomEnum.Personal2));
                    }
                    break;

                case 2:
                case 12:
                    if (GymnastMassage != null)
                    {
                        if (parent != null && GymnastMassage.Forever)
                        {
                            forevers.Remove(GymnastMassage);
                            parent?.GymnastsLocal.Remove(GymnastMassage);
                            GymnastMassage.Forever = false;
                        }
                        BasicDataManager.Delete(GymnastMassage);
                        GymnastMassage = forevers.FirstOrDefault(g => g.Datetime.DayOfWeek == Time.DayOfWeek && g.Datetime.Hour == Time.Hour && g.Datetime.Minute == Time.Minute && (g.Room == RoomEnum.Massage || g.Room == RoomEnum.Massage2));
                    }
                    break;

                case 5:
                case 15:
                    if (GymnastPersonal != null)
                    {
                        if (parent != null && GymnastPersonal.Forever)
                        {
                            forevers.Remove(GymnastPersonal);
                            parent?.GymnastsLocal.Remove(GymnastPersonal);
                            GymnastPersonal.Forever = false;
                        }
                        BasicDataManager.Delete(GymnastPersonal);
                        GymnastPersonal = forevers.FirstOrDefault(g => g.Datetime.DayOfWeek == Time.DayOfWeek && g.Datetime.Hour == Time.Hour && g.Datetime.Minute == Time.Minute && (g.Room == RoomEnum.Personal || g.Room == RoomEnum.FreeSpace));
                    }
                    break;

                case 3:
                    if (GymnastOutdoor != null)
                    {
                        if (parent != null && GymnastOutdoor.Forever)
                        {
                            forevers.Remove(GymnastOutdoor);
                            parent?.GymnastsLocal.Remove(GymnastOutdoor);
                            GymnastOutdoor.Forever = false;
                        }
                        BasicDataManager.Delete(GymnastOutdoor);
                        GymnastOutdoor = forevers.FirstOrDefault(g => g.Datetime.DayOfWeek == Time.DayOfWeek && g.Datetime.Hour == Time.Hour && g.Datetime.Minute == Time.Minute && g.Room == RoomEnum.Outdoor);
                    }
                    break;

                case 4:
                    if (GymnastMassageHalf != null)
                    {
                        if (parent != null && GymnastMassageHalf.Forever)
                        {
                            forevers.Remove(GymnastMassageHalf);
                            parent?.GymnastsLocal.Remove(GymnastMassageHalf);
                            GymnastMassageHalf.Forever = false;
                        }
                        BasicDataManager.Delete(GymnastMassageHalf);
                        GymnastMassageHalf = forevers.FirstOrDefault(g => g.Datetime.DayOfWeek == Time.DayOfWeek && g.Datetime.Hour == Time.Hour && g.Datetime.Minute == Time.Minute && g.Room == RoomEnum.MassageHalf);
                    }
                    break;
            }
            if (parent != null)
            {
                //den epilego swsta ton gia panta
                var t = parent.Days.FirstOrDefault(d => d.Date.DayOfYear == Time.DayOfYear).Hours.Where(h => (h.SelectedF || h.SelectedFB || h.SelectedR || h.SelectedM || h.SelectedMH || h.SelectedP || h.SelectedO));
                foreach (var h in t)
                {
                    if (h.SelectedF && !(h == this && (obj == 0 || obj == 10)))
                    {
                        if (h.GymnastFunctional != null)
                        {
                            if (h.GymnastFunctional.Id > 0)
                            {
                                if (parent != null && h.GymnastFunctional.Forever)
                                    parent?.GymnastsLocal.Remove(h.GymnastFunctional);
                                BasicDataManager.Delete(h.GymnastFunctional);
                            }
                            h.GymnastFunctional = forevers.FirstOrDefault(g => g.Datetime.DayOfWeek == h.Time.DayOfWeek && g.Datetime.Hour == h.Time.Hour && g.Datetime.Minute == h.Time.Minute && (g.Room == RoomEnum.Functional || g.Room == RoomEnum.Fitness));
                        }
                    }
                    if (h.SelectedFB && !(h == this && (obj == 6 || obj == 16)))
                    {
                        if (h.GymnastFunctionalB != null)
                        {
                            if (h.GymnastFunctionalB.Id > 0)
                            {
                                if (parent != null && h.GymnastFunctionalB.Forever)
                                    parent?.GymnastsLocal.Remove(h.GymnastFunctionalB);
                                BasicDataManager.Delete(h.GymnastFunctionalB);
                            }
                            h.GymnastFunctionalB = forevers.FirstOrDefault(g => g.Datetime.DayOfWeek == h.Time.DayOfWeek && g.Datetime.Hour == h.Time.Hour && g.Datetime.Minute == h.Time.Minute && (g.Room == RoomEnum.FunctionalB || g.Room == RoomEnum.Strength));
                        }
                    }
                    if (h.SelectedR && !(h == this && (obj == 1 || obj == 11)))
                    {
                        if (h.GymnastReformer != null)
                        {
                            if (h.GymnastReformer.Id > 0)
                            {
                                if (parent != null && h.GymnastReformer.Forever)
                                    parent?.GymnastsLocal.Remove(h.GymnastReformer);
                                BasicDataManager.Delete(h.GymnastReformer);
                            }
                            h.GymnastReformer = forevers.FirstOrDefault(g => g.Datetime.DayOfWeek == h.Time.DayOfWeek && g.Datetime.Hour == h.Time.Hour && g.Datetime.Minute == h.Time.Minute && (g.Room == RoomEnum.Pilates || g.Room == RoomEnum.Personal2));
                        }
                    }
                    if (h.SelectedP && !(h == this && (obj == 5 || obj == 15)))
                    {
                        if (h.GymnastPersonal != null)
                        {
                            if (h.GymnastPersonal.Id > 0)
                            {
                                if (parent != null && h.GymnastPersonal.Forever)
                                    parent?.GymnastsLocal.Remove(h.GymnastPersonal);
                                BasicDataManager.Delete(h.GymnastPersonal);
                            }
                            h.GymnastPersonal = forevers.FirstOrDefault(g => g.Datetime.DayOfWeek == h.Time.DayOfWeek && g.Datetime.Hour == h.Time.Hour && g.Datetime.Minute == h.Time.Minute && (g.Room == RoomEnum.Personal || g.Room == RoomEnum.FreeSpace));
                        }
                    }
                    if (h.SelectedM && !(h == this && (obj == 2 || obj == 12)))
                    {
                        if (h.GymnastMassage != null)
                        {
                            if (h.GymnastMassage.Id > 0)
                            {
                                if (parent != null && h.GymnastMassage.Forever)
                                    parent?.GymnastsLocal.Remove(h.GymnastMassage);
                                BasicDataManager.Delete(h.GymnastMassage);
                            }
                            h.GymnastMassage = forevers.FirstOrDefault(g => g.Datetime.DayOfWeek == h.Time.DayOfWeek && g.Datetime.Hour == h.Time.Hour && g.Datetime.Minute == h.Time.Minute && (g.Room == RoomEnum.Massage || g.Room == RoomEnum.Massage2));
                        }
                    }

                    if (h.SelectedMH && !(h == this && obj == 4))
                    {
                        if (h.GymnastMassageHalf != null)
                        {
                            if (h.GymnastMassageHalf.Id > 0)
                            {
                                if (parent != null && h.GymnastMassageHalf.Forever)
                                    parent?.GymnastsLocal.Remove(h.GymnastMassageHalf);
                                BasicDataManager.Delete(h.GymnastMassageHalf);
                            }
                            h.GymnastMassageHalf = forevers.FirstOrDefault(g => g.Datetime.DayOfWeek == h.Time.DayOfWeek && g.Datetime.Hour == h.Time.Hour && g.Datetime.Minute == h.Time.Minute && g.Room == RoomEnum.MassageHalf);
                        }
                    }

                    if (h.SelectedO && !(h == this && obj == 3))
                    {
                        if (h.GymnastOutdoor != null)
                        {
                            if (h.GymnastOutdoor.Id > 0)
                            {
                                if (parent != null && h.GymnastOutdoor.Forever)
                                    parent?.GymnastsLocal.Remove(h.GymnastOutdoor);
                                BasicDataManager.Delete(h.GymnastOutdoor);
                            }
                            h.GymnastOutdoor = forevers.FirstOrDefault(g => g.Datetime.DayOfWeek == h.Time.DayOfWeek && g.Datetime.Hour == h.Time.Hour && g.Datetime.Minute == h.Time.Minute && g.Room == RoomEnum.Outdoor);
                        }
                    }
                }
            }

            BasicDataManager.Add(new ProgramChange
            {
                Date = DateTime.Now,
                InstanceGuid = StaticResources.Guid,
                GymNum = gymNum,
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
                case "10":
                    SelectedApointmentFunctional.Canceled = !SelectedApointmentFunctional.Canceled;
                    SelectedApointmentFunctional.RaisePropertyChanged(nameof(Apointment.ApColor));
                    TryCancelForUser(SelectedApointmentFunctional, SelectedApointmentFunctional.Canceled);
                    break;

                case "6":
                case "16":
                    SelectedApointmentFB.Canceled = !SelectedApointmentFB.Canceled;
                    SelectedApointmentFB.RaisePropertyChanged(nameof(Apointment.ApColor));
                    TryCancelForUser(SelectedApointmentFB, SelectedApointmentFB.Canceled);
                    break;

                case "1":
                case "11":
                    SelectedApointmentReformer.Canceled = !SelectedApointmentReformer.Canceled;
                    SelectedApointmentReformer.RaisePropertyChanged(nameof(Apointment.ApColor));
                    TryCancelForUser(SelectedApointmentReformer, SelectedApointmentReformer.Canceled);

                    break;

                case "2":
                case "12":
                    SelectedAppointmentMassage.Canceled = !SelectedAppointmentMassage.Canceled;
                    SelectedAppointmentMassage.RaisePropertyChanged(nameof(Apointment.ApColor));
                    TryCancelForUser(SelectedAppointmentMassage, SelectedAppointmentMassage.Canceled);

                    break;

                case "5":
                case "15":
                    SelectedAppointmentPersonal.Canceled = !SelectedAppointmentPersonal.Canceled;
                    SelectedAppointmentPersonal.RaisePropertyChanged(nameof(Apointment.ApColor));
                    TryCancelForUser(SelectedAppointmentPersonal, SelectedAppointmentPersonal.Canceled);

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
            }

            await BasicDataManager.SaveAsync();
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private async Task ToggleEnabled(RoomEnum room)
        {
            bool disable = false;
            if (room == RoomEnum.Functional || room == RoomEnum.Fitness)
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
            else if (room == RoomEnum.FunctionalB || room == RoomEnum.Strength)
            {
                if (ClosedHourFB != null)
                {
                    disable = false;
                    BasicDataManager.Context.Delete(ClosedHourFB);
                    ClosedHourFB = null;
                }
                else
                {
                    disable = true;
                    ClosedHourFB = new ClosedHour { Date = Time, Room = room };
                    BasicDataManager.Add(ClosedHourFB);
                }
                RaisePropertyChanged(nameof(ClosedColorFB));
            }
            else if (room == RoomEnum.Pilates || room == RoomEnum.Personal2)
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
            else if (room == RoomEnum.Personal || room == RoomEnum.FreeSpace)
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
            else if (room == RoomEnum.Massage || room == RoomEnum.Massage2)
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
                var t = parent.Days.FirstOrDefault(d => d.Date.DayOfYear == Time.DayOfYear).Hours.Where(h => (h.SelectedF || h.SelectedFB || h.SelectedR || h.SelectedM || h.SelectedMH || h.SelectedP || h.SelectedO));
                foreach (var h in t)
                {
                    if (h.SelectedF && !(h == this && (room == RoomEnum.Functional || room == RoomEnum.Fitness)))
                    {
                        if (h.ClosedHour0 != null && !disable)
                        {
                            BasicDataManager.Context.Delete(h.ClosedHour0);
                            h.ClosedHour0 = null;
                        }
                        else if (h.ClosedHour0 == null && disable)
                        {
                            h.ClosedHour0 = new ClosedHour { Date = h.Time, Room = gymNum == 0 ? RoomEnum.Functional : RoomEnum.Fitness };
                            BasicDataManager.Add(h.ClosedHour0);
                        }
                        h.RaisePropertyChanged(nameof(ClosedColor0));
                    }
                    if (h.SelectedFB && !(h == this && (room == RoomEnum.FunctionalB || room == RoomEnum.Strength)))
                    {
                        if (h.ClosedHourFB != null && !disable)
                        {
                            BasicDataManager.Context.Delete(h.ClosedHourFB);
                            h.ClosedHourFB = null;
                        }
                        else if (h.ClosedHourFB == null && disable)
                        {
                            h.ClosedHourFB = new ClosedHour { Date = h.Time, Room = gymNum == 0 ? RoomEnum.FunctionalB : RoomEnum.Strength };
                            BasicDataManager.Add(h.ClosedHourFB);
                        }
                        h.RaisePropertyChanged(nameof(ClosedColorFB));
                    }
                    if (h.SelectedR && !(h == this && (room == RoomEnum.Pilates || room == RoomEnum.Personal2)))
                    {
                        if (h.ClosedHour1 != null && !disable)
                        {
                            BasicDataManager.Context.Delete(h.ClosedHour1);
                            h.ClosedHour1 = null;
                        }
                        else if (h.ClosedHour1 == null && disable)
                        {
                            h.ClosedHour1 = new ClosedHour { Date = h.Time, Room = gymNum == 0 ? RoomEnum.Pilates : RoomEnum.Personal2 };
                            BasicDataManager.Add(h.ClosedHour1);
                        }
                        h.RaisePropertyChanged(nameof(ClosedColor1));
                    }
                    if (h.SelectedP && !(h == this && (room == RoomEnum.Personal || room == RoomEnum.FreeSpace)))
                    {
                        if (h.ClosedHourPersonal != null && !disable)
                        {
                            BasicDataManager.Context.Delete(h.ClosedHourPersonal);
                            h.ClosedHourPersonal = null;
                        }
                        else if (h.ClosedHourPersonal == null && disable)
                        {
                            h.ClosedHourPersonal = new ClosedHour { Date = h.Time, Room = gymNum == 0 ? RoomEnum.Personal : RoomEnum.FreeSpace };
                            BasicDataManager.Add(h.ClosedHourPersonal);
                        }
                        h.RaisePropertyChanged(nameof(ClosedColorPersonal));
                    }
                    if (h.SelectedM && !(h == this && (room == RoomEnum.Massage || room == RoomEnum.Massage2)))
                    {
                        if (h.ClosedHourMassage != null && !disable)
                        {
                            BasicDataManager.Context.Delete(h.ClosedHourMassage);
                            h.ClosedHourMassage = null;
                        }
                        else if (h.ClosedHourMassage == null && disable)
                        {
                            h.ClosedHourMassage = new ClosedHour { Date = h.Time, Room = gymNum == 0 ? RoomEnum.Massage : RoomEnum.Massage2 };
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
                GymNum = gymNum,
                InstanceGuid = StaticResources.Guid,
                From = Time,
                To = Time.AddHours(1)
            });
            await BasicDataManager.SaveAsync();
        }

        private async Task ToggleEnabledForEver(RoomEnum room)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            List<Hour> selectedHours = null;
            if (parent != null)
            {
                selectedHours = parent.Days.FirstOrDefault(d => d.Date.DayOfYear == Time.DayOfYear).Hours
                    .Where(h => (h.SelectedF || h.SelectedFB || h.SelectedR || h.SelectedM || h.SelectedMH || h.SelectedP || h.SelectedO)).ToList();
                //    if (t.Any())
                //    {
                //        MessageBox.Show("Προς το παρόν δεν μπορείτε να (απ)ενεργοποιήσετε για 3 μήνες πολλά κουτάκια ταυτόχρονα.");
                //        Mouse.OverrideCursor = Cursors.Arrow;
                //        return;
                //    }
            }
            List<ClosedHour> ClosedHours = await BasicDataManager.Context.GetAllClosedHoursAsync(room, Time, selectedHours);
            if (selectedHours == null)
            {
                selectedHours = new List<Hour>();
            }
            if (!selectedHours.Any())
            {
                selectedHours.Add(this);
            }
            DateTime limit = Time.AddMonths(3);
            DateTime tmpTime = Time;
            int counter = 0;
            foreach (var h in selectedHours)
            {
                if (h.SelectedF)
                {
                    limit = h.Time.AddMonths(3);
                    tmpTime = h.Time;
                    while (tmpTime < limit)
                    {
                        if (!ClosedHours.Any(c => c.Date == tmpTime && (c.Room == RoomEnum.Functional || c.Room == RoomEnum.Fitness)))
                        {
                            BasicDataManager.Add(new ClosedHour { Date = tmpTime, Room = gymNum == 0 ? RoomEnum.Functional : RoomEnum.Fitness });
                            counter++;
                            if (counter > 30)
                            {
                                await BasicDataManager.SaveAsync();
                                counter = 0;
                            }
                        }
                        tmpTime = tmpTime.AddDays(7);
                    }
                }
                if (h.SelectedFB)
                {
                    limit = h.Time.AddMonths(3);
                    tmpTime = h.Time;
                    while (tmpTime <= limit)
                    {
                        if (!ClosedHours.Any(c => c.Date == tmpTime && (c.Room == RoomEnum.FunctionalB || c.Room == RoomEnum.Strength)))
                        {
                            BasicDataManager.Add(new ClosedHour { Date = tmpTime, Room = gymNum == 0 ? RoomEnum.FunctionalB : RoomEnum.Strength });
                            counter++;
                            if (counter > 30)
                            {
                                await BasicDataManager.SaveAsync();
                                counter = 0;
                            }
                        }
                        tmpTime = tmpTime.AddDays(7);
                    }
                }
                if (h.SelectedR)
                {
                    limit = h.Time.AddMonths(3);
                    tmpTime = h.Time;
                    while (tmpTime <= limit)
                    {
                        if (!ClosedHours.Any(c => c.Date == tmpTime && (c.Room == RoomEnum.Pilates || c.Room == RoomEnum.Personal2)))
                        {
                            BasicDataManager.Add(new ClosedHour { Date = tmpTime, Room = gymNum == 0 ? RoomEnum.Pilates : RoomEnum.Personal2 });
                            counter++;
                            if (counter > 30)
                            {
                                await BasicDataManager.SaveAsync();
                                counter = 0;
                            }
                        }
                        tmpTime = tmpTime.AddDays(7);
                    }
                }
                if (h.SelectedP)
                {
                    limit = h.Time.AddMonths(3);
                    tmpTime = h.Time;
                    while (tmpTime <= limit)
                    {
                        if (!ClosedHours.Any(c => c.Date == tmpTime && (c.Room == RoomEnum.Personal || c.Room == RoomEnum.FreeSpace)))
                        {
                            BasicDataManager.Add(new ClosedHour { Date = tmpTime, Room = gymNum == 0 ? RoomEnum.Personal : RoomEnum.FreeSpace });
                            counter++;
                            if (counter > 30)
                            {
                                await BasicDataManager.SaveAsync();
                                counter = 0;
                            }
                        }
                        tmpTime = tmpTime.AddDays(7);
                    }
                }
                if (h.SelectedM)
                {
                    limit = h.Time.AddMonths(3);
                    tmpTime = h.Time;
                    while (tmpTime <= limit)
                    {
                        if (!ClosedHours.Any(c => c.Date == tmpTime && (c.Room == RoomEnum.Massage | c.Room == RoomEnum.Massage2)))
                        {
                            BasicDataManager.Add(new ClosedHour { Date = tmpTime, Room = gymNum == 0 ? RoomEnum.Massage : RoomEnum.Massage2 });
                            counter++;
                            if (counter > 30)
                            {
                                await BasicDataManager.SaveAsync();
                                counter = 0;
                            }
                        }
                        tmpTime = tmpTime.AddDays(7);
                    }
                }
                if (h.SelectedMH)
                {
                    limit = h.Time.AddMonths(3);
                    tmpTime = h.Time;
                    while (tmpTime <= limit)
                    {
                        if (!ClosedHours.Any(c => c.Date == tmpTime && c.Room == RoomEnum.MassageHalf))
                        {
                            BasicDataManager.Add(new ClosedHour { Date = tmpTime, Room = RoomEnum.MassageHalf });
                            counter++;
                            if (counter > 30)
                            {
                                await BasicDataManager.SaveAsync();
                                counter = 0;
                            }
                        }
                        tmpTime = tmpTime.AddDays(7);
                    }
                }
                if (h.SelectedO)
                {
                    limit = h.Time.AddMonths(3);
                    tmpTime = h.Time;
                    while (tmpTime <= limit)
                    {
                        if (!ClosedHours.Any(c => c.Date == tmpTime && c.Room == RoomEnum.Outdoor))
                        {
                            BasicDataManager.Add(new ClosedHour { Date = tmpTime, Room = RoomEnum.Outdoor });
                            counter++;
                            if (counter > 30)
                            {
                                await BasicDataManager.SaveAsync();
                                counter = 0;
                            }
                        }
                        tmpTime = tmpTime.AddDays(7);
                    }
                }
            }

            BasicDataManager.Add(new ProgramChange
            {
                Date = DateTime.Now,
                InstanceGuid = StaticResources.Guid,
                GymNum = gymNum,
                From = Time,
                To = limit
            });

            await BasicDataManager.SaveAsync();
            Messenger.Default.Send(new UpdateClosedHoursMessage());
            Mouse.OverrideCursor = Cursors.Arrow;
            RaisePropertyChanged(nameof(ClosedColor1));
            RaisePropertyChanged(nameof(ClosedColorFB));
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
                case "10":
                    SelectedApointmentFunctional.Waiting = !SelectedApointmentFunctional.Waiting;
                    SelectedApointmentFunctional.RaisePropertyChanged(nameof(Apointment.ApColor));
                    BasicDataManager.Add(new ProgramChange
                    {
                        Date = DateTime.Now,
                        InstanceGuid = StaticResources.Guid,
                        GymNum = gymNum,
                        From = SelectedApointmentFunctional.DateTime,
                        To = SelectedApointmentFunctional.DateTime.AddHours(1)
                    });
                    break;

                case "6":
                case "16":
                    SelectedApointmentFB.Waiting = !SelectedApointmentFB.Waiting;
                    SelectedApointmentFB.RaisePropertyChanged(nameof(Apointment.ApColor));
                    BasicDataManager.Add(new ProgramChange
                    {
                        Date = DateTime.Now,
                        InstanceGuid = StaticResources.Guid,
                        GymNum = gymNum,
                        From = SelectedApointmentFB.DateTime,
                        To = SelectedApointmentFB.DateTime.AddHours(1)
                    });
                    break;

                case "1":
                case "11":
                    SelectedApointmentReformer.Waiting = !SelectedApointmentReformer.Waiting;
                    SelectedApointmentReformer.RaisePropertyChanged(nameof(Apointment.ApColor));
                    BasicDataManager.Add(new ProgramChange
                    {
                        Date = DateTime.Now,
                        InstanceGuid = StaticResources.Guid,
                        GymNum = gymNum,
                        From = SelectedApointmentReformer.DateTime,
                        To = SelectedApointmentReformer.DateTime.AddHours(1)
                    });
                    break;

                case "2":
                case "12":
                    SelectedAppointmentMassage.Waiting = !SelectedAppointmentMassage.Waiting;
                    SelectedAppointmentMassage.RaisePropertyChanged(nameof(Apointment.ApColor));
                    BasicDataManager.Add(new ProgramChange
                    {
                        Date = DateTime.Now,
                        InstanceGuid = StaticResources.Guid,
                        GymNum = gymNum,
                        From = SelectedAppointmentMassage.DateTime,
                        To = SelectedAppointmentMassage.DateTime.AddHours(1)
                    });
                    break;

                case "5":
                case "15":
                    SelectedAppointmentPersonal.Waiting = !SelectedAppointmentPersonal.Waiting;
                    SelectedAppointmentPersonal.RaisePropertyChanged(nameof(Apointment.ApColor));
                    BasicDataManager.Add(new ProgramChange
                    {
                        Date = DateTime.Now,
                        InstanceGuid = StaticResources.Guid,
                        GymNum = gymNum,
                        From = SelectedAppointmentPersonal.DateTime,
                        To = SelectedAppointmentPersonal.DateTime.AddHours(1)
                    });
                    break;

                case "3":
                    SelectedAppointmentOutdoor.Waiting = !SelectedAppointmentOutdoor.Waiting;
                    SelectedAppointmentOutdoor.RaisePropertyChanged(nameof(Apointment.ApColor));
                    BasicDataManager.Add(new ProgramChange
                    {
                        Date = DateTime.Now,
                        InstanceGuid = StaticResources.Guid,
                        GymNum = gymNum,
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
                        GymNum = gymNum,
                        From = SelectedAppointmentMassageHalf.DateTime,
                        To = SelectedAppointmentMassageHalf.DateTime.AddHours(1)
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