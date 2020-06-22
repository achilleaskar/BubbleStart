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

        private decimal _WeightValue;

        #endregion Fields

        #region Properties

        [NotMapped]
        public decimal BMI => Math.Round(WeightValue / (Customer != null ? (Customer.Height * Customer.Height / 10000) : (Height * Height / 10000)), 2);

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

        public DateTime DateOfMeasure
        {
            get => _DateOfMeasure;

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

        public decimal WeightValue
        {
            get
            {
                if (_WeightValue>1000)
                {
                    return _WeightValue / 100;
                }
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