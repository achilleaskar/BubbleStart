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

                _Date = value;
                RaisePropertyChanged();
            }
        }

        #endregion Properties
    }
}