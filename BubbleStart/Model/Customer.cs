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
        private bool _ActiveCustomer;
        private string _Address;
        private bool _Alcohol;
        private int _AlcoholUsage;
        private ObservableCollection<Apointment> _Apointments;
        private DateTime _AppointmentTime;
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
        private ForceDisable _ForceDisable;
        private bool _Gender;
        private int _Height;
        private string _HistoryDuration;
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
        private bool _Loaded;
        private bool _Medicine;
        private string _MedicineText;
        private string _Name;
        private decimal _NewWeight;
        private decimal _NextPayment;
        private string _Notes;
        private int _NumOfShowUps;
        private DateTime _OldShowUpDate;
        private decimal _PaymentAmount;
        private bool _PaymentReciept;
        private ObservableCollection<Payment> _Payments;
        private ICollectionView _PaymentsFunctionalCollectionView;
        private ICollectionView _PaymentsMassCollectionView;
        private ICollectionView _PaymentsOnlineCollectionView;
        private PaymentType _PaymentType;
        private bool _Popup1Open;

        private bool _PreferedHand;

        private int _ProgramDuration;

        //  private bool _Pregnancy;
        private decimal _ProgramPrice;

        private string _ProgramResult;

        private ObservableCollection<Program> _Programs;

        private ICollectionView _ProgramsFunctionalCollectionView;

        private ICollectionView _ProgramsMassageColelctionView;

        private ICollectionView _ProgramsOnlineColelctionView;

        private bool _ReasonInjury;

        private bool _ReasonPower;

        private bool _ReasonSlim;

        private bool _ReasonVeltiwsh;

        private decimal _RemainingAmount;

        private Change _SelectedChange;
        private Program _SelectedMasageProgram;

        private ShowUp _SelectedMassShowUp;

        private ShowUp _SelectedOnlineShowUp;

        private Payment _SelectedFunctionalPayment;

        private Payment _SelectedMassPayment;

        private Payment _SelectedOnlinePayment;

        private Program _SelectedFunctionalProgram;

        private Program _SelectedProgramMassageToDelete;

        private Program _SelectedOnlineProgram;
        private Program _SelectedProgramOnlineToDelete;

        private Program _SelectedProgramFunctionalToDelete;

        private ShowUp _SelectedFunctionalShowUp;

        private decimal _ShowUpPrice;

        private ObservableCollection<ShowUp> _ShowUps;

        private ICollectionView _ShowUpsFunctionalCollectionView;

        private ICollectionView _ShowUpsMassCollectionView;

        private ICollectionView _ShowUpsOnlineCollectionView;

        private bool _Smoker;

        //private bool _Signed;
        private int _SmokingUsage;

        private DateTime _StartDate;

        private string _SureName;

        private bool _Surgery;

        private string _SurgeryInfo;

        private string _Tel;

        private bool _WantToQuit;

        private ObservableCollection<Weight> _WeightHistory;

        private DateTime _HistoryFrom = DateTime.Today.AddMonths(-3);

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

        public string Active
        {
            get
            {
                if (RemainingTrainingDays > 0)
                    if (SelectedFunctionalProgram != null)
                        return $"ΝΑΙ (έως {SelectedFunctionalProgram.StartDay.AddMonths(SelectedFunctionalProgram.Months):dd/MM})";
                if (RemainingPilatesDays > 0)
                    if (SelectedPilatesProgram != null)
                        return $"ΝΑΙ (έως {SelectedPilatesProgram.StartDay.AddMonths(SelectedPilatesProgram.Months):dd/MM})";
                if (RemainingFunctionalPilatesDays > 0)
                    if (SelectedFunctionalPilatesProgram != null)
                        return $"ΝΑΙ (έως {SelectedFunctionalPilatesProgram.StartDay.AddMonths(SelectedFunctionalPilatesProgram.Months):dd/MM})";
                if (RemainingOnlineDays > 0)
                    if (SelectedOnlineProgram != null)
                        return $"ΝΑΙ (έως {SelectedOnlineProgram.StartDay.AddMonths(SelectedOnlineProgram.Months):dd/MM})";
                if (RemainingOutDoorDays > 0)
                    if (SelectedOutdoorProgram != null)
                        return $"ΝΑΙ (έως {SelectedOutdoorProgram.StartDay.AddMonths(SelectedOutdoorProgram.Months):dd/MM})";
                if (RemainingMassageDays > 0)
                    if (SelectedMasageProgram != null)
                        return $"ΝΑΙ (έως {SelectedMasageProgram.StartDay.AddMonths(SelectedMasageProgram.Months):dd/MM})";
                if (RemainingYogaDays > 0)
                    if (SelectedYogaProgram != null)
                        return $"ΝΑΙ (έως {SelectedYogaProgram.StartDay.AddMonths(SelectedYogaProgram.Months):dd/MM})";
                if (RemainingAerialYogaDays > 0)
                    if (SelectedAerialYogaProgram != null)
                        return $"ΝΑΙ (έως {SelectedAerialYogaProgram.StartDay.AddMonths(SelectedAerialYogaProgram.Months):dd/MM})";
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
        public RelayCommand CancelChangesAsyncCommand { get; set; }

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
                var changesCv = CollectionViewSource.GetDefaultView(Changes);
                changesCv.SortDescriptions.Add(new SortDescription(nameof(Change.Date), ListSortDirection.Descending));
                RaisePropertyChanged();
            }
        }

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
        public RelayCommand<ShowUp> DeleteShowUpCommand { get; set; }

        [NotMapped]
        public RelayCommand<Payment> DeletePaymentCommand { get; set; }

        [NotMapped]
        public RelayCommand<Program> DeleteProgramCommand { get; set; }

        [NotMapped]
        public RelayCommand<object> DeleteItemCommand { get; set; }

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

        public TimeSpan Duration => LastShowUp != null ? DateTime.Now.Subtract(LastShowUp.Arrived) : new TimeSpan(0);

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
            get => _Illness ?? (_Illness = new Illness());

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
                {
                    ShowUpPrice = ProgramPrice / value;
                }
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

        public ObservableCollection<Payment> Payments
        {
            get => _Payments;

            set
            { //TODO
                if (_Payments == value)
                {
                    return;
                }

                _Payments = value;
                Payments.CollectionChanged += PaymentsCollectionChanged;

                PaymentsFunctionalCollectionView = new ListCollectionView(Payments) { Filter = ProgramsFunctionalFilter };
                PaymentsFunctionalCollectionView.SortDescriptions.Add(new SortDescription("Date", ListSortDirection.Descending));
                PaymentsFunctionalCollectionView.SortDescriptions.Add(new SortDescription("Id", ListSortDirection.Descending));

                PaymentsPilatesCollectionView = new ListCollectionView(Payments) { Filter = ProgramsPilatesFilter };
                PaymentsPilatesCollectionView.SortDescriptions.Add(new SortDescription("Date", ListSortDirection.Descending));
                PaymentsPilatesCollectionView.SortDescriptions.Add(new SortDescription("Id", ListSortDirection.Descending));

                PaymentsFunctionalPilatesCollectionView = new ListCollectionView(Payments) { Filter = ProgramsPilatesFunctionalFilter };
                PaymentsFunctionalPilatesCollectionView.SortDescriptions.Add(new SortDescription("Date", ListSortDirection.Descending));
                PaymentsFunctionalPilatesCollectionView.SortDescriptions.Add(new SortDescription("Id", ListSortDirection.Descending));

                PaymentsYogaCollectionView = new ListCollectionView(Payments) { Filter = ProgramsYogaFilter };
                PaymentsYogaCollectionView.SortDescriptions.Add(new SortDescription("Date", ListSortDirection.Descending));
                PaymentsYogaCollectionView.SortDescriptions.Add(new SortDescription("Id", ListSortDirection.Descending));

                PaymentsAerialYogaCollectionView = new ListCollectionView(Payments) { Filter = ProgramsAerialYogaFilter };
                PaymentsAerialYogaCollectionView.SortDescriptions.Add(new SortDescription("Date", ListSortDirection.Descending));
                PaymentsAerialYogaCollectionView.SortDescriptions.Add(new SortDescription("Id", ListSortDirection.Descending));

                PaymentsMassCollectionView = new ListCollectionView(Payments) { Filter = ProgramsMassFilter };
                PaymentsMassCollectionView.SortDescriptions.Add(new SortDescription("Date", ListSortDirection.Descending));
                PaymentsMassCollectionView.SortDescriptions.Add(new SortDescription("Id", ListSortDirection.Descending));

                PaymentsOnlineCollectionView = new ListCollectionView(Payments) { Filter = ProgramsOnlineFilter };
                PaymentsOnlineCollectionView.SortDescriptions.Add(new SortDescription("Date", ListSortDirection.Descending));
                PaymentsOnlineCollectionView.SortDescriptions.Add(new SortDescription("Id", ListSortDirection.Descending));

                PaymentsOutDoorCollectionView = new ListCollectionView(Payments) { Filter = ProgramsOutdoorFilter };
                PaymentsOutDoorCollectionView.SortDescriptions.Add(new SortDescription("Date", ListSortDirection.Descending));
                PaymentsOutDoorCollectionView.SortDescriptions.Add(new SortDescription("Id", ListSortDirection.Descending));

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

        private ICollectionView _PaymentsPilatesCollectionView;

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

        private ICollectionView _PaymentsFunctionalPilatesCollectionView;

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

        private ICollectionView _PaymentsYogaCollectionView;

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

        private ICollectionView _PaymentsAerialYogaCollectionView;

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

        private ICollectionView _PaymentsOutDoorCollectionView;

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
        public ObservableCollection<Program> Programs
        {
            get => _Programs;

            set
            { //TODO
                if (_Programs == value)
                {
                    return;
                }

                _Programs = value;
                Programs.CollectionChanged += ProgramsCollectionChanged;

                ProgramsFunctionalCollectionView = new ListCollectionView(Programs);
                ProgramsFunctionalCollectionView.SortDescriptions.Add(new SortDescription("StartDay", ListSortDirection.Descending));
                ProgramsFunctionalCollectionView.Filter = ProgramsFunctionalFilter;

                ProgramsPilatesCollectionView = new ListCollectionView(Programs);
                ProgramsPilatesCollectionView.SortDescriptions.Add(new SortDescription("StartDay", ListSortDirection.Descending));
                ProgramsPilatesCollectionView.Filter = ProgramsPilatesFilter;

                ProgramsFunctionalPilatesCollectionView = new ListCollectionView(Programs);
                ProgramsFunctionalPilatesCollectionView.SortDescriptions.Add(new SortDescription("StartDay", ListSortDirection.Descending));
                ProgramsFunctionalPilatesCollectionView.Filter = ProgramsPilatesFunctionalFilter;

                ProgramsMassageColelctionView = new ListCollectionView(Programs);
                ProgramsMassageColelctionView.SortDescriptions.Add(new SortDescription("StartDay", ListSortDirection.Descending));
                ProgramsMassageColelctionView.Filter = ProgramsMassFilter;

                ProgramsOnlineColelctionView = new ListCollectionView(Programs);
                ProgramsOnlineColelctionView.SortDescriptions.Add(new SortDescription("StartDay", ListSortDirection.Descending));
                ProgramsOnlineColelctionView.Filter = ProgramsOnlineFilter;

                ProgramsOutdoorCollectionView = new ListCollectionView(Programs);
                ProgramsOutdoorCollectionView.SortDescriptions.Add(new SortDescription("StartDay", ListSortDirection.Descending));
                ProgramsOutdoorCollectionView.Filter = ProgramsOutdoorFilter;

                ProgramsYogaCollectionView = new ListCollectionView(Programs);
                ProgramsYogaCollectionView.SortDescriptions.Add(new SortDescription("StartDay", ListSortDirection.Descending));
                ProgramsYogaCollectionView.Filter = ProgramsYogaFilter;

                ProgramsAerialYogaCollectionView = new ListCollectionView(Programs);
                ProgramsAerialYogaCollectionView.SortDescriptions.Add(new SortDescription("StartDay", ListSortDirection.Descending));
                ProgramsAerialYogaCollectionView.Filter = ProgramsAerialYogaFilter;

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
                //if (value != null)
                //{
                //    ProgramsFunctionalCollectionView.SortDescriptions.Add(new SortDescription("StartDay", ListSortDirection.Descending));
                //}
                //ProgramsFunctionalCollectionView.Filter = ProgramsFunctionalFilter;
                RaisePropertyChanged();
            }
        }

        private ICollectionView _ProgramsPilatesCollectionView;

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

        private ICollectionView _ProgramsFunctionalPilatesCollectionView;

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

        private ICollectionView _ProgramsOutdoorCollectionView;

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

        private ICollectionView _ProgramsYogaCollectionView;

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

        private ICollectionView _ProgramsAerialYogaCollectionView;

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

        [NotMapped]
        public RelayCommand AddItemCommand { get; set; }

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

        public int RemainingTrainingDays => Programs.Where(p => p.ProgramTypeO.ProgramMode == ProgramMode.functional).Sum(p => p.RemainingDays);
        public int RemainingPilatesDays => Programs.Where(p => p.ProgramTypeO.ProgramMode == ProgramMode.pilates).Sum(p => p.RemainingDays);
        public int RemainingFunctionalPilatesDays => Programs.Where(p => p.ProgramTypeO.ProgramMode == ProgramMode.pilatesFunctional).Sum(p => p.RemainingDays);
        public int RemainingMassageDays => Programs.Where(p => p.ProgramTypeO.ProgramMode == ProgramMode.massage).Sum(p => p.RemainingDays);
        public int RemainingOnlineDays => Programs.Where(p => p.ProgramTypeO.ProgramMode == ProgramMode.online).Sum(p => p.RemainingDays);
        public int RemainingOutDoorDays => Programs.Where(p => p.ProgramTypeO.ProgramMode == ProgramMode.outdoor).Sum(p => p.RemainingDays);
        public int RemainingYogaDays => Programs.Where(p => p.ProgramTypeO.ProgramMode == ProgramMode.yoga).Sum(p => p.RemainingDays);
        public int RemainingAerialYogaDays => Programs.Where(p => p.ProgramTypeO.ProgramMode == ProgramMode.aerialYoga).Sum(p => p.RemainingDays);

        public DateTime LastBuy => GetLastBuy();

        private DateTime GetLastBuy()
        {
            if (Programs.Count > 0)
            {
                var pa = Programs.OrderByDescending(p => p.DayOfIssue).First();
                if (pa != null)
                {
                    return pa.DayOfIssue;
                }
            }
            return new DateTime();
        }

        public int RemainingYogaFullDays => RemainingYogaDays + RemainingAerialYogaDays;
        public int RemainingTrainingFullDays => RemainingFunctionalPilatesDays + RemainingTrainingDays + RemainingPilatesDays;

        [NotMapped]
        public RelayCommand SaveChangesAsyncCommand { get; set; }

        [NotMapped]
        public RelayCommand ShowHistoryCommand { get; set; }

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

        private Program _SelectedFunctionalPilatesProgram;

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

        private Program _SelectedPilatesProgram;

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

        private Program _SelectedOutdoorProgram;

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

        private Program _SelectedYogaProgram;

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

        private Program _SelectedAerialYogaProgram;

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

        private ShowUp _SelectedPilatesShowUp;

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

        private ShowUp _SelectedFunctionalPilatesShowUp;

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

        private ShowUp _SelectedOutdoorShowUp;

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

        private ShowUp _SelectedYogaShowUp;

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

        private ShowUp _SelectedAerialYogaShowUp;

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

        private Payment _SelectedPilatesPayment;

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

        private Payment _SelectedFunctionalPilatesPayment;

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

        private Payment _SelectedOutdoorPayment;

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

        private Payment _SelectedYogaPayment;

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

        private Payment _SelectedAerialYogaPayment;

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

                PaymentAmount = SelectedProgramMassageToDelete != null && SelectedProgramMassageToDelete.RemainingAmount > 0
                    ? SelectedProgramMassageToDelete.RemainingAmount
                    : 0;

                _SelectedProgramMassageToDelete = value;
                RaisePropertyChanged();
            }
        }

        private Program _SelectedProgramPilatesToDelete;

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

                PaymentAmount = SelectedProgramPilatesToDelete != null && SelectedProgramPilatesToDelete.RemainingAmount > 0 ? SelectedProgramPilatesToDelete.RemainingAmount : 0;
                _SelectedProgramPilatesToDelete = value;
                RaisePropertyChanged();
            }
        }

        private Program _SelectedProgramFunctionalPilatesToDelete;

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
                PaymentAmount = SelectedProgramFunctionalPilatesToDelete != null && SelectedProgramFunctionalPilatesToDelete.RemainingAmount > 0 ? SelectedProgramFunctionalPilatesToDelete.RemainingAmount : 0;
                _SelectedProgramFunctionalPilatesToDelete = value;
                RaisePropertyChanged();
            }
        }

        private Program _SelectedProgramOutDoorToDelete;

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
                PaymentAmount = SelectedProgramOutDoorToDelete != null && SelectedProgramOutDoorToDelete.RemainingAmount > 0 ? SelectedProgramOutDoorToDelete.RemainingAmount : 0;

                _SelectedProgramOutDoorToDelete = value;
                RaisePropertyChanged();
            }
        }

        private Program _SelectedProgramYogaToDelete;

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
                PaymentAmount = SelectedProgramYogaToDelete != null && SelectedProgramYogaToDelete.RemainingAmount > 0 ? SelectedProgramYogaToDelete.RemainingAmount : 0;

                _SelectedProgramYogaToDelete = value;
                RaisePropertyChanged();
            }
        }

        private Program _SelectedProgramAerialYogaToDelete;

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

                PaymentAmount = SelectedProgramAerialYogaToDelete != null && SelectedProgramAerialYogaToDelete.RemainingAmount > 0 ? SelectedProgramAerialYogaToDelete.RemainingAmount : 0;

                _SelectedProgramAerialYogaToDelete = value;
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

                PaymentAmount = SelectedProgramOnlineToDelete != null && SelectedProgramOnlineToDelete.RemainingAmount > 0 ? SelectedProgramOnlineToDelete.RemainingAmount : 0;

                _SelectedProgramOnlineToDelete = value;
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

                PaymentAmount = SelectedProgramFunctionalToDelete != null && SelectedProgramFunctionalToDelete.RemainingAmount > 0 ? SelectedProgramFunctionalToDelete.RemainingAmount : 0;

                _SelectedProgramFunctionalToDelete = value;
                RaisePropertyChanged();
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
        public RelayCommand<Payment> SetToProgramCommand { get; set; }

        [NotMapped]
        public bool ShowedUpToday => ShowUps.Any(s => s.Arrived.Date == DateTime.Today);

        [NotMapped]
        public RelayCommand ShowPreviusDataCommand { get; set; }

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
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(ProgramPrice));
            }
        }

        public ObservableCollection<ShowUp> ShowUps
        {
            get => _ShowUps;

            set
            {          //TODO
                if (_ShowUps == value)
                {
                    return;
                }

                _ShowUps = value;
                ShowUps.CollectionChanged += ShowUps_CollectionChanged;

                ShowUpsFunctionalCollectionView = new ListCollectionView(ShowUps);
                ShowUpsFunctionalCollectionView.SortDescriptions.Add(new SortDescription("Arrived", ListSortDirection.Descending));
                ShowUpsFunctionalCollectionView.Filter = ProgramsFunctionalFilter;

                ShowUpsPilatesCollectionView = new ListCollectionView(ShowUps);
                ShowUpsPilatesCollectionView.SortDescriptions.Add(new SortDescription("Arrived", ListSortDirection.Descending));
                ShowUpsPilatesCollectionView.Filter = ProgramsPilatesFilter;

                ShowUpsFunctionalPilatesCollectionView = new ListCollectionView(ShowUps);
                ShowUpsFunctionalPilatesCollectionView.SortDescriptions.Add(new SortDescription("Arrived", ListSortDirection.Descending));
                ShowUpsFunctionalPilatesCollectionView.Filter = ProgramsPilatesFunctionalFilter;

                ShowUpsMassCollectionView = new ListCollectionView(ShowUps);
                ShowUpsMassCollectionView.SortDescriptions.Add(new SortDescription("Arrived", ListSortDirection.Descending));
                ShowUpsMassCollectionView.Filter = ProgramsMassFilter;

                ShowUpsOnlineCollectionView = new ListCollectionView(ShowUps);
                ShowUpsOnlineCollectionView.SortDescriptions.Add(new SortDescription("Arrived", ListSortDirection.Descending));
                ShowUpsOnlineCollectionView.Filter = ProgramsOnlineFilter;

                ShowUpsOutDoorCollectionView = new ListCollectionView(ShowUps);
                ShowUpsOutDoorCollectionView.SortDescriptions.Add(new SortDescription("Arrived", ListSortDirection.Descending));
                ShowUpsOutDoorCollectionView.Filter = ProgramsOutdoorFilter;

                ShowUpsYogaCollectionView = new ListCollectionView(ShowUps);
                ShowUpsYogaCollectionView.SortDescriptions.Add(new SortDescription("Arrived", ListSortDirection.Descending));
                ShowUpsYogaCollectionView.Filter = ProgramsYogaFilter;

                ShowUpsAerialYogaCollectionView = new ListCollectionView(ShowUps);
                ShowUpsAerialYogaCollectionView.SortDescriptions.Add(new SortDescription("Arrived", ListSortDirection.Descending));
                ShowUpsAerialYogaCollectionView.Filter = ProgramsAerialYogaFilter;

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

        private ICollectionView _ShowUpsPilatesCollectionView;

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

        private ICollectionView _ShowUpsFunctionalPilatesCollectionView;

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

        private ICollectionView _ShowUpsYogaCollectionView;

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

        private ICollectionView _ShowUpsAerialYogaCollectionView;

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

        private ICollectionView _ShowUpsOutDoorCollectionView;

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

        [NotMapped]
        public RelayCommand<ShowUp> Toggle30_60Command { get; set; }

        [NotMapped]
        public RelayCommand<ShowUp> ToggleIsPresentCommand { get; set; }

        [NotMapped]
        public RelayCommand<ShowUp> ToggleRealCommand { get; set; }

        [NotMapped]
        public RelayCommand<object[]> ToggleShowUpCommand { get; set; }

        public string TypeOfProgram =>
            SelectedFunctionalProgram != null ? SelectedFunctionalProgram.ToString() :
            SelectedPilatesProgram != null ? SelectedPilatesProgram.ToString() :
            SelectedFunctionalPilatesProgram != null ? SelectedFunctionalPilatesProgram.ToString() :
            SelectedMasageProgram != null ? SelectedMasageProgram.ToString() :
            SelectedOnlineProgram != null ? SelectedOnlineProgram.ToString() :
            SelectedOutdoorProgram != null ? SelectedOutdoorProgram.ToString() :
            SelectedYogaProgram != null ? SelectedYogaProgram.ToString() :
            SelectedAerialYogaProgram != null ? SelectedAerialYogaProgram.ToString() :
            "Ανενεργό";

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




        private bool _Psek;


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
                ShowUps = new ObservableCollection<ShowUp>();
            if (Payments == null)
                Payments = new ObservableCollection<Payment>();
            if (Programs == null)
                Programs = new ObservableCollection<Program>();
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
            OpenPopup1Command = new RelayCommand(() => { Popup1Open = true; });
            ToggleShowUpCommand = new RelayCommand<object[]>((par) => TogleShowUp(par), CanToggleShowUp);

            SetToProgramCommand = new RelayCommand<Payment>(async (obj) => { await SetToProgram(obj); }, CanSet);
            ReleasePaymentCommand = new RelayCommand<Payment>(ReleasePayment, CanReleasePayment);
            AddItemCommand = new RelayCommand(AddItem, CanAddItem);

            OldShowUps = new ObservableCollection<ShowUp>();
            NextAppointments = new ObservableCollection<Apointment>();

            //DisableCustomerCommand = new RelayCommand(DisableCustomer);
            OldShowUpDate = DateOfPayment = DateOfIssue = StartDate = DateTime.Today;
            SelectedProgramType = null;
        }

        private bool CanToggleReal(ShowUp arg)
        {
            return arg != null;
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

        private ObservableCollection<Apointment> _NextAppointments;

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

        private ObservableCollection<ShowUp> _OldShowUps;

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

        private async Task ShowHistory()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            OldShowUps = new ObservableCollection<ShowUp>((await BasicDataManager.Context.GetAllShowUpsInRangeAsyncsAsync(HistoryFrom, DateTime.Now, Id, true)).OrderByDescending(a => a.Arrived));
            var apps = await BasicDataManager.Context.Context.Apointments.Where(a => a.Customer.Id == Id && a.DateTime >= HistoryFrom).ToListAsync();
            NextAppointments = new ObservableCollection<Apointment>(apps.Where(a => a.DateTime > DateTime.Now).OrderBy(a => a.DateTime));
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void DeleteItem(object obj)
        {
            if (obj is ItemPurchase ip && Items.Contains(ip))
            {
                Items.Remove(ip);
            }
        }

        private void AddItem()
        {
            Items.Add(new ItemPurchase { Item = SelectedItem, Size = SelectedSize, Date = DateTime.Today, Color = SelectedColor });
        }

        private ClothColors? _SelectedColor;

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

        private SizeEnum? _SelectedSize;

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

        private Item _SelectedItem;

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

        private bool _Vacinated;

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

        private bool _CertifOfIllness;

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

        private bool CanAddItem()
        {
            return (SelectedSize != null || SelectedItem?.Id == 3) && SelectedItem != null;
        }

        //public async void SetColors2()
        //{
        //    if (!Loaded) return;
        //    if (Programs != null && ShowUps != null && ShowUps.Count > 0 && Programs.Count > 0)
        //    {
        //        foreach (var prog in Programs)
        //            prog.Color = new SolidColorBrush(Colors.Transparent);
        //        foreach (var showUp in ShowUps)
        //            showUp.Color = new SolidColorBrush(Colors.Transparent);
        //        int progIndex = 0;
        //        var programsReversed = Programs.Where(o => o.ProgramMode != ProgramMode.massage && o.ProgramMode != ProgramMode.online && o.ProgramMode != ProgramMode.outdoor).OrderBy(p => p.StartDay).ThenByDescending(p => p.Id).ToList();
        //        var Limit = programsReversed.Count > 0 ? programsReversed[0].StartDay : new DateTime();
        //        var showUpsReserved = ShowUps.Where(s => s.Arrived >= Limit && s.ProgramModeNew == ProgramMode.functional).OrderBy(r => r.Arrived).ThenByDescending(r => r.Id).ToList();
        //        int counter = 0;
        //        Program selProg = null;
        //        foreach (var showUp in showUpsReserved)
        //        {
        //            if (showUp.Id == 4405)
        //            {
        //            }
        //            if (selProg == null || selProg.RemainingDays == 0 || (selProg.ProgramType == 19 && showUp.Arrived.Date > selProg.AddMonth().AddDays(1)))
        //            {
        //                if (progIndex >= programsReversed.Count)
        //                {
        //                    if (selProg != null)
        //                    {
        //                        selProg.RemainingDays = 0;
        //                    }
        //                    break;
        //                }
        //                counter = 1;
        //                selProg = programsReversed[progIndex];
        //                if (selProg.ProgramType == 19 && showUp.Arrived.Date > selProg.AddMonth().AddDays(1))
        //                {
        //                    progIndex++;
        //                    selProg = programsReversed[progIndex];
        //                }
        //                selProg.RemainingDays = selProg.Showups;
        //                selProg.Color = progIndex % 2 == 0 ? new SolidColorBrush(Colors.LightGreen) : new SolidColorBrush(Colors.Orange);
        //                progIndex++;
        //            }

        //            if (selProg == null || selProg.StartDay > showUp.Arrived || (selProg.RemainingDays == 0 && progIndex == programsReversed.Count))
        //                continue;
        //            showUp.Color = selProg.Color;
        //            showUp.Prog = selProg;
        //            showUp.Count = counter++;
        //            selProg.RemainingDays--;
        //        }
        //        if (progIndex < programsReversed.Count)
        //        {
        //            selProg = programsReversed[progIndex];
        //            selProg.RemainingDays = selProg.Showups;
        //        }
        //        //SelectedProgram = selProg;
        //        for (int i = progIndex; i < programsReversed.Count; i++)
        //        {
        //            selProg = programsReversed[progIndex];
        //            selProg.Color = progIndex % 2 == 0 ? new SolidColorBrush(Colors.LightGreen) : new SolidColorBrush(Colors.Orange);
        //        }
        //        programsReversed = Programs.Where(o => o.ProgramMode == ProgramMode.massage).OrderBy(p => p.StartDay).ThenBy(p => p.Id).ToList();
        //        Limit = programsReversed.Count > 0 ? programsReversed[0].StartDay : new DateTime();
        //        showUpsReserved = ShowUps.Where(s => s.Arrived >= Limit && s.ProgramModeNew == ProgramMode.massage).OrderBy(r => r.Arrived).ThenByDescending(r => r.Id).ToList();
        //        selProg = null; progIndex = 0;
        //        foreach (var showUp in showUpsReserved)
        //        {
        //            if ((selProg == null || selProg.RemainingDays == 0) && progIndex < programsReversed.Count)
        //            {
        //                counter = 1;
        //                selProg = programsReversed[progIndex];
        //                selProg.RemainingDays = selProg.Showups;
        //                selProg.Color = progIndex % 2 == 0 ? new SolidColorBrush(Colors.Aqua) : new SolidColorBrush(Colors.LightBlue);
        //                progIndex++;
        //            }

        //            if (selProg == null || selProg.StartDay > showUp.Arrived || (selProg.RemainingDays == 0 && progIndex == programsReversed.Count))
        //                continue;
        //            showUp.Color = selProg.Color;
        //            showUp.Count = counter++;
        //            showUp.Prog = selProg;
        //            selProg.RemainingDays--;
        //        }
        //        if (progIndex < programsReversed.Count)
        //        {
        //            selProg = programsReversed[progIndex];
        //            selProg.RemainingDays = selProg.Showups;
        //        }
        //        //SelectedMasage = selProg;
        //        for (int i = progIndex; i < programsReversed.Count; i++)
        //        {
        //            selProg = programsReversed[progIndex];
        //            selProg.Color = progIndex % 2 == 0 ? new SolidColorBrush(Colors.Aqua) : new SolidColorBrush(Colors.LightBlue);
        //        }
        //        programsReversed = Programs.Where(o => o.ProgramMode == ProgramMode.online).OrderBy(p => p.StartDay).ThenBy(p => p.Id).ToList();
        //        Limit = programsReversed.Count > 0 ? programsReversed[0].StartDay : new DateTime();
        //        showUpsReserved = ShowUps.Where(s => s.Arrived >= Limit && s.ProgramModeNew == ProgramMode.online).OrderBy(r => r.Arrived).ThenByDescending(r => r.Id).ToList();
        //        selProg = null; progIndex = 0;
        //        foreach (var showUp in showUpsReserved)
        //        {
        //            if ((selProg == null || selProg.RemainingDays == 0) && progIndex < programsReversed.Count)
        //            {
        //                counter = 1;
        //                selProg = programsReversed[progIndex];
        //                selProg.RemainingDays = selProg.Showups;
        //                selProg.Color = progIndex % 2 == 0 ? new SolidColorBrush(Colors.MediumSeaGreen) : new SolidColorBrush(Colors.MediumSpringGreen);
        //                progIndex++;
        //            }

        //            if (selProg == null || selProg.StartDay > showUp.Arrived || (selProg.RemainingDays == 0 && progIndex == programsReversed.Count))
        //                continue;
        //            showUp.Color = selProg.Color;
        //            showUp.Count = counter++;
        //            showUp.Prog = selProg;
        //            selProg.RemainingDays--;
        //        }
        //        if (progIndex < programsReversed.Count)
        //        {
        //            selProg = programsReversed[progIndex];
        //            selProg.RemainingDays = selProg.Showups;
        //        }
        //        //SelectedProgramOnline = selProg;
        //        for (int i = progIndex; i < programsReversed.Count; i++)
        //        {
        //            selProg = programsReversed[progIndex];
        //            selProg.Color = progIndex % 2 == 0 ? new SolidColorBrush(Colors.MediumSeaGreen) : new SolidColorBrush(Colors.MediumSpringGreen);
        //        }
        //        programsReversed = Programs.Where(o => o.ProgramMode == ProgramMode.outdoor).OrderBy(p => p.StartDay).ThenBy(p => p.Id).ToList();
        //        Limit = programsReversed.Count > 0 ? programsReversed[0].StartDay : new DateTime();
        //        showUpsReserved = ShowUps.Where(s => s.Arrived >= Limit && s.ProgramModeNew == ProgramMode.outdoor).OrderBy(r => r.Arrived).ThenByDescending(r => r.Id).ToList();
        //        selProg = null; progIndex = 0;
        //        foreach (var showUp in showUpsReserved)
        //        {
        //            if ((selProg == null || selProg.RemainingDays == 0) && progIndex < programsReversed.Count)
        //            {
        //                counter = 1;
        //                selProg = programsReversed[progIndex];
        //                selProg.RemainingDays = selProg.Showups;
        //                selProg.Color = progIndex % 2 == 0 ? new SolidColorBrush(Colors.LightPink) : new SolidColorBrush(Colors.HotPink);
        //                progIndex++;
        //            }

        //            if (selProg == null || selProg.StartDay > showUp.Arrived || (selProg.RemainingDays == 0 && progIndex == programsReversed.Count))
        //                continue;
        //            showUp.Color = selProg.Color;
        //            showUp.Count = counter++;
        //            showUp.Prog = selProg;
        //            selProg.RemainingDays--;
        //        }
        //        if (progIndex < programsReversed.Count)
        //        {
        //            selProg = programsReversed[progIndex];
        //            selProg.RemainingDays = selProg.Showups;
        //        }
        //        //SelectedProgramOnline = selProg;
        //        for (int i = progIndex; i < programsReversed.Count; i++)
        //        {
        //            selProg = programsReversed[progIndex];
        //            selProg.Color = progIndex % 2 == 0 ? new SolidColorBrush(Colors.LightPink) : new SolidColorBrush(Colors.HotPink);
        //        }
        //        SetRemaining();
        //    }
        //    //if (Programs != null && Payments != null && Payments.Count > 0 && Programs.Count > 0)
        //    //{
        //    //    Program selProg;
        //    //    int progIndex = 0;
        //    //    var programsReversed = Programs.OrderBy(p => p.StartDay).ThenBy(s => s.Id).ToList();
        //    //    selProg = programsReversed[progIndex];
        //    //    decimal remainingAmount = programsReversed[progIndex].Amount;
        //    //    decimal extraAmount = 0;
        //    //    decimal sum = 0;

        //    //    foreach (Payment payment in Payments.OrderBy(s => s.Date).ThenBy(s => s.Id))
        //    //    {
        //    //        sum += payment.Amount;
        //    //        if (payment.Amount < remainingAmount)
        //    //        {
        //    //            payment.Color = selProg.Color;
        //    //            remainingAmount -= payment.Amount;
        //    //        }
        //    //        else if (payment.Amount > remainingAmount)
        //    //        {
        //    //            //if (progIndex < programsReversed.Count && payment.Amount == remainingAmount + programsReversed[progIndex + 1].Amount)
        //    //            //{
        //    //            //}
        //    //            //else
        //    //            //{
        //    //            payment.Color = new SolidColorBrush(Colors.Green);
        //    //            extraAmount = payment.Amount;
        //    //            while (extraAmount > 0 && extraAmount >= selProg.Amount)
        //    //            {
        //    //                extraAmount -= remainingAmount;
        //    //                selProg.Color = new SolidColorBrush(Colors.Green);
        //    //                progIndex++;
        //    //                if (progIndex < programsReversed.Count)
        //    //                {
        //    //                    selProg = programsReversed[progIndex];
        //    //                    remainingAmount = selProg.Amount;
        //    //                }
        //    //                else
        //    //                {
        //    //                    break;
        //    //                }
        //    //            }
        //    //            //  }
        //    //            // remainingAmount -= selProg.Amount - extraAmount;
        //    //        }
        //    //        else
        //    //        {
        //    //            payment.Color = selProg.Color;
        //    //            progIndex++;
        //    //            if (progIndex < programsReversed.Count)
        //    //            {
        //    //                selProg = programsReversed[progIndex];
        //    //                remainingAmount = selProg.Amount;
        //    //            }
        //    //            else
        //    //                break;
        //    //        }
        //    //    }

        //    //    foreach (var p in Programs.OrderBy(p => p.DayOfIssue).ThenBy(s => s.Id))
        //    //    {
        //    //        p.PaidCol = sum >= p.Amount;
        //    //        sum -= p.Amount;
        //    //    }
        //    //}

        //    //RaisePropertyChanged(nameof(SelectedMasage));
        //    //RaisePropertyChanged(nameof(SelectedProgram));
        //    //RaisePropertyChanged(nameof(RemainingMassageDays));
        //    //RaisePropertyChanged(nameof(RemainingTrainingDays));
        //    //RaisePropertyChanged(nameof(RemainingDays));

        //    //ShowUpsCollectionView.Refresh();
        //    //ShowUpsMassCollectionView.Refresh();
        //    //ShowUpsOnlineCollectionView.Refresh();

        //    //PaymentsCollectionView.Refresh();
        //    //PaymentsMassCollectionView.Refresh();
        //    //PaymentsOnlineCollectionView.Refresh();

        //    //ProgramsColelctionView.Refresh();
        //    //ProgramsMassageColelctionView.Refresh();
        //    //ProgramsOnlineColelctionView.Refresh();

        //    //if (Id == 34)
        //    //{
        //    //    foreach (var showup in ShowUps)
        //    //    {
        //    //        if (showup.Prog != null)
        //    //        {
        //    //            showup.ProgramModeNew = showup.Prog.ProgramTypeO.ProgramMode;
        //    //        }
        //    //        else
        //    //        {
        //    //            showup.ProgramModeNew = showup.ProgramMode;
        //    //        }
        //    //    }

        //    //    if (BasicDataManager.HasChanges())
        //    //    {
        //    //        await BasicDataManager.SaveAsync();
        //    //    }
        //    //    Console.WriteLine($"Customer with id {Id} done");
        //    //}
        //}

        public async void SetColors()
        {
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
                    programsReversed = Programs.Where(o => o.ProgramTypeO.ProgramMode == mode).OrderBy(p => p.StartDay).ThenBy(p => p.Id).ToList();
                    Limit = programsReversed.Count > 0 ? programsReversed[0].StartDay : new DateTime();
                    showUpsReserved = ShowUps.Where(s => s.Arrived >= Limit && s.ProgramModeNew == mode).OrderBy(r => r.Arrived).ThenByDescending(r => r.Id).ToList();
                    selProg = null; progIndex = 0;
                    foreach (var showUp in showUpsReserved)
                    {
                        if ((selProg == null || selProg.RemainingDays == 0 || (selProg.ProgramTypeO.Id == 20 && showUp.Arrived.Date > selProg.AddMonth().AddDays(1))) && progIndex < programsReversed.Count)
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
                            selProg = programsReversed[progIndex];
                            if (selProg.ProgramTypeO.Id == 20 && showUp.Arrived.Date > selProg.AddMonth().AddDays(1))
                            {
                                progIndex++;
                                selProg = programsReversed[progIndex];
                            }
                            selProg.RemainingDays = selProg.Showups;
                            switch (mode)
                            {
                                case ProgramMode.functional:
                                case ProgramMode.massage:
                                case ProgramMode.online:
                                case ProgramMode.yoga:
                                    selProg.Color = progIndex % 2 == 0 ? new SolidColorBrush(Colors.LightGreen) : new SolidColorBrush(Colors.Orange);
                                    break;

                                case ProgramMode.outdoor:
                                case ProgramMode.pilates:
                                case ProgramMode.aerialYoga:
                                    selProg.Color = progIndex % 2 == 0 ? new SolidColorBrush(Colors.Aqua) : new SolidColorBrush(Colors.LightBlue);
                                    break;

                                case ProgramMode.pilatesFunctional:
                                default:
                                    selProg.Color = progIndex % 2 == 0 ? new SolidColorBrush(Colors.MediumSeaGreen) : new SolidColorBrush(Colors.MediumSpringGreen);
                                    break;
                            }
                            progIndex++;
                        }

                        if (selProg == null || selProg.StartDay > showUp.Arrived || (selProg.RemainingDays == 0 && progIndex == programsReversed.Count))
                            continue;
                        showUp.Color = selProg.Color;
                        showUp.Count = counter++;
                        showUp.Prog = selProg;
                        selProg.RemainingDays--;
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

                        default:
                            break;
                    }
                    for (int i = progIndex; i < programsReversed.Count; i++)
                    {
                        selProg = programsReversed[progIndex];
                        switch (mode)
                        {
                            case ProgramMode.functional:
                            case ProgramMode.massage:
                            case ProgramMode.online:
                            case ProgramMode.yoga:
                                selProg.Color = progIndex % 2 == 0 ? new SolidColorBrush(Colors.LightGreen) : new SolidColorBrush(Colors.Orange);
                                break;

                            case ProgramMode.outdoor:
                            case ProgramMode.pilates:
                            case ProgramMode.aerialYoga:
                                selProg.Color = progIndex % 2 == 0 ? new SolidColorBrush(Colors.Aqua) : new SolidColorBrush(Colors.LightBlue);
                                break;

                            case ProgramMode.pilatesFunctional:
                            default:
                                selProg.Color = progIndex % 2 == 0 ? new SolidColorBrush(Colors.MediumSeaGreen) : new SolidColorBrush(Colors.MediumSpringGreen);
                                break;
                        }
                    }
                }

                //programsReversed = Programs.Where(o => o.ProgramMode == ProgramMode.online).OrderBy(p => p.StartDay).ThenBy(p => p.Id).ToList();
                //Limit = programsReversed.Count > 0 ? programsReversed[0].StartDay : new DateTime();
                //showUpsReserved = ShowUps.Where(s => s.Arrived >= Limit && s.ProgramMode == ProgramMode.online).OrderBy(r => r.Arrived).ThenByDescending(r => r.Id).ToList();
                //selProg = null; progIndex = 0;
                //foreach (var showUp in showUpsReserved)
                //{
                //    if ((selProg == null || selProg.RemainingDays == 0) && progIndex < programsReversed.Count)
                //    {
                //        counter = 1;
                //        selProg = programsReversed[progIndex];
                //        selProg.RemainingDays = selProg.Showups;
                //        selProg.Color = progIndex % 2 == 0 ? new SolidColorBrush(Colors.MediumSeaGreen) : new SolidColorBrush(Colors.MediumSpringGreen);
                //        progIndex++;
                //    }

                //    if (selProg == null || selProg.StartDay > showUp.Arrived || (selProg.RemainingDays == 0 && progIndex == programsReversed.Count))
                //        continue;
                //    showUp.Color = selProg.Color;
                //    showUp.Count = counter++;
                //    showUp.Prog = selProg;
                //    selProg.RemainingDays--;
                //}
                //if (progIndex < programsReversed.Count)
                //{
                //    selProg = programsReversed[progIndex];
                //    selProg.RemainingDays = selProg.Showups;
                //}
                //SelectedProgramOnline = selProg;
                //for (int i = progIndex; i < programsReversed.Count; i++)
                //{
                //    selProg = programsReversed[progIndex];
                //    selProg.Color = progIndex % 2 == 0 ? new SolidColorBrush(Colors.MediumSeaGreen) : new SolidColorBrush(Colors.MediumSpringGreen);
                //}
                //programsReversed = Programs.Where(o => o.ProgramMode == ProgramMode.outdoor).OrderBy(p => p.StartDay).ThenBy(p => p.Id).ToList();
                //Limit = programsReversed.Count > 0 ? programsReversed[0].StartDay : new DateTime();
                //showUpsReserved = ShowUps.Where(s => s.Arrived >= Limit && s.ProgramMode == ProgramMode.outdoor).OrderBy(r => r.Arrived).ThenByDescending(r => r.Id).ToList();
                //selProg = null; progIndex = 0;
                //foreach (var showUp in showUpsReserved)
                //{
                //    if ((selProg == null || selProg.RemainingDays == 0) && progIndex < programsReversed.Count)
                //    {
                //        counter = 1;
                //        selProg = programsReversed[progIndex];
                //        selProg.RemainingDays = selProg.Showups;
                //        selProg.Color = progIndex % 2 == 0 ? new SolidColorBrush(Colors.LightPink) : new SolidColorBrush(Colors.HotPink);
                //        progIndex++;
                //    }

                //    if (selProg == null || selProg.StartDay > showUp.Arrived || (selProg.RemainingDays == 0 && progIndex == programsReversed.Count))
                //        continue;
                //    showUp.Color = selProg.Color;
                //    showUp.Count = counter++;
                //    showUp.Prog = selProg;
                //    selProg.RemainingDays--;
                //}
                //if (progIndex < programsReversed.Count)
                //{
                //    selProg = programsReversed[progIndex];
                //    selProg.RemainingDays = selProg.Showups;
                //}
                //SelectedProgramOnline = selProg;
                //for (int i = progIndex; i < programsReversed.Count; i++)
                //{
                //    selProg = programsReversed[progIndex];
                //    selProg.Color = progIndex % 2 == 0 ? new SolidColorBrush(Colors.LightPink) : new SolidColorBrush(Colors.HotPink);
                //}
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

            RaisePropertyChanged(nameof(RemainingTrainingDays));
            RaisePropertyChanged(nameof(RemainingPilatesDays));
            RaisePropertyChanged(nameof(RemainingFunctionalPilatesDays));
            RaisePropertyChanged(nameof(RemainingMassageDays));
            RaisePropertyChanged(nameof(RemainingOnlineDays));
            RaisePropertyChanged(nameof(RemainingOutDoorDays));
            RaisePropertyChanged(nameof(RemainingYogaDays));
            RaisePropertyChanged(nameof(RemainingAerialYogaDays));

            RaisePropertyChanged(nameof(Active));

            UpdateCollections();

            //foreach (var showup in ShowUps)
            //{
            //    if (showup.Prog!=null && showup.ProgramMode != showup.Prog.ProgramTypeO.ProgramMode)
            //    {
            //        showup.ProgramMode = showup.Prog.ProgramTypeO.ProgramMode;
            //    }
            //}

            //if (BasicDataManager.HasChanges())
            //{
            //    await BasicDataManager.SaveAsync();
            //}
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
            ShowUpsFunctionalCollectionView.Refresh();
            ShowUpsPilatesCollectionView.Refresh();
            ShowUpsFunctionalPilatesCollectionView.Refresh();
            ShowUpsMassCollectionView.Refresh();
            ShowUpsOnlineCollectionView.Refresh();
            ShowUpsOutDoorCollectionView.Refresh();
            ShowUpsYogaCollectionView.Refresh();
            ShowUpsAerialYogaCollectionView.Refresh();

            PaymentsFunctionalCollectionView.Refresh();
            PaymentsFunctionalPilatesCollectionView.Refresh();
            PaymentsPilatesCollectionView.Refresh();
            PaymentsMassCollectionView.Refresh();
            PaymentsOnlineCollectionView.Refresh();
            PaymentsOutDoorCollectionView.Refresh();
            PaymentsYogaCollectionView.Refresh();
            PaymentsAerialYogaCollectionView.Refresh();

            ProgramsFunctionalCollectionView.Refresh();
            ProgramsPilatesCollectionView.Refresh();
            ProgramsFunctionalPilatesCollectionView.Refresh();
            ProgramsMassageColelctionView.Refresh();
            ProgramsOnlineColelctionView.Refresh();
            ProgramsOutdoorCollectionView.Refresh();
            ProgramsYogaCollectionView.Refresh();
            ProgramsAerialYogaCollectionView.Refresh();
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

        private ProgramType _SelectedProgramType;

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

                if (value.Id == 20)
                {
                    NumOfShowUps = 30;
                    ProgramDuration = 1;
                }

                _SelectedProgramType = value;
                RaisePropertyChanged();
            }
        }

        public void ValidateProgram()
        {
            if (Programs.Any(p => p.StartDay <= StartDate && StartDate < p.StartDay.AddMonths(p.Months).AddDays(5) && p.RemainingDays > 0))
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
            else if (ProgramDuration < 1)
            {
                ProgramResult = "Προσοχή, δεν έχετε επιλέξει διάρκεια";
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
            Programs.Add(new Program { Amount = ProgramPrice, DayOfIssue = DateOfIssue, Showups = NumOfShowUps, ProgramTypeO = SelectedProgramType, Months = ProgramDuration, StartDay = StartDate, Paid = par == 1 });
            Changes.Add(new Change($"Προστέθηκε νέο ΠΑΚΕΤΟ {(SelectedProgramType != null ? SelectedProgramType.ToString() : "Σφάλμα")} με {NumOfShowUps} συνεδρίες, κόστος {StaticResources.DecimalToString(ProgramPrice)}, έναρξη {StartDate:dd/MM/yy}{(par == 1 ? "," : " και")}" +
                $" διάρκεια {ProgramDuration} μήνες {(par == 1 ? "και πληρώθηκε" : "χωρίς να πληρωθεί")}", StaticResources.User)
            { Program = Programs.Last() });
        }

        internal async Task AddPayment()
        {
            Payment p = new Payment { Amount = PaymentAmount, Date = DateOfPayment, User = StaticResources.User, PaymentType = PaymentType, Reciept = PaymentReciept };
            if (SelectedProgramFunctionalToDelete != null)
            {
                SelectedProgramFunctionalToDelete.Payments.Add(p);
                p.Program = SelectedProgramFunctionalToDelete;
                RaisePropertyChanged(nameof(PaymentVisibility));
                // await SaveChanges();
                SelectedProgramFunctionalToDelete.CalculateRemainingAmount();
            }
            else if (SelectedProgramMassageToDelete != null)
            {
                SelectedProgramMassageToDelete.Payments.Add(p);
                p.Program = SelectedProgramMassageToDelete;
                RaisePropertyChanged(nameof(PaymentVisibility));
                // await SaveChanges();
                SelectedProgramMassageToDelete.CalculateRemainingAmount();
            }
            else if (SelectedProgramOnlineToDelete != null)
            {
                SelectedProgramOnlineToDelete.Payments.Add(p);
                p.Program = SelectedProgramOnlineToDelete;
                RaisePropertyChanged(nameof(PaymentVisibility));
                // await SaveChanges();
                SelectedProgramOnlineToDelete.CalculateRemainingAmount();
            }
            Payments.Add(p);
            GetRemainingDays();
            //RaiseAllChanged();
            CommandManager.InvalidateRequerySuggested();
        }

        internal void MakeProgramPayment()
        {
            Payments.Add(new Payment { Amount = ProgramPrice, Date = DateOfIssue, User = StaticResources.User });
        }

        internal bool ProgramDataCheck()
        {
            ValidateProgram();
            return ProgramPrice >= 0 && SelectedProgramType != null && NumOfShowUps > 0 && ProgramDuration > 0;
        }

        internal void ShowedUp(bool arrived, ProgramMode mode, bool is30min = false)
        {
            IsPracticing = arrived;
            if (mode == ProgramMode.massage)
            {
                int remain = RemainingMassageDays;
                ShowUps.Add(new ShowUp { Arrive = arrived, Arrived = DateTime.Now, ProgramModeNew = mode, Is30min = is30min });
                if (RemainingMassageDays != 0) return;
                MessageBox.Show(remain > 0
                    ? $"Αυτή ήταν η τελευταία συνεδρία μασάζ του {ToString()}"
                    : $"Οι συνεδρίες μασάζ του {ToString()} έχουν τελειώσει");
            }
            else if (mode == ProgramMode.functional)
            {
                int remain = RemainingTrainingDays;
                ShowUps.Add(new ShowUp { Arrive = arrived, Arrived = DateTime.Now, ProgramModeNew = mode, Is30min = is30min });
                if (RemainingTrainingDays != 0) return;
                MessageBox.Show(remain > 0
                    ? $"Αυτή ήταν η τελευταία συνεδρία γυμναστικής του {ToString()}"
                    : $"Οι συνεδρίες γυμναστικής του {ToString()} έχουν τελειώσει");
            }
            else if (mode == ProgramMode.online)
            {
                int remain = RemainingOnlineDays;
                ShowUps.Add(new ShowUp { Arrive = arrived, Arrived = DateTime.Now, ProgramModeNew = mode, Is30min = is30min });
                if (RemainingOnlineDays != 0) return;
                MessageBox.Show(remain > 0
                    ? $"Αυτή ήταν η τελευταία συνεδρία Online του {ToString()}"
                    : $"Οι συνεδρίες Online του {ToString()} έχουν τελειώσει");
            }
            else if (mode == ProgramMode.outdoor)
            {
                int remain = RemainingOutDoorDays;
                ShowUps.Add(new ShowUp { Arrive = arrived, Arrived = DateTime.Now, ProgramModeNew = mode, Is30min = is30min });
                if (RemainingOutDoorDays != 0) return;
                MessageBox.Show(remain > 0
                    ? $"Αυτή ήταν η τελευταία συνεδρία OutDoor του {ToString()}"
                    : $"Οι συνεδρίες OutDoor του {ToString()} έχουν τελειώσει");
            }
        }

        private async Task AddOldShowUp(int programMode)
        {
            // Changes.Add(new Change($"Προστέθηκε ΠΑΡΟΥΣΙΑ {Enum.GetName(typeof(ProgramMode), programMode)} για  {OldShowUpDate: dd/MM/yy}", StaticResources.User));
            ShowUps.Add(new ShowUp { Arrived = OldShowUpDate, ProgramModeNew = (ProgramMode)programMode, Left = new DateTime(1234, 1, 1), Is30min = Is30min });
            Popup1Open = false;
            SetColors();
            // await BasicDataManager.SaveAsync();
        }

        private bool CanAddPayment()
        {
            return PaymentAmount > 0 && PaymentAmount <= RemainingAmount && (
                (SelectedProgramFunctionalToDelete != null && PaymentAmount <= SelectedProgramFunctionalToDelete.RemainingAmount) ||
                (SelectedProgramMassageToDelete != null && PaymentAmount <= SelectedProgramMassageToDelete.RemainingAmount) ||
                (SelectedProgramOnlineToDelete != null && PaymentAmount <= SelectedProgramOnlineToDelete.RemainingAmount));
        }

        private bool CanMakeBooking(string arg) => ProgramDataCheck();

        private bool CanSaveChanges()
        {
            return BasicDataManager.HasChanges();
        }

        private bool CanSet(Payment p)
        {
            return p != null && p.Program == null;
        }

        private bool CanReleasePayment(Payment p)
        {
            return p.Program != null;
        }

        private bool CanToggleShowUp(object[] arg) => arg[0] is ShowUp s && (string)arg[1] != ((int)s.ProgramModeNew).ToString();

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

        private async Task DeleteShowUp(ShowUp showup)
        {
            if (showup == null)
                return;
            Changes.Add(new Change($"Διαγράφηκε ΠΑΡΟΥΣΊΑ {StaticResources.GetDescription(showup.ProgramModeNew)} με ημερομηνία {showup.Arrived:ddd dd/MM/yy}", StaticResources.User));

            BasicDataManager.Delete(showup);
            // await BasicDataManager.SaveAsync();
        }

        private async Task DeletePayment(Payment payment)
        {
            Changes.Add(new Change($"Διαγράφηκε ΠΛΗΡΩΜΉ {StaticResources.DecimalToString(payment.Amount)} που είχε γίνει {payment.Date:ddd dd/MM/yy} για γυμναστική", StaticResources.User));
            if (payment.Program != null && payment.Program.Payments.Any(p => p.Id == payment.Id))
            {
                payment.Program.Payments.Remove(payment);
            }
            BasicDataManager.Delete(payment);
            RaisePropertyChanged(nameof(PaymentVisibility));
            //await BasicDataManager.SaveAsync();
        }

        private async Task DeleteProgram(Program prog)
        {
            if (prog == null)
            {
                return;
            }
            Changes.Add(new Change($"Διαγράφηκε ΠΡΌΓΡΑΜΜΑ {prog} που είχε καταχωρηθεί {prog.DayOfIssue:ddd dd/MM/yy} με " +
                $"διάρκεια {prog.Showups} μήνες αξίας {StaticResources.DecimalToString(prog.Amount)} και έναρξη {prog.StartDay:ddd dd/MM/yy}", StaticResources.User));

            BasicDataManager.Delete(prog);
            // await BasicDataManager.SaveAsync();
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
                                sb.Append($"Τύπος πακέτου από '{(original ?? "κενό")}' σε '{(current ?? "κενό")}', ");
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
        }

        private void PaymentPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Payment.Amount) && Loaded)
                CalculateRemainingAmount();
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

            if (Loaded)
            {
                CalculateRemainingAmount();
                GetRemainingDays();
            }

            RaisePropertyChanged(nameof(PaymentVisibility));
        }

        private void ProgramPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!Loaded || (e.PropertyName != nameof(Program.Amount) && e.PropertyName != nameof(Program.Showups) && e.PropertyName != nameof(Program.ProgramTypeO))) return;
            CalculateRemainingAmount();
            if (e.PropertyName == nameof(Program.Showups))
            {
                ((IEditableCollectionView)ProgramsFunctionalCollectionView).CommitEdit();
                ((IEditableCollectionView)ProgramsPilatesCollectionView).CommitEdit();
                ((IEditableCollectionView)ProgramsFunctionalPilatesCollectionView).CommitEdit();
                ((IEditableCollectionView)ProgramsMassageColelctionView).CommitEdit();
                ((IEditableCollectionView)ProgramsOnlineColelctionView).CommitEdit();
                ((IEditableCollectionView)ProgramsOutdoorCollectionView).CommitEdit();
                ((IEditableCollectionView)ProgramsYogaCollectionView).CommitEdit();
                ((IEditableCollectionView)ProgramsAerialYogaCollectionView).CommitEdit();
            }
            if (e.PropertyName != nameof(Program.Amount))
                GetRemainingDays();
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

            if (!Loaded) return;
            RaisePropertyChanged(nameof(LastBuy));
            CalculateRemainingAmount();
            GetRemainingDays();
        }

        private bool ProgramsFunctionalFilter(object obj)
        {
            try
            {
                return (obj is Program p && p.ProgramTypeO.ProgramMode == ProgramMode.functional) ||
                    (obj is ShowUp s && s.ProgramModeNew == ProgramMode.functional) ||
                    (obj is Payment a && (a.Program == null || a.Program.ProgramTypeO.ProgramMode == ProgramMode.functional));
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
                return (obj is Program p && p.ProgramTypeO.ProgramMode == ProgramMode.pilates) ||
                    (obj is ShowUp s && s.ProgramModeNew == ProgramMode.pilates) ||
                    (obj is Payment a && a.Program != null && a.Program.ProgramTypeO.ProgramMode == ProgramMode.pilates);
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
                return (obj is Program p && p.ProgramTypeO.ProgramMode == ProgramMode.pilatesFunctional) ||
                    (obj is ShowUp s && s.ProgramModeNew == ProgramMode.pilatesFunctional) ||
                    (obj is Payment a && a.Program != null && a.Program.ProgramTypeO.ProgramMode == ProgramMode.pilatesFunctional);
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
                return (obj is Program p && p.ProgramTypeO.ProgramMode == ProgramMode.massage) ||
                    (obj is ShowUp s && s.ProgramModeNew == ProgramMode.massage) ||
                    (obj is Payment a && a.Program != null && a.Program.ProgramTypeO.ProgramMode == ProgramMode.massage);
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
                return (obj is Program p && p.ProgramTypeO.ProgramMode == ProgramMode.online) ||
                    (obj is ShowUp s && s.ProgramModeNew == ProgramMode.online) ||
                  (obj is Payment a && a.Program != null && a.Program.ProgramTypeO.ProgramMode == ProgramMode.online);
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
                return (obj is Program p && p.ProgramTypeO.ProgramMode == ProgramMode.outdoor) ||
                    (obj is ShowUp s && s.ProgramModeNew == ProgramMode.outdoor) ||
                   (obj is Payment a && a.Program != null && a.Program.ProgramTypeO.ProgramMode == ProgramMode.outdoor);
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
                return (obj is Program p && p.ProgramTypeO.ProgramMode == ProgramMode.yoga) ||
                    (obj is ShowUp s && s.ProgramModeNew == ProgramMode.yoga) ||
                   (obj is Payment a && a.Program != null && a.Program.ProgramTypeO.ProgramMode == ProgramMode.yoga);
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool ProgramsAerialYogaFilter(object obj)
        {
            try
            {
                return (obj is Program p && p.ProgramTypeO.ProgramMode == ProgramMode.aerialYoga) ||
                    (obj is ShowUp s && s.ProgramModeNew == ProgramMode.aerialYoga) ||
                   (obj is Payment a && a.Program != null && a.Program.ProgramTypeO.ProgramMode == ProgramMode.aerialYoga);
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

        //private bool ProgramsOutDoorFilter(object obj)
        //{
        //    try
        //    {
        //        return (obj is Program p && p.ProgramMode == ProgramMode.outdoor) ||
        //            (obj is ShowUp s && s.ProgramMode == ProgramMode.outdoor) ||
        //            (obj is Payment a && a.Program != null && a.Program.ProgramMode == ProgramMode.outdoor);
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }
        //}
        private async Task SaveChanges()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            GetRemainingDays();
            CaptureChanges();
            await BasicDataManager.SaveAsync();
            // if (FromProgram)
            // {
            Messenger.Default.Send(new UpdateProgramMessage());
            FromProgram = false;
            // }
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
            p.RaisePropertyChanged(nameof(Payment.PaymentColor));
            await BasicDataManager.SaveAsync();
            GetRemainingDays();
        }

        private async Task ShowPreviewsData()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            Loaded = false;
            var c = await BasicDataManager.Context.GetFullCustomerById(Id);
            Loaded = true;

            SetColors();
            IsActiveColor = GetCustomerColor();
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void ShowUps_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            IsActiveColor = GetCustomerColor();
            GetRemainingDays();
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

        private void TogleShowUp(object[] props)
        {
            if (props[0] is ShowUp su && props[1] is string st && int.TryParse(st, out int i))
            {
                su.ProgramModeNew = (ProgramMode)i;
            }
            else
            {
                return;
            }
            GetRemainingDays();
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
    }
}