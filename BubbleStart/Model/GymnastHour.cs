using BubbleStart.Helpers;
using System;

namespace BubbleStart.Model
{
    public class GymnastHour : BaseModel
    {
        private DateTime _Datetime;

        public DateTime Datetime
        {
            get
            {
                return _Datetime;
            }

            set
            {
                if (_Datetime == value)
                {
                    return;
                }

                _Datetime = value;
                RaisePropertyChanged();
            }
        }

        private bool _Forever;

        public bool Forever
        {
            get
            {
                return _Forever;
            }

            set
            {
                if (_Forever == value)
                {
                    return;
                }

                _Forever = value;
                RaisePropertyChanged();
            }
        }

        private RoomEnum _Room;

        public RoomEnum Room
        {
            get
            {
                return _Room;
            }

            set
            {
                if (_Room == value)
                {
                    return;
                }

                _Room = value;
                RaisePropertyChanged();
            }
        }

        private User _Gymnast;

        public User Gymnast
        {
            get
            {
                return _Gymnast;
            }

            set
            {
                if (_Gymnast == value)
                {
                    return;
                }

                _Gymnast = value;
                RaisePropertyChanged();
            }
        }

        public int? Gymnast_Id { get; set; }
    }
}