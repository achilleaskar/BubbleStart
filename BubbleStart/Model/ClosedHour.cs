using System;

namespace BubbleStart.Model
{
    public class ClosedHour : BaseModel
    {
        private DateTime _Date;

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




        private int _Room;


        public int Room
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