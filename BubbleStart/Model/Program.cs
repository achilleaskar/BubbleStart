using System;

namespace BubbleStart.Model
{
    public class Program : BaseModel
    {
        private DateTime _StartDay;

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





        private ProgramTypes _ProgramType;


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
        private DateTime _DayOfIssue;

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

        private int _Duration;

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

        private int _Amount;

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
    }
}