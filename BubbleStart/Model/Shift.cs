using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BubbleStart.Model
{
    [Table("BubbleShifts")]
    public class Shift : BaseModel
    {
        #region Fields

        private DateTime _From;

        private DateTime? _FromB;

        private string _Name;
        private bool _Parted;
        private DateTime _To;

        private DateTime? _ToB;

        #endregion Fields

        #region Properties

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
                RaisePropertyChanged();
            }
        }

        public DateTime? FromB
        {
            get
            {
                return _FromB;
            }

            set
            {
                if (_FromB == value)
                {
                    return;
                }

                _FromB = value;
                RaisePropertyChanged();
            }
        }

        public string Name
        {
            get
            {
                return _Name;
            }

            set
            {
                if (_Name == value)
                {
                    return;
                }

                _Name = value;
                RaisePropertyChanged();
            }
        }

        public bool Parted
        {
            get
            {
                return _Parted;
            }

            set
            {
                if (_Parted == value)
                {
                    return;
                }

                _Parted = value;
                RaisePropertyChanged();
            }
        }

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
                RaisePropertyChanged();
            }
        }

        public DateTime? ToB
        {
            get
            {
                return _ToB;
            }

            set
            {
                if (_ToB == value)
                {
                    return;
                }

                _ToB = value;
                RaisePropertyChanged();
            }
        }

        #endregion Properties

        public override string ToString()
        {
            if (Id == 3)
            {
                return "Ρεπό";
            }
            return $"{Name} ({From.ToString("HH:mm")}-{To.ToString("HH:mm")}" + (FromB.HasValue && ToB.HasValue ? $" & {FromB.Value.ToString("HH:mm")}-{ToB.Value.ToString("HH:mm")})" : ")");
        }
    }
}