using System;

namespace BubbleStart.Model
{
    public class Expense : BaseModel
    {

        public Expense()
        {
            Date = DateTime.Now;
        }
        #region Fields

        private decimal _Amount;
        private DateTime _Date;

        private string _Reason;

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

                _Date = value;
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