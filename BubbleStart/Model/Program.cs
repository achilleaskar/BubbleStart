using System;
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

        #region Enums

        public enum ProgramTypes
        {
            ReformerPilates,
            Pilates,
            Functional,
            PilatesFunctional,
            freeUse,
            MedicalExersise,
            dokimastiko
        }

        #endregion Enums

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

        [NotMapped]
        public SolidColorBrush Color
        {
            get
            {
                return _Color;
            }

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
            get
            {
                return _DayOfIssue;
            }

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
            get
            {
                return _Months;
            }

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

        public bool Paid
        {
            get
            {
                return _Paid;
            }

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
            get
            {
                return _PaidCol;
            }

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
            get
            {
                return _ProgramType;
            }

            set
            {
                if (_ProgramType == value)
                {
                    return;
                }

                _ProgramType = value;
                RaisePropertyChanged();
            }
        }

        [NotMapped]
        public decimal ShowUpPrice => Amount > 0 ? Amount / Showups : 0;

        public int Showups
        {
            get
            {
                return _Showups;
            }

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
            get
            {
                return _StartDay;
            }

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
                    return "Ελέυθερη Χρήση";

                case ProgramTypes.MedicalExersise:
                    return "Medical Exercise";
                case ProgramTypes.dokimastiko:
                    return "Δοκιμαστικό";
            }
            return "Ανενεργό";
        }

        #endregion Methods

    }
}