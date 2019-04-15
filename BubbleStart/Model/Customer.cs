using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Windows.Media;
using System.Windows.Threading;

namespace BubbleStart.Model
{
    [Table("BubbleCustomers")]
    public class Customer : BaseModel
    {
        #region Constructors



        private int _ProgramPrice;

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
                RaisePropertyChanged();
            }
        }
        public Customer()
        {
            FirstDate = DateTime.Today;
            Illness = new Illness();
            WeightHistory = new ObservableCollection<Weight>();
            ShowUps = new ObservableCollection<ShowUp>();
            SetPriceCommand = new RelayCommand<int>(SetPrice);
            WeightHistory.CollectionChanged += WeigthsChanged;
            Payments = new ObservableCollection<Payment>();
            Programs = new ObservableCollection<Program>();
            DispatcherTimer timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMinutes(1)
            };
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        internal bool ProgramDataCheck()
        {
            return ProgramPrice >= 0 && ProgramTypeIndex >= 0;
        }

        private int _ProgramTypeIndex;

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

        public string TypeOfProgram => GettypeOfProgram();

        private string GettypeOfProgram()
        {
            if (SelectedProgram != null)
            {
                switch (SelectedProgram.ProgramType)
                {
                    case Program.ProgramTypes.daily30:
                        return "Ημερήσιο 30'";

                    case Program.ProgramTypes.daily60:
                        return "Ημερήσιο 60'";

                    case Program.ProgramTypes.pilates2:
                        return "Reformer Pilates (1-2)";

                    case Program.ProgramTypes.functional2:
                        return "Functional Training(1-2)";

                    case Program.ProgramTypes.pilates5:
                        return "Reformer Pilates (3-5)";

                    case Program.ProgramTypes.functional5:
                        return "Functional Training (3-5)";

                    case Program.ProgramTypes.freeUse:
                        return "Ελέυθερη Χρήση";
                }
            }
            return "Ανενεργό";
        }

        #endregion Constructors

        public void SelectProperProgram()
        {
            foreach (var p in Programs)
            {
                if (p.StartDay <= DateTime.Today && (DateTime.Today - p.StartDay).Days <= 35)
                {
                }
            }
        }

        private Program _SelectedProgram;

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

        public int RemainingDays => GetRemainingDays();

        private int GetRemainingDays()
        {
            int duration = 0;
            if (SelectedProgram != null)
            {
                duration = SelectedProgram.Duration;
                foreach (var showUp in ShowUps.Select(s => s.Arrived >= SelectedProgram.StartDay))
                {
                    duration--;
                }
            }

            return duration;
        }

        public string Active => SelectedProgram != null && RemainingDays > 0 ? "ΝΑΙ" : "ΟΧΙ";

        public string FullName => Name + " " + SureName;

        #region Fields

        private string _Address;

        private bool _Alcohol;

        private int _AlcoholUsage;

        private int _District;

        private DateTime _DOB;

        private string _Email;

        private DateTime _FirstDate;

        private bool _Gender;

        private int _Height;

        private int _HistoryDuration;

        private string _HistoryKind;

        private bool _HistoryNotFirstTime;

        private int _HistoryTimesPerWeek;

        private Illness _Illness;

        private bool _IsPracticing;

        private bool _IsSelected;

        private string _Job;

        private bool _Medicine;

        private string _MedicineText;
        private bool _MyProperty;

        private string _Name;

        private float _NewWeight;

        private ObservableCollection<Payment> _Payments;

        private bool _PreferedHand;

        private bool _Pregnancy;

        private ObservableCollection<Program> _Programs;
        private bool _ReasonInjury;

        private bool _ReasonPower;

        private bool _ReasonSlim;

        private bool _ReasonVeltiwsh;

        private ObservableCollection<ShowUp> _ShowUps;

        private bool _Smoker;

        private int _SmokingUsage;

        private string _SureName;

        private bool _Surgery;

        private string _SurgeryInfo;

        private string _Tel;

        private bool _WantToQuit;

        private ObservableCollection<Weight> _WeightHistory;

        #endregion Fields

        #region Properties

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




        private DateTime _StartDate;

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

        internal void AddNewProgram()
        {
            Programs.Add(new Program { Amount = ProgramPrice, DayOfIssue = DateTime.Now, Duration = ProgramTypeIndex < 2 ? 1 : 8, ProgramType = (Program.ProgramTypes)ProgramTypeIndex, StartDay = StartDate });
        }

        public int Age => (new DateTime() + DateTime.Now.Subtract(DOB)).Year;

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

        public int District
        {
            get
            {
                return _District;
            }

            set
            {
                if (_District == value)
                {
                    return;
                }

                _District = value;
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
                RaisePropertyChanged();
            }
        }

        public TimeSpan Duration => (DateTime.Now.Subtract(LastShowUp.Arrived));

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

        public int HistoryDuration
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

        public bool MyProperty
        {
            get
            {
                return _MyProperty;
            }

            set
            {
                if (_MyProperty == value)
                {
                    return;
                }

                _MyProperty = value;
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

        public RelayCommand<int> SetPriceCommand { get; set; }

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

        internal void MakePayment()
        {
            Payments.Add(new Payment { Amount = ProgramPrice });
        }

        internal void ShowedUp(bool arrived)
        {
            IsPracticing = arrived;
            ShowUps.Add(new ShowUp { Arrive = arrived, Arrived = DateTime.Now });
        }

        private int CalculateRemainingAmount()
        {
            int sum = 0;
            int remainingAmount = 0;
            foreach (Program program in Programs)
            {
                sum += program.Amount;
            }
            foreach (var payment in Payments)
            {
                remainingAmount -= payment.Amount;
            }
            return remainingAmount;
        }

        private void SetPrice(int price)
        {
            if (ShowUps != null && ShowUps.Count > 0)
            {
                LastShowUp.Amount = price;
            }
        }

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