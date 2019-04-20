using BubbleStart.Database;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace BubbleStart.Model
{
    [Table("BubbleCustomers")]
    public class Customer : BaseModel
    {
        #region Constructors

        public Customer()
        {
            FirstDate = DateTime.Today;
            Illness = new Illness();
            WeightHistory = new ObservableCollection<Weight>();
            ShowUps = new ObservableCollection<ShowUp>();
            Payments = new ObservableCollection<Payment>();
            Programs = new ObservableCollection<Program>();
            WeightHistory.CollectionChanged += WeigthsChanged;
            Payments.CollectionChanged += PaymentsCollectionChanged;
            DispatcherTimer timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMinutes(1)
            };
            timer.Tick += Timer_Tick;
            timer.Start();
            BookCommand = new RelayCommand<string>(async (obj) => { await MakeBooking(obj); }, CanMakeBooking);
            AddOldShowUpCommand = new RelayCommand(AddOldShowUp);
            SaveChangesAsyncCommand = new RelayCommand(async () => { await SaveChanges(); });
            PaymentCommand = new RelayCommand(AddPayment, CanAddPayment);
        }





        private int _ProgramDuration;

        [NotMapped]
        public int ProgramDuration
        {
            get
            {
                return _ProgramDuration;
            }

            set
            {
                if (_ProgramDuration == value)
                {
                    return;
                }

                _ProgramDuration = value;
                RaisePropertyChanged();
            }
        }
        private bool CanAddPayment()
        {
            return PaymentAmount > 0 && PaymentAmount <= RemainingAmount;
        }

        [NotMapped]
        public RelayCommand PaymentCommand { get; set; }

        #endregion Constructors

        #region Fields

        private bool _ActiveCustomer;

        private string _Address;

        private bool _Alcohol;

        private int _AlcoholUsage;

        private string _DistrictText;

        private DateTime _DOB;

        private string _Email;

        private string _ExtraNotes;

        private string _ExtraReasons;

        private DateTime _FirstDate;

        private bool _Gender;

        private int _Height;

        private string _HistoryDuration;

        private string _HistoryKind;

        private bool _HistoryNotFirstTime;

        private int _HistoryTimesPerWeek;

        private Illness _Illness;

        private SolidColorBrush _IsActiveColor;

        private bool _IsDateValid;

        private bool _IsManualyActive;

        private bool _IsPracticing;

        private bool _IsSelected;

        private string _Job;

        private bool _Medicine;

        private string _MedicineText;

        private string _Name;

        private float _NewWeight;

        private int _NumOfShowUps;

        private DateTime _OldShowUpDate;

        private int _PaymentAmount;

        private ObservableCollection<Payment> _Payments;

        private bool _PreferedHand;

        private bool _Pregnancy;

        private int _ProgramPrice;

        private string _ProgramResult;

        private ObservableCollection<Program> _Programs;

        private int _ProgramTypeIndex;

        private bool _ReasonInjury;

        private bool _ReasonPower;

        private bool _ReasonSlim;

        private bool _ReasonVeltiwsh;

        private Program _SelectedProgram;

        private int _ShowUpPrice;

        private ObservableCollection<ShowUp> _ShowUps;

        private bool _Smoker;

        private int _SmokingUsage;

        private DateTime _StartDate;

        private string _SureName;

        private bool _Surgery;

        private string _SurgeryInfo;

        private string _Tel;

        private bool _WantToQuit;

        private ObservableCollection<Weight> _WeightHistory;

        #endregion Fields

        #region Properties

        public string Active => SelectedProgram != null && RemainingDays > 0 ? $"ΝΑΙ (έως {SelectedProgram.StartDay.AddMonths(1).ToString("dd/MM")})" : "ΟΧΙ";

        [NotMapped]
        public bool ActiveCustomer
        {
            get
            {
                if (IsActiveColor == null)
                {
                    _IsActiveColor = GetCustomerColor();
                }
                return _ActiveCustomer;
            }

            set
            {
                if (_ActiveCustomer == value)
                {
                    return;
                }

                _ActiveCustomer = value;
                RaisePropertyChanged();
            }
        }

        [NotMapped]
        public RelayCommand AddOldShowUpCommand { get; set; }

        public string Address
        {
            get
            {
                return _Address;
            }

            set
            {
                if (_Address == value)
                {
                    return;
                }

                _Address = value.ToUpper();
                RaisePropertyChanged();
            }
        }

        public int Age => DOB < DateTime.Today ? (new DateTime() + DateTime.Now.Subtract(DOB)).Year : 0;

        public bool Alcohol
        {
            get
            {
                return _Alcohol;
            }

            set
            {
                if (_Alcohol == value)
                {
                    return;
                }

                _Alcohol = value;
                RaisePropertyChanged();
            }
        }

        public int AlcoholUsage
        {
            get
            {
                return _AlcoholUsage;
            }

            set
            {
                if (_AlcoholUsage == value)
                {
                    return;
                }

                _AlcoholUsage = value;
                RaisePropertyChanged();
            }
        }

        [NotMapped]
        public SolidColorBrush BackGround
        {
            get
            {
                if (IsPracticing)
                {
                    return new SolidColorBrush(Colors.LightGreen);
                }
                return new SolidColorBrush(Colors.White);
            }
        }

        [NotMapped]
        public float BMI
        {
            get
            {
                return (float)Math.Round((WeightHistory.Count > 0 && LastWeight > 0 && Height > 0) ? LastWeight / (Height * Height / 10000) : 0, 2); ;
            }
        }

        [NotMapped]
        public RelayCommand<string> BookCommand { get; set; }

        [NotMapped]
        public GenericRepository Context { get; set; }

        public string DistrictText
        {
            get
            {
                return _DistrictText;
            }

            set
            {
                if (_DistrictText == value)
                {
                    return;
                }

                _DistrictText = value;
                RaisePropertyChanged();
            }
        }

        [Required]
        [DataType(DataType.Date, ErrorMessage = "Η ημερομηνία δεν έχει τη σωστή μορφή")]
        public DateTime DOB
        {
            get
            {
                return _DOB;
            }

            set
            {
                if (_DOB == value)
                {
                    return;
                }

                _DOB = value;
                RaisePropertyChanged(nameof(Age));
                RaisePropertyChanged();
            }
        }

        public TimeSpan Duration => LastShowUp != null ? (DateTime.Now.Subtract(LastShowUp.Arrived)) : new TimeSpan(0);

        [StringLength(30, MinimumLength = 0)]
        [DataType(DataType.EmailAddress, ErrorMessage = "Το Email δεν έχει τη σωστή μορφή")]
        [EmailAddress(ErrorMessage = "Το Email δεν έχει τη σωστή μορφή")]
        public string Email
        {
            get
            {
                return _Email;
            }

            set
            {
                if (_Email == value)
                {
                    return;
                }

                _Email = value;
                RaisePropertyChanged();
            }
        }

        public string ExtraNotes
        {
            get
            {
                return _ExtraNotes;
            }

            set
            {
                if (_ExtraNotes == value)
                {
                    return;
                }

                _ExtraNotes = value;
                RaisePropertyChanged();
            }
        }

        public string ExtraReasons
        {
            get
            {
                return _ExtraReasons;
            }

            set
            {
                if (_ExtraReasons == value)
                {
                    return;
                }

                _ExtraReasons = value;
                RaisePropertyChanged();
            }
        }

        public DateTime FirstDate
        {
            get
            {
                return _FirstDate;
            }

            set
            {
                if (_FirstDate == value)
                {
                    return;
                }

                _FirstDate = value;
                RaisePropertyChanged();
            }
        }

        public string FullName => Name + " " + SureName;

        public bool Gender
        {
            get
            {
                return _Gender;
            }

            set
            {
                if (_Gender == value)
                {
                    return;
                }

                _Gender = value;
                RaisePropertyChanged();
            }
        }

        public int Height
        {
            get
            {
                return _Height;
            }

            set
            {
                if (_Height == value)
                {
                    return;
                }

                _Height = value;
                RaisePropertyChanged();
            }
        }

        public string HistoryDuration
        {
            get
            {
                return _HistoryDuration;
            }

            set
            {
                if (_HistoryDuration == value)
                {
                    return;
                }

                _HistoryDuration = value;
                RaisePropertyChanged();
            }
        }

        public string HistoryKind
        {
            get
            {
                return _HistoryKind;
            }

            set
            {
                if (_HistoryKind == value)
                {
                    return;
                }

                _HistoryKind = value;
                RaisePropertyChanged();
            }
        }

        public bool HistoryNotFirstTime
        {
            get
            {
                return _HistoryNotFirstTime;
            }

            set
            {
                if (_HistoryNotFirstTime == value)
                {
                    return;
                }

                _HistoryNotFirstTime = value;
                RaisePropertyChanged();
            }
        }

        public int HistoryTimesPerWeek
        {
            get
            {
                return _HistoryTimesPerWeek;
            }

            set
            {
                if (_HistoryTimesPerWeek == value)
                {
                    return;
                }

                _HistoryTimesPerWeek = value;
                RaisePropertyChanged();
            }
        }

        public Illness Illness
        {
            get
            {
                return _Illness;
            }

            set
            {
                if (_Illness == value)
                {
                    return;
                }

                _Illness = value;
                RaisePropertyChanged();
                Illness.PropertyChanged += Illness_PropertyChanged;
            }
        }

        [NotMapped]
        public SolidColorBrush IsActiveColor
        {
            get
            {
                if (_IsActiveColor == null)
                {
                    _IsActiveColor = GetCustomerColor();
                }
                return _IsActiveColor;
            }

            set
            {
                if (_IsActiveColor == value)
                {
                    return;
                }

                _IsActiveColor = value;
                RaisePropertyChanged();
            }
        }

        [NotMapped]
        public bool IsDateValid
        {
            get
            {
                return _IsDateValid;
            }

            set
            {
                if (_IsDateValid == value)
                {
                    return;
                }

                _IsDateValid = value;
                RaisePropertyChanged();
            }
        }

        public bool IsManualyActive
        {
            get
            {
                return _IsManualyActive;
            }

            set
            {
                if (_IsManualyActive == value)
                {
                    return;
                }

                _IsManualyActive = value;
                RaisePropertyChanged();
            }
        }

        public bool IsNotPracticing => !IsPracticing;

        [NotMapped]
        public bool IsPracticing
        {
            get
            {
                return _IsPracticing;
            }

            set
            {
                if (_IsPracticing == value)
                {
                    return;
                }

                _IsPracticing = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(IsNotPracticing));
                RaisePropertyChanged(nameof(BackGround));
            }
        }

        [NotMapped]
        public bool IsSelected
        {
            get
            {
                return _IsSelected;
            }

            set
            {
                if (_IsSelected == value)
                {
                    return;
                }

                _IsSelected = value;
                RaisePropertyChanged();
            }
        }

        public string Job
        {
            get
            {
                return _Job;
            }

            set
            {
                if (_Job == value)
                {
                    return;
                }

                _Job = value.ToUpper();
                RaisePropertyChanged();
            }
        }

        public ShowUp LastShowUp => ShowUps != null && ShowUps.Count > 0 ? ShowUps.OrderBy(x => x.Id).Last() : null;

        public float LastWeight => (float)Math.Round((WeightHistory.Count > 0 && WeightHistory.OrderBy(x => x.Id).Last().WeightValue > 0) ? WeightHistory.Last().WeightValue : 0, 2);

        public bool Medicine
        {
            get
            {
                return _Medicine;
            }

            set
            {
                if (_Medicine == value)
                {
                    return;
                }

                _Medicine = value;
                RaisePropertyChanged();
            }
        }

        public string MedicineText
        {
            get
            {
                return _MedicineText;
            }

            set
            {
                if (_MedicineText == value)
                {
                    return;
                }

                _MedicineText = value;
                RaisePropertyChanged();
            }
        }

        [Required(ErrorMessage = "Το όνομα απαιτείται!")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Το όνομα μπορεί να είναι απο 3 έως 20 χαρακτήρες.")]
        public string Name
        {
            get
            {
                return _Name;
            }

            set
            {
                if (_Name == value)
                {
                    return;
                }

                _Name = value.ToUpper();
                RaisePropertyChanged();
            }
        }

        [NotMapped]
        public float NewWeight
        {
            get
            {
                return _NewWeight;
            }

            set
            {
                if (_NewWeight == value)
                {
                    return;
                }

                _NewWeight = value;
                RaisePropertyChanged();
            }
        }

        public bool NotManualyACtive => !IsManualyActive;

        [NotMapped]
        public int NumOfShowUps
        {
            get
            {
                return _NumOfShowUps;
            }

            set
            {
                if (_NumOfShowUps == value)
                {
                    return;
                }

                _NumOfShowUps = value;
                if (value > 0)
                {
                    ShowUpPrice = ProgramPrice / value;
                }
                RaisePropertyChanged();
            }
        }

        [NotMapped]
        public DateTime OldShowUpDate
        {
            get
            {
                return _OldShowUpDate;
            }

            set
            {
                if (_OldShowUpDate == value)
                {
                    return;
                }

                _OldShowUpDate = value;
                RaisePropertyChanged();
            }
        }

        [NotMapped]
        public int PaymentAmount
        {
            get
            {
                return _PaymentAmount;
            }

            set
            {
                if (_PaymentAmount == value)
                {
                    return;
                }

                _PaymentAmount = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<Payment> Payments
        {
            get
            {
                return _Payments;
            }

            set
            {
                if (_Payments == value)
                {
                    return;
                }

                _Payments = value;
                RaisePropertyChanged();
            }
        }

        public bool PreferedHand
        {
            get
            {
                return _PreferedHand;
            }

            set
            {
                if (_PreferedHand == value)
                {
                    return;
                }

                _PreferedHand = value;
                RaisePropertyChanged();
            }
        }

        public bool Pregnancy
        {
            get
            {
                return _Pregnancy;
            }

            set
            {
                if (_Pregnancy == value)
                {
                    return;
                }

                _Pregnancy = value;
                RaisePropertyChanged();
            }
        }

        [NotMapped]
        public int ProgramPrice
        {
            get
            {
                return _ProgramPrice;
            }

            set
            {
                if (_ProgramPrice == value)
                {
                    return;
                }

                _ProgramPrice = value;
                if (NumOfShowUps > 0)
                {
                    _ShowUpPrice = ProgramPrice / NumOfShowUps;
                }
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(ShowUpPrice));
            }
        }

        [NotMapped]
        public string ProgramResult
        {
            get
            {
                return _ProgramResult;
            }

            set
            {
                if (_ProgramResult == value)
                {
                    return;
                }

                _ProgramResult = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<Program> Programs
        {
            get
            {
                return _Programs;
            }

            set
            {
                if (_Programs == value)
                {
                    return;
                }

                _Programs = value;
                RaisePropertyChanged();
            }
        }

        [NotMapped]
        public int ProgramTypeIndex
        {
            get
            {
                return _ProgramTypeIndex;
            }

            set
            {
                if (_ProgramTypeIndex == value)
                {
                    return;
                }

                _ProgramTypeIndex = value;

                RaisePropertyChanged();
            }
        }

        public bool ReasonInjury
        {
            get
            {
                return _ReasonInjury;
            }

            set
            {
                if (_ReasonInjury == value)
                {
                    return;
                }

                _ReasonInjury = value;
                RaisePropertyChanged();
            }
        }

        public bool ReasonPower
        {
            get
            {
                return _ReasonPower;
            }

            set
            {
                if (_ReasonPower == value)
                {
                    return;
                }

                _ReasonPower = value;
                RaisePropertyChanged();
            }
        }

        public bool ReasonSlim
        {
            get
            {
                return _ReasonSlim;
            }

            set
            {
                if (_ReasonSlim == value)
                {
                    return;
                }

                _ReasonSlim = value;
                RaisePropertyChanged();
            }
        }

        public bool ReasonVeltiwsh
        {
            get
            {
                return _ReasonVeltiwsh;
            }

            set
            {
                if (_ReasonVeltiwsh == value)
                {
                    return;
                }

                _ReasonVeltiwsh = value;
                RaisePropertyChanged();
            }
        }

        public int RemainingAmount
        {
            get
            {
                return CalculateRemainingAmount();
            }
        }

        public int RemainingDays => GetRemainingDays(SelectedProgram);

        [NotMapped]
        public RelayCommand SaveChangesAsyncCommand { get; set; }

        [NotMapped]
        public Program SelectedProgram
        {
            get
            {
                return _SelectedProgram;
            }

            set
            {
                if (_SelectedProgram == value)
                {
                    return;
                }

                _SelectedProgram = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(RemainingDays));
                RaisePropertyChanged(nameof(Active));
                RaisePropertyChanged(nameof(TypeOfProgram));
            }
        }

        [NotMapped]
        public int ShowUpPrice
        {
            get
            {
                return _ShowUpPrice;
            }

            set
            {
                if (_ShowUpPrice == value)
                {
                    return;
                }

                _ShowUpPrice = value;
                if (NumOfShowUps > 0)
                {
                    _ProgramPrice = NumOfShowUps * ShowUpPrice;
                }
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(ProgramPrice));
            }
        }

        public ObservableCollection<ShowUp> ShowUps
        {
            get
            {
                return _ShowUps;
            }

            set
            {
                if (_ShowUps == value)
                {
                    return;
                }

                _ShowUps = value;
                RaisePropertyChanged();
            }
        }

        public bool Smoker
        {
            get
            {
                return _Smoker;
            }

            set
            {
                if (_Smoker == value)
                {
                    return;
                }

                _Smoker = value;
                RaisePropertyChanged();
            }
        }

        public int SmokingUsage
        {
            get
            {
                return _SmokingUsage;
            }

            set
            {
                if (_SmokingUsage == value)
                {
                    return;
                }

                _SmokingUsage = value;
                RaisePropertyChanged();
            }
        }

        [NotMapped]
        public DateTime StartDate
        {
            get
            {
                if (_StartDate.Year < 2019)
                    _StartDate = DateTime.Today;
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

        [Required(ErrorMessage = "Το επίθετο απαιτείται!")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Το Επίθετο μπορεί να είναι απο 3 έως 20 χαρακτήρες.")]
        public string SureName
        {
            get
            {
                return _SureName;
            }

            set
            {
                if (_SureName == value)
                {
                    return;
                }

                _SureName = value.ToUpper();
                RaisePropertyChanged();
            }
        }

        public bool Surgery
        {
            get
            {
                return _Surgery;
            }

            set
            {
                if (_Surgery == value)
                {
                    return;
                }

                _Surgery = value;
                RaisePropertyChanged();
            }
        }

        public string SurgeryInfo
        {
            get
            {
                return _SurgeryInfo;
            }

            set
            {
                if (_SurgeryInfo == value)
                {
                    return;
                }

                _SurgeryInfo = value;
                RaisePropertyChanged();
            }
        }

        [Required(ErrorMessage = "Το Τηλέφωνο απαιτείται!")]
        [StringLength(18, MinimumLength = 3, ErrorMessage = "Το τηλέφωνο πρέπει να είναι τουλάχιστον 10 χαρακτήρες.")]
        [Phone(ErrorMessage = "Το τηλέφωνο δν έχει τη σωστή μορφή")]
        public string Tel
        {
            get
            {
                return _Tel;
            }

            set
            {
                if (_Tel == value)
                {
                    return;
                }

                _Tel = value;
                RaisePropertyChanged();
            }
        }

        public int TypeOfProgram => SelectedProgram != null ? (int)SelectedProgram.ProgramType : -1;

        public bool WantToQuit
        {
            get
            {
                return _WantToQuit;
            }

            set
            {
                if (_WantToQuit == value)
                {
                    return;
                }

                _WantToQuit = value;
                RaisePropertyChanged();
            }
        }

        //GettypeOfProgram();
        public ObservableCollection<Weight> WeightHistory
        {
            get
            {
                return _WeightHistory;
            }

            set
            {
                if (_WeightHistory == value)
                {
                    return;
                }

                _WeightHistory = value;
                RaisePropertyChanged();
            }
        }

        #endregion Properties

        #region Methods

        public int GetRemainingDays(Program program)
        {
            int showups = 0;
            if (program != null)
            {
                showups = program.Showups;
                foreach (ShowUp showUp in ShowUps.Where(s => s.Arrived >= program.StartDay))
                {
                    showups--;
                }
            }

            return showups;
        }

        public void SelectProperProgram()
        {
            foreach (var p in Programs)
            {
                if (p.StartDay <= DateTime.Today && (DateTime.Today - p.StartDay).Days <= 35 && GetRemainingDays(p) > 0)
                {
                    SelectedProgram = p;
                }
            }
        }

        public void ValidateProgram()
        {


            if (Programs.Any(p => p.StartDay <= StartDate && StartDate < p.StartDay.AddMonths(p.Months).AddDays(5) && GetRemainingDays(p) > 0))
            {
                IsDateValid = false;
                ProgramResult = "Προσοχή, υπάρχει ήδη ενεργό πακέτο αυτη την ημερομηνία";
                return;
            }
            else
            {
                IsDateValid = true;
            }

            if (ProgramTypeIndex < 0)
            {
                ProgramResult = "Επιλέξτε τύπο πακέτου";
                return;
            }
            else if (ProgramPrice == 0)
            {
                ProgramResult = "Προσοχή, δεν έχει επιλεγεί τιμή";
                return;
            }
            else if (ProgramDuration <1)
            {
                ProgramResult = "Προσοχή, δεν έχετε επιλέξει διάρκεια";
                return;
            }
            else if (StartDate < DateTime.Today)
            {
                ProgramResult = "Προσοχή, η επιλεγμένη ημερομηνία έχει περάσει";
                return;
            }
            else
            {
                ProgramResult = "";
                return;
            }
        }

        internal void AddNewProgram()
        {
            Programs.Add(new Program { Amount = ProgramPrice, DayOfIssue = DateTime.Now, Showups = NumOfShowUps, ProgramType = (Program.ProgramTypes)ProgramTypeIndex, Months = ProgramDuration, StartDay = StartDate });
        }

        internal bool ProgramDataCheck()
        {
            ValidateProgram();
            return ProgramPrice >= 0 && ProgramTypeIndex >= 0 && IsDateValid && NumOfShowUps > 0 && ProgramDuration > 0;
        }

        internal void ShowedUp(bool arrived)
        {
            IsPracticing = arrived;
            ShowUps.Add(new ShowUp { Arrive = arrived, Arrived = DateTime.Now });
        }

        private void AddOldShowUp()
        {
            Context.Add(new ShowUp { Arrived = OldShowUpDate, Left = OldShowUpDate, });
        }

        private int CalculateRemainingAmount()
        {
            int sum = 0;
            int remainingAmount = 0;
            foreach (Program program in Programs)
            {
                sum += program.Amount;
            }
            remainingAmount = sum;
            foreach (var payment in Payments)
            {
                remainingAmount -= payment.Amount;
            }
            return remainingAmount;
        }

        private bool CanMakeBooking(string arg)
        {
            return ProgramDataCheck() ;
        }

        private bool CanMakePayment()
        {
            return PaymentAmount <= RemainingAmount;
        }

        private SolidColorBrush GetCustomerColor()
        {
            if (IsManualyActive)
            {
                ActiveCustomer = true;
                return new SolidColorBrush(Colors.LightGreen);
            }
            DateTime lastShowUp;
            if (ShowUps.Count > 0)
            {
                lastShowUp = ShowUps.OrderBy(s => s.Arrived).FirstOrDefault().Arrived;
                if (lastShowUp > DateTime.Today.AddDays(-30))
                {
                    ActiveCustomer = true;
                    return new SolidColorBrush(Colors.Green);
                }
                else if (lastShowUp > DateTime.Today.AddDays(-60))
                {
                    ActiveCustomer = false;
                    return new SolidColorBrush(Colors.Orange);
                }
            }

            ActiveCustomer = false;
            //IsActiveColor.Opacity = 0.6;
            return new SolidColorBrush(Colors.Red);
        }

        private void Illness_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(Illness.Problems) && e.PropertyName != nameof(Illness.SelectedIllness) && e.PropertyName != nameof(Illness.SelectedIllnessPropertyName) && Attribute.IsDefined(typeof(Illness).GetProperty(e.PropertyName), typeof(DisplayNameAttribute)))
            {
                Illness.RaisePropertyChanged(nameof(Illness.Problems));
            }
        }

        private async Task MakeBooking(string obj)
        {
            AddNewProgram();
            if (int.Parse(obj) == 1)
            {
                MakeProgramPayment();
            }

            await Context.SaveAsync();
            ProgramPrice = 0;
            ProgramTypeIndex = 0;
            StartDate = DateTime.Today;
            RaisePropertyChanged(nameof(RemainingAmount));
        }

        internal void MakeProgramPayment()
        {
            Payments.Add(new Payment { Amount = ProgramPrice });
        }

        internal void AddPayment()
        {
            Payments.Add(new Payment { Amount = PaymentAmount });
            PaymentAmount = 0;
            RaisePropertyChanged(nameof(RemainingAmount));
        }

        private void PaymentsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(RemainingAmount));
        }

        private async Task SaveChanges()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            SelectProperProgram();
            GetRemainingDays(SelectedProgram);
            await Context.SaveAsync();
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        //private string GettypeOfProgram()
        //{
        //    if (SelectedProgram != null)
        //    {
        //        switch (SelectedProgram.ProgramType)
        //        {
        //            case Program.ProgramTypes.daily30:
        //                return "Ημερήσιο 30'";

        //            case Program.ProgramTypes.daily60:
        //                return "Ημερήσιο 60'";

        //            case Program.ProgramTypes.pilates2:
        //                return "Reformer Pilates (1-2)";

        //            case Program.ProgramTypes.functional2:
        //                return "Functional Training(1-2)";

        //            case Program.ProgramTypes.pilates5:
        //                return "Reformer Pilates (3-5)";

        //            case Program.ProgramTypes.functional5:
        //                return "Functional Training (3-5)";

        //            case Program.ProgramTypes.freeUse:
        //                return "Ελέυθερη Χρήση";
        //        }
        //    }
        //    return "Ανενεργό";
        //}

        private void Timer_Tick(object sender, EventArgs e)
        {
            RaisePropertyChanged(nameof(Duration));
        }

        private void WeigthsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(BMI));
        }

        #endregion Methods
    }
}