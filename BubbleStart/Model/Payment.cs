using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Windows.Media;
using BubbleStart.Helpers;

namespace BubbleStart.Model
{
    [Table("BubblePayments")]
    public class Payment : BaseModel
    {

        private bool _IsSelected;

        public IList<Change> Changes { get; set; }





        private bool _Reciept;


        public bool Reciept
        {
            get
            {
                return _Reciept;
            }

            set
            {
                if (_Reciept == value)
                {
                    return;
                }

                _Reciept = value;
                RaisePropertyChanged();
            }
        }




        private PaymentType _PaymentType;


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


        private Program _Program;


        public Program Program
        {
            get => _Program;

            set
            {
                if (_Program == value)
                {
                    return;
                }

                _Program = value;
                RaisePropertyChanged();
            }
        }


        public SolidColorBrush PaymentColor => Program != null ? Program.Color : new SolidColorBrush(Colors.White);



        public Payment()
        {
            Date = DateTime.Now;

        }
        private SolidColorBrush _Color;

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
        #region Fields

        private decimal _Amount;
        private Customer _Customer;
        private DateTime _Date;

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

        public Customer Customer
        {
            get => _Customer;

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

        public DateTime Date
        {
            get => _Date;

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