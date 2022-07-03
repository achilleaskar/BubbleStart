using BubbleStart.Helpers;
using BubbleStart.Messages;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace BubbleStart.Model
{
    [Table("BubbleCustomers")]
    public class Customer : BaseModel
    {
        public Customer()
        {
        }

        public Customer(bool isnew)
        {
            if (isnew)
            {
                InitialLoad();
                Illness = new Illness();
            }
        }

        #region Fields

        private bool _ActiveCustomer;
        private string _Address;
        private bool _Alcohol;
        private int _AlcoholUsage;
        private ObservableCollection<Apointment> _Apointments;
        private DateTime _AppointmentTime;
        private BasicDataManager _BasicDataManager;
        private bool _CanAdd;
        private bool _CertifOfIllness;
        private ObservableCollection<Change> _Changes;
        private DateTime _DateOfIssue;
        private DateTime _DateOfPayment;
        private string _DistrictText;
        private DateTime _DOB;
        private bool _Doctor;
        private string _Email;
        private string _ExtraNotes;
        private string _ExtraReasons;
        private DateTime _FirstDate;
        private ForceDisable _ForceDisable;
        private bool _Gender;
        private int _Height;
        private string _HistoryDuration;
        private DateTime _HistoryFrom = DateTime.Today.AddMonths(-3);
        private string _HistoryKind;
        private bool _HistoryNotFirstTime;
        private int _HistoryTimesPerWeek;
        private Illness _Illness;
        private bool _Is30min;
        private SolidColorBrush _IsActiveColor;
        private bool _IsDateValid;
        private bool _IsPracticing;
        private bool _IsSelected;
        private ObservableCollection<ItemPurchase> _Items;
        private string _Job;
        private ShowUp _LastGymShowUp;
        private bool _Loaded;
        private bool _Medicine;
        private string _MedicineText;
        private string _Name;
        private decimal _NewWeight;
        private ObservableCollection<Apointment> _NextAppointments;
        private decimal _NextPayment;
        private string _Notes;
        private int _NumOfShowUps;
        private DateTime _OldShowUpDate;
        private ObservableCollection<ShowUp> _OldShowUps;
        private decimal _PaymentAmount;
        private bool _PaymentReciept;
        private List<Payment> _Payments;
        private ICollectionView _PaymentsAerialYogaCollectionView;
        private ICollectionView _PaymentsFunctionalCollectionView;
        private ICollectionView _PaymentsFunctionalPilatesCollectionView;
        private ICollectionView _PaymentsMassCollectionView;
        private ICollectionView _PaymentsMedicalCollectionView;
        private ICollectionView _PaymentsOnlineCollectionView;
        private ICollectionView _PaymentsOutDoorCollectionView;
        private ICollectionView _PaymentsPersonalCollectionView;
        private ICollectionView _PaymentsPilatesCollectionView;
        private ObservableCollection<PaymentSum> _PaymentSums;
        private ICollectionView _PaymentsYogaCollectionView;
        private PaymentType _PaymentType;
        private bool _Popup1Open;

        private bool _PopupFinishOpen;
        private bool _PreferedHand;

        private int _ProgramDuration;

        //  private bool _Pregnancy;
        private decimal _ProgramPrice;

        private string _ProgramResult;
        private List<Program> _Programs;
        private ICollectionView _ProgramsAerialYogaCollectionView;
        private ICollectionView _ProgramsFunctionalCollectionView;
        private ICollectionView _ProgramsFunctionalPilatesCollectionView;
        private ICollectionView _ProgramsMassageColelctionView;
        private ICollectionView _ProgramsMedicalCollectionView;
        private ICollectionView _ProgramsOnlineColelctionView;
        private ICollectionView _ProgramsOutdoorCollectionView;
        private ICollectionView _ProgramsPersonalCollectionView;
        private ICollectionView _ProgramsPilatesCollectionView;
        private ICollectionView _ProgramsYogaCollectionView;
        private bool _Psek;
        private bool _ReasonInjury;
        private bool _ReasonPower;
        private bool _ReasonSlim;
        private bool _ReasonVeltiwsh;
        private decimal _RemainingAmount;
        private DateTime _ResetDate;
        private List<BodyPartSelection> _SecBodyParts;
        private Payment _SelectedAerialYogaPayment;
        private Program _SelectedAerialYogaProgram;
        private ShowUp _SelectedAerialYogaShowUp;
        private BodyPart _SelectedBodyPart;
        private Change _SelectedChange;
        private ClothColors? _SelectedColor;
        private Payment _SelectedFunctionalPayment;
        private Payment _SelectedFunctionalPilatesPayment;
        private Program _SelectedFunctionalPilatesProgram;
        private ShowUp _SelectedFunctionalPilatesShowUp;
        private Program _SelectedFunctionalProgram;
        private ShowUp _SelectedFunctionalShowUp;
        private Item _SelectedItem;
        private Program _SelectedMasageProgram;
        private Payment _SelectedMassPayment;
        private ShowUp _SelectedMassShowUp;
        private Payment _SelectedMedicalPayment;
        private Program _SelectedMedicalProgram;
        private ShowUp _SelectedMedicalShowUp;
        private Payment _SelectedOnlinePayment;
        private Program _SelectedOnlineProgram;
        private ShowUp _SelectedOnlineShowUp;
        private Payment _SelectedOutdoorPayment;
        private Program _SelectedOutdoorProgram;
        private ShowUp _SelectedOutdoorShowUp;
        private Payment _SelectedPersonalPayment;
        private Program _SelectedPersonalProgmam;
        private ShowUp _SelectedPersonalShowUp;
        private Payment _SelectedPilatesPayment;
        private Program _SelectedPilatesProgram;
        private ShowUp _SelectedPilatesShowUp;
        private Program _SelectedProgramAerialYogaToDelete;
        private Program _SelectedProgramFunctionalPilatesToDelete;
        private Program _SelectedProgramFunctionalToDelete;
        private Program _SelectedProgramMassageToDelete;
        private Program _SelectedProgramMedicalToDelete;
        private Program _SelectedProgramOnlineToDelete;
        private Program _SelectedProgramOutDoorToDelete;
        private Program _SelectedProgramPersonalToDelete;
        private Program _SelectedProgramPilatesToDelete;
        private ProgramType _SelectedProgramType;
        private Program _SelectedProgramYogaToDelete;
        private ShowUp _SelectedShowUpToEditBP;
        private SizeEnum? _SelectedSize;
        private Payment _SelectedYogaPayment;
        private Program _SelectedYogaProgram;
        private ShowUp _SelectedYogaShowUp;
        private decimal _ShowUpPrice;
        private List<ShowUp> _ShowUps;
        private ICollectionView _ShowUpsAerialYogaCollectionView;
        private ICollectionView _ShowUpsFunctionalCollectionView;
        private ICollectionView _ShowUpsFunctionalPilatesCollectionView;
        private ICollectionView _ShowUpsMassCollectionView;
        private ICollectionView _ShowUpsMedicalCollectionView;
        private ICollectionView _ShowUpsOnlineCollectionView;
        private ICollectionView _ShowUpsOutDoorCollectionView;
        private ICollectionView _ShowUpsPersonalCollectionView;
        private ICollectionView _ShowUpsPilatesCollectionView;
        private ICollectionView _ShowUpsYogaCollectionView;
        private bool _Smoker;

        //private bool _Signed;
        private int _SmokingUsage;

        private DateTime _StartDate;
        private string _SureName;
        private bool _Surgery;
        private string _SurgeryInfo;
        private string _Tel;
        private bool _ThirdDose;

        private bool _Vacinated;

        private bool _WantToQuit;

        private ObservableCollection<Weight> _WeightHistory;

        #endregion Fields

        #region Properties

        private bool _Maliar;

        public bool Maliar
        {
            get
            {
                return _Maliar;
            }

            set
            {
                if (_Maliar == value)
                {
                    return;
                }

                _Maliar = value;
                RaisePropertyChanged();
            }
        }

        public string Active
        {
            get
            {
                if (RemainingTrainingDays > 0 && SelectedFunctionalProgram != null)
                    return $"ΝΑΙ (έως {SelectedFunctionalProgram.StartDay.AddMonths(SelectedFunctionalProgram.Months):dd/MM})";
                if (RemainingPilatesDays > 0 && SelectedPilatesProgram != null)
                    return $"ΝΑΙ (έως {SelectedPilatesProgram.StartDay.AddMonths(SelectedPilatesProgram.Months):dd/MM})";
                if (RemainingFunctionalPilatesDays > 0 && SelectedFunctionalPilatesProgram != null)
                    return $"ΝΑΙ (έως {SelectedFunctionalPilatesProgram.StartDay.AddMonths(SelectedFunctionalPilatesProgram.Months):dd/MM})";
                if (RemainingOnlineDays > 0 && SelectedOnlineProgram != null)
                    return $"ΝΑΙ (έως {SelectedOnlineProgram.StartDay.AddMonths(SelectedOnlineProgram.Months):dd/MM})";
                if (RemainingOutDoorDays > 0 && SelectedOutdoorProgram != null)
                    return $"ΝΑΙ (έως {SelectedOutdoorProgram.StartDay.AddMonths(SelectedOutdoorProgram.Months):dd/MM})";
                if (RemainingMassageDays > 0 && SelectedMasageProgram != null)
                    return $"ΝΑΙ (έως {SelectedMasageProgram.StartDay.AddMonths(SelectedMasageProgram.Months):dd/MM})";
                if (RemainingYogaDays > 0 && SelectedYogaProgram != null)
                    return $"ΝΑΙ (έως {SelectedYogaProgram.StartDay.AddMonths(SelectedYogaProgram.Months):dd/MM})";
                if (RemainingAerialYogaDays > 0 && SelectedAerialYogaProgram != null)
                    return $"ΝΑΙ (έως {SelectedAerialYogaProgram.StartDay.AddMonths(SelectedAerialYogaProgram.Months):dd/MM})";
                if (RemainingPersonalDays > 0 && SelectedPersonalProgmam != null)
                    return $"ΝΑΙ (έως {SelectedPersonalProgmam.StartDay.AddMonths(SelectedPersonalProgmam.Months):dd/MM})";
                if (RemainingMedicalDays > 0 && SelectedMedicalProgram != null)
                    return $"ΝΑΙ (έως {SelectedMedicalProgram.StartDay.AddMonths(SelectedMedicalProgram.Months):dd/MM})";
                return "OXI";
            }
        }

        [NotMapped]
        public bool ActiveCustomer
        {
            get
            { //TODO
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
        public RelayCommand AddItemCommand { get; set; }

        [NotMapped]
        public RelayCommand<int> AddOldShowUpCommand { get; set; }

        public string Address
        {
            get => _Address;

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
            get => _Alcohol;

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
            get => _AlcoholUsage;

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
            get => _Apointments;

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
        public DateTime AppointmentTime
        {
            get
            {
                return _AppointmentTime;
            }

            set
            {
                if (_AppointmentTime == value)
                {
                    return;
                }

                _AppointmentTime = value;
                RaisePropertyChanged();
            }
        }

        [NotMapped]
        public SolidColorBrush BackGround => IsPracticing ? new SolidColorBrush(Colors.LightGreen) : new SolidColorBrush(Colors.White);

        [NotMapped]
        public BasicDataManager BasicDataManager
        {
            get => _BasicDataManager;

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
        public decimal BMI => Math.Round((WeightHistory.Count > 0 && LastWeight > 0 && Height > 0) ? LastWeight / (Height * Height / (decimal)10000) : 0, 2);

        [NotMapped]
        public RelayCommand<string> BookCommand { get; set; }

        [NotMapped]
        public bool CanAdd
        {
            get
            {
                return _CanAdd;
            }

            set
            {
                if (_CanAdd == value)
                {
                    return;
                }

                _CanAdd = value;
                RaisePropertyChanged();
            }
        }

        [NotMapped]
        public RelayCommand CancelChangesAsyncCommand { get; set; }

        public bool CertifOfIllness
        {
            get
            {
                return _CertifOfIllness;
            }

            set
            {
                if (_CertifOfIllness == value)
                {
                    return;
                }

                _CertifOfIllness = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<Change> Changes
        {
            get => _Changes;

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
        public RelayCommand CustomerLeftCommand { get; set; }

        [NotMapped]
        public DateTime DateOfIssue
        {
            get => _DateOfIssue;

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
            get => _DateOfPayment;

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
        public RelayCommand<object> DeleteItemCommand { get; set; }

        [NotMapped]
        public RelayCommand<Payment> DeletePaymentCommand { get; set; }

        [NotMapped]
        public RelayCommand<Program> DeleteProgramCommand { get; set; }

        [NotMapped]
        public RelayCommand<ShowUp> DeleteShowUpCommand { get; set; }

        public string DistrictText
        {
            get => _DistrictText;

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

        //[NotMapped]
        //public RelayCommand DisableCustomerCommand { get; set; }
        [Required]
        [DataType(DataType.Date, ErrorMessage = "Η ημερομηνία δεν έχει τη σωστή μορφή")]
        public DateTime DOB
        {
            get => _DOB;

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

        public bool Doctor
        {
            get
            {
                return _Doctor;
            }

            set
            {
                if (_Doctor == value)
                {
                    return;
                }

                _Doctor = value;
                RaisePropertyChanged();
            }
        }

        public TimeSpan Duration => LastShowUp != null ? DateTime.Now.Subtract(LastShowUp.Arrived) : new TimeSpan(0);

        [NotMapped]
        public bool EditedInCustomerManagement { get; internal set; }

        [StringLength(30, MinimumLength = 0)]
        [DataType(DataType.EmailAddress, ErrorMessage = "Το Email δεν έχει τη σωστή μορφή")]
        [EmailAddress(ErrorMessage = "Το Email δεν έχει τη σωστή μορφή")]
        public string Email
        {
            get => _Email;

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
            get => _ExtraNotes;

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
            get => _ExtraReasons;

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
            get => _FirstDate;

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

        public ForceDisable ForceDisable
        {
            get => _ForceDisable;

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

        [NotMapped]
        public bool FromProgram { get; set; } = false;

        [NotMapped]
        public bool Full { get; set; }

        public string FullName => Name + " " + SureName;

        public bool Gender
        {
            get => _Gender;

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
            get => _Height;

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
            get => _HistoryDuration;

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

        [NotMapped]
        public DateTime HistoryFrom
        {
            get
            {
                return _HistoryFrom;
            }

            set
            {
                if (_HistoryFrom == value)
                {
                    return;
                }

                _HistoryFrom = value;
                RaisePropertyChanged();
            }
        }

        public string HistoryKind
        {
            get => _HistoryKind;

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
            get => _HistoryNotFirstTime;

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
            get => _HistoryTimesPerWeek;

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
            get => _Illness;

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
        public bool Is30min
        {
            get
            {
                return _Is30min;
            }

            set
            {
                if (_Is30min == value)
                {
                    return;
                }

                _Is30min = value;
                RaisePropertyChanged();
            }
        }

        [NotMapped]
        public SolidColorBrush IsActiveColor
        {
            get => _IsActiveColor ?? (_IsActiveColor = GetCustomerColor()); //TODO

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
            get => _IsDateValid;

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

        public bool IsNotPracticing => !IsPracticing;

        [NotMapped]
        public bool IsPracticing
        {
            get => _IsPracticing;

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
            get => _IsSelected;

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

        public ObservableCollection<ItemPurchase> Items
        {
            get
            {
                return _Items;
            }

            set
            {
                if (_Items == value)
                {
                    return;
                }

                _Items = value;
                RaisePropertyChanged();
            }
        }

        public string Job
        {
            get => _Job;

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

        public DateTime LastBuy => GetLastBuy();

        [NotMapped]
        public ShowUp LastGymShowUp
        {
            get
            {
                return _LastGymShowUp;
            }

            set
            {
                if (_LastGymShowUp == value)
                {
                    return;
                }

                _LastGymShowUp = value;
                RaisePropertyChanged();
            }
        }

        public string LastPart => GetLastPart();

        public ShowUp LastShowUp => ShowUps != null && ShowUps.Count > 0 ? ShowUps.OrderBy(x => x.Id).Last() : null;

        public decimal LastWeight => Math.Round((WeightHistory.Count > 0 && WeightHistory.OrderBy(x => x.Id).Last().WeightValue > 0) ? WeightHistory.Last().WeightValue : 0, 2);

        [NotMapped]
        public bool Loaded
        {
            get => _Loaded;

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
            get => _Medicine;

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
            get => _MedicineText;

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
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Το όνομα μπορεί να είναι από 3 έως 20 χαρακτήρες.")]
        public string Name
        {
            get => _Name;

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
            get => _NewWeight;

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
        public ObservableCollection<Apointment> NextAppointments
        {
            get
            {
                return _NextAppointments;
            }

            set
            {
                if (_NextAppointments == value)
                {
                    return;
                }

                _NextAppointments = value;
                RaisePropertyChanged();
            }
        }

        [NotMapped]
        public decimal NextPayment
        {
            get => _NextPayment;

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

        [StringLength(1000)]
        public string Notes
        {
            get
            {
                return _Notes;
            }

            set
            {
                if (_Notes == value)
                {
                    return;
                }

                _Notes = value;
                RaisePropertyChanged();
            }
        }

        [NotMapped]
        public int NumOfShowUps
        {
            get => _NumOfShowUps;

            set
            {
                if (_NumOfShowUps == value)
                {
                    return;
                }

                _NumOfShowUps = value;
                if (value > 0)
                    ShowUpPrice = ProgramPrice / value;
                else
                    ShowUpPrice = 0;

                RaisePropertyChanged();
            }
        }

        [NotMapped]
        public DateTime OldShowUpDate
        {
            get => _OldShowUpDate;

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
        public ObservableCollection<ShowUp> OldShowUps
        {
            get
            {
                return _OldShowUps;
            }

            set
            {
                if (_OldShowUps == value)
                {
                    return;
                }

                _OldShowUps = value;
                RaisePropertyChanged();
            }
        }

        [NotMapped]
        public RelayCommand OpenPopup1Command { get; set; }

        [NotMapped]
        public decimal PaymentAmount
        {
            get => _PaymentAmount;

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

        [NotMapped]
        public bool PaymentReciept
        {
            get
            {
                return _PaymentReciept;
            }

            set
            {
                if (_PaymentReciept == value)
                {
                    return;
                }

                _PaymentReciept = value;
                RaisePropertyChanged();
            }
        }

        public List<Payment> Payments
        {
            get => _Payments;

            set
            { //TODO
                if (_Payments == value)
                {
                    return;
                }

                _Payments = value;
                if (value != null)
                {
                    //Payments.CollectionChanged += PaymentsCollectionChanged;
                    UpdatePaymentsCollections();
                }
                else
                    ClearPaymentsCollections();

                RaisePropertyChanged();
            }
        }

        public ICollectionView PaymentsAerialYogaCollectionView
        {
            get
            {
                return _PaymentsAerialYogaCollectionView;
            }

            set
            {
                if (_PaymentsAerialYogaCollectionView == value)
                {
                    return;
                }

                _PaymentsAerialYogaCollectionView = value;
                RaisePropertyChanged();
            }
        }

        public ICollectionView PaymentsFunctionalCollectionView
        {
            get => _PaymentsFunctionalCollectionView;

            set
            {
                if (_PaymentsFunctionalCollectionView == value)
                {
                    return;
                }

                _PaymentsFunctionalCollectionView = value;
                RaisePropertyChanged();
            }
        }

        public ICollectionView PaymentsFunctionalPilatesCollectionView
        {
            get
            {
                return _PaymentsFunctionalPilatesCollectionView;
            }

            set
            {
                if (_PaymentsFunctionalPilatesCollectionView == value)
                {
                    return;
                }

                _PaymentsFunctionalPilatesCollectionView = value;
                RaisePropertyChanged();
            }
        }

        public ICollectionView PaymentsMassCollectionView
        {
            get => _PaymentsMassCollectionView;

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

        public ICollectionView PaymentsMedicalCollectionView
        {
            get
            {
                return _PaymentsMedicalCollectionView;
            }

            set
            {
                if (_PaymentsMedicalCollectionView == value)
                {
                    return;
                }

                _PaymentsMedicalCollectionView = value;
                RaisePropertyChanged();
            }
        }

        public ICollectionView PaymentsOnlineCollectionView
        {
            get => _PaymentsOnlineCollectionView;

            set
            {
                if (_PaymentsOnlineCollectionView == value)
                {
                    return;
                }

                _PaymentsOnlineCollectionView = value;
                RaisePropertyChanged();
            }
        }

        public ICollectionView PaymentsOutDoorCollectionView
        {
            get
            {
                return _PaymentsOutDoorCollectionView;
            }

            set
            {
                if (_PaymentsOutDoorCollectionView == value)
                {
                    return;
                }

                _PaymentsOutDoorCollectionView = value;
                RaisePropertyChanged();
            }
        }

        public ICollectionView PaymentsPersonalCollectionView
        {
            get
            {
                return _PaymentsPersonalCollectionView;
            }

            set
            {
                if (_PaymentsPersonalCollectionView == value)
                {
                    return;
                }

                _PaymentsPersonalCollectionView = value;
                RaisePropertyChanged();
            }
        }

        public ICollectionView PaymentsPilatesCollectionView
        {
            get
            {
                return _PaymentsPilatesCollectionView;
            }

            set
            {
                if (_PaymentsPilatesCollectionView == value)
                {
                    return;
                }

                _PaymentsPilatesCollectionView = value;
                RaisePropertyChanged();
            }
        }

        [NotMapped]
        public ObservableCollection<PaymentSum> PaymentSums
        {
            get
            {
                return _PaymentSums;
            }

            set
            {
                if (_PaymentSums == value)
                {
                    return;
                }

                _PaymentSums = value;
                RaisePropertyChanged();
            }
        }

        public ICollectionView PaymentsYogaCollectionView
        {
            get
            {
                return _PaymentsYogaCollectionView;
            }

            set
            {
                if (_PaymentsYogaCollectionView == value)
                {
                    return;
                }

                _PaymentsYogaCollectionView = value;
                RaisePropertyChanged();
            }
        }

        [NotMapped]
        public PaymentType PaymentType
        {
            get
            {
                return _PaymentType;
            }

            set
            {
                if (_PaymentType == value)
                {
                    return;
                }

                _PaymentType = value;
                RaisePropertyChanged();
            }
        }

        public Visibility PaymentVisibility => RemainingAmount > 0 ? Visibility.Visible : Visibility.Collapsed;

        [NotMapped]
        public bool Popup1Open
        {
            get
            {
                return _Popup1Open;
            }

            set
            {
                if (_Popup1Open == value)
                {
                    return;
                }

                _Popup1Open = value;
                RaisePropertyChanged();
            }
        }

        [NotMapped]
        public bool PopupFinishOpen
        {
            get
            {
                return _PopupFinishOpen;
            }

            set
            {
                if (_PopupFinishOpen == value)
                {
                    return;
                }

                _PopupFinishOpen = value;
                RaisePropertyChanged();
            }
        }

        public bool PreferedHand
        {
            get => _PreferedHand;

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

        [NotMapped]
        public int ProgramDuration
        {
            get => _ProgramDuration;

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

        //        _Pregnancy = value;
        //        RaisePropertyChanged();
        //    }
        //}
        [NotMapped]
        public decimal ProgramPrice
        {
            get => _ProgramPrice;

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

        //    set
        //    {
        //        if (_Pregnancy == value)
        //        {
        //            return;
        //        }
        [NotMapped]
        public string ProgramResult
        {
            get => _ProgramResult;

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

        //public bool Pregnancy
        //{
        //    get => _Pregnancy;
        public List<Program> Programs
        {
            get => _Programs;

            set
            { //TODO
                if (_Programs == value)
                {
                    return;
                }

                _Programs = value;
                if (value != null)
                {
                    //Programs.CollectionChanged += ProgramsCollectionChanged;
                    UpdateProgramsCollections();
                }
                else
                    ClearProgramsCollections();
                RaisePropertyChanged();
            }
        }

        public ICollectionView ProgramsAerialYogaCollectionView
        {
            get
            {
                return _ProgramsAerialYogaCollectionView;
            }

            set
            {
                if (_ProgramsAerialYogaCollectionView == value)
                {
                    return;
                }

                _ProgramsAerialYogaCollectionView = value;
                RaisePropertyChanged();
            }
        }

        public ICollectionView ProgramsFunctionalCollectionView
        {
            get => _ProgramsFunctionalCollectionView;

            set
            {
                if (_ProgramsFunctionalCollectionView == value)
                {
                    return;
                }

                _ProgramsFunctionalCollectionView = value;
                RaisePropertyChanged();
            }
        }

        public ICollectionView ProgramsFunctionalPilatesCollectionView
        {
            get
            {
                return _ProgramsFunctionalPilatesCollectionView;
            }

            set
            {
                if (_ProgramsFunctionalPilatesCollectionView == value)
                {
                    return;
                }

                _ProgramsFunctionalPilatesCollectionView = value;

                RaisePropertyChanged();
            }
        }

        public ICollectionView ProgramsMassageColelctionView
        {
            get => _ProgramsMassageColelctionView;

            set
            {
                if (_ProgramsMassageColelctionView == value)
                {
                    return;
                }

                _ProgramsMassageColelctionView = value;

                RaisePropertyChanged();
            }
        }

        public ICollectionView ProgramsMedicalCollectionView
        {
            get
            {
                return _ProgramsMedicalCollectionView;
            }

            set
            {
                if (_ProgramsMedicalCollectionView == value)
                {
                    return;
                }

                _ProgramsMedicalCollectionView = value;
                RaisePropertyChanged();
            }
        }

        public ICollectionView ProgramsOnlineColelctionView
        {
            get => _ProgramsOnlineColelctionView;

            set
            {
                if (_ProgramsOnlineColelctionView == value)
                {
                    return;
                }

                _ProgramsOnlineColelctionView = value;
                RaisePropertyChanged();
            }
        }

        public ICollectionView ProgramsOutdoorCollectionView
        {
            get
            {
                return _ProgramsOutdoorCollectionView;
            }

            set
            {
                if (_ProgramsOutdoorCollectionView == value)
                {
                    return;
                }

                _ProgramsOutdoorCollectionView = value;
                RaisePropertyChanged();
            }
        }

        public ICollectionView ProgramsPersonalCollectionView
        {
            get
            {
                return _ProgramsPersonalCollectionView;
            }

            set
            {
                if (_ProgramsPersonalCollectionView == value)
                {
                    return;
                }

                _ProgramsPersonalCollectionView = value;
                RaisePropertyChanged();
            }
        }

        public ICollectionView ProgramsPilatesCollectionView
        {
            get
            {
                return _ProgramsPilatesCollectionView;
            }

            set
            {
                if (_ProgramsPilatesCollectionView == value)
                {
                    return;
                }

                _ProgramsPilatesCollectionView = value;

                RaisePropertyChanged();
            }
        }

        public ICollectionView ProgramsYogaCollectionView
        {
            get
            {
                return _ProgramsYogaCollectionView;
            }

            set
            {
                if (_ProgramsYogaCollectionView == value)
                {
                    return;
                }

                _ProgramsYogaCollectionView = value;
                RaisePropertyChanged();
            }
        }

        public bool Psek
        {
            get
            {
                return _Psek;
            }

            set
            {
                if (_Psek == value)
                {
                    return;
                }

                _Psek = value;
                RaisePropertyChanged();
            }
        }

        public bool ReasonInjury
        {
            get => _ReasonInjury;

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
            get => _ReasonPower;

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
            get => _ReasonSlim;

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
            get => _ReasonVeltiwsh;

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
        public RelayCommand<Payment> ReleasePaymentCommand { get; set; }

        public int RemainingAerialYogaDays => Programs.Where(p => p.ProgramTypeO?.ProgramMode == ProgramMode.aerialYoga).Sum(p => p.RemainingDays);

        [NotMapped]
        public decimal RemainingAmount
        {
            get => _RemainingAmount;

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

        public int RemainingFunctionalPilatesDays => Programs.Where(p => p.ProgramTypeO?.ProgramMode == ProgramMode.pilatesFunctional).Sum(p => p.RemainingDays);

        public int RemainingMassageDays => Programs.Where(p => p.ProgramTypeO?.ProgramMode == ProgramMode.massage).Sum(p => p.RemainingDays);

        public int RemainingMedicalDays => Programs.Where(p => p.ProgramTypeO?.ProgramMode == ProgramMode.medical).Sum(p => p.RemainingDays);

        public int RemainingOnlineDays => Programs.Where(p => p.ProgramTypeO?.ProgramMode == ProgramMode.online).Sum(p => p.RemainingDays);

        public int RemainingOutDoorDays => Programs.Where(p => p.ProgramTypeO?.ProgramMode == ProgramMode.outdoor).Sum(p => p.RemainingDays);

        public int RemainingPersonalDays => Programs.Where(p => p.ProgramTypeO?.ProgramMode == ProgramMode.personal).Sum(p => p.RemainingDays);

        public int RemainingPilatesDays => Programs.Where(p => p.ProgramTypeO?.ProgramMode == ProgramMode.pilates).Sum(p => p.RemainingDays);

        public int RemainingTrainingDays => Programs.Where(p => p.ProgramTypeO?.ProgramMode == ProgramMode.functional).Sum(p => p.RemainingDays);

        public int RemainingTrainingFullDays => RemainingFunctionalPilatesDays + RemainingTrainingDays + RemainingPilatesDays + RemainingPersonalDays + RemainingMedicalDays;

        public int RemainingYogaDays => Programs.Where(p => p.ProgramTypeO?.ProgramMode == ProgramMode.yoga).Sum(p => p.RemainingDays);

        public int RemainingYogaFullDays => RemainingYogaDays + RemainingAerialYogaDays;

        public DateTime ResetDate
        {
            get
            {
                return _ResetDate;
            }

            set
            {
                if (_ResetDate == value)
                {
                    return;
                }

                _ResetDate = value;
                RaisePropertyChanged();
            }
        }

        [NotMapped]
        public RelayCommand SaveChangesAsyncCommand { get; set; }

        [NotMapped]
        public List<BodyPartSelection> SecBodyParts
        {
            get
            {
                return _SecBodyParts;
            }

            set
            {
                if (_SecBodyParts == value)
                {
                    return;
                }

                _SecBodyParts = value;
                RaisePropertyChanged();
            }
        }

        [NotMapped]
        public Payment SelectedAerialYogaPayment
        {
            get
            {
                return _SelectedAerialYogaPayment;
            }

            set
            {
                if (_SelectedAerialYogaPayment == value)
                {
                    return;
                }

                _SelectedAerialYogaPayment = value;
                RaisePropertyChanged();
            }
        }

        [NotMapped]
        public Program SelectedAerialYogaProgram
        {
            get
            {
                return _SelectedAerialYogaProgram;
            }

            set
            {
                if (_SelectedAerialYogaProgram == value)
                {
                    return;
                }

                _SelectedAerialYogaProgram = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(Active));
                RaisePropertyChanged(nameof(TypeOfProgram));
            }
        }

        [NotMapped]
        public ShowUp SelectedAerialYogaShowUp
        {
            get
            {
                return _SelectedAerialYogaShowUp;
            }

            set
            {
                if (_SelectedAerialYogaShowUp == value)
                {
                    return;
                }

                _SelectedAerialYogaShowUp = value;
                RaisePropertyChanged();
            }
        }

        [NotMapped]
        public BodyPart SelectedBodyPart
        {
            get
            {
                return _SelectedBodyPart;
            }

            set
            {
                if (_SelectedBodyPart == value)
                {
                    return;
                }
                CanAdd = value != BodyPart.Unknown;
                _SelectedBodyPart = value;
                RaisePropertyChanged();
            }
        }

        [NotMapped]
        public Change SelectedChange
        {
            get
            {
                return _SelectedChange;
            }

            set
            {
                if (_SelectedChange == value)
                {
                    return;
                }

                _SelectedChange = value;
                SelectedChange_Changed(value);
                RaisePropertyChanged();
            }
        }

        [NotMapped]
        public ClothColors? SelectedColor
        {
            get
            {
                return _SelectedColor;
            }

            set
            {
                if (_SelectedColor == value)
                {
                    return;
                }

                _SelectedColor = value;
                RaisePropertyChanged();
            }
        }

        [NotMapped]
        public Payment SelectedFunctionalPayment
        {
            get => _SelectedFunctionalPayment;

            set
            {
                if (_SelectedFunctionalPayment == value)
                {
                    return;
                }

                _SelectedFunctionalPayment = value;
                RaisePropertyChanged();
            }
        }

        [NotMapped]
        public Payment SelectedFunctionalPilatesPayment
        {
            get
            {
                return _SelectedFunctionalPilatesPayment;
            }

            set
            {
                if (_SelectedFunctionalPilatesPayment == value)
                {
                    return;
                }

                _SelectedFunctionalPilatesPayment = value;
                RaisePropertyChanged();
            }
        }

        [NotMapped]
        public Program SelectedFunctionalPilatesProgram
        {
            get
            {
                return _SelectedFunctionalPilatesProgram;
            }

            set
            {
                if (_SelectedFunctionalPilatesProgram == value)
                {
                    return;
                }

                _SelectedFunctionalPilatesProgram = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(Active));
                RaisePropertyChanged(nameof(TypeOfProgram));
            }
        }

        [NotMapped]
        public ShowUp SelectedFunctionalPilatesShowUp
        {
            get
            {
                return _SelectedFunctionalPilatesShowUp;
            }

            set
            {
                if (_SelectedFunctionalPilatesShowUp == value)
                {
                    return;
                }

                _SelectedFunctionalPilatesShowUp = value;
                RaisePropertyChanged();
            }
        }

        [NotMapped]
        public Program SelectedFunctionalProgram
        {
            get => _SelectedFunctionalProgram;

            set
            {
                if (_SelectedFunctionalProgram == value)
                {
                    return;
                }

                _SelectedFunctionalProgram = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(Active));
                RaisePropertyChanged(nameof(TypeOfProgram));
            }
        }

        [NotMapped]
        public ShowUp SelectedFunctionalShowUp
        {
            get => _SelectedFunctionalShowUp;

            set
            {
                if (_SelectedFunctionalShowUp == value)
                {
                    return;
                }

                _SelectedFunctionalShowUp = value;
                RaisePropertyChanged();
            }
        }

        [NotMapped]
        public Item SelectedItem
        {
            get
            {
                return _SelectedItem;
            }

            set
            {
                if (_SelectedItem == value)
                {
                    return;
                }

                _SelectedItem = value;
                RaisePropertyChanged();
            }
        }

        [NotMapped]
        public Program SelectedMasageProgram
        {
            get => _SelectedMasageProgram;

            set
            {
                if (_SelectedMasageProgram == value)
                {
                    return;
                }

                _SelectedMasageProgram = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(Active));
                RaisePropertyChanged(nameof(TypeOfProgram));
            }
        }

        [NotMapped]
        public Payment SelectedMassPayment
        {
            get => _SelectedMassPayment;

            set
            {
                if (_SelectedMassPayment == value)
                {
                    return;
                }

                _SelectedMassPayment = value;
                RaisePropertyChanged();
            }
        }

        [NotMapped]
        public ShowUp SelectedMassShowUp
        {
            get => _SelectedMassShowUp;

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
        public Payment SelectedMedicalPayment
        {
            get
            {
                return _SelectedMedicalPayment;
            }

            set
            {
                if (_SelectedMedicalPayment == value)
                {
                    return;
                }

                _SelectedMedicalPayment = value;
                RaisePropertyChanged();
            }
        }

        [NotMapped]
        public Program SelectedMedicalProgram
        {
            get
            {
                return _SelectedMedicalProgram;
            }

            set
            {
                if (_SelectedMedicalProgram == value)
                {
                    return;
                }

                _SelectedMedicalProgram = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(Active));
                RaisePropertyChanged(nameof(TypeOfProgram));
            }
        }

        [NotMapped]
        public ShowUp SelectedMedicalShowUp
        {
            get
            {
                return _SelectedMedicalShowUp;
            }

            set
            {
                if (_SelectedMedicalShowUp == value)
                {
                    return;
                }

                _SelectedMedicalShowUp = value;
                RaisePropertyChanged();
            }
        }

        [NotMapped]
        public Payment SelectedOnlinePayment
        {
            get => _SelectedOnlinePayment;

            set
            {
                if (_SelectedOnlinePayment == value)
                {
                    return;
                }

                _SelectedOnlinePayment = value;
                RaisePropertyChanged();
            }
        }

        [NotMapped]
        public Program SelectedOnlineProgram
        {
            get => _SelectedOnlineProgram;

            set
            {
                if (_SelectedOnlineProgram == value)
                {
                    return;
                }

                _SelectedOnlineProgram = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(Active));
                RaisePropertyChanged(nameof(TypeOfProgram));
            }
        }

        [NotMapped]
        public ShowUp SelectedOnlineShowUp
        {
            get => _SelectedOnlineShowUp;

            set
            {
                if (_SelectedOnlineShowUp == value)
                {
                    return;
                }

                _SelectedOnlineShowUp = value;
                RaisePropertyChanged();
            }
        }

        [NotMapped]
        public Payment SelectedOutdoorPayment
        {
            get
            {
                return _SelectedOutdoorPayment;
            }

            set
            {
                if (_SelectedOutdoorPayment == value)
                {
                    return;
                }

                _SelectedOutdoorPayment = value;
                RaisePropertyChanged();
            }
        }

        [NotMapped]
        public Program SelectedOutdoorProgram
        {
            get
            {
                return _SelectedOutdoorProgram;
            }

            set
            {
                if (_SelectedOutdoorProgram == value)
                {
                    return;
                }

                _SelectedOutdoorProgram = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(Active));
                RaisePropertyChanged(nameof(TypeOfProgram));
            }
        }

        [NotMapped]
        public ShowUp SelectedOutdoorShowUp
        {
            get
            {
                return _SelectedOutdoorShowUp;
            }

            set
            {
                if (_SelectedOutdoorShowUp == value)
                {
                    return;
                }

                _SelectedOutdoorShowUp = value;
                RaisePropertyChanged();
            }
        }

        [NotMapped]
        public Payment SelectedPersonalPayment
        {
            get
            {
                return _SelectedPersonalPayment;
            }

            set
            {
                if (_SelectedPersonalPayment == value)
                {
                    return;
                }

                _SelectedPersonalPayment = value;
                RaisePropertyChanged();
            }
        }

        [NotMapped]
        public Program SelectedPersonalProgmam
        {
            get
            {
                return _SelectedPersonalProgmam;
            }

            set
            {
                if (_SelectedPersonalProgmam == value)
                {
                    return;
                }

                _SelectedPersonalProgmam = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(Active));
                RaisePropertyChanged(nameof(TypeOfProgram));
            }
        }

        [NotMapped]
        public ShowUp SelectedPersonalShowUp
        {
            get
            {
                return _SelectedPersonalShowUp;
            }

            set
            {
                if (_SelectedPersonalShowUp == value)
                {
                    return;
                }

                _SelectedPersonalShowUp = value;
                RaisePropertyChanged();
            }
        }

        [NotMapped]
        public Payment SelectedPilatesPayment
        {
            get
            {
                return _SelectedPilatesPayment;
            }

            set
            {
                if (_SelectedPilatesPayment == value)
                {
                    return;
                }

                _SelectedPilatesPayment = value;
                RaisePropertyChanged();
            }
        }

        [NotMapped]
        public Program SelectedPilatesProgram
        {
            get
            {
                return _SelectedPilatesProgram;
            }

            set
            {
                if (_SelectedPilatesProgram == value)
                {
                    return;
                }

                _SelectedPilatesProgram = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(Active));
                RaisePropertyChanged(nameof(TypeOfProgram));
            }
        }

        [NotMapped]
        public ShowUp SelectedPilatesShowUp
        {
            get
            {
                return _SelectedPilatesShowUp;
            }

            set
            {
                if (_SelectedPilatesShowUp == value)
                {
                    return;
                }

                _SelectedPilatesShowUp = value;
                RaisePropertyChanged();
            }
        }

        [NotMapped]
        public Program SelectedProgramAerialYogaToDelete
        {
            get
            {
                return _SelectedProgramAerialYogaToDelete;
            }

            set
            {
                if (_SelectedProgramAerialYogaToDelete == value)
                {
                    return;
                }

                SelectedProgramFunctionalToDelete = null;
                SelectedProgramPilatesToDelete = null;
                SelectedProgramFunctionalPilatesToDelete = null;
                SelectedProgramOnlineToDelete = null;
                SelectedProgramOutDoorToDelete = null;
                SelectedProgramMassageToDelete = null;
                SelectedProgramYogaToDelete = null;
                SelectedProgramPersonalToDelete = null;
                SelectedProgramMedicalToDelete = null;

                _SelectedProgramAerialYogaToDelete = value;

                PaymentAmount = SelectedProgramAerialYogaToDelete != null && SelectedProgramAerialYogaToDelete.RemainingAmount > 0 ? SelectedProgramAerialYogaToDelete.RemainingAmount : 0;

                RaisePropertyChanged();
            }
        }

        [NotMapped]
        public Program SelectedProgramFunctionalPilatesToDelete
        {
            get
            {
                return _SelectedProgramFunctionalPilatesToDelete;
            }

            set
            {
                if (_SelectedProgramFunctionalPilatesToDelete == value)
                {
                    return;
                }

                SelectedProgramFunctionalToDelete = null;
                SelectedProgramPilatesToDelete = null;
                SelectedProgramOnlineToDelete = null;
                SelectedProgramOutDoorToDelete = null;
                SelectedProgramMassageToDelete = null;
                SelectedProgramYogaToDelete = null;
                SelectedProgramAerialYogaToDelete = null;
                SelectedProgramPersonalToDelete = null;
                SelectedProgramMedicalToDelete = null;

                _SelectedProgramFunctionalPilatesToDelete = value;
                PaymentAmount = SelectedProgramFunctionalPilatesToDelete != null && SelectedProgramFunctionalPilatesToDelete.RemainingAmount > 0 ? SelectedProgramFunctionalPilatesToDelete.RemainingAmount : 0;
                RaisePropertyChanged();
            }
        }

        [NotMapped]
        public Program SelectedProgramFunctionalToDelete
        {
            get => _SelectedProgramFunctionalToDelete;

            set
            {
                if (_SelectedProgramFunctionalToDelete == value)
                {
                    return;
                }

                SelectedProgramPilatesToDelete = null;
                SelectedProgramFunctionalPilatesToDelete = null;
                SelectedProgramOnlineToDelete = null;
                SelectedProgramOutDoorToDelete = null;
                SelectedProgramMassageToDelete = null;
                SelectedProgramYogaToDelete = null;
                SelectedProgramAerialYogaToDelete = null;
                SelectedProgramPersonalToDelete = null;
                SelectedProgramMedicalToDelete = null;

                _SelectedProgramFunctionalToDelete = value;
                PaymentAmount = SelectedProgramFunctionalToDelete != null && SelectedProgramFunctionalToDelete.RemainingAmount > 0 ? SelectedProgramFunctionalToDelete.RemainingAmount : 0;

                RaisePropertyChanged();
            }
        }

        [NotMapped]
        public Program SelectedProgramMassageToDelete
        {
            get => _SelectedProgramMassageToDelete;

            set
            {
                if (_SelectedProgramMassageToDelete == value)
                {
                    return;
                }

                SelectedProgramFunctionalToDelete = null;
                SelectedProgramPilatesToDelete = null;
                SelectedProgramFunctionalPilatesToDelete = null;
                SelectedProgramOnlineToDelete = null;
                SelectedProgramOutDoorToDelete = null;
                SelectedProgramYogaToDelete = null;
                SelectedProgramAerialYogaToDelete = null;
                SelectedProgramPersonalToDelete = null;
                SelectedProgramMedicalToDelete = null;

                _SelectedProgramMassageToDelete = value;
                PaymentAmount = SelectedProgramMassageToDelete != null && SelectedProgramMassageToDelete.RemainingAmount > 0
                    ? SelectedProgramMassageToDelete.RemainingAmount : 0;

                RaisePropertyChanged();
            }
        }

        [NotMapped]
        public Program SelectedProgramMedicalToDelete
        {
            get
            {
                return _SelectedProgramMedicalToDelete;
            }

            set
            {
                if (_SelectedProgramMedicalToDelete == value)
                {
                    return;
                }

                SelectedProgramFunctionalToDelete = null;
                SelectedProgramPilatesToDelete = null;
                SelectedProgramFunctionalPilatesToDelete = null;
                SelectedProgramMassageToDelete = null;
                SelectedProgramOnlineToDelete = null;
                SelectedProgramOutDoorToDelete = null;
                SelectedProgramYogaToDelete = null;
                SelectedProgramAerialYogaToDelete = null;
                SelectedProgramPersonalToDelete = null;

                _SelectedProgramMedicalToDelete = value;
                PaymentAmount = SelectedProgramMedicalToDelete != null && SelectedProgramMedicalToDelete.RemainingAmount > 0
                    ? SelectedProgramMedicalToDelete.RemainingAmount : 0;

                RaisePropertyChanged();
            }
        }

        [NotMapped]
        public Program SelectedProgramOnlineToDelete
        {
            get => _SelectedProgramOnlineToDelete;

            set
            {
                if (_SelectedProgramOnlineToDelete == value)
                {
                    return;
                }

                SelectedProgramFunctionalToDelete = null;
                SelectedProgramPilatesToDelete = null;
                SelectedProgramFunctionalPilatesToDelete = null;
                SelectedProgramOutDoorToDelete = null;
                SelectedProgramMassageToDelete = null;
                SelectedProgramYogaToDelete = null;
                SelectedProgramAerialYogaToDelete = null;
                SelectedProgramPersonalToDelete = null;
                SelectedProgramMedicalToDelete = null;

                _SelectedProgramOnlineToDelete = value;
                PaymentAmount = SelectedProgramOnlineToDelete != null && SelectedProgramOnlineToDelete.RemainingAmount > 0 ? SelectedProgramOnlineToDelete.RemainingAmount : 0;

                RaisePropertyChanged();
            }
        }

        [NotMapped]
        public Program SelectedProgramOutDoorToDelete
        {
            get
            {
                return _SelectedProgramOutDoorToDelete;
            }

            set
            {
                if (_SelectedProgramOutDoorToDelete == value)
                {
                    return;
                }
                SelectedProgramFunctionalToDelete = null;
                SelectedProgramPilatesToDelete = null;
                SelectedProgramFunctionalPilatesToDelete = null;
                SelectedProgramOnlineToDelete = null;
                SelectedProgramMassageToDelete = null;
                SelectedProgramYogaToDelete = null;
                SelectedProgramAerialYogaToDelete = null;
                SelectedProgramPersonalToDelete = null;
                SelectedProgramMedicalToDelete = null;

                _SelectedProgramOutDoorToDelete = value;
                PaymentAmount = SelectedProgramOutDoorToDelete != null && SelectedProgramOutDoorToDelete.RemainingAmount > 0 ? SelectedProgramOutDoorToDelete.RemainingAmount : 0;
                RaisePropertyChanged();
            }
        }

        [NotMapped]
        public Program SelectedProgramPersonalToDelete
        {
            get
            {
                return _SelectedProgramPersonalToDelete;
            }

            set
            {
                if (_SelectedProgramPersonalToDelete == value)
                {
                    return;
                }

                SelectedProgramFunctionalToDelete = null;
                SelectedProgramPilatesToDelete = null;
                SelectedProgramFunctionalPilatesToDelete = null;
                SelectedProgramOnlineToDelete = null;
                SelectedProgramOutDoorToDelete = null;
                SelectedProgramYogaToDelete = null;
                SelectedProgramMassageToDelete = null;
                SelectedProgramAerialYogaToDelete = null;
                SelectedProgramMedicalToDelete = null;

                _SelectedProgramPersonalToDelete = value;
                PaymentAmount = SelectedProgramPersonalToDelete != null && SelectedProgramPersonalToDelete.RemainingAmount > 0
                    ? SelectedProgramPersonalToDelete.RemainingAmount : 0;

                RaisePropertyChanged();
            }
        }

        [NotMapped]
        public Program SelectedProgramPilatesToDelete
        {
            get
            {
                return _SelectedProgramPilatesToDelete;
            }

            set
            {
                if (_SelectedProgramPilatesToDelete == value)
                {
                    return;
                }
                SelectedProgramFunctionalToDelete = null;
                SelectedProgramFunctionalPilatesToDelete = null;
                SelectedProgramOnlineToDelete = null;
                SelectedProgramOutDoorToDelete = null;
                SelectedProgramMassageToDelete = null;
                SelectedProgramYogaToDelete = null;
                SelectedProgramAerialYogaToDelete = null;
                SelectedProgramPersonalToDelete = null;
                SelectedProgramMedicalToDelete = null;

                _SelectedProgramPilatesToDelete = value;
                PaymentAmount = SelectedProgramPilatesToDelete != null && SelectedProgramPilatesToDelete.RemainingAmount > 0 ? SelectedProgramPilatesToDelete.RemainingAmount : 0;
                RaisePropertyChanged();
            }
        }

        [NotMapped]
        public ProgramType SelectedProgramType
        {
            get
            {
                return _SelectedProgramType;
            }

            set
            {
                if (_SelectedProgramType == value)
                {
                    return;
                }

                if (value?.Id == 20)
                {
                    NumOfShowUps = 30;
                    ProgramDuration = 1;
                }
                if (value?.Id == 20)
                {
                    NumOfShowUps = 30;
                    ProgramDuration = 1;
                }

                _SelectedProgramType = value;
                RaisePropertyChanged();
            }
        }

        [NotMapped]
        public Program SelectedProgramYogaToDelete
        {
            get
            {
                return _SelectedProgramYogaToDelete;
            }

            set
            {
                if (_SelectedProgramYogaToDelete == value)
                {
                    return;
                }

                SelectedProgramFunctionalToDelete = null;
                SelectedProgramPilatesToDelete = null;
                SelectedProgramFunctionalPilatesToDelete = null;
                SelectedProgramOnlineToDelete = null;
                SelectedProgramOutDoorToDelete = null;
                SelectedProgramMassageToDelete = null;
                SelectedProgramAerialYogaToDelete = null;
                SelectedProgramPersonalToDelete = null;
                SelectedProgramMedicalToDelete = null;

                _SelectedProgramYogaToDelete = value;
                PaymentAmount = SelectedProgramYogaToDelete != null && SelectedProgramYogaToDelete.RemainingAmount > 0 ? SelectedProgramYogaToDelete.RemainingAmount : 0;

                RaisePropertyChanged();
            }
        }

        [NotMapped]
        public ShowUp SelectedShowUpToEditBP
        {
            get
            {
                return _SelectedShowUpToEditBP;
            }

            set
            {
                if (_SelectedShowUpToEditBP == value)
                {
                    return;
                }
                SelectedBodyPart = value.BodyPart;

                _SelectedShowUpToEditBP = value;
                RaisePropertyChanged();
            }
        }

        [NotMapped]
        public SizeEnum? SelectedSize
        {
            get
            {
                return _SelectedSize;
            }

            set
            {
                if (_SelectedSize == value)
                {
                    return;
                }

                _SelectedSize = value;
                RaisePropertyChanged();
            }
        }

        [NotMapped]
        public Payment SelectedYogaPayment
        {
            get
            {
                return _SelectedYogaPayment;
            }

            set
            {
                if (_SelectedYogaPayment == value)
                {
                    return;
                }

                _SelectedYogaPayment = value;
                RaisePropertyChanged();
            }
        }

        [NotMapped]
        public Program SelectedYogaProgram
        {
            get
            {
                return _SelectedYogaProgram;
            }

            set
            {
                if (_SelectedYogaProgram == value)
                {
                    return;
                }

                _SelectedYogaProgram = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(Active));
                RaisePropertyChanged(nameof(TypeOfProgram));
            }
        }

        [NotMapped]
        public ShowUp SelectedYogaShowUp
        {
            get
            {
                return _SelectedYogaShowUp;
            }

            set
            {
                if (_SelectedYogaShowUp == value)
                {
                    return;
                }

                _SelectedYogaShowUp = value;
                RaisePropertyChanged();
            }
        }

        [NotMapped]
        public RelayCommand<Payment> SetToProgramCommand { get; set; }

        [NotMapped]
        public bool ShowedUpToday => ShowUps.Any(s => s.Arrived.Date == DateTime.Today);

        [NotMapped]
        public RelayCommand ShowHistoryCommand { get; set; }

        [NotMapped]
        public RelayCommand ShowPreviusDataCommand { get; set; }

        [NotMapped]
        public RelayCommand ShowSumsCommand { get; set; }

        [NotMapped]
        public decimal ShowUpPrice
        {
            get => _ShowUpPrice;

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
                else
                    _ShowUpPrice = 0;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(ProgramPrice));
            }
        }

        public List<ShowUp> ShowUps
        {
            get => _ShowUps;

            set
            {          //TODO
                if (_ShowUps == value)
                {
                    return;
                }

                _ShowUps = value;
                if (value != null)
                {
                    //ShowUps.CollectionChanged += ShowUps_CollectionChanged;
                    UpdateShowUpsCollections();
                }
                else
                    ClearShowUpsCollections();

                RaisePropertyChanged();
            }
        }

        public ICollectionView ShowUpsAerialYogaCollectionView
        {
            get
            {
                return _ShowUpsAerialYogaCollectionView;
            }

            set
            {
                if (_ShowUpsAerialYogaCollectionView == value)
                {
                    return;
                }

                _ShowUpsAerialYogaCollectionView = value;
                RaisePropertyChanged();
            }
        }

        public ICollectionView ShowUpsFunctionalCollectionView
        {
            get => _ShowUpsFunctionalCollectionView;

            set
            {
                if (_ShowUpsFunctionalCollectionView == value)
                {
                    return;
                }

                _ShowUpsFunctionalCollectionView = value;

                RaisePropertyChanged();
            }
        }

        public ICollectionView ShowUpsFunctionalPilatesCollectionView
        {
            get
            {
                return _ShowUpsFunctionalPilatesCollectionView;
            }

            set
            {
                if (_ShowUpsFunctionalPilatesCollectionView == value)
                {
                    return;
                }

                _ShowUpsFunctionalPilatesCollectionView = value;
                RaisePropertyChanged();
            }
        }

        public ICollectionView ShowUpsMassCollectionView
        {
            get => _ShowUpsMassCollectionView;

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

        public ICollectionView ShowUpsMedicalCollectionView
        {
            get
            {
                return _ShowUpsMedicalCollectionView;
            }

            set
            {
                if (_ShowUpsMedicalCollectionView == value)
                {
                    return;
                }

                _ShowUpsMedicalCollectionView = value;
                RaisePropertyChanged();
            }
        }

        public ICollectionView ShowUpsOnlineCollectionView
        {
            get => _ShowUpsOnlineCollectionView;

            set
            {
                if (_ShowUpsOnlineCollectionView == value)
                {
                    return;
                }

                _ShowUpsOnlineCollectionView = value;
                RaisePropertyChanged();
            }
        }

        public ICollectionView ShowUpsOutDoorCollectionView
        {
            get
            {
                return _ShowUpsOutDoorCollectionView;
            }

            set
            {
                if (_ShowUpsOutDoorCollectionView == value)
                {
                    return;
                }

                _ShowUpsOutDoorCollectionView = value;
                RaisePropertyChanged();
            }
        }

        public ICollectionView ShowUpsPersonalCollectionView
        {
            get
            {
                return _ShowUpsPersonalCollectionView;
            }

            set
            {
                if (_ShowUpsPersonalCollectionView == value)
                {
                    return;
                }

                _ShowUpsPersonalCollectionView = value;
                RaisePropertyChanged();
            }
        }

        public ICollectionView ShowUpsPilatesCollectionView
        {
            get
            {
                return _ShowUpsPilatesCollectionView;
            }

            set
            {
                if (_ShowUpsPilatesCollectionView == value)
                {
                    return;
                }

                _ShowUpsPilatesCollectionView = value;
                RaisePropertyChanged();
            }
        }

        public ICollectionView ShowUpsYogaCollectionView
        {
            get
            {
                return _ShowUpsYogaCollectionView;
            }

            set
            {
                if (_ShowUpsYogaCollectionView == value)
                {
                    return;
                }

                _ShowUpsYogaCollectionView = value;
                RaisePropertyChanged();
            }
        }

        public bool Smoker
        {
            get => _Smoker;

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

        //        _Signed = value;
        //        RaisePropertyChanged();
        //    }
        //}
        public int SmokingUsage
        {
            get => _SmokingUsage;

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

        //    set
        //    {
        //        if (_Signed == value)
        //        {
        //            return;
        //        }
        [NotMapped]
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

        //public bool Signed
        //{
        //    get => _Signed;
        [Required(ErrorMessage = "Το επίθετο απαιτείται!")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Το Επίθετο μπορεί να είναι από 3 έως 20 χαρακτήρες.")]
        public string SureName
        {
            get => _SureName;

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
            get => _Surgery;

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
            get => _SurgeryInfo;

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
        [Phone(ErrorMessage = "Το τηλέφωνο δεν έχει τη σωστή μορφή")]
        public string Tel
        {
            get => _Tel;

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

        public bool ThirdDose
        {
            get
            {
                return _ThirdDose;
            }

            set
            {
                if (_ThirdDose == value)
                {
                    return;
                }

                _ThirdDose = value;
                RaisePropertyChanged();
            }
        }

        [NotMapped]
        public RelayCommand<ShowUp> Toggle30_60Command { get; set; }

        [NotMapped]
        public RelayCommand<ShowUp> ToggleIsPresentCommand { get; set; }

        [NotMapped]
        public RelayCommand<ShowUp> ToggleIsTestCommand { get; set; }

        [NotMapped]
        public RelayCommand<object[]> TogglePilFuncCommand { get; set; }

        [NotMapped]
        public RelayCommand<ShowUp> ToggleRealCommand { get; set; }

        [NotMapped]
        public RelayCommand<object[]> ToggleShowUpCommand { get; set; }

        public string TypeOfProgram =>
            SelectedFunctionalProgram?.RemainingDays > 0 ? SelectedFunctionalProgram.ToString() :
            SelectedPilatesProgram?.RemainingDays > 0 ? SelectedPilatesProgram.ToString() :
            SelectedFunctionalPilatesProgram?.RemainingDays > 0 ? SelectedFunctionalPilatesProgram.ToString() :
            SelectedMasageProgram?.RemainingDays > 0 ? SelectedMasageProgram.ToString() :
            SelectedOnlineProgram?.RemainingDays > 0 ? SelectedOnlineProgram.ToString() :
            SelectedOutdoorProgram?.RemainingDays > 0 ? SelectedOutdoorProgram.ToString() :
            SelectedYogaProgram?.RemainingDays > 0 ? SelectedYogaProgram.ToString() :
            SelectedAerialYogaProgram?.RemainingDays > 0 ? SelectedAerialYogaProgram.ToString() :
            SelectedPersonalProgmam?.RemainingDays > 0 ? SelectedPersonalProgmam.ToString() :
            SelectedMedicalProgram?.RemainingDays > 0 ? SelectedMedicalProgram.ToString() :
            "Ανενεργό";

        public bool Vacinated
        {
            get
            {
                return _Vacinated;
            }

            set
            {
                if (_Vacinated == value)
                {
                    return;
                }

                _Vacinated = value;
                RaisePropertyChanged();
            }
        }

        public bool WantToQuit
        {
            get => _WantToQuit;

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
            get => _WeightHistory;

            set
            {
                if (_WeightHistory == value)
                {
                    return;
                }

                _WeightHistory = value;
                _WeightHistory.CollectionChanged += WeigthsChanged;
                RaisePropertyChanged();
            }
        }

        [NotMapped]
        private bool Changed { get; set; } = false;

        #endregion Properties

        #region Methods

        public decimal CalculateRemainingAmount()
        {
            if (!Loaded)
                return 0;
            decimal sum = Programs.Sum(program => program.Amount);
            decimal remainingAmount = Payments.Aggregate(sum, (current, payment) => current - payment.Amount);
            PaymentAmount = RemainingAmount = remainingAmount;
            IsActiveColor = GetCustomerColor();
            return remainingAmount;
        }

        public SolidColorBrush GetCustomerColor()
        {
            if (!Loaded)
            {
                return new SolidColorBrush(Colors.Fuchsia);
            }
            if (ForceDisable == ForceDisable.forceEnable)
            {
                ActiveCustomer = true;
                return new SolidColorBrush(Colors.Green);
            }
            if (ForceDisable == ForceDisable.forceDisable)
            {
                ActiveCustomer = false;
                return new SolidColorBrush(Colors.Orange);
            }
            if (ShowUps.Count > 0)
            {
                var lastShUp = ShowUps.OrderByDescending(s => s.Arrived).FirstOrDefault();
                var lastShowUp = new DateTime();
                if (lastShUp != null)
                    lastShowUp = lastShUp.Arrived;
                if (RemainingAmount > 0)
                {
                    ActiveCustomer = true;

                    return new SolidColorBrush(Colors.Red);
                }
                else if (lastShowUp > DateTime.Today.AddDays(-45))
                {
                    ActiveCustomer = true;

                    return new SolidColorBrush(Colors.Green);
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

        public void GetRemainingDays()
        {
            SetColors();
        }

        public void InitialLoad()
        {
            if (FirstDate.Year < 2000)
                FirstDate = DateTime.Today;
            if (WeightHistory == null)
                WeightHistory = new ObservableCollection<Weight>();
            if (ShowUps == null)
                ShowUps = new List<ShowUp>();
            if (Payments == null)
                Payments = new List<Payment>();
            if (Programs == null)
                Programs = new List<Program>();
            if (Changes == null)
                Changes = new ObservableCollection<Change>();
            if (Apointments == null)
                Apointments = new ObservableCollection<Apointment>();
            if (Items == null)
                Items = new ObservableCollection<ItemPurchase>();
            var timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(19)
            };
            timer.Tick += Timer_Tick;
            timer.Start();
            ShowPreviusDataCommand = new RelayCommand(async () => { await ShowPreviewsData(); });
            ShowSumsCommand = new RelayCommand(async () => { await ShowSums(); });
            BookCommand = new RelayCommand<string>(async obj => { await MakeBooking(obj); }, CanMakeBooking);
            DeleteItemCommand = new RelayCommand<object>((obj) => DeleteItem(obj));
            AddOldShowUpCommand = new RelayCommand<int>(async obj => { await AddOldShowUp(obj); });
            ShowHistoryCommand = new RelayCommand(async () => await ShowHistory());
            SaveChangesAsyncCommand = new RelayCommand(async () => { await SaveChanges(); }, CanSaveChanges);
            CancelChangesAsyncCommand = new RelayCommand(RollBackChanges, CanSaveChanges);
            PaymentCommand = new RelayCommand(async () => { await AddPayment(); }, CanAddPayment);

            DeleteShowUpCommand = new RelayCommand<ShowUp>(async (obj) => { await DeleteShowUp(obj); }, CanDeleteShowUp);
            DeletePaymentCommand = new RelayCommand<Payment>(async (obj) => { await DeletePayment(obj); }, CanDeletePayment);
            DeleteProgramCommand = new RelayCommand<Program>(async (obj) => { await DeleteProgram(obj); }, CanDeleteProgram);

            ToggleRealCommand = new RelayCommand<ShowUp>(async (obj) => { await TogleReal(obj); }, CanToggleReal);
            Toggle30_60Command = new RelayCommand<ShowUp>(Toggle30_60, CanTogle30_60);
            ToggleIsPresentCommand = new RelayCommand<ShowUp>(TogglePresent, CanToglePresent);
            ToggleIsTestCommand = new RelayCommand<ShowUp>(ToggleTest, CanToglePresent);
            OpenPopup1Command = new RelayCommand(() => { Popup1Open = true; });
            ToggleShowUpCommand = new RelayCommand<object[]>((par) => TogleShowUp(par), CanToggleShowUp);
            TogglePilFuncCommand = new RelayCommand<object[]>((par) => TogleShowUpPilFun(par), CanToggleShowUpPilFunc);

            SetToProgramCommand = new RelayCommand<Payment>(async (obj) => { await SetToProgram(obj); }, CanSet);
            ReleasePaymentCommand = new RelayCommand<Payment>(ReleasePayment, CanReleasePayment);
            AddItemCommand = new RelayCommand(AddItem, CanAddItem);

            OldShowUps = new ObservableCollection<ShowUp>();
            NextAppointments = new ObservableCollection<Apointment>();

            //DisableCustomerCommand = new RelayCommand(DisableCustomer);
            OldShowUpDate = DateOfPayment = DateOfIssue = StartDate = DateTime.Today;
            SelectedProgramType = null;
            SecBodyParts = new List<BodyPartSelection>();

            CustomerLeftCommand = new RelayCommand(CustomerLeft);
        }



        public void ResetList()
        {
            foreach (var item in SecBodyParts)
            {
                item.Selected = false;
            }
        }

        public void SetColors()
        {
            DateTime startDate = Full ? new DateTime() : ResetDate;

            if (!Loaded) return;
            if (Programs != null && ShowUps != null && ShowUps.Count > 0 && Programs.Count > 0)
            {
                foreach (var prog in Programs)
                    prog.Color = new SolidColorBrush(Colors.Transparent);
                foreach (var showUp in ShowUps)
                    showUp.Color = new SolidColorBrush(Colors.Transparent);

                int progIndex = 0;
                List<Program> programsReversed;
                DateTime Limit;
                List<ShowUp> showUpsReserved;
                int counter = 0;
                Program selProg = null;
                foreach (ProgramMode mode in (ProgramMode[])Enum.GetValues(typeof(ProgramMode)))
                {
                    programsReversed = Programs.Where(o => o.StartDay >= startDate && o?.ProgramTypeO?.ProgramMode == mode).OrderBy(p => p.StartDay).ThenBy(p => p.Id).ToList();
                    Limit = programsReversed.Count > 0 ? programsReversed[0].StartDay : new DateTime();
                    showUpsReserved = ShowUps.Where(s => s.Arrived >= startDate && s.Arrived >= Limit && s.ProgramModeNew == mode).OrderBy(r => r.Arrived).ThenByDescending(r => r.Id).ToList();
                    selProg = null;
                    progIndex = 0;
                    foreach (var showUp in showUpsReserved)
                    {
                        if (selProg == null || (selProg.Showups > 0 && selProg.RemainingDays == 0) ||
                            (selProg.Showups == 0 && showUp.Arrived.Date > selProg.AddMonth(selProg.Months))
                            //||
                            //(selProg.ProgramTypeO.Id == 20 && showUp.Arrived.Date > selProg.AddMonth(1)) || //afto na fygei otan vgalume ta palia miniaia
                            //selProg.ProgramTypeO.Id == 21 && showUp.Arrived.Date > selProg.AddMonth(3)//kai afto to idio
                            && progIndex < programsReversed.Count)
                        {
                            if (progIndex >= programsReversed.Count)
                            {
                                if (selProg != null)
                                {
                                    selProg.RemainingDays = 0;
                                }
                                break;
                            }
                            counter = 1;
                            if (selProg != null)
                                selProg.RemainingDays = 0;
                            selProg = programsReversed[progIndex];
                            if ((selProg.Showups == 0 && showUp.Arrived.Date > selProg.AddMonth(selProg.Months))
                                //||
                                //(selProg.ProgramTypeO.Id == 20 && showUp.Arrived.Date > selProg.AddMonth(1)) ||
                                //(selProg.ProgramTypeO.Id == 21 && showUp.Arrived.Date > selProg.AddMonth(3))
                                )
                            {
                                progIndex++;
                                if (progIndex < programsReversed.Count)
                                {
                                    if (selProg != null)
                                        selProg.RemainingDays = 0;
                                    selProg = programsReversed[progIndex];
                                }
                                else
                                {
                                    selProg = null;
                                    break;
                                }
                            }
                            selProg.RemainingDays = selProg.Showups > 0 ? selProg.Showups : 0;
                            switch (mode)
                            {
                                case ProgramMode.functional:
                                case ProgramMode.massage:
                                case ProgramMode.online:
                                case ProgramMode.yoga:
                                case ProgramMode.personal:
                                    selProg.Color = progIndex % 2 == 0 ? new SolidColorBrush(Colors.LightGreen) : new SolidColorBrush(Colors.Orange);
                                    break;

                                case ProgramMode.outdoor:
                                case ProgramMode.pilates:
                                case ProgramMode.aerialYoga:
                                case ProgramMode.medical:
                                    selProg.Color = progIndex % 2 == 0 ? new SolidColorBrush(Colors.Aqua) : new SolidColorBrush(Colors.LightBlue);
                                    break;

                                case ProgramMode.pilatesFunctional:
                                default:
                                    selProg.Color = progIndex % 2 == 0 ? new SolidColorBrush(Colors.MediumSeaGreen) : new SolidColorBrush(Colors.MediumSpringGreen);
                                    break;
                            }
                            progIndex++;
                        }

                        if (selProg == null || selProg.StartDay > showUp.Arrived || (selProg.Showups > 0 && selProg.RemainingDays == 0 && progIndex == programsReversed.Count))
                            continue;
                        showUp.Color = selProg.Color;
                        showUp.Count = counter++;
                        if (showUp.Prog == null || showUp.Prog.Id != selProg.Id)
                        {
                            showUp.Prog = selProg;
                            Changed = true;
                        }
                        if (selProg.Showups > 0)
                            selProg.RemainingDays--;
                    }
                    if (selProg?.Showups == 0)
                    {
                        selProg.SetRemainingDays();
                    }
                    if (progIndex < programsReversed.Count)
                    {
                        selProg = programsReversed[progIndex];
                        selProg.RemainingDays = selProg.Showups;
                    }
                    switch (mode)
                    {
                        case ProgramMode.functional:
                            SelectedFunctionalProgram = selProg;
                            break;

                        case ProgramMode.massage:
                            SelectedMasageProgram = selProg;
                            break;

                        case ProgramMode.online:
                            SelectedOnlineProgram = selProg;
                            break;

                        case ProgramMode.outdoor:
                            SelectedOutdoorProgram = selProg;
                            break;

                        case ProgramMode.pilates:
                            SelectedPilatesProgram = selProg;
                            break;

                        case ProgramMode.yoga:
                            SelectedYogaProgram = selProg;
                            break;

                        case ProgramMode.pilatesFunctional:
                            SelectedFunctionalPilatesProgram = selProg;
                            break;

                        case ProgramMode.aerialYoga:
                            SelectedAerialYogaProgram = selProg;
                            break;

                        case ProgramMode.personal:
                            SelectedPersonalProgmam = selProg;
                            break;

                        case ProgramMode.medical:
                            SelectedMedicalProgram = selProg;
                            break;

                        default:
                            break;
                    }
                    for (int i = progIndex; i < programsReversed.Count; i++)
                    {
                        selProg = programsReversed[progIndex];
                        if (selProg?.Showups == 0)
                        {
                            selProg.SetRemainingDays();
                        }
                        switch (mode)
                        {
                            case ProgramMode.functional:
                            case ProgramMode.massage:
                            case ProgramMode.online:
                            case ProgramMode.yoga:
                            case ProgramMode.personal:
                                selProg.Color = progIndex % 2 == 0 ? new SolidColorBrush(Colors.LightGreen) : new SolidColorBrush(Colors.Orange);
                                break;

                            case ProgramMode.outdoor:
                            case ProgramMode.pilates:
                            case ProgramMode.aerialYoga:
                            case ProgramMode.medical:
                                selProg.Color = progIndex % 2 == 0 ? new SolidColorBrush(Colors.Aqua) : new SolidColorBrush(Colors.LightBlue);
                                break;

                            case ProgramMode.pilatesFunctional:
                            default:
                                selProg.Color = progIndex % 2 == 0 ? new SolidColorBrush(Colors.MediumSeaGreen) : new SolidColorBrush(Colors.MediumSpringGreen);
                                break;
                        }
                    }
                    if (Changed)
                    {
                        BasicDataManager.Context.Save();
                        Changed = false;
                    }
                }

                SetRemaining();
            }

            RaisePropertyChanged(nameof(RemainingTrainingDays));
            RaisePropertyChanged(nameof(RemainingPilatesDays));
            RaisePropertyChanged(nameof(RemainingFunctionalPilatesDays));
            RaisePropertyChanged(nameof(RemainingMassageDays));
            RaisePropertyChanged(nameof(RemainingOnlineDays));
            RaisePropertyChanged(nameof(RemainingOutDoorDays));
            RaisePropertyChanged(nameof(RemainingYogaDays));
            RaisePropertyChanged(nameof(RemainingAerialYogaDays));
            RaisePropertyChanged(nameof(RemainingPersonalDays));
            RaisePropertyChanged(nameof(RemainingMedicalDays));
            RaisePropertyChanged(nameof(RemainingYogaFullDays));
            RaisePropertyChanged(nameof(RemainingTrainingFullDays));

            RaisePropertyChanged(nameof(Active));

            UpdateCollections();
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

        public void UpdateCollections()
        {
            try
            {
                if (!Loaded)
                {
                    return;
                }
                UpdateProgramsCollections();
                UpdatePaymentsCollections();
                UpdateShowUpsCollections();
                ShowUpsFunctionalCollectionView?.Refresh();
                ShowUpsPilatesCollectionView?.Refresh();
                ShowUpsFunctionalPilatesCollectionView?.Refresh();
                ShowUpsMassCollectionView?.Refresh();
                ShowUpsOnlineCollectionView?.Refresh();
                ShowUpsOutDoorCollectionView?.Refresh();
                ShowUpsYogaCollectionView?.Refresh();
                ShowUpsAerialYogaCollectionView?.Refresh();
                ShowUpsMedicalCollectionView?.Refresh();
                ShowUpsPersonalCollectionView?.Refresh();

                PaymentsFunctionalCollectionView?.Refresh();
                PaymentsFunctionalPilatesCollectionView?.Refresh();
                PaymentsPilatesCollectionView?.Refresh();
                PaymentsMassCollectionView?.Refresh();
                PaymentsOnlineCollectionView?.Refresh();
                PaymentsOutDoorCollectionView?.Refresh();
                PaymentsYogaCollectionView?.Refresh();
                PaymentsAerialYogaCollectionView?.Refresh();
                PaymentsPersonalCollectionView?.Refresh();
                PaymentsMedicalCollectionView?.Refresh();

                ProgramsFunctionalCollectionView?.Refresh();
                ProgramsPilatesCollectionView?.Refresh();
                ProgramsFunctionalPilatesCollectionView?.Refresh();
                ProgramsMassageColelctionView?.Refresh();
                ProgramsOnlineColelctionView?.Refresh();
                ProgramsOutdoorCollectionView?.Refresh();
                ProgramsYogaCollectionView?.Refresh();
                ProgramsAerialYogaCollectionView?.Refresh();
                ProgramsPersonalCollectionView?.Refresh();
                ProgramsMedicalCollectionView?.Refresh();
            }
            catch (Exception ex)
            {
            }
        }

        public void UpdateSelections(object selectedItem)
        {
            foreach (var su in ShowUps)
            {
                su.IsSelected = false;
            }
            foreach (var pr in Programs)
            {
                pr.IsSelected = false;
            }
            foreach (var pay in Payments)
            {
                pay.IsSelected = false;
            }
            if (selectedItem is ShowUp s)
            {
                s.IsSelected = true;
                if (s.Prog == null)
                {
                    return;
                }
                s.Prog.IsSelected = true;
                foreach (var paym in Payments)
                {
                    if (paym.Program != null && paym.Program.Id == s.Prog.Id)
                    {
                        paym.IsSelected = true;
                    }
                }
            }
            else if (selectedItem is Program p)
            {
                p.IsSelected = true;
                foreach (var show in ShowUps)
                {
                    if (show.Prog != null && show.Prog.Id == p.Id)
                    {
                        show.IsSelected = true;
                    }
                }
                foreach (var paym in Payments)
                {
                    if (paym.Program != null && paym.Program.Id == p.Id)
                    {
                        paym.IsSelected = true;
                    }
                }
            }
            else if (selectedItem is Payment pay)
            {
                pay.IsSelected = true;
                if (pay.Program == null)
                {
                    return;
                }
                pay.Program.IsSelected = true;
                foreach (var show in ShowUps)
                {
                    if (show.Prog != null && show.Prog.Id == pay.Program.Id)
                    {
                        show.IsSelected = true;
                    }
                }
            }
        }

        public void ValidateProgram()
        {
            if (Programs?.Any(p => p.StartDay <= StartDate && StartDate < p.StartDay.AddMonths(p.Months).AddDays(5) && p.RemainingDays > 0) == true)
            {
                IsDateValid = false;
                ProgramResult = "Προσοχή, υπάρχει ήδη ενεργό πακέτο αυτή την ημερομηνία";
                return;
            }

            IsDateValid = true;

            if (SelectedProgramType == null)
            {
                ProgramResult = "Επιλέξτε τύπο πακέτου";
            }
            else if (ProgramPrice == 0)
            {
                ProgramResult = "Προσοχή, δεν έχει επιλεγεί τιμή";
            }
            else if (ProgramDuration < 1 && NumOfShowUps < 1)
            {
                ProgramResult = "Προσοχή, πρέπει να επιλέξετε διάρκεια η συνεδρίες";
            }
            else if (ProgramDuration > 0 && NumOfShowUps > 0)
            {
                ProgramResult = "Προσοχή, μπορείτε να επιλέξετε μόνο διάρκεια η μόνο συνεδρίες";
            }
            else if (StartDate < DateTime.Today)
            {
                ProgramResult = "Προσοχή, η επιλεγμένη ημερομηνία έχει περάσει";
            }
            else
            {
                ProgramResult = "";
            }
        }

        internal void AddNewProgram(int par)
        {
            var p = new Program { Amount = ProgramPrice, DayOfIssue = DateOfIssue, Showups = NumOfShowUps, ProgramTypeO = SelectedProgramType, Months = ProgramDuration, StartDay = StartDate, Paid = par == 1 };
            Programs.Add(p);
            Changes.Add(new Change($"Προστέθηκε νέο ΠΑΚΕΤΟ {(SelectedProgramType != null ? SelectedProgramType.ToString() : "Σφάλμα")} με {NumOfShowUps} συνεδρίες, κόστος {StaticResources.DecimalToString(ProgramPrice)}, έναρξη {StartDate:dd/MM/yy}{(par == 1 ? "," : " και")}" +
                $" διάρκεια {ProgramDuration} μήνες {(par == 1 ? "και πληρώθηκε" : "χωρίς να πληρωθεί")}", StaticResources.User)
            { Program = p });
            p.CalculateRemainingAmount();
            p.PropertyChanged += ProgramPropertyChanged;
            if (Loaded)
            {
                CalculateRemainingAmount();
                SetColors();
            }

            RaisePropertyChanged(nameof(PaymentVisibility));
        }

        internal async Task AddPayment()
        {
            Payment p = new Payment { Amount = PaymentAmount, Date = DateOfPayment, User = StaticResources.User, PaymentType = PaymentType, Reciept = PaymentReciept };
            if (SelectedProgramFunctionalToDelete != null)
            {
                SelectedProgramFunctionalToDelete.Payments.Add(p);
                p.Program = SelectedProgramFunctionalToDelete;
                RaisePropertyChanged(nameof(PaymentVisibility));
                SelectedProgramFunctionalToDelete.CalculateRemainingAmount();
            }
            else if (SelectedProgramPilatesToDelete != null)
            {
                SelectedProgramPilatesToDelete.Payments.Add(p);
                p.Program = SelectedProgramPilatesToDelete;
                RaisePropertyChanged(nameof(PaymentVisibility));
                SelectedProgramPilatesToDelete.CalculateRemainingAmount();
            }
            else if (SelectedProgramFunctionalPilatesToDelete != null)
            {
                SelectedProgramFunctionalPilatesToDelete.Payments.Add(p);
                p.Program = SelectedProgramFunctionalPilatesToDelete;
                RaisePropertyChanged(nameof(PaymentVisibility));
                SelectedProgramFunctionalPilatesToDelete.CalculateRemainingAmount();
            }
            else if (SelectedProgramMassageToDelete != null)
            {
                SelectedProgramMassageToDelete.Payments.Add(p);
                p.Program = SelectedProgramMassageToDelete;
                RaisePropertyChanged(nameof(PaymentVisibility));
                SelectedProgramMassageToDelete.CalculateRemainingAmount();
            }
            else if (SelectedProgramOnlineToDelete != null)
            {
                SelectedProgramOnlineToDelete.Payments.Add(p);
                p.Program = SelectedProgramOnlineToDelete;
                RaisePropertyChanged(nameof(PaymentVisibility));
                SelectedProgramOnlineToDelete.CalculateRemainingAmount();
            }
            else if (SelectedProgramOutDoorToDelete != null)
            {
                SelectedProgramOutDoorToDelete.Payments.Add(p);
                p.Program = SelectedProgramOutDoorToDelete;
                RaisePropertyChanged(nameof(PaymentVisibility));
                SelectedProgramOutDoorToDelete.CalculateRemainingAmount();
            }
            else if (SelectedProgramYogaToDelete != null)
            {
                SelectedProgramYogaToDelete.Payments.Add(p);
                p.Program = SelectedProgramYogaToDelete;
                RaisePropertyChanged(nameof(PaymentVisibility));
                SelectedProgramYogaToDelete.CalculateRemainingAmount();
            }
            else if (SelectedProgramAerialYogaToDelete != null)
            {
                SelectedProgramAerialYogaToDelete.Payments.Add(p);
                p.Program = SelectedProgramAerialYogaToDelete;
                RaisePropertyChanged(nameof(PaymentVisibility));
                SelectedProgramAerialYogaToDelete.CalculateRemainingAmount();
            }
            else if (SelectedProgramPersonalToDelete != null)
            {
                SelectedProgramPersonalToDelete.Payments.Add(p);
                p.Program = SelectedProgramPersonalToDelete;
                RaisePropertyChanged(nameof(PaymentVisibility));
                SelectedProgramPersonalToDelete.CalculateRemainingAmount();
            }
            else if (SelectedProgramMedicalToDelete != null)
            {
                SelectedProgramMedicalToDelete.Payments.Add(p);
                p.Program = SelectedProgramMedicalToDelete;
                RaisePropertyChanged(nameof(PaymentVisibility));
                SelectedProgramMedicalToDelete.CalculateRemainingAmount();
            }
            Payments.Add(p);
            p.PropertyChanged += PaymentPropertyChanged;
            if (Loaded)
            {
                CalculateRemainingAmount();
                SetColors();
            }

            RaisePropertyChanged(nameof(PaymentVisibility));
            CommandManager.InvalidateRequerySuggested();
        }

        internal bool HasActiveProgram(ProgramMode programMode)
        {
            switch (programMode)
            {
                case ProgramMode.functional:
                    return SelectedFunctionalProgram?.RemainingDays > 0;

                case ProgramMode.massage:
                    return SelectedMasageProgram?.RemainingDays > 0;

                case ProgramMode.online:
                    return SelectedOnlineProgram?.RemainingDays > 0;

                case ProgramMode.outdoor:
                    return SelectedOutdoorProgram?.RemainingDays > 0;

                case ProgramMode.pilates:
                    return SelectedPilatesProgram?.RemainingDays > 0;

                case ProgramMode.yoga:
                    return SelectedYogaProgram?.RemainingDays > 0;

                case ProgramMode.pilatesFunctional:
                    return SelectedPilatesProgram?.RemainingDays > 0;

                case ProgramMode.aerialYoga:
                    return SelectedAerialYogaProgram?.RemainingDays > 0;

                case ProgramMode.personal:
                    return SelectedPersonalProgmam?.RemainingDays > 0;

                case ProgramMode.medical:
                    return SelectedMedicalProgram?.RemainingDays > 0;

                default:
                    return false;
            }
        }

        internal void MakeProgramPayment()
        {
            var p = new Payment { Amount = ProgramPrice, Date = DateOfIssue, User = StaticResources.User };
            Payments.Add(p);
            p.PropertyChanged += PaymentPropertyChanged;
            if (Loaded)
            {
                CalculateRemainingAmount();
                SetColors();
            }
        }

        internal bool ProgramDataCheck()
        {
            ValidateProgram();
            return ProgramPrice >= 0 && SelectedProgramType != null && ((NumOfShowUps > 0 && ProgramDuration == 0) || (NumOfShowUps == 0 && ProgramDuration > 0));
        }

        internal void ShowedUp(bool arrived, ProgramMode mode, bool is30min = false, int? secondaryProgMode = 0)
        {
            if (mode != ProgramMode.massage && ShowUps.Any(s => s.ProgramModeNew != ProgramMode.massage && s.Arrived.Date == DateTime.Today))
            {
                if (MessageBox.Show("Υπάρχει ήδη μία παρουσία σήμερα, Θέλετε σίγουρα να συνεχίσετε?", "Προσοχή", MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.Yes)
                {
                    return;
                }
            }

            IsPracticing = arrived;
            int remain;
            switch (mode)
            {
                case ProgramMode.functional:
                    remain = RemainingTrainingDays;
                    ShowUps.Add(new ShowUp { Arrive = arrived, Arrived = DateTime.Now, ProgramModeNew = mode, Is30min = is30min });
                    ShowUps_CollectionChanged();
                    if (RemainingTrainingDays != 0) return;
                    MessageBox.Show(remain > 0
                        ? $"Αυτή ήταν η τελευταία συνεδρία γυμναστικής του {ToString()}"
                        : $"Οι συνεδρίες γυμναστικής του {ToString()} έχουν τελειώσει");
                    break;

                case ProgramMode.massage:
                    remain = RemainingMassageDays;
                    ShowUps.Add(new ShowUp { Arrive = arrived, Arrived = DateTime.Now, ProgramModeNew = mode, Is30min = is30min });
                    ShowUps_CollectionChanged();
                    if (RemainingMassageDays != 0) return;
                    MessageBox.Show(remain > 0
                        ? $"Αυτή ήταν η τελευταία συνεδρία μασάζ του {ToString()}"
                        : $"Οι συνεδρίες μασάζ του {ToString()} έχουν τελειώσει");
                    break;

                case ProgramMode.online:
                    remain = RemainingOnlineDays;
                    ShowUps.Add(new ShowUp { Arrive = arrived, Arrived = DateTime.Now, ProgramModeNew = mode, Is30min = is30min });
                    ShowUps_CollectionChanged();
                    if (RemainingOnlineDays != 0) return;
                    MessageBox.Show(remain > 0
                        ? $"Αυτή ήταν η τελευταία συνεδρία Online του {ToString()}"
                        : $"Οι συνεδρίες Online του {ToString()} έχουν τελειώσει");
                    break;

                case ProgramMode.outdoor:
                    remain = RemainingOutDoorDays;
                    ShowUps.Add(new ShowUp { Arrive = arrived, Arrived = DateTime.Now, ProgramModeNew = mode, Is30min = is30min });
                    ShowUps_CollectionChanged();
                    if (RemainingOutDoorDays != 0) return;
                    MessageBox.Show(remain > 0
                        ? $"Αυτή ήταν η τελευταία συνεδρία OutDoor του {ToString()}"
                        : $"Οι συνεδρίες OutDoor του {ToString()} έχουν τελειώσει");
                    break;

                case ProgramMode.pilates:
                    remain = RemainingPilatesDays;
                    ShowUps.Add(new ShowUp { Arrive = arrived, Arrived = DateTime.Now, ProgramModeNew = mode, Is30min = is30min });
                    ShowUps_CollectionChanged();
                    if (RemainingPilatesDays != 0) return;
                    MessageBox.Show(remain > 0
                        ? $"Αυτή ήταν η τελευταία συνεδρία Pilates του {ToString()}"
                        : $"Οι συνεδρίες Pilates του {ToString()} έχουν τελειώσει");
                    break;

                case ProgramMode.yoga:
                    remain = RemainingYogaDays;
                    ShowUps.Add(new ShowUp { Arrive = arrived, Arrived = DateTime.Now, ProgramModeNew = mode, Is30min = is30min });
                    ShowUps_CollectionChanged();
                    if (RemainingYogaDays != 0) return;
                    MessageBox.Show(remain > 0
                        ? $"Αυτή ήταν η τελευταία συνεδρία yoga του {ToString()}"
                        : $"Οι συνεδρίες yoga του {ToString()} έχουν τελειώσει");
                    break;

                case ProgramMode.pilatesFunctional:
                    remain = RemainingFunctionalPilatesDays;
                    ShowUps.Add(new ShowUp { Arrive = arrived, Arrived = DateTime.Now, ProgramModeNew = mode, Is30min = is30min, ProgramMode = (ProgramMode)secondaryProgMode });
                    ShowUps_CollectionChanged();
                    if (RemainingFunctionalPilatesDays != 0) return;
                    MessageBox.Show(remain > 0
                        ? $"Αυτή ήταν η τελευταία συνεδρία functional-pilates yoga του {ToString()}"
                        : $"Οι συνεδρίες functional-pilates του {ToString()} έχουν τελειώσει");
                    break;

                case ProgramMode.aerialYoga:
                    remain = RemainingAerialYogaDays;
                    ShowUps.Add(new ShowUp { Arrive = arrived, Arrived = DateTime.Now, ProgramModeNew = mode, Is30min = is30min });
                    ShowUps_CollectionChanged();
                    if (RemainingAerialYogaDays != 0) return;
                    MessageBox.Show(remain > 0
                        ? $"Αυτή ήταν η τελευταία συνεδρία Aerial-Yoga του {ToString()}"
                        : $"Οι συνεδρίες Aerial-Yoga του {ToString()} έχουν τελειώσει");
                    break;

                case ProgramMode.personal:
                    remain = RemainingPersonalDays;
                    ShowUps.Add(new ShowUp { Arrive = arrived, Arrived = DateTime.Now, ProgramModeNew = mode, Is30min = is30min });
                    ShowUps_CollectionChanged();
                    if (RemainingPersonalDays != 0) return;
                    MessageBox.Show(remain > 0
                        ? $"Αυτή ήταν η τελευταία συνεδρία Personal του {ToString()}"
                        : $"Οι συνεδρίες Personal του {ToString()} έχουν τελειώσει");
                    break;

                case ProgramMode.medical:
                    remain = RemainingMedicalDays;
                    ShowUps.Add(new ShowUp { Arrive = arrived, Arrived = DateTime.Now, ProgramModeNew = mode, Is30min = is30min });
                    ShowUps_CollectionChanged();
                    if (RemainingMedicalDays != 0) return;
                    MessageBox.Show(remain > 0
                        ? $"Αυτή ήταν η τελευταία συνεδρία Medical του {ToString()}"
                        : $"Οι συνεδρίες Aerial-Yoga του {ToString()} έχουν τελειώσει");
                    break;

                default:
                    MessageBox.Show("Error");
                    break;
            }
            ShowUps_CollectionChanged();
        }

        private void AddItem()
        {
            Items.Add(new ItemPurchase { Item = SelectedItem, Size = SelectedSize, Date = DateTime.Today, Color = SelectedColor });
        }

        private async Task AddOldShowUp(int programMode)
        {
            if (programMode != 1 && ShowUps.Any(s => s.ProgramModeNew != ProgramMode.massage && s.Arrived.Date == OldShowUpDate))
            {
                if (MessageBox.Show("Υπάρχει ήδη μία παρουσία σήμερα, Θέλετε σίγουρα να συνεχίσετε?", "Προσοχή", MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.Yes)
                {
                    return;
                }
            }
            // Changes.Add(new Change($"Προστέθηκε ΠΑΡΟΥΣΙΑ {Enum.GetName(typeof(ProgramMode), programMode)} για  {OldShowUpDate: dd/MM/yy}", StaticResources.User));
            if (programMode > 50)
                ShowUps.Add(new ShowUp { Arrived = OldShowUpDate, ProgramModeNew = (ProgramMode)(programMode / 10), Left = new DateTime(1234, 1, 1), Is30min = Is30min, ProgramMode = (ProgramMode)(programMode % 10) });
            else
                ShowUps.Add(new ShowUp { Arrived = OldShowUpDate, ProgramModeNew = (ProgramMode)programMode, Left = new DateTime(1234, 1, 1), Is30min = Is30min });
            Popup1Open = false;
            ShowUps_CollectionChanged();
            // await BasicDataManager.SaveAsync();
        }

        private bool CanAddItem()
        {
            return (SelectedSize != null || SelectedItem?.Id == 3) && SelectedItem != null;
        }

        private bool CanAddPayment()
        {
            return PaymentAmount > 0 && PaymentAmount <= RemainingAmount && (
                (SelectedProgramFunctionalToDelete != null && PaymentAmount <= SelectedProgramFunctionalToDelete.RemainingAmount) ||
                (SelectedProgramPilatesToDelete != null && PaymentAmount <= SelectedProgramPilatesToDelete.RemainingAmount) ||
                (SelectedProgramFunctionalPilatesToDelete != null && PaymentAmount <= SelectedProgramFunctionalPilatesToDelete.RemainingAmount) ||
                (SelectedProgramMassageToDelete != null && PaymentAmount <= SelectedProgramMassageToDelete.RemainingAmount) ||
                (SelectedProgramOnlineToDelete != null && PaymentAmount <= SelectedProgramOnlineToDelete.RemainingAmount) ||
                (SelectedProgramOutDoorToDelete != null && PaymentAmount <= SelectedProgramOutDoorToDelete.RemainingAmount) ||
                (SelectedProgramYogaToDelete != null && PaymentAmount <= SelectedProgramYogaToDelete.RemainingAmount) ||
                (SelectedProgramMedicalToDelete != null && PaymentAmount <= SelectedProgramMedicalToDelete.RemainingAmount) ||
                (SelectedProgramPersonalToDelete != null && PaymentAmount <= SelectedProgramPersonalToDelete.RemainingAmount) ||
                (SelectedProgramAerialYogaToDelete != null && PaymentAmount <= SelectedProgramAerialYogaToDelete.RemainingAmount));
        }

        private bool CanDeletePayment(Payment arg)
        {
            return arg != null;
        }

        private bool CanDeleteProgram(Program prog)
        {
            return prog != null;
        }

        private bool CanDeleteShowUp(ShowUp showup)
        {
            return showup != null;
        }

        private bool CanMakeBooking(string arg) => ProgramDataCheck();

        private bool CanReleasePayment(Payment p)
        {
            return p.Program != null;
        }

        private bool CanSaveChanges()
        {
            return BasicDataManager.HasChanges();
        }

        private bool CanSet(Payment p)
        {
            return p != null && p.Program == null;
        }

        private bool CanToggleReal(ShowUp arg)
        {
            return arg != null;
        }

        private bool CanToggleShowUp(object[] arg) => arg[0] is ShowUp s && (string)arg[1] != ((int)s.ProgramModeNew).ToString();

        private bool CanToggleShowUpPilFunc(object[] arg) => arg[0] is ShowUp s && (string)arg[1] != ((int)s.ProgramMode).ToString();

        private bool CanTogle30_60(ShowUp v)
        {
            return v != null;
        }

        private bool CanToglePresent(ShowUp v)
        {
            return v != null;
        }

        private void CaptureChanges()
        {
            var relatioships = DbContextExtensions.GetRelationships(BasicDataManager.Context.Context).ToList();

            if (relatioships.Count > 0)
            {
                try
                {
                    var sb = new StringBuilder();
                    var ru = relatioships.Where(r => r.Item1 is Program && r.Item2 is ProgramType).ToList();
                    if (ru.Count == 2)
                    {
                        Changes.Add(new Change
                        {
                            Date = DateTime.Now,
                            Description = $"Ο τύπος του προγράμματος άλλαξε από '{(ru[1].Item2 as ProgramType).ProgramName}' σε '{(ru[0].Item2 as ProgramType).ProgramName}'",
                            Program = ru[0].Item1 as Program,
                            User = StaticResources.User
                        });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            var changes = from e in BasicDataManager.Context.Context.ChangeTracker.Entries()
                          where e.State == EntityState.Modified
                          select e;
            foreach (DbEntityEntry change in changes)
            {
                switch (change.Entity)
                {
                    case Program p:
                        GetProgramChanges(change);
                        break;

                    case Payment c:
                        GetPaymentChanges(change);
                        break;

                    case ShowUp c:
                        GetShowUpChanges(change);
                        break;

                    default:
                        break;
                }
            }
        }

        private void ClearPaymentsCollections()
        {
            PaymentsFunctionalCollectionView = null;
            PaymentsPilatesCollectionView = null;
            PaymentsFunctionalPilatesCollectionView = null;
            PaymentsYogaCollectionView = null;
            PaymentsAerialYogaCollectionView = null;
            PaymentsMassCollectionView = null;
            PaymentsOnlineCollectionView = null;
            PaymentsOutDoorCollectionView = null;
            PaymentsPersonalCollectionView = null;
            PaymentsMedicalCollectionView = null;
        }

        private void ClearProgramsCollections()
        {
            ProgramsFunctionalCollectionView = null;
            ProgramsPilatesCollectionView = null;
            ProgramsFunctionalPilatesCollectionView = null;
            ProgramsMassageColelctionView = null;
            ProgramsOnlineColelctionView = null;
            ProgramsOutdoorCollectionView = null;
            ProgramsYogaCollectionView = null;
            ProgramsAerialYogaCollectionView = null;
            ProgramsPersonalCollectionView = null;
            ProgramsMedicalCollectionView = null;
        }

        private void ClearShowUpsCollections()
        {
            ShowUpsFunctionalCollectionView = null;
            ShowUpsPilatesCollectionView = null;
            ShowUpsFunctionalPilatesCollectionView = null;
            ShowUpsMassCollectionView = null;
            ShowUpsOnlineCollectionView = null;
            ShowUpsOutDoorCollectionView = null;
            ShowUpsYogaCollectionView = null;
            ShowUpsAerialYogaCollectionView = null;
            ShowUpsMedicalCollectionView = null;
            ShowUpsPersonalCollectionView = null;
        }

        private void CustomerLeft()
        {
            SelectedShowUpToEditBP.BodyPart = SelectedBodyPart;
            SelectedShowUpToEditBP.SecBodyPartsString = string.Join(",", SecBodyParts.Where(x => x.Selected).Select(t => (int)t.SecBodyPart));
            SelectedShowUpToEditBP.RaisePropertyChanged("SecBodyPartsDesc");

            ResetList();

            PopupFinishOpen = false;
        }

        private void DeleteItem(object obj)
        {
            if (obj is ItemPurchase ip && Items.Contains(ip))
            {
                Items.Remove(ip);
            }
        }

        private async Task DeletePayment(Payment payment)
        {
            Changes.Add(new Change($"Διαγράφηκε ΠΛΗΡΩΜΉ {StaticResources.DecimalToString(payment.Amount)} που είχε γίνει {payment.Date:ddd dd/MM/yy} για γυμναστική", StaticResources.User));
            if (payment.Program != null && payment.Program.Payments.Any(p => p.Id == payment.Id))
            {
                payment.Program.Payments.Remove(payment);
            }
            BasicDataManager.Delete(payment);
            UpdateCollections();
            if (Loaded)
            {
                CalculateRemainingAmount();
                SetColors();
            }

            RaisePropertyChanged(nameof(PaymentVisibility));
            //await BasicDataManager.SaveAsync();
        }

        private async Task DeleteProgram(Program prog)
        {
            prog.PropertyChanged -= ProgramPropertyChanged;
            if (prog == null)
            {
                return;
            }
            Changes.Add(new Change($"Διαγράφηκε ΠΡΌΓΡΑΜΜΑ {prog} που είχε καταχωρηθεί {prog.DayOfIssue:ddd dd/MM/yy} με " +
                $"διάρκεια {prog.Showups} μήνες αξίας {StaticResources.DecimalToString(prog.Amount)} και έναρξη {prog.StartDay:ddd dd/MM/yy}", StaticResources.User));

            BasicDataManager.Delete(prog);
            UpdateCollections();


            if (Loaded)
            {
                CalculateRemainingAmount();
                SetColors();
            }

            RaisePropertyChanged(nameof(PaymentVisibility));
            // await BasicDataManager.SaveAsync();
        }

        private async Task DeleteShowUp(ShowUp showup)
        {
            if (showup == null)
                return;
            Changes.Add(new Change($"Διαγράφηκε ΠΑΡΟΥΣΊΑ {StaticResources.GetDescription(showup.ProgramModeNew)} με ημερομηνία {showup.Arrived:ddd dd/MM/yy}", StaticResources.User));

            BasicDataManager.Delete(showup);
            UpdateCollections();
            ShowUps_CollectionChanged();
            // await BasicDataManager.SaveAsync();
        }

        private DateTime GetLastBuy()
        {
            if (Programs.Count > 0)
            {
                var pa = Programs?.OrderByDescending(p => p.DayOfIssue).FirstOrDefault();
                if (pa != null)
                {
                    return pa.DayOfIssue;
                }
            }
            return new DateTime();
        }

        private string GetLastPart()
        {
            LastGymShowUp = ShowUps?.Where(s => s.ProgramModeNew != ProgramMode.aerialYoga && s.ProgramModeNew != ProgramMode.yoga && s.ProgramModeNew != ProgramMode.massage)
                .OrderByDescending(t => t.Left)?.FirstOrDefault();
            if (LastGymShowUp != null)
                return StaticResources.GetDescription(LastGymShowUp.BodyPart);
            return "";
        }

        private void GetPaymentChanges(DbEntityEntry change)
        {
            if (change.Entity is Payment p)
            {
                var sb = new StringBuilder();

                foreach (string propertyName in change.OriginalValues.PropertyNames)
                {
                    var original = change.OriginalValues[propertyName];
                    var current = change.CurrentValues[propertyName];

                    if (!Equals(original, current))
                    {
                        switch (propertyName)
                        {
                            case nameof(Payment.Amount):
                                if (original is decimal or)
                                    sb.Append($"Ποσό από '{StaticResources.DecimalToString(or)}' σε '{StaticResources.DecimalToString(p.Amount)}', ");
                                break;

                            case nameof(Payment.Date):
                                sb.Append($"Ημερομηνία '{(DateTime)original:dd/MM/yyyy}' σε '{p.Date:dd/MM/yyyy}', ");
                                break;

                            case nameof(Payment.Reciept):
                                if (p.Reciept)
                                    sb.Append($"Απόδειξη από ΌΧΙ σε ΝΑΊ, ");
                                else
                                    sb.Append($"Απόδειξη από ΝΑΊ σε ΌΧΙ, ");
                                break;

                            case nameof(Payment.PaymentType):
                                if (p.PaymentType == PaymentType.Cash)
                                    sb.Append($"Τρ. Πληρωμής από ΚΆΡΤΑ σε ΜΕΤΡΗΤΆ, ");
                                else
                                    sb.Append($"Τρ. Πληρωμής από ΜΕΤΡΗΤΆ σε ΚΆΡΤΑ, ");
                                break;
                        }
                    }
                }
                if (sb.Length > 0)
                {
                    Changes.Add(new Change { Date = DateTime.Now, Description = sb.ToString().TrimEnd(new[] { ' ', ',' }), Payment = p, User = StaticResources.User });
                }
            }
        }

        private void GetProgramChanges(DbEntityEntry change)
        {
            if (change.Entity is Program p)
            {
                var sb = new StringBuilder();

                foreach (string propertyName in change.OriginalValues.PropertyNames)
                {
                    var original = change.OriginalValues[propertyName];
                    var current = change.CurrentValues[propertyName];

                    if (!Equals(original, current))
                    {
                        switch (propertyName)
                        {
                            case nameof(Program.Amount):
                                if (original is decimal or)
                                    sb.Append($"Κόστος από '{StaticResources.DecimalToString(or)}' σε '{StaticResources.DecimalToString(p.Amount)}', ");
                                break;

                            case nameof(Program.DayOfIssue):
                                sb.Append($"Ημερομηνία αγοράς '{(DateTime)original:dd/MM/yyyy}' σε '{p.DayOfIssue:dd/MM/yyyy}', ");
                                break;

                            case nameof(Program.StartDay):
                                sb.Append($"Έναρξη από '{(DateTime)original:dd/MM/yyyy}' σε '{p.StartDay:dd/MM/yyyy}', ");
                                break;

                            case nameof(Program.Months):
                                sb.Append($"Διάρκεια από '{original}' σε '{current}' μήνες, ");
                                break;

                            case nameof(Program.ProgramTypeO):
                                sb.Append($"Τύπος πακέτου από '{original ?? "κενό"}' σε '{current ?? "κενό"}', ");
                                break;

                            case nameof(Program.Showups):
                                sb.Append($"Συνεδρίες από '{original}' σε '{current}', ");
                                break;
                        }
                    }
                }
                if (sb.Length > 0)
                {
                    Changes.Add(new Change { Date = DateTime.Now, Description = sb.ToString().TrimEnd(new[] { ' ', ',' }), Program = p, User = StaticResources.User });
                }
            }
        }

        private void GetShowUpChanges(DbEntityEntry change)
        {
            if (change.Entity is ShowUp p)
            {
                var sb = new StringBuilder();

                foreach (string propertyName in change.OriginalValues.PropertyNames)
                {
                    var original = change.OriginalValues[propertyName];
                    var current = change.CurrentValues[propertyName];

                    if (!Equals(original, current))
                    {
                        switch (propertyName)
                        {
                            case nameof(ShowUp.Arrived):
                                sb.Append($"Ημερομηνία '{(DateTime)original:dd/MM/yyyy hh:mm}' σε '{p.Arrived:dd/MM/yyyy hh:mm}', ");
                                break;
                        }
                    }
                }
                if (sb.Length > 0)
                {
                    Changes.Add(new Change { Date = DateTime.Now, Description = sb.ToString().TrimEnd(new[] { ' ', ',' }), ShowUp = p, User = StaticResources.User });
                }
            }
        }

        private void Illness_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(Illness.Problems) && e.PropertyName != nameof(Illness.SelectedIllness) && e.PropertyName != nameof(Illness.SelectedIllnessPropertyName) && Attribute.IsDefined(typeof(Illness).GetProperty(e.PropertyName) ?? throw new InvalidOperationException(), typeof(DisplayNameAttribute)))
            {
                Illness.RaisePropertyChanged(nameof(Illness.Problems));
            }
        }

        private async Task MakeBooking(string obj)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            int par = int.Parse(obj);
            AddNewProgram(par);
            if (par == 1)
            {
                MakeProgramPayment();
                Programs.Last().Payments.Add(Payments.Last());
                Payments.Last().Program = Programs.Last();
                Programs.Last().CalculateRemainingAmount();
            }
            CalculateRemainingAmount();
            RaisePropertyChanged(nameof(PaymentVisibility));

            ProgramPrice = ShowUpPrice = 0;
            ProgramDuration = NumOfShowUps = 0;
            SelectedProgramType = null;
            DateOfIssue = StartDate = DateTime.Today;
            RaisePropertyChanged(nameof(RemainingAmount));
            if (ForceDisable == ForceDisable.forceDisable)
                ForceDisable = ForceDisable.normal;
            // await BasicDataManager.SaveAsync();
            //PaymentsCollectionView.Refresh();
            //PaymentsMassCollectionView.Refresh();
            //PaymentsOnlineCollectionView.Refresh();

            SetColors();
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void PaymentPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Payment.Amount) && Loaded)
                CalculateRemainingAmount();
        }

        private void PaymentsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            //if (e.Action == NotifyCollectionChangedAction.Remove)
            //{
            //    foreach (Payment item in e.OldItems)
            //    {
            //        //Removed items
            //        item.PropertyChanged -= PaymentPropertyChanged;
            //    }
            //}
            //else if (e.Action == NotifyCollectionChangedAction.Add)
            //{
            //    foreach (Payment item in e.NewItems)
            //    {
            //        //Added items
            //        item.PropertyChanged += PaymentPropertyChanged;
            //    }
            //}

            //if (Loaded)
            //{
            //    CalculateRemainingAmount();
            //    GetRemainingDays();
            //}

            //RaisePropertyChanged(nameof(PaymentVisibility));
        }

        private void ProgramPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!Loaded || (e.PropertyName != nameof(Program.StartDay) && e.PropertyName != nameof(Program.Months) && e.PropertyName != nameof(Program.Amount) && e.PropertyName != nameof(Program.Showups) && e.PropertyName != nameof(Program.ProgramTypeO))) return;
            CalculateRemainingAmount();
            if (e.PropertyName == nameof(Program.Showups) || e.PropertyName == nameof(Program.Months))
            {
                ((IEditableCollectionView)ProgramsFunctionalCollectionView).CommitEdit();
                ((IEditableCollectionView)ProgramsPilatesCollectionView).CommitEdit();
                ((IEditableCollectionView)ProgramsFunctionalPilatesCollectionView).CommitEdit();
                ((IEditableCollectionView)ProgramsMassageColelctionView).CommitEdit();
                ((IEditableCollectionView)ProgramsOnlineColelctionView).CommitEdit();
                ((IEditableCollectionView)ProgramsOutdoorCollectionView).CommitEdit();
                ((IEditableCollectionView)ProgramsYogaCollectionView).CommitEdit();
                ((IEditableCollectionView)ProgramsAerialYogaCollectionView).CommitEdit();
                ((IEditableCollectionView)ProgramsPersonalCollectionView).CommitEdit();
                ((IEditableCollectionView)ProgramsMedicalCollectionView).CommitEdit();
            }
            if (e.PropertyName == nameof(Program.ProgramTypeO) && sender is Program p && p.ProgramTypeO != null && p.ShowUpsList != null && EditedInCustomerManagement)
            {
                foreach (var s in p.ShowUpsList)
                {
                    if (s.ProgramModeNew == ProgramMode.pilates && p.ProgramTypeO.ProgramMode == ProgramMode.pilatesFunctional)
                    {
                        s.ProgramMode = ProgramMode.pilates;
                    }
                    if (s.ProgramModeNew == ProgramMode.yoga && p.ProgramTypeO.ProgramMode == ProgramMode.pilatesFunctional)
                    {
                        s.ProgramMode = ProgramMode.yoga;
                    }
                    s.ProgramModeNew = p.ProgramTypeO.ProgramMode;
                }
            }
            if (e.PropertyName != nameof(Program.Amount))
                SetColors();
            else
            {
                if (SelectedFunctionalProgram != null)
                    SelectedFunctionalProgram.CalculateRemainingAmount();
                if (SelectedPilatesProgram != null)
                    SelectedPilatesProgram.CalculateRemainingAmount();
                if (SelectedFunctionalPilatesProgram != null)
                    SelectedFunctionalPilatesProgram.CalculateRemainingAmount();
                if (SelectedMasageProgram != null)
                    SelectedMasageProgram.CalculateRemainingAmount();
                if (SelectedOnlineProgram != null)
                    SelectedOnlineProgram.CalculateRemainingAmount();
                if (SelectedOutdoorProgram != null)
                    SelectedOutdoorProgram.CalculateRemainingAmount();
                if (SelectedYogaProgram != null)
                    SelectedYogaProgram.CalculateRemainingAmount();
                if (SelectedAerialYogaProgram != null)
                    SelectedAerialYogaProgram.CalculateRemainingAmount();
            }
        }

        private bool ProgramsAerialYogaFilter(object obj)
        {
            try
            {
                if (StaticResources.User.Level > 2)
                {
                    return obj is ShowUp s1 && ((ShowUps.Count > 0 && s1 == ShowUps[0]) || (ShowUps.Count > 1 && s1 == ShowUps[1]) || s1.Arrived.Date == DateTime.Today) && s1.ProgramModeNew == ProgramMode.aerialYoga;
                }

                return (obj is Program p && (Full || p.StartDay >= ResetDate) && p.ProgramTypeO?.ProgramMode == ProgramMode.aerialYoga) ||
                    (obj is ShowUp s && (Full || s.Arrived >= ResetDate) && s.ProgramModeNew == ProgramMode.aerialYoga) ||
                    (obj is Payment a && (Full || a.Program?.StartDay >= ResetDate || (a.Program == null && a.Date > ResetDate)) && a.Program != null && a.Program.ProgramTypeO?.ProgramMode == ProgramMode.aerialYoga);
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void ProgramsCollectionChanged()
        {
            //if (e.Action == NotifyCollectionChangedAction.Remove)
            //{
            //    foreach (Program item in e.OldItems)
            //    {
            //        //Removed items
            //        item.PropertyChanged -= ProgramPropertyChanged;
            //    }
            //}
            //else if (e.Action == NotifyCollectionChangedAction.Add)
            //{
            //    foreach (Program item in e.NewItems)
            //    {
            //        //Added items
            //        item.PropertyChanged += ProgramPropertyChanged;
            //    }
            //}

            //if (!Loaded) return;
            //RaisePropertyChanged(nameof(LastBuy));
            //CalculateRemainingAmount();
            //GetRemainingDays();
        }

        private bool ProgramsFunctionalFilter(object obj)
        {
            try
            {
                if (StaticResources.User.Level > 2)
                {
                    return obj is ShowUp s1 && ((ShowUps.Count > 0 && s1 == ShowUps[0]) || (ShowUps.Count > 1 && s1 == ShowUps[1]) || s1.Arrived.Date==DateTime.Today) && s1.ProgramModeNew == ProgramMode.functional;
                }

                return (obj is Program p && (Full || p.StartDay >= ResetDate) && p.ProgramTypeO?.ProgramMode == ProgramMode.functional) ||
                    (obj is ShowUp s && (Full || s.Arrived >= ResetDate) && s.ProgramModeNew == ProgramMode.functional) ||
                    (obj is Payment a && (Full || a.Program?.StartDay >= ResetDate || (a.Program == null && a.Date > ResetDate)) && (a.Program == null || a.Program.ProgramTypeO?.ProgramMode == ProgramMode.functional));
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool ProgramsMassFilter(object obj)
        {
            try
            {
                if (StaticResources.User.Level > 2)
                {
                    return obj is ShowUp s1 && ((ShowUps.Count > 0 && s1 == ShowUps[0]) || (ShowUps.Count > 1 && s1 == ShowUps[1]) || s1.Arrived.Date == DateTime.Today) && s1.ProgramModeNew == ProgramMode.massage;
                }

                return (obj is Program p && (Full || p.StartDay >= ResetDate) && p.ProgramTypeO?.ProgramMode == ProgramMode.massage) ||
                    (obj is ShowUp s && (Full || s.Arrived >= ResetDate) && s.ProgramModeNew == ProgramMode.massage) ||
                    (obj is Payment a && (Full || a.Program?.StartDay >= ResetDate || (a.Program == null && a.Date > ResetDate)) && a.Program != null && a.Program.ProgramTypeO?.ProgramMode == ProgramMode.massage);
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool ProgramsMedicalFilter(object obj)
        {
            try
            {
                if (StaticResources.User.Level > 2)
                {
                    return obj is ShowUp s1 && ((ShowUps.Count > 0 && s1 == ShowUps[0]) || (ShowUps.Count > 1 && s1 == ShowUps[1]) || s1.Arrived.Date == DateTime.Today) && s1.ProgramModeNew == ProgramMode.medical;
                }

                return (obj is Program p && (Full || p.StartDay >= ResetDate) && p.ProgramTypeO?.ProgramMode == ProgramMode.medical) ||
                    (obj is ShowUp s && (Full || s.Arrived >= ResetDate) && s.ProgramModeNew == ProgramMode.medical) ||
                    (obj is Payment a && (Full || a.Program?.StartDay >= ResetDate || (a.Program == null && a.Date > ResetDate)) && a.Program != null && a.Program.ProgramTypeO?.ProgramMode == ProgramMode.medical);
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool ProgramsOnlineFilter(object obj)
        {
            try
            {
                if (StaticResources.User.Level > 2)
                {
                    return obj is ShowUp s1 && ((ShowUps.Count > 0 && s1 == ShowUps[0]) || (ShowUps.Count > 1 && s1 == ShowUps[1]) || s1.Arrived.Date == DateTime.Today) && s1.ProgramModeNew == ProgramMode.online;
                }

                return (obj is Program p && (Full || p.StartDay >= ResetDate) && p.ProgramTypeO?.ProgramMode == ProgramMode.online) ||
                    (obj is ShowUp s && (Full || s.Arrived >= ResetDate) && s.ProgramModeNew == ProgramMode.online) ||
                    (obj is Payment a && (Full || a.Program?.StartDay >= ResetDate || (a.Program == null && a.Date > ResetDate)) && a.Program != null && a.Program.ProgramTypeO?.ProgramMode == ProgramMode.online);
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool ProgramsOutdoorFilter(object obj)
        {
            try
            {
                if (StaticResources.User.Level > 2)
                {
                    return obj is ShowUp s1 && ((ShowUps.Count > 0 && s1 == ShowUps[0]) || (ShowUps.Count > 1 && s1 == ShowUps[1]) || s1.Arrived.Date == DateTime.Today) && s1.ProgramModeNew == ProgramMode.outdoor;
                }

                return (obj is Program p && (Full || p.StartDay >= ResetDate) && p.ProgramTypeO?.ProgramMode == ProgramMode.outdoor) ||
                    (obj is ShowUp s && (Full || s.Arrived >= ResetDate) && s.ProgramModeNew == ProgramMode.outdoor) ||
                    (obj is Payment a && (Full || a.Program?.StartDay >= ResetDate || (a.Program == null && a.Date > ResetDate)) && a.Program != null && a.Program.ProgramTypeO?.ProgramMode == ProgramMode.outdoor);
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool ProgramsPersonallFilter(object obj)
        {
            try
            {
                if (StaticResources.User.Level > 2)
                {
                    return obj is ShowUp s1 && ((ShowUps.Count > 0 && s1 == ShowUps[0]) || (ShowUps.Count > 1 && s1 == ShowUps[1]) || s1.Arrived.Date == DateTime.Today) && s1.ProgramModeNew == ProgramMode.personal;
                }

                return (obj is Program p && (Full || p.StartDay >= ResetDate) && p.ProgramTypeO?.ProgramMode == ProgramMode.personal) ||
                    (obj is ShowUp s && (Full || s.Arrived >= ResetDate) && s.ProgramModeNew == ProgramMode.personal) ||
                    (obj is Payment a && (Full || a.Program?.StartDay >= ResetDate || (a.Program == null && a.Date > ResetDate)) && a.Program != null && a.Program.ProgramTypeO?.ProgramMode == ProgramMode.personal);
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool ProgramsPilatesFilter(object obj)
        {
            try
            {
                if (StaticResources.User.Level > 2)
                {
                    return obj is ShowUp s1 && ((ShowUps.Count > 0 && s1 == ShowUps[0]) || (ShowUps.Count > 1 && s1 == ShowUps[1]) || s1.Arrived.Date == DateTime.Today) && s1.ProgramModeNew == ProgramMode.pilates;
                }

                return (obj is Program p && (Full || p.StartDay >= ResetDate) && p.ProgramTypeO?.ProgramMode == ProgramMode.pilates) ||
                    (obj is ShowUp s && (Full || s.Arrived >= ResetDate) && s.ProgramModeNew == ProgramMode.pilates) ||
                    (obj is Payment a && (Full || a.Program?.StartDay >= ResetDate || (a.Program == null && a.Date > ResetDate)) && a.Program != null && a.Program.ProgramTypeO?.ProgramMode == ProgramMode.pilates);
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool ProgramsPilatesFunctionalFilter(object obj)
        {
            try
            {
                if (StaticResources.User.Level > 2)
                {
                    return obj is ShowUp s1 && ((ShowUps.Count > 0 && s1 == ShowUps[0]) || (ShowUps.Count > 1 && s1 == ShowUps[1]) || s1.Arrived.Date == DateTime.Today) && s1.ProgramModeNew == ProgramMode.pilatesFunctional;
                }

                return (obj is Program p && (Full || p.StartDay >= ResetDate) && p.ProgramTypeO?.ProgramMode == ProgramMode.pilatesFunctional) ||
                    (obj is ShowUp s && (Full || s.Arrived >= ResetDate) && s.ProgramModeNew == ProgramMode.pilatesFunctional) ||
                    (obj is Payment a && (Full || a.Program?.StartDay >= ResetDate || (a.Program == null && a.Date > ResetDate)) && a.Program != null && a.Program.ProgramTypeO?.ProgramMode == ProgramMode.pilatesFunctional);
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool ProgramsYogaFilter(object obj)
        {
            try
            {
                if (StaticResources.User.Level > 2)
                {
                    return obj is ShowUp s1 && ((ShowUps.Count > 0 && s1 == ShowUps[0]) || (ShowUps.Count > 1 && s1 == ShowUps[1]) || s1.Arrived.Date == DateTime.Today) && s1.ProgramModeNew == ProgramMode.yoga;
                }

                return (obj is Program p && (Full || p.StartDay >= ResetDate) && p.ProgramTypeO?.ProgramMode == ProgramMode.yoga) ||
                    (obj is ShowUp s && (Full || s.Arrived >= ResetDate) && s.ProgramModeNew == ProgramMode.yoga) ||
                    (obj is Payment a && (Full || a.Program?.StartDay >= ResetDate || (a.Program == null && a.Date > ResetDate)) && a.Program != null && a.Program.ProgramTypeO?.ProgramMode == ProgramMode.yoga);
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void RaiseAllChanged()
        {
            RaisePropertyChanged(nameof(Payments));
            RaisePropertyChanged(nameof(PaymentsFunctionalCollectionView));
            RaisePropertyChanged(nameof(PaymentsPilatesCollectionView));
            RaisePropertyChanged(nameof(PaymentsFunctionalPilatesCollectionView));
            RaisePropertyChanged(nameof(PaymentsOnlineCollectionView));
            RaisePropertyChanged(nameof(PaymentsOutDoorCollectionView));
            RaisePropertyChanged(nameof(PaymentsMassCollectionView));
            RaisePropertyChanged(nameof(PaymentsYogaCollectionView));
            RaisePropertyChanged(nameof(PaymentsAerialYogaCollectionView));
        }

        private void ReleasePayment(Payment p)
        {
            if (p != null)
            {
                p.Program = null;
                var pr = Programs.FirstOrDefault(pa => pa.Payments.Any(t => t == p));
                pr.Payments.Remove(p);
            }
            SetColors();
        }

        private void RollBackChanges()
        {
            BasicDataManager.RollBack();
            CommandManager.InvalidateRequerySuggested();
        }

        private async Task SaveChanges()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            SetColors();
            CaptureChanges();
            await BasicDataManager.SaveAsync();
            Messenger.Default.Send(new UpdateProgramMessage());
            FromProgram = false;
            CalculateRemainingAmount();
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void SelectedChange_Changed(Change change)
        {
            if (change != null)
                if (change.Payment != null)
                    UpdateSelections(change.Payment);
                else if (change.ShowUp != null)
                    UpdateSelections(change.ShowUp);
                else if (change.Program != null)
                    UpdateSelections(change.Program);
                else
                    UpdateSelections(null);
        }

        private async Task SetToProgram(Payment p)
        {
            if (p == null)
            {
                return;
            }
            if (SelectedProgramFunctionalToDelete != null)
            {
                SelectedProgramFunctionalToDelete.Payments.Add(p);
                p.Program = SelectedProgramFunctionalToDelete;
            }
            else if (SelectedProgramPilatesToDelete != null)
            {
                SelectedProgramPilatesToDelete.Payments.Add(p);
                p.Program = SelectedProgramPilatesToDelete;
            }
            else if (SelectedProgramFunctionalPilatesToDelete != null)
            {
                SelectedProgramFunctionalPilatesToDelete.Payments.Add(p);
                p.Program = SelectedProgramFunctionalPilatesToDelete;
            }
            else if (SelectedProgramMassageToDelete != null)
            {
                SelectedProgramMassageToDelete.Payments.Add(p);
                p.Program = SelectedProgramMassageToDelete;
            }
            else if (SelectedProgramOnlineToDelete != null)
            {
                SelectedProgramOnlineToDelete.Payments.Add(p);
                p.Program = SelectedProgramOnlineToDelete;
            }
            else if (SelectedProgramOutDoorToDelete != null)
            {
                SelectedProgramOutDoorToDelete.Payments.Add(p);
                p.Program = SelectedProgramOutDoorToDelete;
            }
            else if (SelectedProgramYogaToDelete != null)
            {
                SelectedProgramYogaToDelete.Payments.Add(p);
                p.Program = SelectedProgramYogaToDelete;
            }
            else if (SelectedProgramAerialYogaToDelete != null)
            {
                SelectedProgramAerialYogaToDelete.Payments.Add(p);
                p.Program = SelectedProgramAerialYogaToDelete;
            }
            else if (SelectedProgramPersonalToDelete != null)
            {
                SelectedProgramPersonalToDelete.Payments.Add(p);
                p.Program = SelectedProgramPersonalToDelete;
            }
            else if (SelectedProgramMedicalToDelete != null)
            {
                SelectedProgramMedicalToDelete.Payments.Add(p);
                p.Program = SelectedProgramMedicalToDelete;
            }
            p.RaisePropertyChanged(nameof(Payment.PaymentColor));
            await BasicDataManager.SaveAsync();
            SetColors();
        }

        private async Task ShowHistory()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            OldShowUps = new ObservableCollection<ShowUp>((await BasicDataManager.Context.GetAllShowUpsInRangeAsyncsAsync(HistoryFrom, DateTime.Now, Id, true)).OrderByDescending(a => a.Arrived));
            var apps = await BasicDataManager.Context.Context.Apointments.Where(a => a.Customer.Id == Id && a.DateTime >= HistoryFrom).ToListAsync();
            NextAppointments = new ObservableCollection<Apointment>(apps.Where(a => a.DateTime > DateTime.Now).OrderBy(a => a.DateTime));
            UpdateCollections();
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private async Task ShowPreviewsData()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            //Loaded = false;
            //Programs = null;
            //ShowUps = null;
            //Payments = null;
            var _itemsLock = new object();
            BindingOperations.EnableCollectionSynchronization(Changes, _itemsLock);
            lock (_itemsLock)
            {
                BasicDataManager.Context.GetFullCustomerById(Id);
            }

            Loaded = Full = true;
            //if (BasicDataManager.HasChanges())
            //{
            //    await BasicDataManager.SaveAsync();
            //}
            UpdateProgramsCollections();
            UpdatePaymentsCollections();
            UpdateShowUpsCollections();
            UpdateCollections();
            SetColors();
            IsActiveColor = GetCustomerColor();
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private async Task ShowSums()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            var pays = new ObservableCollection<PaymentSum>();
            var programs = await BasicDataManager.Context.Context.Programs.Where(c => c.Customer.Id == Id).ToListAsync();
            PaymentSum paySum;
            foreach (var prog in programs)
            {
                paySum = pays.FirstOrDefault(s => s.From <= prog.StartDay && s.To >= prog.StartDay);
                if (paySum != null)
                    paySum.Amount += prog.Amount;
                else
                    pays.Add(new PaymentSum
                    {
                        Amount = prog.Amount,
                        From = new DateTime(prog.StartDay.Month >= 9 ? prog.StartDay.Year : (prog.StartDay.Year + 1), 9, 1),
                        To = new DateTime(prog.StartDay.Month >= 9 ? prog.StartDay.Year + 1 : prog.StartDay.Year, 9, 1)
                    });
            }
            foreach (var p in pays)
            {
                p.Year = p.From.Year % 100 + "-" + p.To.Year % 100;
            }
            PaymentSums = new ObservableCollection<PaymentSum>(pays.OrderBy(o => o.From));
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void ShowUps_CollectionChanged()
        {
            IsActiveColor = GetCustomerColor();
            SetColors();
            RaisePropertyChanged(nameof(ShowedUpToday));
            RaisePropertyChanged(nameof(ShowUpsFunctionalCollectionView));
            RaisePropertyChanged(nameof(ShowUpsPilatesCollectionView));
            RaisePropertyChanged(nameof(ShowUpsFunctionalPilatesCollectionView));
            RaisePropertyChanged(nameof(ShowUpsMassCollectionView));
            RaisePropertyChanged(nameof(ShowUpsOnlineCollectionView));
            RaisePropertyChanged(nameof(ShowUpsOutDoorCollectionView));
            RaisePropertyChanged(nameof(ShowUpsYogaCollectionView));
            RaisePropertyChanged(nameof(ShowUpsAerialYogaCollectionView));
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            RaisePropertyChanged(nameof(Duration));
        }

        private void Toggle30_60(ShowUp su)
        {
            if (su == null)
                return;

            su.Is30min = !su.Is30min;
        }

        private void TogglePresent(ShowUp su)
        {
            if (su == null)
            {
                return;
            }
            su.Present = !su.Present;
        }

        private void ToggleTest(ShowUp su)
        {
            if (su == null)
            {
                return;
            }
            su.Test = !su.Test;
        }

        private async Task TogleReal(ShowUp showup)
        {
            if (showup == null)
            {
                return;
            }
            showup.Real = !showup.Real;
            //await BasicDataManager.SaveAsync();
        }

        private void TogleShowUp(object[] props)
        {
            if (props[0] is ShowUp su && props[1] is string st && int.TryParse(st, out int i))
            {
                if (su.ProgramModeNew == ProgramMode.pilates && i == 6)
                {
                    su.ProgramMode = ProgramMode.pilates;
                }
                su.ProgramModeNew = (ProgramMode)i;
            }
            else
            {
                return;
            }
            SetColors();
        }

        private void TogleShowUpPilFun(object[] props)
        {
            if (props[0] is ShowUp su && props[1] is string st)
            {
                if (st == "4")
                    su.ProgramMode = ProgramMode.pilates;
                else if (st == "0")
                    su.ProgramMode = ProgramMode.functional;
                else if (st == "5")
                    su.ProgramMode = ProgramMode.yoga;
                su.RaisePropertyChanged(nameof(su.Type));
            }
            else
            {
                return;
            }
        }

        private void UpdatePaymentsCollections()
        {
            if (!Loaded)
            {
                return;
            }

            if (PaymentsFunctionalCollectionView == null)
            {
                PaymentsFunctionalCollectionView = new ListCollectionView(Payments) { Filter = ProgramsFunctionalFilter };
                PaymentsFunctionalCollectionView.SortDescriptions.Add(new SortDescription("Date", ListSortDirection.Descending));
                PaymentsFunctionalCollectionView.SortDescriptions.Add(new SortDescription("Id", ListSortDirection.Descending));
            }
            if (PaymentsPilatesCollectionView == null)
            {
                PaymentsPilatesCollectionView = new ListCollectionView(Payments) { Filter = ProgramsPilatesFilter };
                PaymentsPilatesCollectionView.SortDescriptions.Add(new SortDescription("Date", ListSortDirection.Descending));
                PaymentsPilatesCollectionView.SortDescriptions.Add(new SortDescription("Id", ListSortDirection.Descending));
            }
            if (PaymentsFunctionalPilatesCollectionView == null)
            {
                PaymentsFunctionalPilatesCollectionView = new ListCollectionView(Payments) { Filter = ProgramsPilatesFunctionalFilter };
                PaymentsFunctionalPilatesCollectionView.SortDescriptions.Add(new SortDescription("Date", ListSortDirection.Descending));
                PaymentsFunctionalPilatesCollectionView.SortDescriptions.Add(new SortDescription("Id", ListSortDirection.Descending));
            }
            if (PaymentsYogaCollectionView == null)
            {
                PaymentsYogaCollectionView = new ListCollectionView(Payments) { Filter = ProgramsYogaFilter };
                PaymentsYogaCollectionView.SortDescriptions.Add(new SortDescription("Date", ListSortDirection.Descending));
                PaymentsYogaCollectionView.SortDescriptions.Add(new SortDescription("Id", ListSortDirection.Descending));
            }
            if (PaymentsAerialYogaCollectionView == null)
            {
                PaymentsAerialYogaCollectionView = new ListCollectionView(Payments) { Filter = ProgramsAerialYogaFilter };
                PaymentsAerialYogaCollectionView.SortDescriptions.Add(new SortDescription("Date", ListSortDirection.Descending));
                PaymentsAerialYogaCollectionView.SortDescriptions.Add(new SortDescription("Id", ListSortDirection.Descending));
            }
            if (PaymentsMassCollectionView == null)
            {
                PaymentsMassCollectionView = new ListCollectionView(Payments) { Filter = ProgramsMassFilter };
                PaymentsMassCollectionView.SortDescriptions.Add(new SortDescription("Date", ListSortDirection.Descending));
                PaymentsMassCollectionView.SortDescriptions.Add(new SortDescription("Id", ListSortDirection.Descending));
            }
            if (PaymentsOnlineCollectionView == null)
            {
                PaymentsOnlineCollectionView = new ListCollectionView(Payments) { Filter = ProgramsOnlineFilter };
                PaymentsOnlineCollectionView.SortDescriptions.Add(new SortDescription("Date", ListSortDirection.Descending));
                PaymentsOnlineCollectionView.SortDescriptions.Add(new SortDescription("Id", ListSortDirection.Descending));
            }
            if (PaymentsOutDoorCollectionView == null)
            {
                PaymentsOutDoorCollectionView = new ListCollectionView(Payments) { Filter = ProgramsOutdoorFilter };
                PaymentsOutDoorCollectionView.SortDescriptions.Add(new SortDescription("Date", ListSortDirection.Descending));
                PaymentsOutDoorCollectionView.SortDescriptions.Add(new SortDescription("Id", ListSortDirection.Descending));
            }
            if (PaymentsPersonalCollectionView == null)
            {
                PaymentsPersonalCollectionView = new ListCollectionView(Payments) { Filter = ProgramsPersonallFilter };
                PaymentsPersonalCollectionView.SortDescriptions.Add(new SortDescription("Date", ListSortDirection.Descending));
                PaymentsPersonalCollectionView.SortDescriptions.Add(new SortDescription("Id", ListSortDirection.Descending));
            }
            if (PaymentsMedicalCollectionView == null)
            {
                PaymentsMedicalCollectionView = new ListCollectionView(Payments) { Filter = ProgramsMedicalFilter };
                PaymentsMedicalCollectionView.SortDescriptions.Add(new SortDescription("Date", ListSortDirection.Descending));
                PaymentsMedicalCollectionView.SortDescriptions.Add(new SortDescription("Id", ListSortDirection.Descending));
            }
        }

        private void UpdateProgramsCollections()
        {
            if (!Loaded)
            {
                return;
            }
            if (ProgramsFunctionalCollectionView == null)
            {
                ProgramsFunctionalCollectionView = new ListCollectionView(Programs)
                {
                    //ProgramsFunctionalCollectionView.SortDescriptions.Add(new SortDescription("StartDay", ListSortDirection.Descending));
                    Filter = ProgramsFunctionalFilter
                };
            }
            if (ProgramsPilatesCollectionView == null)
            {
                ProgramsPilatesCollectionView = new ListCollectionView(Programs)
                {
                    //ProgramsPilatesCollectionView.SortDescriptions.Add(new SortDescription("StartDay", ListSortDirection.Descending));
                    Filter = ProgramsPilatesFilter
                };
            }
            if (ProgramsFunctionalPilatesCollectionView == null)
            {
                ProgramsFunctionalPilatesCollectionView = new ListCollectionView(Programs)
                {
                    //ProgramsFunctionalPilatesCollectionView.SortDescriptions.Add(new SortDescription("StartDay", ListSortDirection.Descending));
                    Filter = ProgramsPilatesFunctionalFilter
                };
            }
            if (ProgramsMassageColelctionView == null)
            {
                ProgramsMassageColelctionView = new ListCollectionView(Programs)
                {
                    // ProgramsMassageColelctionView.SortDescriptions.Add(new SortDescription("StartDay", ListSortDirection.Descending));
                    Filter = ProgramsMassFilter
                };
            }
            if (ProgramsOnlineColelctionView == null)
            {
                ProgramsOnlineColelctionView = new ListCollectionView(Programs)
                {
                    //ProgramsOnlineColelctionView.SortDescriptions.Add(new SortDescription("StartDay", ListSortDirection.Descending));
                    Filter = ProgramsOnlineFilter
                };
            }
            if (ProgramsOutdoorCollectionView == null)
            {
                ProgramsOutdoorCollectionView = new ListCollectionView(Programs)
                {
                    //ProgramsOutdoorCollectionView.SortDescriptions.Add(new SortDescription("StartDay", ListSortDirection.Descending));
                    Filter = ProgramsOutdoorFilter
                };
            }
            if (ProgramsYogaCollectionView == null)
            {
                ProgramsYogaCollectionView = new ListCollectionView(Programs)
                {
                    // ProgramsYogaCollectionView.SortDescriptions.Add(new SortDescription("StartDay", ListSortDirection.Descending));
                    Filter = ProgramsYogaFilter
                };
            }
            if (ProgramsAerialYogaCollectionView == null)
            {
                ProgramsAerialYogaCollectionView = new ListCollectionView(Programs)
                {
                    // ProgramsAerialYogaCollectionView.SortDescriptions.Add(new SortDescription("StartDay", ListSortDirection.Descending));
                    Filter = ProgramsAerialYogaFilter
                };
            }
            if (ProgramsPersonalCollectionView == null)
            {
                ProgramsPersonalCollectionView = new ListCollectionView(Programs)
                {
                    // ProgramsPersonalCollectionView.SortDescriptions.Add(new SortDescription("StartDay", ListSortDirection.Descending));
                    Filter = ProgramsPersonallFilter
                };
            }
            if (ProgramsMedicalCollectionView == null)
            {
                ProgramsMedicalCollectionView = new ListCollectionView(Programs)
                {
                    //  ProgramsMedicalCollectionView.SortDescriptions.Add(new SortDescription("StartDay", ListSortDirection.Descending));
                    Filter = ProgramsMedicalFilter
                };
            }
        }

        private void UpdateShowUpsCollections()
        {
            if (!Loaded)
            {
                return;
            }

            if (ShowUpsFunctionalCollectionView == null)
            {
                ShowUpsFunctionalCollectionView = new ListCollectionView(ShowUps)
                {
                    Filter = ProgramsFunctionalFilter
                };
                ShowUpsFunctionalCollectionView.SortDescriptions.Add(new SortDescription("Arrived", ListSortDirection.Descending));
            }
            if (ShowUpsPilatesCollectionView == null)
            {
                ShowUpsPilatesCollectionView = new ListCollectionView(ShowUps)
                {
                    Filter = ProgramsPilatesFilter
                };
                ShowUpsPilatesCollectionView.SortDescriptions.Add(new SortDescription("Arrived", ListSortDirection.Descending));
            }
            if (ShowUpsFunctionalPilatesCollectionView == null)
            {
                ShowUpsFunctionalPilatesCollectionView = new ListCollectionView(ShowUps)
                {
                    Filter = ProgramsPilatesFunctionalFilter
                };
                ShowUpsFunctionalPilatesCollectionView.SortDescriptions.Add(new SortDescription("Arrived", ListSortDirection.Descending));
            }
            if (ShowUpsMassCollectionView == null)
            {
                ShowUpsMassCollectionView = new ListCollectionView(ShowUps)
                {
                    Filter = ProgramsMassFilter
                };
                ShowUpsMassCollectionView.SortDescriptions.Add(new SortDescription("Arrived", ListSortDirection.Descending));
            }
            if (ShowUpsOnlineCollectionView == null)
            {
                ShowUpsOnlineCollectionView = new ListCollectionView(ShowUps)
                {
                    Filter = ProgramsOnlineFilter
                };
                ShowUpsOnlineCollectionView.SortDescriptions.Add(new SortDescription("Arrived", ListSortDirection.Descending));
            }
            if (ShowUpsOutDoorCollectionView == null)
            {
                ShowUpsOutDoorCollectionView = new ListCollectionView(ShowUps)
                {
                    Filter = ProgramsOutdoorFilter
                };
                ShowUpsOutDoorCollectionView.SortDescriptions.Add(new SortDescription("Arrived", ListSortDirection.Descending));
            }
            if (ShowUpsYogaCollectionView == null)
            {
                ShowUpsYogaCollectionView = new ListCollectionView(ShowUps)
                {
                    Filter = ProgramsYogaFilter
                };
                ShowUpsYogaCollectionView.SortDescriptions.Add(new SortDescription("Arrived", ListSortDirection.Descending));
            }
            if (ShowUpsAerialYogaCollectionView == null)
            {
                ShowUpsAerialYogaCollectionView = new ListCollectionView(ShowUps)
                {
                    Filter = ProgramsAerialYogaFilter
                };
                ShowUpsAerialYogaCollectionView.SortDescriptions.Add(new SortDescription("Arrived", ListSortDirection.Descending));
            }
            if (ShowUpsMedicalCollectionView == null)
            {
                ShowUpsMedicalCollectionView = new ListCollectionView(ShowUps)
                {
                    Filter = ProgramsMedicalFilter
                };
                ShowUpsMedicalCollectionView.SortDescriptions.Add(new SortDescription("Arrived", ListSortDirection.Descending));
            }
            if (ShowUpsPersonalCollectionView == null)
            {
                ShowUpsPersonalCollectionView = new ListCollectionView(ShowUps)
                {
                    Filter = ProgramsPersonallFilter
                };
                ShowUpsPersonalCollectionView.SortDescriptions.Add(new SortDescription("Arrived", ListSortDirection.Descending));
            }
        }

        //private void TryRefrehAllCollections()
        //{
        //    GetRemainingDays();
        //    ProgramsMassageColelctionView = new CollectionViewSource { Source = Programs }.View;

        //    ProgramsColelctionView = new CollectionViewSource { Source = Programs }.View;
        //    ProgramsColelctionView.SortDescriptions.Add(new SortDescription("StartDay", ListSortDirection.Descending));
        //    ProgramsColelctionView.Filter = ProgramsgymFilter;

        //    ShowUpsCollectionView = new CollectionViewSource { Source = ShowUps }.View;
        //    ShowUpsCollectionView.SortDescriptions.Add(new SortDescription("Arrived", ListSortDirection.Descending));
        //    ShowUpsCollectionView.Filter = ProgramsgymFilter;

        //    ShowUpsMassCollectionView = new CollectionViewSource { Source = ShowUps }.View;
        //    ShowUpsMassCollectionView.SortDescriptions.Add(new SortDescription("Arrived", ListSortDirection.Descending));
        //    ShowUpsMassCollectionView.Filter = ProgramsmasFilter; ;
        //}

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
            if (!Loaded)
            {
                return;
            }
            RaisePropertyChanged(nameof(BMI));
        }

        #endregion Methods
    }

    public class PaymentSum : BaseModel
    {
        #region Fields

        private decimal _Amount;
        private DateTime _From;

        private DateTime _To;

        private string _Year;

        #endregion Fields

        #region Properties

        public decimal Amount
        {
            get
            {
                return _Amount;
            }

            set
            {
                if (_Amount == value)
                {
                    return;
                }

                _Amount = value;
                RaisePropertyChanged();
            }
        }

        public DateTime From
        {
            get
            {
                return _From;
            }

            set
            {
                if (_From == value)
                {
                    return;
                }

                _From = value;
                RaisePropertyChanged();
            }
        }

        public DateTime To
        {
            get
            {
                return _To;
            }

            set
            {
                if (_To == value)
                {
                    return;
                }

                _To = value;
                RaisePropertyChanged();
            }
        }

        public string Year
        {
            get
            {
                return _Year;
            }

            set
            {
                if (_Year == value)
                {
                    return;
                }

                _Year = value;
                RaisePropertyChanged();
            }
        }

        #endregion Properties
    }
}