using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BubbleStart.Model
{
    [Table("BubblePayments")]
    public class Payment : BaseModel
    {
        #region Constructors

        public Payment()
        {
            Date = DateTime.Now;
        }

        #endregion Constructors

        #region Fields

        private int _Amount;
        private DateTime _Date;

        #endregion Fields

        #region Properties







        private Customer _Customer;


        public Customer Customer
        {
            get
            {
                return _Customer;
            }

            set
            {
                if (_Customer == value)
                {
                    return;
                }

                _Customer = value;
                RaisePropertyChanged();
            }
        }

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

        public DateTime Date
        {
            get
            {
                return _Date;
            }

            set
            {
                if (_Date == value)
                {
                    return;
                }
                if (value == null)
                {
                    RaisePropertyChanged();
                    return;
                }

                _Date = value;
                RaisePropertyChanged();
            }
        }

        #endregion Properties
    }
}