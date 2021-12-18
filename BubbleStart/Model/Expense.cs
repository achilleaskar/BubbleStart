using BubbleStart.Helpers;
using System;

namespace BubbleStart.Model
{
    public class Expense : BaseModel
    {
        public Expense()
        {
            Date = From = To = DateTime.Now;
        }

        #region Fields

        private decimal _Amount;
        private DateTime _Date;

        private string _Reason;

        private User _User;

        #endregion Fields

        private DateTime _From;

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
                if (To <= value)
                {
                    To = value;
                }
                RaisePropertyChanged();
            }
        }

        private DateTime _To;

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
                if (value < From)
                {
                    From = value;
                }
                RaisePropertyChanged();
            }
        }

        #region Properties

        private ExpenseCategory _ExpenseCategory;

        public ExpenseCategory ExpenseCategory
        {
            get
            {
                return _ExpenseCategory;
            }

            set
            {
                if (_ExpenseCategory == value)
                {
                    return;
                }

                _ExpenseCategory = value;
                RaisePropertyChanged();
            }
        }

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

        public DateTime Date
        {
            get => _Date;

            set
            {
                if (_Date == value)
                {
                    return;
                }

                _Date = value;
               // From = value;
                RaisePropertyChanged();
            }
        }

        public string Reason
        {
            get => _Reason;

            set
            {
                if (_Reason == value)
                {
                    return;
                }

                _Reason = value;
                RaisePropertyChanged();
            }
        }

        public User User
        {
            get => _User;

            set
            {
                if (_User == value)
                {
                    return;
                }

                _User = value;
                RaisePropertyChanged();
            }
        }

        #endregion Properties
    }
}