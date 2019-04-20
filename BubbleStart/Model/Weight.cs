using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BubbleStart.Model
{
    public class Weight : BaseModel
    {
        #region Constructors

        public Weight()
        {
            DateOfMeasure = DateTime.Today;
        }

        #endregion Constructors

        #region Fields

        private Customer _Customer;

        private DateTime _DateOfMeasure;

        private float _WeightValue;

        #endregion Fields

        #region Properties

        [NotMapped]
        public float BMI
        {
            get
            {
                return (float)Math.Round(WeightValue / (Customer != null ? (Customer.Height * Customer.Height / 10000) : (Height * Height / 10000)), 2); ;
            }
        }

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

        public DateTime DateOfMeasure
        {
            get
            {
                return _DateOfMeasure;
            }

            set
            {
                if (_DateOfMeasure == value)
                {
                    return;
                }

                _DateOfMeasure = value;
                RaisePropertyChanged();
            }
        }

        [NotMapped]
        public int Height { get; set; }

        public float WeightValue
        {
            get
            {
                return _WeightValue;
            }

            set
            {
                if (_WeightValue == value)
                {
                    return;
                }

                _WeightValue = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(BMI));
            }
        }

        #endregion Properties
    }
}