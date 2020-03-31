using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using BubbleStart.Helpers;
using DocumentFormat.OpenXml.InkML;
using GalaSoft.MvvmLight.CommandWpf;

namespace BubbleStart.Model
{
    [Table("BubbleCustomers")]
    public class Customer : BaseModel
    {
        #region Constructors

        public Customer()
        {
            FirstDate = DateTime.Today;
            WeightHistory = new ObservableCollection<Weight>();
            ShowUps = new ObservableCollection<ShowUp>();
            Payments = new ObservableCollection<Payment>();
            Programs = new ObservableCollection<Program>();
            Changes = new ObservableCollection<Change>();
            Apointments = new ObservableCollection<Apointment>();
            WeightHistory.CollectionChanged += WeigthsChanged;
            ShowPreviusDataCommand = new RelayCommand(async () => { await ShowPreviewsData(); });
            DispatcherTimer timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMinutes(1)
            };
            timer.Tick += Timer_Tick;
            timer.Start();
            BookCommand = new RelayCommand<string>(async (obj) => { await MakeBooking(obj); }, CanMakeBooking);
            AddOldShowUpCommand = new RelayCommand(async () => { await AddOldShowUp(); });
            SaveChangesAsyncCommand = new RelayCommand(async () => { await SaveChanges(); }, CanSaveChanges);
            PaymentCommand = new RelayCommand(async () => { await AddPayment(); }, CanAddPayment);
            DeleteShowUpCommand = new RelayCommand(async () => { await DeleteShowUp(); }, SelectedShowUp != null);
            DeletemassShowUpCommand = new RelayCommand(async () => { await DeletemassShowUp(); }, SelectedMassShowUp != null);
            ToggleRealCommand = new RelayCommand(async () => { await TogleReal(); }, SelectedShowUp != null);
            ToggleRealbCommand = new RelayCommand(async () => { await TogleRealb(); }, SelectedShowUp != null);
            ToggleMassageCommand = new RelayCommand(async () => { await TogleMassage(); }, SelectedShowUp != null);
            ToggleMassagebCommand = new RelayCommand(async () => { await TogleMassageb(); }, SelectedMassShowUp != null);
            DeletePaymentCommand = new RelayCommand(async () => { await DeletePayment(); }, SelectedPayment != null);
            DeletePaymentMassCommand = new RelayCommand(async () => { await DeletePaymentMass(); }, SelectedPaymentMass != null);
            SetToProgramCommand = new RelayCommand(async () => { await SetToPayment(); }, CanSet);
            DeleteProgramCommand = new RelayCommand(async () => { await DeleteProgram(); }, SelectedProgramToDelete != null);
            DeleteProgramMassCommand = new RelayCommand(async () => { await DeleteProgramMass(); }, SelectedProgramMassageToDelete != null);
            OldShowUpDate = DateOfPayment = DateOfIssue = StartDate = DateTime.Today;
            DisableCustomerCommand = new RelayCommand(DisableCustomer);
            ProgramTypeIndex = -1;
        }

        #endregion Constructors

        #region Fields

        private bool _ActiveCustomer;

        private string _Address;

        private bool _Alcohol;

        private int _AlcoholUsage;

        private ObservableCollection<Apointment> _Apointments;

        private BasicDataManager _BasicDataManager;

        private ObservableCollection<Change> _Changes;

        private DateTime _DateOfIssue;

        private DateTime _DateOfPayment;

        private string _DistrictText;

        private DateTime _DOB;

        private string _Email;

        private string _ExtraNotes;

        private string _ExtraReasons;

        private DateTime _FirstDate;

        private bool _ForceDisable;

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

        private bool _IsMassage;

        private bool _IsPracticing;

        private bool _IsSelected;

        private string _Job;

        private bool _Loaded;

        private bool _Medicine;

        private string _MedicineText;

        private string _Name;

        private decimal _NewWeight;

        private decimal _NextPayment;

        private int _NumOfShowUps;

        private DateTime _OldShowUpDate;

        private decimal _PaymentAmount;

        private ObservableCollection<Payment> _Payments;

        private ICollectionView _PaymentsCollectionView;

        private ICollectionView _PaymentsMassCollectionView;

        private bool _PreferedHand;

        private bool _Pregnancy;

        private int _ProgramDuration;

        private decimal _ProgramPrice;

        private string _ProgramResult;

        private ObservableCollection<Program> _Programs;

        private ICollectionView _ProgramsColelctionView;

        private ICollectionView _ProgramsMassageColelctionView;

        private int _ProgramTypeIndex;

        private bool _ReasonInjury;

        private bool _ReasonPower;

        private bool _ReasonSlim;

        private bool _ReasonVeltiwsh;

        private decimal _RemainingAmount;

        private Program _SelectedMasage;

        private ShowUp _SelectedMassShowUp;

        private Payment _SelectedPayment;

        private Payment _SelectedPaymentMass;

        private Program _SelectedProgram;

        private Program _SelectedProgramMassageToDelete;

        private Program _SelectedProgramToDelete;

        private ShowUp _SelectedShowUp;

