using BubbleStart.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Windows.Media;

namespace BubbleStart.Model
{
    public class Program : BaseModel
    {
        #region Fields

        private decimal _Amount;
        private SolidColorBrush _Color;
        private DateTime _DayOfIssue;
        private int _Months;
        private bool _Paid;
        private bool _PaidCol;
        private ProgramTypes _ProgramType;
        private int _Showups;
        private DateTime _StartDay;

        #endregion Fields

        public IList<Change> Changes { get; set; }

        #region Enums

        [NotMapped]
        public int RemainingDays { get; set; }

        public bool IsMassage => Ismassage();

        private ProgramMode _ProgramMode;

        public ProgramMode ProgramMode
        {
            get
            {
                return _ProgramMode;
            }

            set
            {
                if (_ProgramMode == value)
                {
                    return;
                }

                _ProgramMode = value;
                RaisePropertyChanged();
            }
        }

        private bool Ismassage()
        {
            switch (ProgramType)
            {
                case ProgramTypes.masasRel30:
                case ProgramTypes.masazRel50:
                case ProgramTypes.masazTher30:
                case ProgramTypes.masazTher50:
                case ProgramTypes.massage41:
                    return true;
            }
            return false;
        }

        private ObservableCollection<Payment> _Payments = new ObservableCollection<Payment>();

