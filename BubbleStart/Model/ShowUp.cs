using System;

namespace BubbleStart.Model
{
    public class ShowUp : BaseModel
    {
        #region Fields

        private bool _Arrive;
        private DateTime _Left;
        private DateTime _Time;

        #endregion Fields

        #region Properties

        public bool Arrive
        {
            get
            {
                return _Arrive;
            }

            set
            {
                if (_Arrive == value)
                {
                    return;
                }

                _Arrive = value;
                RaisePropertyChanged();
            }
        }

        public DateTime Arrived
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
                if (value == null)
                {
                    RaisePropertyChanged();
                    return;
                }

                _Time = value;
                RaisePropertyChanged();
            }
        }

        public DateTime Left
        {
            get
            {
                return _Left;
            }

            set
            {
                if (_Left == value)
                {
                    return;
                }

                _Left = value;
                RaisePropertyChanged();
            }
        }

        #endregion Properties
    }
}