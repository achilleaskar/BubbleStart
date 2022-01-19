using BubbleStart.Helpers;
using System;
using System.ComponentModel.DataAnnotations;

namespace BubbleStart.Model
{
    public class CustomeTime : BaseModel
    {
        private string _Time;

        [StringLength(10)]
        public string Time
        {
            get
            {
                return _Time;
            }

            set
            {
                if (_Time == value)
                {
                    return;
                }

                _Time = value;
                RaisePropertyChanged();
            }
        }

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
    }
}