        public ObservableCollection<Payment> Payments
        {
            get => _Payments;

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

        [TypeConverter(typeof(EnumDescriptionTypeConverter))]
        public enum ProgramTypes
        {
            [Description("Reformer Pilates")]
            ReformerPilates = 0,

            [Description("Pilates")]
            Pilates = 1,

            [Description("Functional")]
            Functional = 2,

            [Description("Pilates &amp; Functional")]
            PilatesFunctional = 3,

            [Description("Ελεύθερη Χρήση")]
            freeUse = 4,

            [Description("Medical Exercise")]
            MedicalExersise = 5,

            [Description("Personal")]
            dokimastiko = 6,

            [Description("Yoga")]
            yoga = 7,

            [Description("Aerial Yoga")]
            aerial = 8,

            [Description("Μασάζ Χαλαρωτικό 30'")]
            masasRel30 = 9,

            [Description("Μασάζ Χαλαρωτικό 50'")]
            masazRel50 = 10,

            [Description("Μασάζ Θεραπευτικό 30'")]
            masazTher30 = 11,

            [Description("Μασάζ Θεραπευτικό 50'")]
            masazTher50 = 12,

            [Description("Black Friday Deal")]
            blackfriday = 13,

            [Description("4+1 massage")]
            massage41 = 14,

            [Description("Online")]
            online = 15,

            [Description("Summer Deal")]
            summerDeal = 16,

            [Description("OutDoor")]
            OutDoor = 17,

            [Description("September Deal")]
            September = 18,

            [Description("Μηνιαίο πακέτο Γυμναστικής")]
            Month = 19
        }

        #endregion Enums

        #region Properties

        public decimal Amount
        {
            get => _Amount;

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

        [NotMapped]
        public SolidColorBrush Color
        {
            get => _Color;

            set
            {
                if (_Color == value)
                {
                    return;
                }

                _Color = value;
                RaisePropertyChanged();
            }
        }

        public DateTime DayOfIssue
        {
            get => _DayOfIssue;

            set
            {
                if (_DayOfIssue == value)
                {
                    return;
                }
                if (value == null)
                {
                    RaisePropertyChanged();
                    return;
                }

                _DayOfIssue = value;
                RaisePropertyChanged();
            }
        }

        public int Months
        {
            get => _Months;

            set
            {
                if (_Months == value)
                {
                    return;
                }

                _Months = value;
                RaisePropertyChanged();
            }
        }

        public void CalculateRemainingAmount()
        {
            decimal tmpAmount = Amount;
            if (Payments != null && Payments.Count > 0)
            {
                foreach (var p in Payments)
                {
                    tmpAmount -= p.Amount;
                }
            }
            RemainingAmount = tmpAmount;
            if (tmpAmount > 0)
            {
                PaidCol = false;
            }
            else
            {
                PaidCol = true;
            }
        }

        private decimal _RemainingAmount;

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

        public bool Paid
        {
            get => _Paid;

            set
            {
                if (_Paid == value)
                {
                    return;
                }

                _Paid = value;
                RaisePropertyChanged();
            }
        }

        [NotMapped]
        public bool PaidCol
        {
            get => _PaidCol;

            set
            {
                if (_PaidCol == value)
                {
                    return;
                }

                _PaidCol = value;
                RaisePropertyChanged();
            }
        }

        public ProgramTypes ProgramType
        {
            get => _ProgramType;

            set
            {
                if (_ProgramType == value)
                {
                    return;
                }

                _ProgramType = value;
                SetProgramMode();
                RaisePropertyChanged();
            }
        }

        private void SetProgramMode()
        {
            switch (ProgramType)
            {
                case ProgramTypes.masasRel30:
                case ProgramTypes.masazRel50:
                case ProgramTypes.masazTher30:
                case ProgramTypes.masazTher50:
                case ProgramTypes.massage41:
                    ProgramMode = ProgramMode.massage;
                    break;

                case ProgramTypes.online:
                    ProgramMode = ProgramMode.online;
                    break;

                case ProgramTypes.OutDoor:
                    ProgramMode = ProgramMode.outdoor;
                    break;

                default:
                    ProgramMode = ProgramMode.normal;
                    break;
            }
        }

        [NotMapped]
        public decimal ShowUpPrice => Amount > 0 ? Amount / Showups : 0;

        public int Showups
        {
            get => _Showups;

            set
            {
                if (_Showups == value)
                {
                    return;
                }

                _Showups = value;
                RaisePropertyChanged();
            }
        }

        public DateTime StartDay
        {
            get => _StartDay;

            set
            {
                if (_StartDay == value)
                {
                    return;
                }
                if (value == null)
                {
                    RaisePropertyChanged();
                    return;
                }

                _StartDay = value;
                RaisePropertyChanged();
            }
        }

        #endregion Properties

        #region Methods

        private bool _IsSelected;

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

        [NotMapped]
        public string TypeString
        {
            get
            {
                return ToString();
            }

            set
            {
                switch (value)
                {
                    case "Reformer Pilates":
                        ProgramType = ProgramTypes.ReformerPilates;
                        break;

                    case "Pilates":
                        ProgramType = ProgramTypes.Pilates;
                        break;

                    case "Functional":
                        ProgramType = ProgramTypes.Functional;
                        break;

                    case "Pilates & Functional":
                        ProgramType = ProgramTypes.PilatesFunctional;
                        break;

                    case "Ελεύθερη Χρήση":
                        ProgramType = ProgramTypes.freeUse;
                        break;

                    case "Medical Exercise":
                        ProgramType = ProgramTypes.MedicalExersise;
                        break;

                    case "Personal":
                        ProgramType = ProgramTypes.dokimastiko;
                        break;

                    case "Yoga":
                        ProgramType = ProgramTypes.yoga;
                        break;

                    case "Aerial Yoga":
                        ProgramType = ProgramTypes.aerial;
                        break;

                    case "Μασάζ Χαλαρωτικό 30'":
                        ProgramType = ProgramTypes.masasRel30;
                        break;

                    case "Μασάζ Χαλαρωτικό 50'":
                        ProgramType = ProgramTypes.masazRel50;
                        break;

                    case "Μασάζ Θεραπευτικό 30'":
                        ProgramType = ProgramTypes.masazTher30;
                        break;

                    case "Μασάζ Θεραπευτικό 50'":
                        ProgramType = ProgramTypes.masazTher50;
                        break;

                    case "Black Friday Deal":
                        ProgramType = ProgramTypes.blackfriday;
                        break;

                    case "4+1 massage":
                        ProgramType = ProgramTypes.massage41;
                        break;

                    case "Online":
                        ProgramType = ProgramTypes.online;
                        break;

                    case "Summer Deal":
                        ProgramType = ProgramTypes.summerDeal;
                        break;

                    case "OutDoor":
                        ProgramType = ProgramTypes.OutDoor;
                        break;

                    case "September Deal":
                        ProgramType = ProgramTypes.September;
                        break;

                    case "Μηνιαίο πακέτο Γυμναστικής":
                        ProgramType = ProgramTypes.Month;
                        break;
                }
                RaisePropertyChanged();
            }
        }

        public override string ToString()
        {
            switch (ProgramType)
            {
                case ProgramTypes.ReformerPilates:
                    return "Reformer Pilates";

                case ProgramTypes.Pilates:
                    return "Pilates";

                case ProgramTypes.Functional:
                    return "Functional";

                case ProgramTypes.PilatesFunctional:
                    return "Pilates & Functional";

                case ProgramTypes.freeUse:
                    return "Ελεύθερη Χρήση";

                case ProgramTypes.MedicalExersise:
                    return "Medical Exercise";

                case ProgramTypes.dokimastiko:
                    return "Personal";

                case ProgramTypes.yoga:
                    return "Yoga";

                case ProgramTypes.aerial:
                    return "Aerial Yoga";

                case ProgramTypes.masasRel30:
                    return "Μασάζ Χαλαρωτικό 30'";

                case ProgramTypes.masazRel50:
                    return "Μασάζ Χαλαρωτικό 50'";

                case ProgramTypes.masazTher30:
                    return "Μασάζ Θεραπευτικό 30'";

                case ProgramTypes.masazTher50:
                    return "Μασάζ Θεραπευτικό 50'";

                case ProgramTypes.blackfriday:
                    return "Black Friday Deal";

                case ProgramTypes.massage41:
                    return "4+1 massage";

                case ProgramTypes.online:
                    return "Online";

                case ProgramTypes.summerDeal:
                    return "Summer Deal";

                case ProgramTypes.OutDoor:
                    return "OutDoor";

                case ProgramTypes.September:
                    return "September Deal";

                case ProgramTypes.Month:
                    return "Μηνιαίο πακέτο Γυμναστικής";
            }
            return "Ανενεργό";
        }

        internal DateTime AddMonth()
        {
            return StartDay.AddMonths(1).AddDays(-1);
        }

        #endregion Methods
    }
}