        private decimal _ShowUpPrice;

        private ObservableCollection<ShowUp> _ShowUps;

        private ICollectionView _ShowUpsCollectionView;

        private ICollectionView _ShowUpsMassCollectionView;

        private bool _Signed;

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

        public string Active
        {
            get
            {
                if (RemainingTrainingDays > 0)
                    if (SelectedProgram != null)
                        return $"ΝΑΙ (έως {SelectedProgram.StartDay.AddMonths(SelectedProgram.Months).ToString("dd/MM")})";
                if (RemainingMassageDays > 0)
                    if (SelectedMasage != null)
                        return $"ΝΑΙ (έως {SelectedMasage.StartDay.AddMonths(SelectedMasage.Months).ToString("dd/MM")})";
                return "OXI";
            }
        }

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
        public RelayCommand AddOldShowUpCommand { get; }

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
        public BasicDataManager BasicDataManager
        {
            get
            {
                return _BasicDataManager;
            }

            set
            {
                if (_BasicDataManager == value)
                {
                    return;
                }

                _BasicDataManager = value;
                RaisePropertyChanged();
            }
        }

        [NotMapped]
        public decimal BMI
        {
            get
            {
                return Math.Round((WeightHistory.Count > 0 && LastWeight > 0 && Height > 0) ? LastWeight / (Height * Height / 10000) : 0, 2); ;
            }
        }

        [NotMapped]
        public RelayCommand<string> BookCommand { get; set; }

        public ObservableCollection<Change> Changes
        {
            get
            {
                return _Changes;
            }

            set
            {
                if (_Changes == value)
                {
                    return;
                }

                _Changes = value;
                RaisePropertyChanged();
            }
        }

        [NotMapped]
        public DateTime DateOfIssue
        {
            get
            {
                return _DateOfIssue;
            }

            set
            {
                if (_DateOfIssue == value)
                {
                    return;
                }

                _DateOfIssue = value;
                RaisePropertyChanged();
            }
        }

        [NotMapped]
        public DateTime DateOfPayment
        {
            get
            {
                return _DateOfPayment;
            }

            set
            {
                if (_DateOfPayment == value)
                {
                    return;
                }

                _DateOfPayment = value;
                RaisePropertyChanged();
            }
        }

        [NotMapped]
        public RelayCommand DeletemassShowUpCommand { get; set; }

        [NotMapped]
        public RelayCommand DeletePaymentCommand { get; set; }

        [NotMapped]
        public RelayCommand DeletePaymentMassCommand { get; set; }

        [NotMapped]
        public RelayCommand DeleteProgramCommand { get; set; }

        [NotMapped]
        public RelayCommand DeleteProgramMassCommand { get; set; }

        [NotMapped]
        public RelayCommand DeleteShowUpCommand { get; set; }

        [NotMapped]
        public RelayCommand DisableCustomerCommand { get; set; }

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

        public bool Enabled { get; set; } = true;

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

