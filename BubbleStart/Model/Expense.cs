using BubbleStart.Helpers;
using System;

namespace BubbleStart.Model
{
    public class Expense : BaseModel
    {
        #region Constructors

        public Expense()
        {
            Date = From = To = DateTime.Today;
        }

        #endregion Constructors

        #region Fields

        private decimal _Amount;
        private DateTime _Date;
        private DateTime _From;
        private bool _Income;
        private ExpenseCategoryClass _MainCategory;
        private string _Reason;
        private ExpenseCategoryClass _SecondaryCategory;
        private DateTime _To;
        private User _User;

        #endregion Fields

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

        public DateTime Date
        {
            get => _Date;

            set
            {
                if (_Date == value)
                {
                    return;
                }

                if (From == To && To == _Date)
                {
                    From = To = value;
                }
                _Date = value;

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
                if (To <= value)
                {
                    To = value;
                }
                RaisePropertyChanged();
            }
        }

        public bool Income
        {
            get
            {
                return _Income;
            }

            set
            {
                if (_Income == value)
                {
                    return;
                }

                _Income = value;
                RaisePropertyChanged();
            }
        }

        public ExpenseCategoryClass MainCategory
        {
            get
            {
                return _MainCategory;
            }

            set
            {
                if (_MainCategory == value)
                {
                    return;
                }

                _MainCategory = value;
                RaisePropertyChanged();
            }
        }

        public int? MainCategoryId { get; set; }

        public string Reason
        {
            get => _Reason;

            set
            {
                if (_Reason == value)
                {
                    return;
                }

                _Reason = value.ToUpper();
                RaisePropertyChanged();
            }
        }

        public ExpenseCategoryClass SecondaryCategory
        {
            get
            {
                return _SecondaryCategory;
            }

            set
            {
                if (_SecondaryCategory == value)
                {
                    return;
                }

                _SecondaryCategory = value;
                RaisePropertyChanged();
            }
        }

        public int? SecondaryCategoryId { get; set; }

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