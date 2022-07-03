using System;
using BubbleStart.Helpers;

namespace BubbleStart.Model
{
    public class ItemPurchase : BaseModel
    {
        private DateTime _Date;
        private decimal _Price;

        public decimal Price
        {
            get
            {
                return _Price;
            }

            set
            {
                if (_Price == value)
                {
                    return;
                }

                _Price = value;
                RaisePropertyChanged();
            }
        }

        private string _ColorString;

        public string ColorString
        {
            get
            {
                return _ColorString;
            }

            set
            {
                if (_ColorString == value)
                {
                    return;
                }

                _ColorString = value;
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

        private ClothColors? _Color;

        public ClothColors? Color
        {
            get
            {
                return _Color;
            }

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

        private SizeEnum? _Size;

        public SizeEnum? Size
        {
            get
            {
                return _Size;
            }

            set
            {
                if (_Size == value)
                {
                    return;
                }

                _Size = value;
                RaisePropertyChanged();
            }
        }

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

        public int? CustomerId { get; set; }
        public int? ItemId { get; set; }

        private Item _Item;

        public Item Item
        {
            get
            {
                return _Item;
            }

            set
            {
                if (_Item == value)
                {
                    return;
                }

                _Item = value;
                RaisePropertyChanged();
            }
        }
    }
}