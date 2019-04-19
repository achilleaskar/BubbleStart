using System;

namespace BubbleStart.Model
{
    public class Program : BaseModel
    {
        #region Fields

        private int _Amount;
        private DateTime _DayOfIssue;
        private int _Duration;
        private ProgramTypes _ProgramType;
        private DateTime _StartDay;

        #endregion Fields

        #region Enums

        public enum ProgramTypes
        {
            daily30,
            daily60,
            pilates2,
            functional2,
            pilates5,
            functional5,
            freeUse
        }

        #endregion Enums

        #region Properties

        public int Amount
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

                _DayOfIssue = value;
                RaisePropertyChanged();
            }
        }

        public int Duration
        {
            get
            {
                return _Duration;
            }

            set
            {
                if (_Duration == value)
                {
                    return;
                }

                _Duration = value;
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

                _StartDay = value;
                RaisePropertyChanged();
            }
        }

        #endregion Properties
    }
}