        public bool ForceDisable
        {
            get
            {
                return _ForceDisable;
            }

            set
            {
                if (_ForceDisable == value)
                {
                    return;
                }

                _ForceDisable = value;
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

        [Required]
        public Illness Illness
        {
            get
            {
                if (_Illness == null)
                {
                    _Illness = new Illness();
                }
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
                if (Illness != null)
                {
                    Illness.PropertyChanged += Illness_PropertyChanged;
                }
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

        [NotMapped]
        public bool IsMassage
        {
            get
            {
                return _IsMassage;
            }

            set
            {
                if (_IsMassage == value)
                {
                    return;
                }

                _IsMassage = value;
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

        public decimal LastWeight => (decimal)Math.Round((WeightHistory.Count > 0 && WeightHistory.OrderBy(x => x.Id).Last().WeightValue > 0) ? WeightHistory.Last().WeightValue : 0, 2);

        [NotMapped]
        public bool Loaded
        {
            get
            {
                return _Loaded;
            }

            set
            {
                if (_Loaded == value)
                {
                    return;
                }

                _Loaded = value;
                RaisePropertyChanged();
            }
        }

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
        public decimal NewWeight
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

        [NotMapped]
        public decimal NextPayment
        {
            get
            {
                return _NextPayment;
            }

            set
            {
                if (_NextPayment == value || value < 0)
                {
                    return;
                }

                _NextPayment = value;
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
        public decimal PaymentAmount
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
                NextPayment = RemainingAmount - PaymentAmount;
                RaisePropertyChanged();
            }
        }

        [NotMapped]
        public RelayCommand PaymentCommand { get; set; }


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
                Payments.CollectionChanged += PaymentsCollectionChanged;

                PaymentsCollectionView = ProgramsMassageColelctionView = new ListCollectionView(Payments);
                PaymentsCollectionView.Filter = ProgramsgymFilter;
                PaymentsCollectionView.SortDescriptions.Add(new SortDescription("Date", ListSortDirection.Descending));
                PaymentsCollectionView.SortDescriptions.Add(new SortDescription("Id", ListSortDirection.Descending));

                PaymentsMassCollectionView = ProgramsMassageColelctionView = new ListCollectionView(Payments);
                PaymentsMassCollectionView.Filter = ProgramsmasFilter;
                PaymentsMassCollectionView.SortDescriptions.Add(new SortDescription("Date", ListSortDirection.Descending));
                PaymentsMassCollectionView.SortDescriptions.Add(new SortDescription("Id", ListSortDirection.Descending));
                RaisePropertyChanged();
            }
        }

        public ICollectionView PaymentsCollectionView
        {
            get
            {
                return _PaymentsCollectionView;
            }

            set
            {
                if (_PaymentsCollectionView == value)
                {
                    return;
                }

                _PaymentsCollectionView = value;
                RaisePropertyChanged();
            }
        }

        public ICollectionView PaymentsMassCollectionView
        {
            get
            {
                return _PaymentsMassCollectionView;
            }

            set
            {
                if (_PaymentsMassCollectionView == value)
                {
                    return;
                }

                _PaymentsMassCollectionView = value;
                RaisePropertyChanged();
            }
        }

        public Visibility PaymentVisibility => RemainingAmount > 0 ? Visibility.Visible : Visibility.Collapsed;

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

        [NotMapped]
        public decimal ProgramPrice
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
                Programs.CollectionChanged += ProgramsCollectionChanged;

                ProgramsMassageColelctionView = new ListCollectionView(Programs);
                ProgramsMassageColelctionView.SortDescriptions.Add(new SortDescription("StartDay", ListSortDirection.Descending));
                ProgramsMassageColelctionView.Filter = ProgramsmasFilter;

                ProgramsColelctionView = new ListCollectionView(Programs);
                ProgramsColelctionView.SortDescriptions.Add(new SortDescription("StartDay", ListSortDirection.Descending));
                ProgramsColelctionView.Filter = ProgramsgymFilter;

                RaisePropertyChanged();
            }
        }

        public ICollectionView ProgramsColelctionView
        {
            get
            {
                return _ProgramsColelctionView;
            }

            set
            {
                if (_ProgramsColelctionView == value)
                {
                    return;
                }

                _ProgramsColelctionView = value;
                RaisePropertyChanged();
            }
        }

        public ICollectionView ProgramsMassageColelctionView
        {
            get
            {
                return _ProgramsMassageColelctionView;
            }

            set
            {
                if (_ProgramsMassageColelctionView == value)
                {
                    return;
                }

                _ProgramsMassageColelctionView = value;
                //ProgramsMassageColelctionView.SortDescriptions.Add(new SortDescription("StartDay", ListSortDirection.Descending));
                ProgramsMassageColelctionView.Filter = ProgramsmasFilter;
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

        [NotMapped]
        public decimal RemainingAmount
        {
            get
            {
                return _RemainingAmount;
            }

            set
            {
                if (_RemainingAmount == value)
                {
                    return;
                }

                _RemainingAmount = value;
                RaisePropertyChanged();
            }
        }

        public string RemainingDays => $"{RemainingTrainingDays}+{RemainingMassageDays}";

        public int RemainingMassageDays => SelectedMasage != null && SelectedMasage.RemainingDays > 0 ? SelectedMasage.RemainingDays : 0;

        public int RemainingTrainingDays => SelectedProgram != null && SelectedProgram.RemainingDays > 0 ?
            SelectedProgram.RemainingDays : 0;

        [NotMapped]
        public RelayCommand SaveChangesAsyncCommand { get; set; }

        [NotMapped]
        public Program SelectedMasage
        {
            get
            {
                return _SelectedMasage;
            }

            set
            {
                if (_SelectedMasage == value)
                {
                    return;
                }

                _SelectedMasage = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(Active));
                RaisePropertyChanged(nameof(TypeOfProgram));
            }
        }

        [NotMapped]
        public ShowUp SelectedMassShowUp
        {
            get
            {
                return _SelectedMassShowUp;
            }

            set
            {
                if (_SelectedMassShowUp == value)
                {
                    return;
                }

                _SelectedMassShowUp = value;
                RaisePropertyChanged();
            }
        }

        [NotMapped]
        public Payment SelectedPayment
        {
            get
            {
                return _SelectedPayment;
            }

            set
            {
                if (_SelectedPayment == value)
                {
                    return;
                }

                _SelectedPayment = value;
                RaisePropertyChanged();
            }
        }

        [NotMapped]
        public Payment SelectedPaymentMass
        {
            get
            {
                return _SelectedPaymentMass;
            }

            set
            {
                if (_SelectedPaymentMass == value)
                {
                    return;
                }

                _SelectedPaymentMass = value;
                RaisePropertyChanged();
            }
        }

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
                RaisePropertyChanged(nameof(Active));
                RaisePropertyChanged(nameof(TypeOfProgram));
            }
        }

        [NotMapped]
        public Program SelectedProgramMassageToDelete
        {
            get
            {
                return _SelectedProgramMassageToDelete;
            }

            set
            {
                if (_SelectedProgramMassageToDelete == value)
                {
                    return;
                }

                _SelectedProgramMassageToDelete = value;
                SelectedProgramToDelete = null;
                PaymentAmount = SelectedProgramMassageToDelete != null && SelectedProgramMassageToDelete.RemainingAmount > 0
                    ? SelectedProgramMassageToDelete.RemainingAmount
                    : 0;
                RaisePropertyChanged();
            }
        }

        [NotMapped]
        public Program SelectedProgramToDelete
        {
            get
            {
                return _SelectedProgramToDelete;
            }

            set
            {
                if (_SelectedProgramToDelete == value)
                {
                    return;
                }

                _SelectedProgramToDelete = value;
                SelectedProgramMassageToDelete = null;
                PaymentAmount = SelectedProgramToDelete != null && SelectedProgramToDelete.RemainingAmount > 0 ? SelectedProgramToDelete.RemainingAmount : 0;
                RaisePropertyChanged();
            }
        }

        [NotMapped]
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

        [NotMapped]
        public RelayCommand SetToProgramCommand { get; set; }

        [NotMapped]
        public bool ShowedUpToday => ShowUps.Any(s => s.Arrived.Date == DateTime.Today);

        [NotMapped]
        public RelayCommand ShowPreviusDataCommand { get; }

        [NotMapped]
        public decimal ShowUpPrice
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
                ShowUps.CollectionChanged += ShowUps_CollectionChanged;
                ShowUpsCollectionView = new ListCollectionView(ShowUps);
                ShowUpsCollectionView.SortDescriptions.Add(new SortDescription("Arrived", ListSortDirection.Descending));
                ShowUpsCollectionView.Filter = ProgramsgymFilter;

                ShowUpsMassCollectionView = new ListCollectionView(ShowUps);
                ShowUpsMassCollectionView.SortDescriptions.Add(new SortDescription("Arrived", ListSortDirection.Descending));
                ShowUpsMassCollectionView.Filter = ProgramsmasFilter; ;

                RaisePropertyChanged();
            }
        }

        public ICollectionView ShowUpsCollectionView
        {
            get
            {
                //if (_ShowUpsCollectionView.SortDescriptions.Count==0)
                //{
                //}
                return _ShowUpsCollectionView;
            }

            set
            {
                if (_ShowUpsCollectionView == value)
                {
                    return;
                }

                _ShowUpsCollectionView = value;

                RaisePropertyChanged();
            }
        }

        public ICollectionView ShowUpsMassCollectionView
        {
            get
            {
                return _ShowUpsMassCollectionView;
            }

            set
            {
                if (_ShowUpsMassCollectionView == value)
                {
                    return;
                }

                _ShowUpsMassCollectionView = value;
                RaisePropertyChanged();
            }
        }

        public bool Signed
        {
            get
            {
                return _Signed;
            }

            set
            {
                if (_Signed == value)
                {
                    return;
                }

                _Signed = value;
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

        [NotMapped]
        public RelayCommand ToggleMassagebCommand { get; set; }

        [NotMapped]
        public RelayCommand ToggleMassageCommand { get; set; }

        [NotMapped]
        public RelayCommand ToggleRealbCommand { get; set; }

        [NotMapped]
        public RelayCommand ToggleRealCommand { get; set; }

        public string TypeOfProgram => SelectedProgram != null ? SelectedProgram.ToString() :
            SelectedMasage != null ? SelectedMasage.ToString() :
            "Ανενεργό";

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

        public decimal CalculateRemainingAmount()
        {
            decimal sum = 0;
            foreach (Program program in Programs)
            {
                sum += program.Amount;
            }
            decimal remainingAmount = sum;
            foreach (var payment in Payments)
            {
                remainingAmount -= payment.Amount;
            }
            PaymentAmount = RemainingAmount = remainingAmount;
            IsActiveColor = GetCustomerColor();
            return remainingAmount;
        }

        public void GetRemainingDays()
        {
            SetColors();
        }

        public void SetColors()
        {
            if (Loaded)
            {
                if (Programs != null && ShowUps != null && ShowUps.Count > 0 && Programs.Count > 0)
                {
                    Program selProg;
                    int progIndex = 0;
                    var programsReversed = Programs.Where(o => !o.IsMassage).OrderBy(p => p.StartDay).ThenByDescending(p => p.Id).ToList();
                    DateTime Limit;
                    if (programsReversed.Count > 0)
                    {
                        Limit = programsReversed[0].StartDay;
                    }
                    else
                    {
                        Limit = new DateTime();
                    }
                    var showUpsReserved = ShowUps.Where(s => s.Arrived >= Limit).OrderBy(r => r.Arrived).ThenByDescending(r => r.Id).ToList();
                    int counter = 0;
                    selProg = null;
                    foreach (ShowUp showUp in showUpsReserved)
                    {
                        if (showUp.Massage)
                        {
                            continue;
                        }
                        if ((selProg == null || selProg.RemainingDays == 0) && progIndex < programsReversed.Count)
                        {
                            counter = 1;
                            selProg = programsReversed[progIndex];
                            selProg.RemainingDays = selProg.Showups;
                            if (progIndex % 2 == 0)
                            {
                                selProg.Color = new SolidColorBrush(Colors.LightGreen);
                            }
                            else
                            {
                                selProg.Color = new SolidColorBrush(Colors.Orange);
                            }
                            progIndex++;
                        }
                        if (selProg != null && (selProg.RemainingDays != 0 || progIndex != programsReversed.Count))
                        {
                            showUp.Color = selProg.Color;
                            showUp.Count = counter++;
                            selProg.RemainingDays--;
                        }
                    }
                    if (progIndex<programsReversed.Count)
                    {
                        selProg = programsReversed[progIndex];
                        selProg.RemainingDays = selProg.Showups;
                    }
                    SelectedProgram = selProg;
                    for (int i = progIndex; i < programsReversed.Count; i++)
                    {
                        selProg = programsReversed[progIndex];
                        if (progIndex % 2 == 0)
                        {
                            selProg.Color = new SolidColorBrush(Colors.LightGreen);
                        }
                        else
                        {
                            selProg.Color = new SolidColorBrush(Colors.Orange);
                        }
                    }
                    programsReversed = Programs.Where(o => o.IsMassage).OrderBy(p => p.StartDay).ThenBy(p => p.Id).ToList();
                    selProg = null; progIndex = 0;
                    foreach (ShowUp showUp in showUpsReserved)
                    {
                        if (!showUp.Massage)
                        {
                            continue;
                        }
                        if ((selProg == null || selProg.RemainingDays == 0) && progIndex < programsReversed.Count)
                        {
                            counter = 1;
                            selProg = programsReversed[progIndex];
                            selProg.RemainingDays = selProg.Showups;
                            if (progIndex % 2 == 0)
                            {
                                selProg.Color = new SolidColorBrush(Colors.Aqua);
                            }
                            else
                            {
                                selProg.Color = new SolidColorBrush(Colors.LightBlue);
                            }
                            progIndex++;
                        }
                        if (selProg != null && (selProg.RemainingDays != 0 || progIndex != programsReversed.Count))
                        {
                            showUp.Color = selProg.Color;
                            showUp.Count = counter++;

                            selProg.RemainingDays--;
                        }
                    }
                    if (progIndex < programsReversed.Count)
                    {
                        selProg = programsReversed[progIndex];
                        selProg.RemainingDays = selProg.Showups;
                    }
                    SelectedMasage = selProg;
                    for (int i = progIndex; i < programsReversed.Count; i++)
                    {
                        selProg = programsReversed[progIndex];
                        if (progIndex % 2 == 0)
                        {
                            selProg.Color = new SolidColorBrush(Colors.Aqua);
                        }
                        else
                        {
                            selProg.Color = new SolidColorBrush(Colors.LightBlue);
                        }
                    }
                    SetRemaining();
                }
                //if (Programs != null && Payments != null && Payments.Count > 0 && Programs.Count > 0)
                //{
                //    Program selProg;
                //    int progIndex = 0;
                //    var programsReversed = Programs.OrderBy(p => p.StartDay).ThenBy(s => s.Id).ToList();
                //    selProg = programsReversed[progIndex];
                //    decimal remainingAmount = programsReversed[progIndex].Amount;
                //    decimal extraAmount = 0;
                //    decimal sum = 0;

                //    foreach (Payment payment in Payments.OrderBy(s => s.Date).ThenBy(s => s.Id))
                //    {
                //        sum += payment.Amount;
                //        if (payment.Amount < remainingAmount)
                //        {
                //            payment.Color = selProg.Color;
                //            remainingAmount -= payment.Amount;
                //        }
                //        else if (payment.Amount > remainingAmount)
                //        {
                //            //if (progIndex < programsReversed.Count && payment.Amount == remainingAmount + programsReversed[progIndex + 1].Amount)
                //            //{
                //            //}
                //            //else
                //            //{
                //            payment.Color = new SolidColorBrush(Colors.Green);
                //            extraAmount = payment.Amount;
                //            while (extraAmount > 0 && extraAmount >= selProg.Amount)
                //            {
                //                extraAmount -= remainingAmount;
                //                selProg.Color = new SolidColorBrush(Colors.Green);
                //                progIndex++;
                //                if (progIndex < programsReversed.Count)
                //                {
                //                    selProg = programsReversed[progIndex];
                //                    remainingAmount = selProg.Amount;
                //                }
                //                else
                //                {
                //                    break;
                //                }
                //            }
                //            //  }
                //            // remainingAmount -= selProg.Amount - extraAmount;
                //        }
                //        else
                //        {
                //            payment.Color = selProg.Color;
                //            progIndex++;
                //            if (progIndex < programsReversed.Count)
                //            {
                //                selProg = programsReversed[progIndex];
                //                remainingAmount = selProg.Amount;
                //            }
                //            else
                //                break;
                //        }
                //    }

                //    foreach (var p in Programs.OrderBy(p => p.DayOfIssue).ThenBy(s => s.Id))
                //    {
                //        p.PaidCol = sum >= p.Amount;
                //        sum -= p.Amount;
                //    }
                //}

                RaisePropertyChanged(nameof(SelectedMasage));
                RaisePropertyChanged(nameof(SelectedProgram));
                RaisePropertyChanged(nameof(RemainingMassageDays));
                RaisePropertyChanged(nameof(RemainingTrainingDays));
                RaisePropertyChanged(nameof(RemainingDays));
                PaymentsCollectionView.Refresh();
                PaymentsMassCollectionView.Refresh();
            }
        }

        public void SetRemaining()
        {
            foreach (var p in Programs)
            {
                p.CalculateRemainingAmount();
            }
        }

        public override string ToString()
        {
            return SureName + " " + Name;
        }

        public void ValidateProgram()
        {
            if (Programs.Any(p => p.StartDay <= StartDate && StartDate < p.StartDay.AddMonths(p.Months).AddDays(5) && p.RemainingDays > 0))
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
            else if (ProgramDuration < 1)
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

        internal void AddNewProgram(int par)
        {
            Programs.Add(new Program { Amount = ProgramPrice, DayOfIssue = DateOfIssue, Showups = NumOfShowUps, ProgramType = (Program.ProgramTypes)ProgramTypeIndex, Months = ProgramDuration, StartDay = StartDate, Paid = (par == 1) });
        }

        internal async Task AddPayment()
        {
            Payments.Add(new Payment { Amount = PaymentAmount, Date = DateOfPayment, User = StaticResources.User });
            if (SelectedProgramToDelete != null)
            {
                SelectedProgramToDelete.Payments.Add(Payments.Last());
                RaisePropertyChanged(nameof(PaymentVisibility));
                await SaveChanges();
                SelectedProgramToDelete.CalculateRemainingAmount();
            }
            else if (SelectedProgramMassageToDelete != null)
            {
                SelectedProgramMassageToDelete.Payments.Add(Payments.Last());
                RaisePropertyChanged(nameof(PaymentVisibility));
                await SaveChanges();
                SelectedProgramMassageToDelete.CalculateRemainingAmount();
            }
        }

        internal void MakeProgramPayment()
        {
            Payments.Add(new Payment { Amount = ProgramPrice, Date = DateOfIssue, User = StaticResources.User });
        }

        internal bool ProgramDataCheck()
        {
            ValidateProgram();
            return ProgramPrice >= 0 && ProgramTypeIndex >= 0 && NumOfShowUps > 0 && ProgramDuration > 0;
        }

        internal void ShowedUp(bool arrived, bool mass)
        {
            IsPracticing = arrived;
            if (mass)
            {
                int remain = RemainingMassageDays;
                ShowUps.Add(new ShowUp { Arrive = arrived, Arrived = DateTime.Now, Massage = mass });
                if (RemainingMassageDays == 0)
                {
                    if (remain > 0)
                        MessageBox.Show($"Αυτή ήταν η τελευταία συνδερία μασάζ του {ToString()}");
                    else
                        MessageBox.Show($"Οι συνδερίες μασάζ του {ToString()} έχουν τελειώσει");
                }
            }
            else
            {
                int remain = RemainingMassageDays;
                ShowUps.Add(new ShowUp { Arrive = arrived, Arrived = DateTime.Now, Massage = mass });
                if (RemainingTrainingDays == 0)
                {
                    if (remain > 0)
                        MessageBox.Show($"Αυτή ήταν η τελευταία συνδερία γυμναστικής του {ToString()}");

                    else
                        MessageBox.Show($"Οι συνδερίες γυμναστικής του {ToString()} έχουν τελειώσει");
                }
            }
        }

        private async Task AddOldShowUp()
        {
            ShowUps.Add(new ShowUp { Arrived = OldShowUpDate, Massage = IsMassage, Left = new DateTime(1234, 1, 1) });
            await BasicDataManager.SaveAsync();
            IsMassage = false;
        }

        private bool CanAddPayment()
        {
            return PaymentAmount > 0 && PaymentAmount <= RemainingAmount && ((SelectedProgramToDelete != null && PaymentAmount <= SelectedProgramToDelete.RemainingAmount) || (SelectedProgramMassageToDelete != null && PaymentAmount <= SelectedProgramMassageToDelete.RemainingAmount));
        }

        private bool CanMakeBooking(string arg) => ProgramDataCheck();

        private bool CanSaveChanges()
        {
            return BasicDataManager.HasChanges();
        }

        private bool CanSet()
        {
            return (SelectedPayment != null || SelectedPaymentMass != null) && (SelectedProgramToDelete != null || SelectedProgramMassageToDelete != null);
        }

        private async Task DeletemassShowUp()
        {
            Changes.Add(new Change($"Διαγράφηκε ΠΑΡΟΥΣΙΑ massage  με ημερομηνία {SelectedMassShowUp.Arrived.ToString("ddd dd/MM/yy")}", StaticResources.User));

            BasicDataManager.Delete(SelectedMassShowUp);
            await BasicDataManager.SaveAsync();
        }

        private async Task DeletePayment()
        {
            Changes.Add(new Change($"Διαγράφηκε ΠΛΗΡΩΜΗ {SelectedPayment.Amount}€ που είχε γίνει {SelectedPayment.Date.ToString("ddd dd/MM/yy")} για γυμναστική", StaticResources.User));
            if (SelectedPayment.Program != null && SelectedPayment.Program.Payments.Any(p => p.Id == SelectedPayment.Id))
            {
                SelectedPayment.Program.Payments.Remove(SelectedPayment);
            }
            BasicDataManager.Delete(SelectedPayment);
            RaisePropertyChanged(nameof(PaymentVisibility));
            await BasicDataManager.SaveAsync();
        }

        private async Task DeletePaymentMass()
        {
            Changes.Add(new Change($"Διαγράφηκε ΠΛΗΡΩΜΗ {SelectedPaymentMass.Amount}€ που είχε γίνει {SelectedPaymentMass.Date.ToString("ddd dd/MM/yy")} για μασάζ", StaticResources.User));
            if (SelectedPaymentMass.Program != null && SelectedPaymentMass.Program.Payments.Any(p => p.Id == SelectedPaymentMass.Id))
            {
                SelectedPaymentMass.Program.Payments.Remove(SelectedPaymentMass);
            }
            BasicDataManager.Delete(SelectedPaymentMass);
            RaisePropertyChanged(nameof(PaymentVisibility));
            await BasicDataManager.SaveAsync();
        }

        private async Task DeleteProgram()
        {
            Changes.Add(new Change($"Διαγράφηκε ΠΡΟΓΡΑΜΜΑ Γυμναστικής  {SelectedProgramToDelete} που είχε καταχωρηθεί {SelectedProgramToDelete.DayOfIssue.ToString("ddd dd/MM/yy")} με " +
                $"διάρκεια {SelectedProgramToDelete.Showups} Αξίας{SelectedProgramToDelete.Amount} και έναρξη {SelectedProgramToDelete.StartDay.ToString("ddd dd/MM/yy")}", StaticResources.User));

            BasicDataManager.Delete(SelectedProgramToDelete);
            await BasicDataManager.SaveAsync();
        }

        private async Task DeleteProgramMass()
        {
            Changes.Add(new Change($"Διαγράφηκε ΠΡΟΓΡΑΜΜΑ Μασάζ  {SelectedProgramMassageToDelete} που είχε καταχωρηθεί {SelectedProgramMassageToDelete.DayOfIssue.ToString("ddd dd/MM/yy")} με " +
                  $"διάρκεια {SelectedProgramMassageToDelete.Showups} Αξίας{SelectedProgramMassageToDelete.Amount} και έναρξη {SelectedProgramMassageToDelete.StartDay.ToString("ddd dd/MM/yy")}", StaticResources.User));

            BasicDataManager.Delete(SelectedProgramMassageToDelete);
            await BasicDataManager.SaveAsync();
        }

        private async Task DeleteShowUp()
        {
            Changes.Add(new Change($"Διαγράφηκε ΠΑΡΟΥΣΙΑ γυμναστικής με ημερομηνία {SelectedShowUp.Arrived.ToString("ddd dd/MM/yy")}", StaticResources.User));

            BasicDataManager.Delete(SelectedShowUp);
            await BasicDataManager.SaveAsync();
        }

        private void DisableCustomer()
        {
            ForceDisable = !ForceDisable;
            IsActiveColor = GetCustomerColor();
        }

        private SolidColorBrush GetCustomerColor()
        {
            if (IsManualyActive)
            {
                ActiveCustomer = true;
                return new SolidColorBrush(Colors.LightGreen);
            }
            if (ForceDisable)
            {
                ActiveCustomer = false;
                return new SolidColorBrush(Colors.Orange);
            }
            if (ShowUps.Count > 0)
            {
                DateTime lastShowUp = ShowUps.OrderByDescending(s => s.Arrived).FirstOrDefault().Arrived;
                if (lastShowUp > DateTime.Today.AddDays(-45))
                {
                    if (RemainingAmount <= 0)
                    {
                        ActiveCustomer = true;

                        return new SolidColorBrush(Colors.Green);
                    }
                    else if (RemainingAmount > 0)
                    {
                        ActiveCustomer = true;

                        return new SolidColorBrush(Colors.Red);
                    }
                }
                else
                {
                    ActiveCustomer = false;
                    return new SolidColorBrush(Colors.Orange);
                }
            }

            ActiveCustomer = false;
            return new SolidColorBrush(Colors.Orange);
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
            int par = int.Parse(obj);
            AddNewProgram(par);
            if (par == 1)
            {
                MakeProgramPayment();
                Programs.Last().Payments.Add(Payments.Last());
                Programs.Last().CalculateRemainingAmount();
            }
            CalculateRemainingAmount();
            RaisePropertyChanged(nameof(PaymentVisibility));

            ProgramPrice = ShowUpPrice = 0;
            ProgramDuration = NumOfShowUps = 0;
            ProgramTypeIndex = -1;
            DateOfIssue = StartDate = DateTime.Today;
            RaisePropertyChanged(nameof(RemainingAmount));
            ForceDisable = false;
            await BasicDataManager.SaveAsync();
            PaymentsCollectionView.Refresh();
            PaymentsMassCollectionView.Refresh();
        }

        private void PaymentsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (Payment item in e.OldItems)
                {
                    //Removed items
                    item.PropertyChanged -= PaymentPropertyChanged;
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (Payment item in e.NewItems)
                {
                    //Added items
                    item.PropertyChanged += PaymentPropertyChanged;
                }
            }

            CalculateRemainingAmount();
            GetRemainingDays();

            RaisePropertyChanged(nameof(PaymentVisibility));
        }

        private void PaymentPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Payment.Amount))
                CalculateRemainingAmount();

        }

        private void ProgramsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (Program item in e.OldItems)
                {
                    //Removed items
                    item.PropertyChanged -= ProgramPropertyChanged;
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (Program item in e.NewItems)
                {
                    //Added items
                    item.PropertyChanged += ProgramPropertyChanged;
                }
            }
            CalculateRemainingAmount();
            GetRemainingDays();
        }

        private void ProgramPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Program.Amount) || e.PropertyName == nameof(Program.Showups))
            {
                CalculateRemainingAmount();
                GetRemainingDays();
            }

        }

        private bool ProgramsgymFilter(object obj)
        {
            try
            {
                return (obj is Program p && !p.IsMassage) ||
                    (obj is ShowUp s && !s.Massage) ||
                    (obj is Payment a && (a.Program == null || !a.Program.IsMassage));
            }
            catch (Exception ex)
            {
                // MessengerInstance.Send(new ShowExceptionMessage_Message(ex.Message));
                return false;
            }
        }

        private bool ProgramsmasFilter(object obj)
        {
            try
            {
                return (obj is Program p && p.IsMassage) ||
                    (obj is ShowUp s && s.Massage) ||
                    (obj is Payment a && a.Program != null && a.Program.IsMassage);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private async Task SaveChanges()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            GetRemainingDays();
            await BasicDataManager.SaveAsync();
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private async Task SetToPayment()
        {
            if (SelectedProgramToDelete != null)
            {
                SelectedProgramToDelete.Payments.Add(SelectedPayment);
                SelectedPayment.RaisePropertyChanged("PaymentColor");
            }
            else if (SelectedProgramMassageToDelete != null)
            {
                SelectedProgramMassageToDelete.Payments.Add(SelectedPaymentMass);
                SelectedPaymentMass.RaisePropertyChanged("PaymentColor");
            }
            await BasicDataManager.SaveAsync();
            GetRemainingDays();
        }

        private async Task ShowPreviewsData()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            Loaded = false;
            await BasicDataManager.Context.GetFullCustomerById(Id);
            Loaded = true;

            SetColors();
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void ShowUps_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            IsActiveColor = GetCustomerColor();
            GetRemainingDays();
            RaisePropertyChanged(nameof(ShowedUpToday));
            RaisePropertyChanged(nameof(ShowUpsCollectionView));
            RaisePropertyChanged(nameof(ShowUpsMassCollectionView));
            //ShowUpsCollectionView.Refresh();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            RaisePropertyChanged(nameof(Duration));
        }

        private async Task TogleMassage()
        {
            SelectedShowUp.Massage = !SelectedShowUp.Massage;
            await BasicDataManager.SaveAsync();
            GetRemainingDays();
        }

        private async Task TogleMassageb()
        {
            SelectedMassShowUp.Massage = !SelectedMassShowUp.Massage;
            await BasicDataManager.SaveAsync();
            GetRemainingDays();
        }

        private async Task TogleReal()
        {
            SelectedShowUp.Real = !SelectedShowUp.Real;
            await BasicDataManager.SaveAsync();
        }

        private async Task TogleRealb()
        {
            SelectedMassShowUp.Real = !SelectedMassShowUp.Real;
            await BasicDataManager.SaveAsync();
        }

        private void TryRefrehAllCollections()
        {
            GetRemainingDays();
            ProgramsMassageColelctionView = new CollectionViewSource { Source = Programs }.View;

            ProgramsColelctionView = new CollectionViewSource { Source = Programs }.View;
            ProgramsColelctionView.SortDescriptions.Add(new SortDescription("StartDay", ListSortDirection.Descending));
            ProgramsColelctionView.Filter = ProgramsgymFilter;

            ShowUpsCollectionView = new CollectionViewSource { Source = ShowUps }.View;
            ShowUpsCollectionView.SortDescriptions.Add(new SortDescription("Arrived", ListSortDirection.Descending));
            ShowUpsCollectionView.Filter = ProgramsgymFilter;

            ShowUpsMassCollectionView = new CollectionViewSource { Source = ShowUps }.View;
            ShowUpsMassCollectionView.SortDescriptions.Add(new SortDescription("Arrived", ListSortDirection.Descending));
            ShowUpsMassCollectionView.Filter = ProgramsmasFilter; ;
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
        private void WeigthsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(BMI));
        }

        #endregion Methods
    }
}